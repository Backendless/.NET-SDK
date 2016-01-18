using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
#if (NET_40)
using System.Collections.Concurrent;
#endif
using System.Reflection;

using Weborb;
using Weborb.Config;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Reader;
using Weborb.Registry;
using Weborb.Service;

namespace Weborb.Writer
{
  public class ObjectWriter : AbstractReferenceableTypeWriter
  {
#if (FULL_BUILD)
    private static ITypeWriter remoteReferenceWriter = new RemoteReferenceWriter();
#endif
    //private bool configured = false;
    private bool serializePrivate = false;
#if (NET_40)
    private ConcurrentDictionary<Type, ClassDefinition> cachedClassDefs = new ConcurrentDictionary<Type, ClassDefinition>();
#else
    private Dictionary<Type, ClassDefinition> cachedClassDefs = new Dictionary<Type, ClassDefinition>();
#endif
    private object _sync = new object();

    #region ITypeWriter Members

    public override void write( object obj, IProtocolFormatter writer )
    {
      //Log.log( ORBConstants.INFO, "ObjectWriter.write.begin: " + writer.BaseStream.Length );
#if (FULL_BUILD)
      if( obj is IRemote )
      {
        remoteReferenceWriter.write( RemoteReferenceObject.createReference( obj ), writer );
        return;
      }

      SerializationConfigHandler serializationConfig = (SerializationConfigHandler) ORBConfig.GetInstance().GetConfig( "weborb/serialization" );

      if( !configured )
      {
        //serializePrivate = ThreadContext.getORBConfig().serializePrivateFields;
        
        if( serializationConfig != null )
          serializePrivate = serializationConfig.SerializePrivateFields;

        configured = true;
      }
#endif
      Type objectClass = obj.GetType();

      /*
      if( obj is IAutoUpdate )
      {
          //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() && !NetUtils.RequestIsLocal( ThreadContext.currentRequest() ) )
          //    throw new Exception( "auto-update is disabled, this feature is available in WebORB Professional Edition" );

          string id = Guid.NewGuid().ToString();
          objectFields[ "_orbid_" ] = id;
          ThreadContext.currentHttpContext().Cache.Insert( id, new AutoUpdateObjectWrapper( ((IAutoUpdate) obj).GetUpdateHandler(), obj ) );
      }
       */

      ClassDefinition classDef;

      cachedClassDefs.TryGetValue( objectClass, out classDef );
      Dictionary<String, Object> objectFields = new Dictionary<String, Object>();

      if( classDef == null )
      {
        string className = objectClass.IsGenericType && objectClass.FullName != null
                             ? objectClass.FullName.Substring(0, objectClass.FullName.IndexOf("`"))
                             : objectClass.FullName;
        string clientSideMapping = GetClientClass( className );

        if( clientSideMapping != null )
        {
          // if( Log.isLogging( LoggingConstants.DEBUG ) )
          //    Log.log( LoggingConstants.DEBUG, "serializing a named object with client side mapping " + clientSideMapping );

          className = clientSideMapping;

        }
        // Commented out this code in 4.0.0.4. This code does not make sense anymore
        // For one these mappings exist between ServiceNames (or Destination names) and
        // classes. A service class should not be serialized back to the client.
        // If it is serialized, user should use class mapping for that.
        //else
        //{
        //    className = ServiceRegistry.GetReverseMapping( className );

        //    if( Log.isLogging( LoggingConstants.DEBUG ) )
        //        Log.log( LoggingConstants.DEBUG, "serializing object " + className );
        //}

        classDef = getClassDefinition( className, obj );
        lock(_sync)
        {
          cachedClassDefs[objectClass] = classDef;
        }

        // TODO: remove this try/catch
        //try
        //{
        //cachedClassDefs[ objectClass ] = classDef;
        // }
        //catch ( Exception e )
        // {
        //if ( Log.isLogging( LoggingConstants.ERROR ) )
        //  Log.log( LoggingConstants.ERROR, e );
        //System.Diagnostics.Debugger.Launch();
        //}
      }
      else
      {
        if( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "serializing using cached Class Def " + classDef.ClassName );
      }

      Dictionary<string, MemberInfo> members = classDef.Members;

      foreach( KeyValuePair<string, MemberInfo> member in members )
      {
        Object val = null;

        if( member.Value is PropertyInfo )
          try
          {
            val = ( (PropertyInfo) member.Value ).GetValue( obj, null );
          }
          catch( Exception exception )
          {
            if( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Unable to retrieve property/field value from an instance of " + classDef.ClassName + ". Value will be set to null. Property name is " + member.Value.Name );

            if( Log.isLogging( LoggingConstants.EXCEPTION ) )
              Log.log( LoggingConstants.EXCEPTION, exception );
          }
        else
          val = ( (FieldInfo) member.Value ).GetValue( obj );

        String memberName = member.Key;

#if(FULL_BUILD)
        if( serializationConfig != null && serializationConfig.Keywords.Contains( memberName ) )
          memberName = serializationConfig.PrefixForKeywords + memberName;
#endif

        objectFields[ memberName ] = val;
      }

      onWriteObject( obj, classDef.ClassName, objectFields, writer );
    }

    protected virtual ClassDefinition getClassDefinition( String className, Object obj )
    {
#if (FULL_BUILD)
      SerializationConfigHandler serializationConfig = (SerializationConfigHandler) ORBConfig.GetInstance().GetConfig( "weborb/serialization" );
#endif

      ClassDefinition classDef = new ClassDefinition();

      Type objectClass = obj.GetType();
      IPropertyExclusionAttribute[] propExclusion = (IPropertyExclusionAttribute[]) objectClass.GetCustomAttributes( typeof( IPropertyExclusionAttribute ), true );

      while( !Object.ReferenceEquals( objectClass, typeof( object ) ) )
      {
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        if( serializePrivate )
          flags |= BindingFlags.NonPublic;

        PropertyInfo[] props = objectClass.GetProperties( flags );

        for( int i = 0; i < props.Length; i++ )
        {
          if( !props[ i ].CanRead )
            continue;

          bool skipProperty = false;
          IPropertyExclusionAttribute[] propExclusionAttr = (IPropertyExclusionAttribute[]) props[ i ].GetCustomAttributes( typeof( IPropertyExclusionAttribute ), false );

          if( propExclusionAttr.Length > 0 )
            continue;

          foreach( IPropertyExclusionAttribute attr in propExclusion )
          {
            if( attr.ExcludeProperty( obj, props[ i ].Name ) )
            {
              skipProperty = true;
              break;
            }
          }

          if( skipProperty )
            continue;

          if( props[ i ].GetGetMethod().IsStatic )
            if( !cacheStaticField( className, props[ i ].Name ) )
              continue;

          IMemberRenameAttribute[] renamers = (IMemberRenameAttribute[]) props[ i ].GetCustomAttributes( typeof( IMemberRenameAttribute ), true );
          string memberName;

          if( renamers.Length > 0 )
            memberName = renamers[ 0 ].GetClientName( objectClass, props[ i ] );
          else
            memberName = PropertyRenaming.GetRenamingRule( objectClass, props[ i ].Name );
#if(FULL_BUILD)
          if( serializationConfig != null && serializationConfig.Keywords.Contains( memberName ) )
            memberName = serializationConfig.PrefixForKeywords + memberName;
#endif
          if( !classDef.ContainsMember( memberName ) )
          {
            //ITypeWriter typeWriter = MessageWriter.getWriter( props[ i ].PropertyType, null, false );
            classDef.AddMemberInfo( memberName, props[ i ] );
          }
        }

        flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        if( serializePrivate )
          flags |= BindingFlags.NonPublic;

        FieldInfo[] fields = objectClass.GetFields( flags );

        //Log.log( ORBConstants.INFO, "ObjectWriter.write.before writing fields: " + writer.BaseStream.Length );
        //if( Log.isLogging( LoggingConstants.DEBUG ) )
        //   Log.log( LoggingConstants.DEBUG, "number of fields: " + fields.Length );

        for( int i = 0; i < fields.Length; i++ )
        {
          if( fields[ i ].IsLiteral || fields[ i ].IsNotSerialized || fields[ i ].FieldType == typeof( IntPtr ) )
            continue;

          string fieldName = fields[ i ].Name;

          if( fields[ i ].IsStatic )
            if( !cacheStaticField( className, fieldName ) )
              continue;

          IMemberRenameAttribute[] renamers = (IMemberRenameAttribute[]) fields[ i ].GetCustomAttributes( typeof( IMemberRenameAttribute ), true );
          string memberName;

          if( renamers.Length > 0 )
            memberName = renamers[ 0 ].GetClientName( objectClass, fields[ i ] );
          else
            memberName = fields[ i ].Name;
#if( FULL_BUILD )
          if( serializationConfig != null && serializationConfig.Keywords.Contains( memberName ) )
            memberName = serializationConfig.PrefixForKeywords + memberName;
#endif
          if( !classDef.ContainsMember( memberName ) )
          {
            //ITypeWriter typeWriter = MessageWriter.getWriter( fields[ i ].FieldType, null, false );
            classDef.AddMemberInfo( memberName, fields[ i ] );
          }
        }

        objectClass = objectClass.BaseType;
      }

      IDictionary classStaticsCache = ThreadContext.currentWriterCache();

      if( classStaticsCache != null )
        classStaticsCache.Remove( className );

      classDef.ClassName = className;
      return classDef;
    }

    protected virtual void onWriteObject( Object obj, string className, IDictionary objectFields, IProtocolFormatter writer )
    {
      writer.GetObjectSerializer().WriteObject( className, objectFields, writer );
    }

    #endregion

    private bool cacheStaticField( string className, string fieldName )
    {
      IDictionary classStaticsCache = ThreadContext.currentWriterCache();

      if( classStaticsCache == null )
      {
        classStaticsCache = new Dictionary<Object, Object>();
        ThreadContext.setWriterCache( classStaticsCache );
      }

      IList cachedStatics = (IList) classStaticsCache[ className ];

      if( cachedStatics == null )
      {
        cachedStatics = new List<String>();
        classStaticsCache.Add( className, cachedStatics );
      }

      if( cachedStatics.Contains( fieldName ) )
        return false;
      else
        cachedStatics.Add( fieldName );

      return true;
    }

    protected string GetClientClass( string className )
    {
      string clientClass = null;
      string mappingClassName = ORBConstants.CLIENT_MAPPING + className;
      IDictionary props = ThreadContext.getProperties();

      if( props.Contains( mappingClassName ) )
        clientClass = (string) props[ mappingClassName ];

      if( clientClass == null )
        return Types.Types.getClientClassForServerType( className );
      else
        return clientClass;
    }
  }
}

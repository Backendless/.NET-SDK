using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security;

#if !UNIVERSALW8
using System.Security.Permissions;
#endif
using System.Reflection;
using System.Text;
using System.Diagnostics;

using Weborb.Config;
using Weborb.Util;
using Weborb.Types;
using Weborb.Service;
using Weborb.Util.Logging;

namespace Weborb.Reader
  {
  public delegate void ObjectUnderflow( Object sender, IDictionary props );

  public class AnonymousObject : ICacheableAdaptingType
    {
    public event ObjectUnderflow ReportObjectUnderflow;

    private IDictionary properties;
    private bool? containsNumericFields = null;
      // will be used to check object equality for different AnonymousObjects. 
      // it will be easier to chec guid then compare properties between two different AOs
    private Guid anonymousObjectGuid;
    //private Type hashtableType = typeof( Hashtable );
    //private Type dictionaryType = typeof( IDictionary );
    //private Type stringDictionaryType = typeof( StringDictionary );

#if FULL_BUILD
    private static bool accessCheckSuppressed = canAccessFieldsDirectly();

    private static bool canAccessFieldsDirectly()
      {
      ReflectionPermission permission = new ReflectionPermission( ReflectionPermissionFlag.AllFlags );
      return SecurityManager.IsGranted( permission );
      }
#endif

    internal AnonymousObject()
    {
        this.anonymousObjectGuid = Guid.NewGuid();

        ObjectUnderflow listener = (ObjectUnderflow) ThreadContext.getProperties()[ "UnderFlowReceiver" ];

        if( listener != null )
          ReportObjectUnderflow += listener;
    }

    public AnonymousObject( IDictionary properties ) : this()
      {
      this.properties = properties;
      }

    #region Public Properties

    public IDictionary Properties
      {
      get
        {
        return properties;
        }

      set
        {
        properties = value;
        }
      }

    #endregion

    #region IAdaptingType Members

    public Type getDefaultType()
      {
#if (FULL_BUILD)
      return typeof( Hashtable );
#else
            return typeof( Dictionary<Object, Object> );
#endif
      }

    public bool IsAdapting { get; set; }

    public object defaultAdapt()
      {
      return defaultAdapt( new ReferenceCache() );
      }

    public object defaultAdapt( ReferenceCache refCache )
      {      
      // server side support for javascript class aliasing
      if ( properties.Contains( ORBConstants.CLASS_NAME_FIELD ) )
        {
        string clientClassName = (string) ( (IAdaptingType) properties[ ORBConstants.CLASS_NAME_FIELD ] ).defaultAdapt();
        Type type = ORBConfig.GetInstance().getTypeMapper()._getServerTypeForClientClass( clientClassName );
        if ( type != null )
          return adapt( type );
        }

#if (FULL_BUILD)
      Hashtable hashtable = new Hashtable();
#else
      Dictionary<Object, Object> hashtable = new Dictionary<Object, Object>();
#endif

      if( refCache.HasObject( this ) )
        return refCache.GetObject( this );
      else
        refCache.AddObject( this, hashtable );

      ICollection keys = properties.Keys;

      foreach ( object key in keys )
        {
        object obj = properties[ key ];

        if ( obj != null )
          {
          if ( obj is IAdaptingType && refCache.HasObject( (IAdaptingType)obj ) )
            {
            obj = refCache.GetObject( (IAdaptingType)obj );
            }
          else if ( obj is ICacheableAdaptingType )
            {
              ICacheableAdaptingType cacheableType = (ICacheableAdaptingType) obj;
              object result = cacheableType.defaultAdapt( refCache );
              refCache.AddObject( (IAdaptingType) obj, result );
              obj = result;
            }
          else if ( obj is IAdaptingType )
            {
            obj = ( (IAdaptingType)obj ).defaultAdapt();
            }
          }

        hashtable[ key ] = obj;
        }

      return hashtable;
      }

    public object adapt( Type type )
      {
      return adapt( type, new ReferenceCache() );
      }

    public object adapt( Type type, ReferenceCache refCache )
      {
      if ( refCache.HasObject( this, type ) )
        return refCache.GetObject( this, type );

      object obj = ObjectFactories.CreateArgumentObject( type, this );

      if ( obj != null )
        {
        refCache.AddObject( this, type, obj );
        return obj;
        }

      if ( type.Equals( typeof( IAdaptingType ) ) )
        {
        refCache.AddObject( this, type, obj );
        return this;
        }

      if ( !type.IsArray )
        {
        obj = ObjectFactories.CreateServiceObject( type );
        refCache.AddObject( this, type, obj );
        }

      //refCache[ this ] = obj;

      if ( obj is IDictionary )
        {
        IDictionary dictionary = (IDictionary)obj;
        ICollection keys = properties.Keys;

        foreach ( object key in keys )
          {
          object valueObj = properties[ key ];

          if ( valueObj != null )
            {
            if ( type.IsGenericType )
              {
              Type[] args = type.GetGenericArguments();

              if ( valueObj is ICacheableAdaptingType )
                valueObj = ( (ICacheableAdaptingType)valueObj ).adapt( args[ 1 ] );
              else if ( valueObj is IAdaptingType )
                valueObj = ( (IAdaptingType)valueObj ).adapt( args[ 1 ] );
              }
            else
              {
              if ( valueObj is ICacheableAdaptingType )
                valueObj = ( (ICacheableAdaptingType)valueObj ).defaultAdapt( refCache );
              else if ( valueObj is IAdaptingType )
                valueObj = ( (IAdaptingType)valueObj ).defaultAdapt();
              }
            }

          object keyValue = key;

          if ( type.IsGenericType && key is IAdaptingType )
            {
            Type[] args = type.GetGenericArguments();
            keyValue = ( (IAdaptingType)key ).adapt( args[ 0 ] );
            }

          dictionary.Add( keyValue, valueObj );
          }
        }
      else if ( type.IsArray && canBeArray() )
        {
        Type elementType = type.GetElementType();
        Array newArray = Array.CreateInstance( elementType, properties.Count );
        refCache.AddObject( this, type, newArray );

        foreach ( String key in properties.Keys )
          {
          int index;
          int.TryParse( key, out index );
          object valueObj = properties[ key ];

          if ( valueObj is ICacheableAdaptingType )
            valueObj = ( (ICacheableAdaptingType)valueObj ).adapt( elementType, refCache );
          else if ( valueObj is IAdaptingType )
            valueObj = ( (IAdaptingType)valueObj ).adapt( elementType );

          newArray.SetValue( valueObj, index );
          }

        obj = newArray;
        }
      else if ( type.BaseType == null )
        {
        return defaultAdapt( refCache );
        }
#if( FULL_BUILD)
      else if ( obj is StringDictionary || typeof( StringDictionary ).IsAssignableFrom( type ) )
        {
        StringDictionary strDict = (StringDictionary)obj;
        ICollection keys = properties.Keys;

        foreach ( object key in keys )
          {
          string mappedValue;
          object valueObj = properties[ key ];

          if ( valueObj is ICacheableAdaptingType )
            mappedValue = (string)( (ICacheableAdaptingType)valueObj ).adapt( typeof( string ), refCache );
          else if ( valueObj is IAdaptingType )
            mappedValue = (string)( (IAdaptingType)valueObj ).adapt( typeof( string ) );
          else
            mappedValue = valueObj.ToString();

          strDict.Add( key.ToString(), mappedValue );
          }
        }
#endif
      else
        {
        //if( accessCheckSuppressed )
        setFieldsDirect( obj, properties, refCache );
        //else
        //    setFieldsAsBean( obj, properties );
        }

      return obj;
      }

    public IAdaptingType getCacheKey()
    {
      return this;
    }

    public bool canAdaptTo( Type formalArg )
      {
      return typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
          ( !formalArg.IsArray && !formalArg.IsValueType ) ||
          typeof( IDictionary ).IsAssignableFrom( formalArg ) ||
#if (FULL_BUILD)
 typeof( StringDictionary ).IsAssignableFrom( formalArg ) ||
#endif
 ObjectFactories.GetArgumentObjectFactory( formalArg.FullName ) != null ||
          haveMatchingData( formalArg );
      }

    #endregion

    private bool haveMatchingData( Type formalArgType )
      {
      if ( formalArgType.IsArray && canBeArray() )
        {
        return true;
        }
      else
        {

        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        MemberInfo[] members = formalArgType.GetMembers( flags );
        /*FieldInfo[] fields = formalArgType.GetFields( flags );
        PropertyInfo[] props = formalArgType.GetProperties( flags );

        foreach( FieldInfo field in fields )
            if( !properties.Contains( field.Name ) )
                return false;

        foreach( PropertyInfo property in props )
            if( !properties.Contains( property.Name ) )
                return false;
        */

        foreach ( MemberInfo member in members )
          {
          IMemberRenameAttribute[] renamers = (IMemberRenameAttribute[])member.GetCustomAttributes( typeof( IMemberRenameAttribute ), true );
          string memberName = member.Name;

          if ( renamers.Length > 0 )
            memberName = renamers[ 0 ].GetClientName( formalArgType, member );

          if ( !properties.Contains( memberName ) )
            {
#if( FULL_BUILD )
            SerializationConfigHandler serializationConfig = (SerializationConfigHandler)ORBConfig.GetInstance().GetConfig( "weborb/serialization" );

            if( serializationConfig != null && serializationConfig.Keywords.Contains( memberName ) )
              {
              memberName = serializationConfig.PrefixForKeywords + memberName;
              return properties.Contains( memberName );
              }
#endif
            return false;
            }
          }

        return true;
        }
      }

    private bool canBeArray()
      {
      if ( containsNumericFields != null )
        return (bool)containsNumericFields;

      foreach ( string key in properties.Keys )
        {
        int result;

        if ( !int.TryParse( key, out result ) )
          {
          containsNumericFields = false;
          return false;
          }
        }

      containsNumericFields = true;
      return true;
      }

    private void setFieldsDirect( object obj, IDictionary properties, ReferenceCache referenceCache )
      {
        if( ReportUnderflow )
        {
          Dictionary<object, object> propertiesCopy = new Dictionary<object, object>();

          foreach (object key in properties.Keys)
            propertiesCopy.Add( key, properties[ key ] );

          properties = propertiesCopy;
        }

      Type type = obj.GetType();

      

      bool logDebug = Log.isLogging( LoggingConstants.DEBUG );

      while ( !Object.ReferenceEquals( type, typeof( object ) ) )
        {
        FieldInfo[] fields = type.GetFields( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static );

        foreach ( FieldInfo field in fields )
          {
            if( (field.Attributes & FieldAttributes.Literal) == FieldAttributes.Literal )
              continue;

          IMemberRenameAttribute[] renamers = (IMemberRenameAttribute[])field.GetCustomAttributes( typeof( IMemberRenameAttribute ), true );
          string memberName = field.Name;

          if ( renamers.Length > 0 )
            memberName = renamers[ 0 ].GetClientName( type, field );

          object fieldValue = properties[ memberName ];

          if ( fieldValue == null )
            {
#if(FULL_BUILD)
            SerializationConfigHandler serializationConfig = (SerializationConfigHandler)ORBConfig.GetInstance().GetConfig( "weborb/serialization" );

            if( serializationConfig != null && serializationConfig.Keywords.Contains( memberName ) )
              {
              memberName = serializationConfig.PrefixForKeywords + memberName;
              fieldValue = properties[ memberName ];

              if ( fieldValue == null )
                continue;
              }
            else
              {
              continue;
              }
#endif
#if( SILVERLIGHT || PURE_CLIENT_LIB || WINDOWS_PHONE8 )
                      continue;
#endif
            }

          if ( fieldValue is IAdaptingType )
            {
            if ( logDebug )
              {
              Log.log( LoggingConstants.DEBUG, "initializing field " + field.Name );
              Log.log( LoggingConstants.DEBUG, "field type - " + field.FieldType.FullName );
              }

            object val = ObjectFactories.CreateArgumentObject( field.FieldType, (IAdaptingType)fieldValue );

            if ( val != null )
              {
              if ( logDebug )
                Log.log( LoggingConstants.DEBUG, "argument factory created object for the field " + val );

              //referenceCache[ fieldValue ] = val;

              referenceCache.AddObject( (IAdaptingType)fieldValue, field.FieldType, val );

              fieldValue = val;
              }
            else
              {
              if ( logDebug )
                Log.log( LoggingConstants.DEBUG, "argument factory is missing or returned no value. will use type adaptation" );

              if ( fieldValue is ICacheableAdaptingType )
                fieldValue = ( (ICacheableAdaptingType)fieldValue ).adapt( field.FieldType, referenceCache );
              else
                fieldValue = ( (IAdaptingType)fieldValue ).adapt( field.FieldType );
              }
            }

          if( ReportUnderflow )
            properties.Remove( memberName );

          try
            {
            field.SetValue( obj, fieldValue );
            }
          catch ( Exception e )
            {
            if ( Log.isLogging( LoggingConstants.INFO ) )
              Log.log( LoggingConstants.INFO, "field name - " + field.Name );
            throw e;
            }
          }

        PropertyInfo[] props = type.GetProperties( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static );

        foreach ( PropertyInfo prop in props )
          {
          if ( !prop.CanWrite )
            continue;

          IMemberRenameAttribute[] renamers = (IMemberRenameAttribute[])prop.GetCustomAttributes( typeof( IMemberRenameAttribute ), true );
          string memberName = prop.Name;

          if ( renamers.Length > 0 )
            memberName = renamers[ 0 ].GetClientName( type, prop );

          object propValue = properties[ memberName ];

          if ( propValue == null )
            {
#if(FULL_BUILD)
            SerializationConfigHandler serializationConfig = (SerializationConfigHandler)ORBConfig.GetInstance().GetConfig( "weborb/serialization" );

            if( serializationConfig != null && serializationConfig.Keywords.Contains( memberName ) )
              {
              memberName = serializationConfig.PrefixForKeywords + memberName;
              propValue = properties[ memberName ];

              if ( propValue == null )
                continue;
              }
            else
              {
              continue;
              }
#endif
#if(SILVERLIGHT || PURE_CLIENT_LIB || WINDOWS_PHONE8 )
              continue;
#endif
            }

          if ( propValue is IAdaptingType )
            {
            if ( logDebug )
              {
              Log.log( LoggingConstants.DEBUG, "initializing property " + prop.Name );
              Log.log( LoggingConstants.DEBUG, "property type - " + prop.PropertyType.FullName );
              }

            object val = ObjectFactories.CreateArgumentObject( prop.PropertyType, (IAdaptingType)propValue );

            if ( val != null )
              {
              if ( logDebug )
                Log.log( LoggingConstants.DEBUG, "argument factory created object for the field " + val );

              //referenceCache[ propValue ] = val;
              referenceCache.AddObject( (IAdaptingType)propValue, prop.PropertyType, val );

              propValue = val;
              }
            else
              {
              if ( logDebug )
                Log.log( LoggingConstants.DEBUG, "argument factory is missing or returned no value. will use type adaptation" );

              if ( propValue is ICacheableAdaptingType )
                propValue = ( (ICacheableAdaptingType)propValue ).adapt( prop.PropertyType, referenceCache );
              else
                propValue = ( (IAdaptingType)propValue ).adapt( prop.PropertyType );
              }
            }

          if( ReportUnderflow )
            properties.Remove( memberName );

          prop.SetValue( obj, propValue, null );
          }

        type = type.BaseType;
        }

      if( ReportUnderflow && properties.Count > 0 )
        ReportObjectUnderflow( obj, properties );
      }

    private bool ReportUnderflow
    {
      get
      {
        return ReportObjectUnderflow != null;
      }
    }

    private void setFieldsAsBean( object obj, IDictionary properties )
      {
      //TODO: implement
      }

    /*
    public override string ToString()
    {
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append( "Untyped object. Object properties:\n" );

        if( properties == null || properties.Count == 0 )
        {
            strBuilder.Append( "\tno object properties found" );
        }
        else
        {
            IDictionaryEnumerator en = properties.GetEnumerator();

            while( en.MoveNext() )
                strBuilder.Append( "PropKey - " ).Append( en.Key ).Append( "\tPropValue - " ).Append( en.Value ).Append( "\n" );
        }

        return strBuilder.ToString();
    }
     */

    public override bool Equals( object _obj )
      {
      return Equals( _obj, new Dictionary<DictionaryEntry, bool>() );
      }

    // in order to avoid cyclic comparasion we need to remember visited objects
    public bool Equals( object _obj, Dictionary<DictionaryEntry,bool> visitedPairs )
      {
      AnonymousObject obj = _obj as AnonymousObject;

    //  DictionaryEntry comparisionPair = new DictionaryEntry( this, _obj );
    //  visitedPairs[ comparisionPair ] = true;

      if ( obj == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj ) )
        return true;

      return this.anonymousObjectGuid.Equals(obj.anonymousObjectGuid);
/*
      if ( properties.Count != obj.properties.Count )
        return false;

      foreach ( DictionaryEntry dictionaryEntry in properties )
        if ( !obj.properties.Contains( dictionaryEntry.Key ) ||
             ( !visitedPairs.ContainsKey( new DictionaryEntry(obj.properties[ dictionaryEntry.Key ], dictionaryEntry.Value) ) 
               && !((IAdaptingType)obj.properties[ dictionaryEntry.Key ]).Equals( dictionaryEntry.Value, visitedPairs ) )
           )
          {
          visitedPairs.Remove( comparisionPair );
          return false;
          }

      visitedPairs.Remove( comparisionPair );
      return true;*/
      }

    public override int GetHashCode()
      {
        /*
      int res = 0;      
      foreach ( DictionaryEntry dictionaryEntry in properties )
        {        
        if ( !( dictionaryEntry.Value is AnonymousObject || dictionaryEntry.Value is NamedObject ||
                dictionaryEntry.Value is ArrayType || dictionaryEntry.Value is CacheableAdaptingTypeWrapper 
#if FULL_BUILD
                || dictionaryEntry.Value is RemoteReferenceObject
#endif
              ) )
          res ^= dictionaryEntry.Key.GetHashCode() ^ dictionaryEntry.Value.GetHashCode();
        else
          res ^= dictionaryEntry.Key.GetHashCode();
        }

      return res; */

          return anonymousObjectGuid.GetHashCode();
      }
    }
  }
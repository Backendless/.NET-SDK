using System;

#if (FULL_BUILD)
using System.Data;
using System.Web.SessionState;
using System.Data.SqlTypes;
#endif

using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Weborb;
using Weborb.Config;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Reader;
using Weborb.Message;
using Weborb.Types;
using Weborb.Exceptions;
using Weborb.Writer.Specialized;

namespace Weborb.Writer
  {
  public sealed class MessageWriter
    {
    private static Dictionary<Type, ITypeWriter> writers = new Dictionary<Type, ITypeWriter>();
    // this writers come from config and other external places, they override main ones
    private static Dictionary<Type, ITypeWriter> additionalWriters = new Dictionary<Type, ITypeWriter>();
    private static ArrayWriter arrayWriter = new ArrayWriter();
    private static ObjectWriter defaultWriter = new ObjectWriter();
    private static NullWriter nullWriter = new NullWriter();
    private static EnumerationWriter enumWriter = new EnumerationWriter();
    private static CollectionWriter enumerableWriter = new CollectionWriter();
    private static bool logSer;

    static MessageWriter()
      {
      logSer = Log.isLogging( LoggingConstants.SERIALIZATION );
      // numbers and primitive types
      NumberWriter numWriter = new NumberWriter();
      IntegerWriter intWriter = new IntegerWriter();

      writers.Add( typeof( byte ), intWriter );
      writers.Add( typeof( int ), intWriter );
      writers.Add( typeof( short ), intWriter );
      writers.Add( typeof( uint ), intWriter );
      writers.Add( typeof( ushort ), intWriter );

      writers.Add( typeof( float ), numWriter );
      writers.Add( typeof( double ), numWriter );
      writers.Add( typeof( long ), numWriter );
      writers.Add( typeof( ulong ), numWriter );
      writers.Add( typeof( IConvertible ), numWriter );
      writers.Add( typeof( NumberObject ), new NumberObjectWriter() );

      writers.Add( typeof( Boolean ), new BooleanWriter() );

      StringWriter stringWriter = new StringWriter( false );
      writers.Add( typeof( string ), stringWriter );
      writers.Add( typeof( char[] ), stringWriter );
      writers.Add( typeof( Char ), stringWriter );
      writers.Add( typeof( StringBuilder ), stringWriter );

      writers.Add( typeof( DateTime ), new DateWriter( false ) );
      writers.Add( typeof( TimeSpan ), new TimeSpanWriter() );

      writers.Add( typeof( Request ), new AMFMessageWriter() );
      writers.Add( typeof( Header ), new AMFHeaderWriter() );
      writers.Add( typeof( Body ), new AMFBodyWriter() );

      // collections
      writers.Add( typeof( AnonymousObject ), new PropertyBagWriter() );
      writers.Add( typeof( IDictionary ), new BoundPropertyBagWriter() );
      writers.Add( typeof( TypedObject ), new TypedObjectWriter() );
      writers.Add( typeof( IEnumerator ), new EnumeratorWriter() );
      writers.Add( typeof( IWebORBArrayCollection ), new ArrayCollectionWriter() );
      writers.Add( typeof( IWebORBArray ), new ArrayWriter() );   
#if (FULL_BUILD)
      writers.Add( typeof( IWebORBVector<> ), new V3VectorWriter<object>() );
      writers.Add( typeof( StringDictionary ), new StringDictionaryWriter() );
      writers.Add( typeof( TypedDictionary ), new TypedDictionaryWriter() );
      writers.Add( typeof( TypedDataSet ), new TypedDataSetWriter() );
      writers.Add( typeof( TypedDataTable ), new TypedDataTableWriter() );

      // remote references
      writers.Add( typeof( RemoteReferenceObject ), new RemoteReferenceWriter() );

      // sql types
      writers.Add( typeof( DataSet ), new DataSetWriter() );
      writers.Add( typeof( DataTable ), new DataTableWriter() );

      GenericSqlTypeHandler sqlTypesHandler = new GenericSqlTypeHandler();
      writers.Add( typeof( SqlDateTime ), sqlTypesHandler );
      writers.Add( typeof( SqlBinary ), sqlTypesHandler );
      writers.Add( typeof( SqlBoolean ), sqlTypesHandler );
      writers.Add( typeof( SqlByte ), sqlTypesHandler );
      writers.Add( typeof( SqlDecimal ), sqlTypesHandler );
      writers.Add( typeof( SqlDouble ), sqlTypesHandler );
      writers.Add( typeof( SqlInt16 ), sqlTypesHandler );
      writers.Add( typeof( SqlInt32 ), sqlTypesHandler );
      writers.Add( typeof( SqlInt64 ), sqlTypesHandler );
      writers.Add( typeof( SqlMoney ), sqlTypesHandler );
      writers.Add( typeof( SqlSingle ), sqlTypesHandler );
      writers.Add( typeof( SqlString ), sqlTypesHandler );
#endif
      writers.Add( typeof( ServiceException ), new ServiceExceptionWriter() );
      // various .net types
      writers.Add( typeof( Guid ), new GuidWriter() );

      // DOM document

      writers.Add( typeof( Type ), new RuntimeTypeWriter() );

#if (FULL_BUILD)
      XmlWriter xmlWriter = new XmlWriter();
      writers.Add( typeof( System.Xml.XmlDocument ), xmlWriter );
      writers.Add( typeof( System.Xml.XmlElement ), xmlWriter );
      writers.Add( typeof( System.Xml.XmlNode ), xmlWriter );

      DomainObjectWriter domainObjectWriter = new DomainObjectWriter();
      writers.Add( typeof( Weborb.Data.Management.DomainObject ), domainObjectWriter );
      try
        {
        Type hibernateProxyType = TypeLoader.LoadType( "NHibernate.Proxy.INHibernateProxy" );

        if ( hibernateProxyType != null )
          // new version of NHibernate was added
          //writers.Add( hibernateProxyType, new HibernateProxyWriter() );
          writers.Add( hibernateProxyType, new Hibernate2ProxyWriter() );

        }
      catch ( Exception )
        {
        }

#if NET_35 || NET_40
      try
      {
          Type entityObjectType = typeof( System.Data.Objects.DataClasses.EntityObject);

          if (entityObjectType != null)
              writers.Add(entityObjectType, new EntityObjectWriter());
      }
      catch (Exception e)
      {
          Log.log(LoggingConstants.EXCEPTION, e);
      }
#endif

#endif

      }

    public static ObjectWriter DefaultWriter
    {
      get
      {
        return defaultWriter;
      }

      set
      {
        defaultWriter = value;
      }
    }

    public static void AddAdditionalTypeWriter( Type mappedType, ITypeWriter writer )
      {
      //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
      //    throw new Exception( "cannot register custom serializers, this feature is available in WebORB Professional Edition" );

      additionalWriters[ mappedType ] = writer;
      }

    public static void CleanAdditionalWriters()
      {
      additionalWriters.Clear();
      }

    public static void SetEnumerationWriter( EnumerationWriter writer )
      {
      MessageWriter.enumWriter = writer;
      }

    public static void SetEnumerableWriter( CollectionWriter enumerableWriter )
      {
      MessageWriter.enumerableWriter = enumerableWriter;
      }

    /*
    public static Object writeObject( object obj )
    {
        ITypeWriter writer = getWriter( obj );
        return writer.write( obj );
    }
     */

    public static void writeObject( object obj, IProtocolFormatter formatter )
      {
      ITypeWriter typeWriter = getWriter( obj, formatter );
      typeWriter.write( obj, formatter );
      }

    internal static ITypeWriter getWriter( object obj, IProtocolFormatter formatter )
      {
      if ( obj == null || obj is DBNull )
        return nullWriter;

      Type objectType = obj.GetType();
      ITypeWriter writer = formatter.getCachedWriter( objectType );

      if ( writer == null )
        {
        // none of the interfaces matched a writer.
        // perform a lookup for the object class hierarchy
        writer = getWriter( objectType, formatter, true );

        if ( writer == null )
          {
          if ( typeof( IEnumerable ).IsInstanceOfType( obj ) )
            {
            writer = enumerableWriter;
            }
          else
            {
            if ( logSer )
              Log.log( LoggingConstants.SERIALIZATION, "cannot find a writer for the object, will use default writer - " + defaultWriter );

            writer = defaultWriter;
            }
          }

        formatter.addCachedWriter( objectType, writer );
        }

      ITypeWriter referenceWriter = writer.getReferenceWriter();

      if ( referenceWriter != null )
        {
        // if the return object implements IHTTPSessionObject
        // keep it in the http session. The same object will be retrieved
        // on the subsequent invocations
#if (FULL_BUILD)
        if ( obj is IHttpSessionObject )
          {
          IHttpSessionObject httpSessionObject = (IHttpSessionObject)obj;
          string id = httpSessionObject.getID();
          object objectInSession = httpSessionObject.getObject();

          if ( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "placing object into HTTP session. ID - " + id + " object " + obj );

          //TODO: check for acuracy of Add method here
          HttpSessionState session = (HttpSessionState)ThreadContext.currentSession();

          if ( session != null )
            session.Add( id, objectInSession );
          }
#endif

        formatter.setContextWriter( writer );
        writer = referenceWriter;
        }

      // if a writer is found, use it, otherwise use the default writer
      return writer;
      }

    internal static ITypeWriter getWriter( Type type, IProtocolFormatter formatter, bool checkInterfaces )
      {
      // class can be null only when we traverse class hierarchy.
      // when we get to the ver root of it, the superclass is null.
      // return null here, so the outer code can use a default writer
      if ( type == null )
        return null;

      // perform the lookup. Let protocol formatter do the check for any
      // protocol specific type bindings
      ITypeWriter writer = null;

      if ( formatter != null )
        writer = formatter.getWriter( type );

      // check against the additional lookup table (this will override main table)
      if ( writer == null )
        additionalWriters.TryGetValue( type, out writer );

      // check against the main lookup table
      if ( writer == null )
        writers.TryGetValue( type, out writer );

      // if we have a writer - use it
      if ( writer != null )
        return writer;

      if ( type.IsArray )
        {
        if ( logSer )
          Log.log( LoggingConstants.SERIALIZATION, "object is an array returning ArrayWriter" );

        return arrayWriter;
        }
      else if ( typeof( Enum ).IsAssignableFrom( type ) )
        {
        if ( logSer )
          Log.log( LoggingConstants.SERIALIZATION, "object is an enumeration type" );

        return enumWriter;
        }

      if ( checkInterfaces )
        writer = matchInterfaces( type.GetInterfaces() );

      if ( writer != null )
        {
        if ( logSer )
          Log.log( LoggingConstants.SERIALIZATION, "found a writer for an interface - " + writer );

        return writer;
        }

      // go to the super class as the last resort
      return getWriter( type.BaseType, formatter, true );
      }

    private static ITypeWriter matchInterfaces( Type[] interfaces )
      {
      if ( interfaces.Length == 0 )
        return null;

      List<Type> nextLevelInterfaces = new List<Type>();

      // loop through the interfaces and check if there's a writer
      for ( int i = 0; i < interfaces.Length; i++ )
        {
        if ( logSer )
          Log.log( LoggingConstants.SERIALIZATION, "looking up writer for " + interfaces[ i ] );

        ITypeWriter writer = getWriter( interfaces[ i ], null, false );

        // found a writer mapped to the interface - use it
        if ( writer != null )
          {
          if ( logSer )
            Log.log( LoggingConstants.SERIALIZATION, "returning " + writer + " for interface " + interfaces[ i ] );

          return writer;
          }

        //writer = matchInterfaces( interfaces[ i ].GetInterfaces() );

        //if( writer != null )
        //	return writer;

        nextLevelInterfaces.AddRange( interfaces[ i ].GetInterfaces() );
        }

      if ( nextLevelInterfaces.Count > 0 )
        {
        ITypeWriter intfWriter = matchInterfaces( nextLevelInterfaces.ToArray() );

        if ( intfWriter != null )
          return intfWriter;
        }

      if ( logSer )
        Log.log( LoggingConstants.SERIALIZATION, "none of the interfaces matched a writer" );

      return null;
      }
    }
  }

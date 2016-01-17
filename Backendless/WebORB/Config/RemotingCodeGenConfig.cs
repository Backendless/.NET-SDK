using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Weborb.Management.CodeGen;
using Weborb.Management.ServiceBrowser;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
  public class RemotingCodeGenConfig : ORBConfigHandler
  {
    private const String CODEGEN_FORMATS = "formats/codegenFormat";
    private const String CODEGEN_MESSAGING_FORMATS = "messagingDestinations/formats/codegenFormat";
    private const String CODEGEN_MESSAGING_APPLICATION_FORMATS = "messagingApplications/formats/codegenFormat";
    private const String CODEGEN_MESSAGING_APPLICATION_FORMATS_FEATURES = "features/codegenFeature";
    private const String CODEGEN_MESSAGING_APPLICATION_FEATURES = "messagingApplications/features/codegenFeature";
    private const String ID = "id";
    private const String DESCRIPTION = "description";
    private const String NAME = "name";
    private const String XSLT = "xslt";
    private const String EXTENTION = "extension";
    private const String KEEP_FILE = "keepFile";
    private const String EXCLUDED_SERVICE_BROWSERS = "excludedServiceBrowsers";

    private static Dictionary<String, Type> knownTypes = new Dictionary<string, Type>();
    private Dictionary<Type, String> typeMap = new Dictionary<Type, string>();
    private Dictionary<Type, String> javaTypeMap = new Dictionary<Type, string>();
    private Dictionary<Type, String> nativeTypeMap = new Dictionary<Type, string>();
    private bool inspectAllClassesForFlashBuilder = true;
    private bool enableWebORBAuthentication = false;
    private IServiceScannerListener serviceScannerListener;
    private List<IServiceSerializerEventListener> serializerListeners = new List<IServiceSerializerEventListener>();

    static RemotingCodeGenConfig()
    {
      knownTypes.Add( "int", typeof( int ) );
      knownTypes.Add( "uint", typeof( uint ) );
      knownTypes.Add( "System.Int32?", Type.GetType( "System.Int32&" ) );
      knownTypes.Add( "Int32?", Type.GetType( "System.Int32&" ) );
      knownTypes.Add( "DateTime", typeof( DateTime ) );
      knownTypes.Add( "System.DateTime", typeof( DateTime ) );
      knownTypes.Add( "DateTime?", typeof( DateTime? ) );
      knownTypes.Add( "System.DateTime?", typeof( DateTime? ) );
      knownTypes.Add( "int?", typeof( Nullable<int> ) );
      knownTypes.Add( "float?", typeof( Nullable<float> ) );
      knownTypes.Add( "double?", typeof( Nullable<double> ) );
      knownTypes.Add( "decimal?", typeof( Nullable<decimal> ) );
      knownTypes.Add( "short?", typeof( Nullable<short> ) );
      knownTypes.Add( "ushort?", typeof( Nullable<ushort> ) );
      knownTypes.Add( "long?", typeof( Nullable<long> ) );
      knownTypes.Add( "ulong?", typeof( Nullable<ulong> ) );
      knownTypes.Add( "byte?", typeof( Nullable<byte> ) );

      knownTypes.Add( "Nullable<int>", typeof( Nullable<int> ) );
      knownTypes.Add( "Nullable<float>", typeof( Nullable<float> ) );
      knownTypes.Add( "Nullable<double>", typeof( Nullable<double> ) );
      knownTypes.Add( "Nullable<decimal>", typeof( Nullable<decimal> ) );
      knownTypes.Add( "Nullable<short>", typeof( Nullable<short> ) );
      knownTypes.Add( "Nullable<long>", typeof( Nullable<long> ) );
      knownTypes.Add( "Nullable<byte>", typeof( Nullable<byte> ) );

      knownTypes.Add( "String", typeof( String ) );
      knownTypes.Add( "System.String", typeof( String ) );
      knownTypes.Add( "StringBuilder", typeof( StringBuilder ) );
      knownTypes.Add( "System.StringBuilder", typeof( StringBuilder ) );
      knownTypes.Add( "void", typeof( void ) );
      knownTypes.Add( "Void", typeof( void ) );
      knownTypes.Add( "System.Void", typeof( void ) );
      knownTypes.Add( "float", typeof( float ) );
      knownTypes.Add( "double", typeof( double ) );
      knownTypes.Add( "decimal", typeof( decimal ) );
      knownTypes.Add( "short", typeof( short ) );
      knownTypes.Add( "ushort", typeof( ushort ) );
      knownTypes.Add( "long", typeof( long ) );
      knownTypes.Add( "ulong", typeof( ulong ) );
      knownTypes.Add( "byte", typeof( byte ) );
      knownTypes.Add( "bool", typeof( bool ) );
      knownTypes.Add( "int[]", typeof( System.Int32[] ) );
      knownTypes.Add( "float[]", typeof( float[] ) );
      knownTypes.Add( "double[]", typeof( double[] ) );
      knownTypes.Add( "decimal[]", typeof( decimal[] ) );
      knownTypes.Add( "short[]", typeof( short[] ) );
      knownTypes.Add( "long[]", typeof( long[] ) );
      knownTypes.Add( "byte[]", typeof( byte[] ) );
      knownTypes.Add( "bool[]", typeof( bool[] ) );
      knownTypes.Add( "System.Data.DataTable", typeof( System.Data.DataTable ) );
      knownTypes.Add( "System.Data.DataSet", typeof( System.Data.DataSet ) );

    }

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      XmlAttribute keepFileAttr = section.Attributes[ KEEP_FILE ];

      if( keepFileAttr != null )
        try
        {
          CodegenFormat.keepFile = bool.Parse( keepFileAttr.Value );
        }
        catch( Exception e )
        {
          if ( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR, "Cann't parse keepFile attribute of the codeGeneration section from config file" );
        }

      XmlNodeList codegenFormats = section.SelectNodes( CODEGEN_FORMATS );
      foreach( XmlNode node in codegenFormats )
        setupCodegenFormat( node, getORBConfig().CodegenFormats );

      XmlNodeList codegenMessagingFormats = section.SelectNodes( CODEGEN_MESSAGING_FORMATS );
      foreach ( XmlNode node in codegenMessagingFormats )
        setupCodegenFormat( node, getORBConfig().CodegenMessagingFormats );

      XmlNodeList codegenApplicationFeaturesMessagingFormats = section.SelectNodes( CODEGEN_MESSAGING_APPLICATION_FEATURES );
      foreach ( XmlNode node in codegenApplicationFeaturesMessagingFormats )
        SetupMessagingFeatures( node, getORBConfig().CodegenMessagingApplicationFeatures );

      XmlNodeList codegenApplicationMessagingFormats = section.SelectNodes( CODEGEN_MESSAGING_APPLICATION_FORMATS );
      foreach ( XmlNode node in codegenApplicationMessagingFormats )
        SetupMessagingApplicationCodegenFormat( node, getORBConfig().CodegenMessagingApplicationFormats );

      XmlNodeList dotNetToAStypesList = ((XmlElement) section).SelectNodes( "types/dotnet-to-as/type" );

      foreach( XmlNode node in dotNetToAStypesList )
        typeMap[ getType( node.Attributes[ "dotNet" ].Value.Trim() ) ] = node.Attributes[ "as" ].Value.Trim();

      XmlNodeList dotNetToJavatypesList = ( (XmlElement)section ).SelectNodes( "types/dotnet-to-java/type" );

      foreach ( XmlNode node in dotNetToJavatypesList )
      {
        try
        {
          javaTypeMap[getType(node.Attributes["dotNet"].Value.Trim())] = node.Attributes["java"].Value.Trim();
        }
        catch(Exception e)
        {
          if(Log.isLogging(LoggingConstants.WEBORB_EXCEPTION))
            Log.log(LoggingConstants.EXCEPTION, e);
        }
      }

      XmlNodeList nativeDotNetToAStypesList = ((XmlElement) section).SelectNodes( "types/dotnet-to-sl/type" );

      foreach( XmlNode node in nativeDotNetToAStypesList )
        nativeTypeMap[ getType( node.Attributes[ "dotNet" ].Value.Trim() ) ] = node.Attributes[ "sl" ].Value.Trim();

      String inspectAllClassesStr = ((XmlElement) section).SelectSingleNode( "flashBuilder/inspectAllClasses" ).InnerText;
      inspectAllClassesForFlashBuilder = StringUtil.IsTrueConfigValue( inspectAllClassesStr );

      String enableAuthStr = ((XmlElement) section).SelectSingleNode( "flashBuilder/enableWebORBAuthenticaction" ).InnerText;
      enableWebORBAuthentication = StringUtil.IsTrueConfigValue( enableAuthStr );

      XmlNodeList nodes = ((XmlElement) section).SelectNodes( "extensibility/serviceSerializerListener" );

      if( nodes != null && nodes.Count > 0 )
        foreach( XmlNode node in nodes )
        {
          String className = node.InnerText;

          if( className != null && className.Trim().Length > 0 )
            serializerListeners.Add( (IServiceSerializerEventListener) ObjectFactories.CreateServiceObject( className ) );
        }

      XmlNode serviceScannerListenerNode = ( (XmlElement) section ).SelectSingleNode( "extensibility/serviceScannerListener" );

      if( serviceScannerListenerNode != null && serviceScannerListenerNode.InnerText.Trim().Length > 0 )
        serviceScannerListener = (IServiceScannerListener) ObjectFactories.CreateServiceObject( serviceScannerListenerNode.InnerText );

      return this;
    }

    private void SetupMessagingApplicationCodegenFormat( XmlNode formatNode, Dictionary<int, MessagingCodegenFormat> messagingCodegenFormats )
    {
      CodegenFormat format = setupCodegenFormat( formatNode, new Dictionary<int, CodegenFormat>() );
      if ( format == null )
        return;

      MessagingCodegenFormat messagingCodegenFormat = new MessagingCodegenFormat();
      messagingCodegenFormat.id = format.id;
      messagingCodegenFormat.name = format.name;
      messagingCodegenFormat.xslt = format.xslt;
      messagingCodegenFormat.excludedServiceBrowsers = format.excludedServiceBrowsers;

      XmlNodeList nodeList = formatNode.SelectNodes( CODEGEN_MESSAGING_APPLICATION_FORMATS_FEATURES );
      foreach ( XmlNode featureNode in nodeList )
      {
        try
        {
          XmlAttribute idAttr = featureNode.Attributes["id"];
          int featureId = Int32.Parse( idAttr.Value );
          if ( getORBConfig().CodegenMessagingApplicationFeatures.ContainsKey( featureId ) )
          {
            MessagingCodegenFeature feature = getORBConfig().CodegenMessagingApplicationFeatures[featureId];
            messagingCodegenFormat.messagingCodegenFeatures.Add(feature);
          }
          else
          {
            throw new Exception("Format has no such feature");
          }

        }
        catch(Exception e)
        {
          if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
            Log.log( LoggingConstants.EXCEPTION, "Unable to create feature for codegen format with id = " + format.id, e );
        }
      }

      messagingCodegenFormats[messagingCodegenFormat.id] = messagingCodegenFormat;
    }

    private void SetupMessagingFeatures( XmlNode formatNode, Dictionary<int, MessagingCodegenFeature> messagingCodegenFeatures )
    {
      MessagingCodegenFeature messagingCodegenFeature = new MessagingCodegenFeature();
      
      XmlAttribute idAttr = formatNode.Attributes[ID];
      XmlAttribute nameAttr = formatNode.Attributes[NAME];
      XmlAttribute descriptionAttr = formatNode.Attributes[DESCRIPTION];

      try
      {
        if ( idAttr != null )
          messagingCodegenFeature.id = Int32.Parse( idAttr.Value );
        else
        {
          if ( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR, "Codegen feature has no id attribute which is required" );
          return;
        }

        messagingCodegenFeature.name = nameAttr.Value;
        messagingCodegenFeature.shortDescription = descriptionAttr.Value;
        messagingCodegenFeatures[messagingCodegenFeature.id] = messagingCodegenFeature;
      }
      catch ( Exception e )
      {
        if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, "Unable to create codegen format feature with id = " + idAttr.Value, e );
        return;
      }
    }

    private CodegenFormat setupCodegenFormat( XmlNode formatNode, Dictionary<int, CodegenFormat> codegenFormats )
    {
      CodegenFormat format = new CodegenFormat();
      XmlAttribute idAttr = formatNode.Attributes[ ID ];
      XmlAttribute nameAttr = formatNode.Attributes[ NAME ];
      XmlAttribute xsltAttr = formatNode.Attributes[ XSLT ];
      XmlAttribute extensionAttr = formatNode.Attributes[ EXTENTION ];
      XmlAttribute excludedBrowsersAttr = formatNode.Attributes[ EXCLUDED_SERVICE_BROWSERS ];

      try
      {
        if( idAttr != null )
          format.id = Int32.Parse( idAttr.Value );
        else
        {
          if ( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR, "Codegen has no id attribute which is required" );
          return null;
        }

        format.name = nameAttr.Value;
        format.xslt = xsltAttr.Value;

        if( excludedBrowsersAttr != null && excludedBrowsersAttr.Value != null )
          format.excludedServiceBrowsers = excludedBrowsersAttr.Value;

        if( extensionAttr != null )
          try
          {
            format.generatorExtension = (ICodegeneratorExtension) ObjectFactories.CreateServiceObject( extensionAttr.Value );
          }
          catch( Exception e )
          {
            if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
              Log.log( LoggingConstants.EXCEPTION, "Cann't instantinate extension for codegeneration format " + format.name, e );
          }

        codegenFormats[format.id] = format;

        return format;
      }
      catch( Exception e )
      {
        if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, "Unable to create codegen format with id = " + idAttr.Value, e );
        return null;
      }
    }

    public bool InspectAllClassesForFlashBuilder
    {
      get { return inspectAllClassesForFlashBuilder; }
      set { inspectAllClassesForFlashBuilder = value; }
    }

    public bool EnableWebORBAuthenticationForFlashBuilder
    {
      get { return enableWebORBAuthentication; }
      set { enableWebORBAuthentication = value; }
    }

    public IServiceScannerListener ServiceScannerListener
    {
      get
      {
        return serviceScannerListener;
      }
    }

    public List<IServiceSerializerEventListener> ServiceSerializerListeners
    {
      get
      {
        return serializerListeners;
      }
    }

    public Dictionary<Type, String> TypeMap
    {
      get
      {
        return typeMap;
      }
    }

    public Dictionary<Type, String> JavaTypeMap
    {
      get
      {
        return javaTypeMap;
      }
    }

    public Dictionary<Type, String> NativeTypeMap
    {
      get
      {
        return nativeTypeMap;
      }
    }

    public Type getType( String type )
    {
      if( knownTypes.ContainsKey( type ) )
        return knownTypes[ type ];

      Type typeObj = TypeLoader.LoadType( type );

      if( typeObj == null )
        throw new Exception( String.Format( "unable to find type for {0}", type ) );

      return typeObj;
    }
  }
}

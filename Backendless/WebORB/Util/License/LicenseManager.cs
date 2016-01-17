using System;
using System.Net.NetworkInformation;
using System.Threading;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Net;
using System.Web;
using Weborb.Client;
using Weborb.Security;
using Weborb.Cloud;
using Weborb.Config;
using Weborb.Util.Logging;
using Weborb.Messaging.Net.UDP;
using Weborb.Messaging.Server.Scheduling;
using Weborb.Util.IO;
using Weborb.Exceptions;

namespace Weborb.Util.License
{
  internal enum RegistrationStatus { Unknown, Succeeded, Failed };

  public class LicenseManager
  {
    private const String GATEWAY_URL = "http://www.themidnightcoders.com/downloadmanager/weborb.aspx";

    public const int DEVMODE = 0x00;
    public const int COMMUNITY = 0x01;
    public const int ENTERPRISE = 0x02;
    public const int WEBORB_PRODUCT_ID = 3;
    private const string WEBGARDEN_FILE_NAME = "JHGfdeJ43";

    private static Dictionary<int, LicenseManager> instances = new Dictionary<int, LicenseManager>();
    private LicenseKeyInfo licenseInfo = null;
    private static String serverDomain;
    private static FileStream tempFileStream;

    private RegistrationStatus CERegistrationStatus = RegistrationStatus.Unknown;
    private Timer retryTimer;
    private int ceRetryCount = 0;
    private int CE_MAX_RETRY_COUNT = 3;
    private string CERegistrationInfo;
    private const int BROADCAST_INTERVAL = 60 * 1000;
    private static int[] UDP_PORTS;
    private static ArrayList clientIPAddresses = new ArrayList( 5 );
    private MessageCatcher catcher;
    private String broadcastJobID;
    private InstanceID thisInstance;
    private Dictionary<String, InstanceID> nodes;
    private String runningInstanceAddress;

    private int productID; // ID of the product this license manager instance is for
    private int currentVersion;

    private bool _isBlocked;
    private string _blockedErrorMessage;

    private LicenseManager( int productID )
    {
      initialize( productID );
    }

    private static bool? IsWebGardenMode { get; set; }

    internal void initialize( int productID )
    {
      this.productID = productID;

      if( DeploymentMode.IsCloud() )
        return;

        UDP_PORTS = new int[ 10 ];

        for( int i = 0; i < UDP_PORTS.Length; i++ )
          UDP_PORTS[ i ] = 11011 + i;

        ProductInfo product = LicensingService.getProduct( productID );

        if( product == null )
          throw new LicenseException( "Unknown product ID - " + productID );

        this.currentVersion = new AssemblyName( product.assemblyFullName ).Version.Major;
        String licensePath = Path.Combine( Paths.GetLicensesPath(), product.licenseFileName );
        LicenseFile licenseFile = LicenseFile.Load( licensePath );

        if( licenseFile.IsValid )
          licenseInfo = licenseFile.licenseKeyInfo;
        else
          licenseInfo = new LicenseKeyInfo( productID );

        if( licenseInfo.licenseType != DEVMODE && !licenseInfo.oem && licenseInfo.limitOfInstances > 0 )
        {
          nodes = new Dictionary<string, InstanceID>();
          catcher = new MessageCatcher( UDP_PORTS, true );
          catcher.Catch += new MessageCatcherDelegate( broadcastMessageReceived );
          Broadcaster broadcaster = new Broadcaster();
          Scheduler scheduler = Scheduler.getInstance();
          thisInstance = new InstanceID();
          thisInstance.startTime = DateTime.Now;
          thisInstance.licenseKey = licenseInfo.licenseKey;
          thisInstance.maxInstances = licenseInfo.limitOfInstances;

          byte[] dataToSend = Serializer.ToBytes( thisInstance, Serializer.AMF3 );
          BroadcastScheduledJob broadcastJob = new BroadcastScheduledJob( UDP_PORTS, dataToSend );
          broadcastJobID = scheduler.addScheduledJob( BROADCAST_INTERVAL, broadcastJob );
        }

        // register community edition
        if( getLicenseKeyInfo().licenseType == LicenseManager.COMMUNITY )
          RegisterOnServer();

        if( licenseInfo.licenseType == COMMUNITY && IsWebGardenMode == null )
        {
          //if file in temporary directory is busy, so another w3wp weborb process use it and it is Web Garden mode
          string tempFilePath = AppDomain.CurrentDomain.DynamicDirectory + Path.DirectorySeparatorChar +
                                WEBGARDEN_FILE_NAME;
          FileInfo tempFile = new FileInfo( tempFilePath );
          try
          {
            tempFileStream = tempFile.Open( FileMode.Append, FileAccess.Write, FileShare.Read );
            IsWebGardenMode = false;
          }
          catch( IOException )
          {
            IsWebGardenMode = true;
          }
        }
    }

    public bool IsValid( bool includeIPCheck )
    {
      if ( _isBlocked )
        return false;

      if( DeploymentMode.IsCloud() )
      {
        return CloudBillingClient.GetInstance().IsValid();
      }
      else if( licenseInfo.licenseType == COMMUNITY )
      {
        if( IsWebGardenMode == true )
          throw new LicenseException(
            "The Community Edition does not support the Web Garden mode. Please contact Midnight Coders Sales at sales@themidnightcoders.com" );

        if( ThreadContext.currentRequest() != null && ThreadContext.currentRequest().Url.HostNameType == UriHostNameType.Dns )
        {
          if( serverDomain == null )
           serverDomain = GetDomain( ThreadContext.currentRequest().Url );
          else if( serverDomain != GetDomain( ThreadContext.currentRequest().Url ) )
            throw new LicenseException( "Community Edition does not work in multi-domain environment. Please contact Midnight Coders Sales" );
        }
      }

      bool isValid = CERegistrationStatus != RegistrationStatus.Failed && !IsExpired() &&
                     DetectedInstance() == null &&
                     ( licenseInfo.majorVersion == currentVersion || licenseInfo.licenseType == LicenseManager.COMMUNITY ) &&
                     licenseInfo.product == productID;

      if( isValid && includeIPCheck )
        return IsAllowedIP();
      else
        return isValid;
    }

    private static String GetDomain( Uri url )
    {
      int domainStart = url.Host.IndexOf( "www." );
      return domainStart < 0 ? url.Host : url.Host.Substring( domainStart + 4 );
    }

    public LicenseKeyInfo getLicenseKeyInfo()
    {
      return new LicenseKeyInfo( licenseInfo );
    }

    void broadcastMessageReceived( MessageCatcher catcher, IPEndPoint sender, byte[] message )
    {
      InstanceID nodeInfo = (InstanceID) Serializer.FromBytes( message, Serializer.AMF3, false );
      DateTime now = DateTime.Now;

      lock( this )
      {
        if( nodeInfo.sameKey( this.licenseInfo.licenseKey ) )
        {
          if( !nodes.ContainsKey( nodeInfo.ID ) )
            nodes.Add( nodeInfo.ID, nodeInfo );

          nodes[ nodeInfo.ID ].lastBroadcastTime = now;
        }

        List<String> nodesToRemove = new List<String>();

        foreach( KeyValuePair<String, InstanceID> pair in nodes )
          if( now.Ticks - pair.Value.lastBroadcastTime.Ticks > 2 * BROADCAST_INTERVAL * TimeSpan.TicksPerMillisecond )
            nodesToRemove.Add( pair.Key );

        foreach( String nodeID in nodesToRemove )
          nodes.Remove( nodeID );

        bool outOfLimitInstance = true;

        if( nodes.Count > this.licenseInfo.limitOfInstances )
        {
          foreach( InstanceID node in nodes.Values )
            if( thisInstance.startTime <= node.startTime )
            {
              outOfLimitInstance = false;
              break;
            }
        }
        else
        {
          outOfLimitInstance = false;
        }

        if( outOfLimitInstance )
        {
          if( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR, "License conflict detected. Another WebORB instance is running at " + sender.Address.ToString() + ". This instance will stop responding. Consider upgrading your license key" );

          runningInstanceAddress = sender.Address.ToString();
        }
        else
        {
          runningInstanceAddress = null;
        }
      }
    }

    private bool IsAllowedIP()
    {
      if( licenseInfo.licenseType != DEVMODE )
        return true;

      HttpRequest request = ThreadContext.currentRequest();

      if( request == null )
        return true;

      String userAddr = null;

      if( new ArrayList( request.ServerVariables.Keys ).Contains( "HTTP_X_FORWARDED_FOR" ) )
        userAddr = request.ServerVariables[ "HTTP_X_FORWARDED_FOR" ];

      if( userAddr == null || userAddr == "" || userAddr.ToLower().Trim().Equals( "unknown" ) )
        userAddr = request.UserHostAddress;

      if( clientIPAddresses.Contains( userAddr ) )
        return true;

      if( clientIPAddresses.Count >= 5 )
      {
        return false;
      }
      else
      {
        clientIPAddresses.Add( userAddr );
        return true;
      }
    }

    public string GetLicensingError()
    {
      if( DeploymentMode.IsCloud() )
        return CloudBillingClient.GetInstance().GetError();
      else
      {
        if ( IsExpired() )
          return "License key has expired. Contact Midnight Coders to purchase a license";

        else if ( DetectedInstance() != null )
          return "Another instance of WebORB is running on the network. This installation supports only " + licenseInfo.limitOfInstances + ( licenseInfo.limitOfInstances == 1 ? "server" : "servers" ) + " on the network";
        else if ( licenseInfo.majorVersion != currentVersion && licenseInfo.licenseType != LicenseManager.COMMUNITY )
          return "License key must be upgraded to enable new version of the product. Contact Midnight Coders sales. License key version - " + licenseInfo.majorVersion + " , product version - " + currentVersion;
        else if ( licenseInfo.product != productID )
          return "Configured license key is not compatible with the product. License key product ID - " + licenseInfo.product + ", product ID - " + productID;
        else if ( !IsAllowedIP() )
        {
          StringBuilder addresses = new StringBuilder();

          for ( int i = 0; i < clientIPAddresses.Count; i++ )
          {
            addresses.Append( clientIPAddresses[i] );

            if ( i != clientIPAddresses.Count - 1 )
              addresses.Append( ", " );
          }

          return "In development mode requests are only allowed from 5 client IP addresses. WebORB is locked to " + addresses.ToString() + ". Restart WebORB to clear the cache or contact Midnight Coders to get your license key.";
        }
        else if ( CERegistrationStatus == RegistrationStatus.Failed )
          return CERegistrationInfo;
        else if ( _isBlocked )
          return _blockedErrorMessage;
        else
          throw new Exception( "should never get here" );
      }
    }

    public static LicenseManager GetInstance( int productID )
    {
      lock( instances )
      {
        if( !instances.ContainsKey( productID ) )
        {
          LicenseManager lm = new LicenseManager( productID );
          instances.Add( productID, lm );
        }
      }

      return instances[ productID ];
    }

    internal void reset()
    {
      CERegistrationStatus = RegistrationStatus.Unknown;
      ceRetryCount = 0;

      if( catcher != null )
        catcher.Disconnect();

      if( broadcastJobID != null )
        Scheduler.getInstance().removeScheduledJob( broadcastJobID );

      if( instances != null )
        instances.Remove( productID );
    }

    public string DetectedInstance()
    {
      return runningInstanceAddress;
    }

    public bool IsExpired()
    {
      if( DeploymentMode.IsCloud() )
        return false;
      else if( licenseInfo.expireDate.Equals( DateTime.MaxValue ) )
        return false;
      else
        return licenseInfo.expireDate < DateTime.Now;
    }

    public DateTime? getExpDate()
    {
      return licenseInfo.expireDate.Equals( DateTime.MaxValue ) ? (DateTime?) null : licenseInfo.expireDate;
    }

    public bool IsEnterpriseLicense()
    {
      if( DeploymentMode.IsCloud() )
        return true;
      else
        return licenseInfo.licenseKey != null;
    }

    public bool isOEM()
    {
      if( DeploymentMode.IsCloud() )
        return false;
      else
        return licenseInfo.oem;
    }

    public void RegisterOnServer()
    {
      List<string> addresses = new List<string>();
      foreach( NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces() )
        if( nic.OperationalStatus == OperationalStatus.Up )
        {
          string address = nic.GetPhysicalAddress().ToString();
          if( !string.IsNullOrEmpty( address ) )
            addresses.Add( address );
        }

      if( addresses.Count == 0 )
        return;

      WeborbClient weborbClient = new WeborbClient( GATEWAY_URL );
      weborbClient.Invoke<String>( "CELicenseGuardDLL.LicenseKeeper",
        "isLicenseValid",
        new object[] { this.licenseInfo.licenseKey, addresses },
        new Responder<string>(
          delegate( String value )
          {
            CERegistrationInfo = value;
            CERegistrationStatus = !String.IsNullOrEmpty( CERegistrationInfo ) ? RegistrationStatus.Failed :
                                    RegistrationStatus.Succeeded;

          },
          delegate( Fault fault )
          {
            ceRetryCount++;

            // stop retrying
            if( ceRetryCount >= CE_MAX_RETRY_COUNT )
            {
              CERegistrationInfo = fault.Message;
              CERegistrationStatus = RegistrationStatus.Failed;
              retryTimer.Dispose();
              return;
            }

            // start retrying
            if( retryTimer == null )
              retryTimer = new Timer( RegisterOnServerCallback, null, TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 5 ) );
          } ) );
    }

    public void RegisterOnServerCallback( object obj )
    {
      RegisterOnServer();
    }

    internal void Block(string message)
    {
      _isBlocked = true;
      _blockedErrorMessage = message;
    }

    public class InstanceID
    {
      public String ID = Guid.NewGuid().ToString();
      public DateTime startTime;
      public String licenseKey;
      public int maxInstances;
      [NonSerialized()]
      public DateTime lastBroadcastTime;

      internal bool sameKey( String licenceKey )
      {
        if( this.licenseKey == null && licenseKey == null )
          return true;

        return this.licenseKey.Equals( licenseKey );
      }
    }
  }
}
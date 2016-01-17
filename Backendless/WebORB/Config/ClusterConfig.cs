using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Weborb.Cluster;
using Weborb.Config.Configurators;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
  public class ClusterConfig : ORBConfigHandler
  {
    public const string SECTION_NAME = "weborb/cluster";
    public const string DEFAULT_CLUSTER_NAME = "weborb-cluster";
    public const int DEFAULT_PORT = 80;
    public const int DEFAULT_RTMP_PORT = 2037;
    private const string DEFAULT_URI = "weborb5";

    private String _clusterName;
    private int _port;
    private int _rtmpPort;
    private List<String> _potentialNodes = new List<String>();
    private List<String> _nodeDefinitions = new List<String>();
    private bool _isClusterAllowed;
    private int _distributedLockTimeout = 100;
    private string _uri = DEFAULT_URI;
    private int _wildcardsProcessed;

    private List<List<int>> _allVariations = new List<List<int>>();

    public static int RtmpPort
    {
      get { return GetConfig()._rtmpPort; }
    }

    public static bool IsClusterAllowed
    {
      get { return GetConfig() != null && GetConfig()._isClusterAllowed; }
    }

    public static int DistributedLockTimeout
    {
      get { return GetConfig()._distributedLockTimeout; }
    }

    public static string Uri
    {
      get { return GetConfig() == null ? null : GetConfig()._uri; }
    }

    public static List<string> NodeDefinitions
    {
      get { return GetConfig() == null ? null : GetConfig()._nodeDefinitions; }
    }

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      _isClusterAllowed = true;
      _clusterName = section.Attributes[ "name" ] != null ? section.Attributes[ "name" ].Value : DEFAULT_CLUSTER_NAME;

      XmlNode portNode = section.SelectSingleNode("port");
      _port = portNode != null ? int.Parse( portNode.InnerText.Trim() ) : DEFAULT_PORT;
      XmlNode rtmpPortNode = section.SelectSingleNode("rtmp-port");
      _rtmpPort = rtmpPortNode != null ? int.Parse( rtmpPortNode.InnerText.Trim() ) : DEFAULT_RTMP_PORT;
      XmlNode uriNode = section.SelectSingleNode("uri");
      _uri = uriNode != null ? uriNode.InnerText.Trim() : DEFAULT_URI;
      XmlNode enabledNode = section.SelectSingleNode( "enabled" );
      _isClusterAllowed = enabledNode != null ? bool.Parse(enabledNode.InnerText.Trim()) : true;

      XmlNodeList addressNodes = section.SelectNodes( "descendant::nodeAddress" );

      foreach (XmlNode addressNode in addressNodes)
      {
        _nodeDefinitions.Add(addressNode.InnerText.Trim());
        GenerateAllAddresses( addressNode.InnerText.Trim() );
      }
      
      if(addressNodes.Count == 0 && DeploymentMode.IsAmazon())
      {
        ConfigureWithAmazon();
      }


      ClusterServiceFactory clusterServiceFactory = new ClusterServiceFactory();
      ORBConfig.GetInstance().getObjectFactories().AddServiceObjectFactory( "Weborb.Cluster.Cluster", clusterServiceFactory );
      clusterServiceFactory.createObject();
      return this;
    }

    private void ConfigureWithAmazon()
    {
      _potentialNodes.Clear();
      _nodeDefinitions.Clear();
      String accessKey = AmazonConfigurator.GetData(ORBConstants.ACCESS_KEY);
      String secretKey = AmazonConfigurator.GetData(ORBConstants.AWS_SECRET_KEY);

      if (accessKey != null && secretKey != null)
      {
        AmazonEC2Client client = new AmazonEC2Client(new BasicAWSCredentials(accessKey, secretKey));
        String usePublicIPStr = AmazonConfigurator.GetData(ORBConstants.AMAZON_USE_PUBLICIP);
        bool usePublicIP = usePublicIPStr == null ? false : usePublicIPStr.Equals("yes");

        DescribeInstancesRequest request = new DescribeInstancesRequest();
        DescribeInstancesResponse response = client.DescribeInstances(request);
        if (response.IsSetDescribeInstancesResult())
        {
          DescribeInstancesResult result = response.DescribeInstancesResult;

          foreach (Reservation reservation in result.Reservation)
            foreach (RunningInstance instance in reservation.RunningInstance)
            {
              String ipAddress = usePublicIP ? instance.PublicDnsName : instance.PrivateIpAddress;

              if (Log.isLogging(LoggingConstants.CLUSTER))
                Log.log(LoggingConstants.CLUSTER, "Neighbour cloud node was found - " + ipAddress);

              _potentialNodes.Add(ipAddress);
            }
        }

        try
        {
          _port = int.Parse(AmazonConfigurator.GetData(ORBConstants.AMAZON_WEBORB_PORT));
        }
        catch (Exception exception)
        {
          if (Log.isLogging(LoggingConstants.ERROR))
            Log.log(LoggingConstants.ERROR, "Can't parse port from amazon's user data. Will use value from config");
          if (Log.isLogging(LoggingConstants.EXCEPTION))
            Log.log(LoggingConstants.EXCEPTION, exception);
        }

        String clusterName = null;

        try
        {
          clusterName = AmazonConfigurator.GetData(ORBConstants.AMAZON_CLUSTER_NAME);
        }
        catch (Exception exception)
        {
          if (Log.isLogging(LoggingConstants.ERROR))
            Log.log(LoggingConstants.ERROR, "Can't read cluster name from metadata. Will use value from config");
          if (Log.isLogging(LoggingConstants.EXCEPTION))
            Log.log(LoggingConstants.EXCEPTION, exception);
        }

        if (clusterName != null)
          _clusterName = clusterName;
      }
    }

    private int GetNextWildcard( string pattern, int currentIndex )
    {
      int i = currentIndex;
      List<String> wildCards = new List<string>() { "*", "[", "\\" };

      while( i < pattern.Length && !wildCards.Contains( "" + pattern[ i ] ) )
        i++;

      return i;
    }

    private string ReplaceWildcardWithIndex( string pattern, int startWildcardIndex, int lastWildcardIndex )
    {
      String start = pattern.Substring( 0, startWildcardIndex );
      String end = pattern.Substring( lastWildcardIndex + 1 );

      return start + "{" + _wildcardsProcessed + "}" + end;
    }

    private String processBrackets( String pattern, int currentIndex )
    {
      int lastBraceIndex = pattern.IndexOf( ']', currentIndex );

      if( lastBraceIndex == -1 )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Unclosed brace in ip mask in the cluster config section. Some cluster nodes will be skipped!" );

        return null;
      }

      if( Math.Abs( currentIndex - lastBraceIndex ) == 1 )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Empty braces in ip mask in the cluster config section." );

        currentIndex++;
        return null;
      }

      String bracesInfo = pattern.Substring( currentIndex + 1, lastBraceIndex - currentIndex - 1 );
      pattern = ReplaceWildcardWithIndex( pattern, currentIndex, lastBraceIndex );

      // for each single value or diapason create corresponding indexes
      foreach( String range in bracesInfo.Split( ',' ) )
      {
        int hyphenIndex = range.IndexOf( "-" );

        if( hyphenIndex == -1 )
        {
          _allVariations[ _allVariations.Count - 1 ].Add( int.Parse( range.Trim() ) );
        }
        else if( hyphenIndex == 0 || hyphenIndex == range.Length - 1 )
        {
          if( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR, "Wrong ip address mask format. Some cluster nodes will be skipped!" );

          break;
        }
        else
        {
          int firstNumber = int.Parse( range.Substring( 0, hyphenIndex ).Trim() );
          int secondNumber = int.Parse( range.Substring( hyphenIndex + 1 ).Trim() );

          for( int i = firstNumber; i <= secondNumber; i++ )
            _allVariations[ _allVariations.Count - 1 ].Add( i );
        }
      }
      return pattern;
    }

    private void GenerateAllAddresses( string pattern )
    {
      _allVariations.Clear();

      int currentIndex = 0;
      _wildcardsProcessed = 0;
      currentIndex = GetNextWildcard( pattern, currentIndex );

      // extract information from each braces pair, star and \d in pattern
      // and fill allVariations array
      while( currentIndex < pattern.Length )
      {
        _allVariations.Add( new List<int>() );

        switch( pattern[ currentIndex ] )
        {
          case '[':
            pattern = processBrackets( pattern, currentIndex );
            break;
          case '*':
            pattern = ProcessStar( pattern, currentIndex );
            break;
          case '\\':
            pattern = ProcessSlash( pattern, currentIndex );
            break;
        }

        // null pattern means processing error
        if( pattern == null )
          return;

        _wildcardsProcessed++;
        currentIndex = GetNextWildcard( pattern, currentIndex );
      }

      // if wildcards don't exist add pattern directly (it is normal address)
      if( _allVariations.Count == 0 )
      {
        _potentialNodes.Add( pattern );
      }
      else
        GgenerateAllAddresses( 0, pattern );

    }

    private string ProcessStar( string pattern, int currentIndex )
    {
      for( int i = 0; i <= 255; i++ )
        _allVariations[ _allVariations.Count - 1 ].Add( i );

      return ReplaceWildcardWithIndex( pattern, currentIndex, currentIndex );
    }

    private string ProcessSlash( string pattern, int currentIndex )
    {
      if( pattern.Length <= currentIndex + 1 || pattern[ currentIndex + 1 ] != 'd' )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Use \\d for digits from 0 to 9(not single slash) in computer address pattern" );

        return null;
      }

      for( int i = 0; i <= 9; i++ )
        _allVariations[ _allVariations.Count - 1 ].Add( i );

      return ReplaceWildcardWithIndex( pattern, currentIndex, currentIndex + 1 );
    }

    private void GgenerateAllAddresses( int depth, string ipPattern )
    {
      for( int i = 0; i < _allVariations[ depth ].Count; i++ )
      {
        String newIPPattern = ipPattern.Replace( "{" + depth + "}", _allVariations[ depth ][ i ].ToString() );

        if( depth == _wildcardsProcessed - 1 )
        {
          if( !_potentialNodes.Contains( newIPPattern ) )
          {
            if( IsValidIPAddress( newIPPattern ) )
              return;

            _potentialNodes.Add( newIPPattern );
          }
        }
        else
        {
          GgenerateAllAddresses( depth + 1, newIPPattern );
        }
      }
    }

    private bool IsValidIPAddress( string newIPPattern )
    {
      if( Regex.IsMatch( newIPPattern, "^[0-9\\.]+$" ) )
      {
        foreach( String numStr in newIPPattern.Split( '.' ) )
        {
          if( int.Parse( numStr ) > 255 )
          {
            if( Log.isLogging( LoggingConstants.INFO ) )
              Log.log( LoggingConstants.INFO, "IP address " + newIPPattern + " is wrong, it will be skipped" );
            return true;
          }
        }
      }
      return false;
    }

    public static string ClusterName
    {
      get { return GetConfig() == null ? null : GetConfig()._clusterName; }
    }

    public static int Port
    {
      get { return GetConfig() == null ? 0 : GetConfig()._port; }
    }

    public static List<string> PotentialNodes
    {
      get { return GetConfig() == null ? null : GetConfig()._potentialNodes; }
      set { GetConfig()._potentialNodes = value; }
    }

    public static ClusterConfig GetConfig()
    {
      return (ClusterConfig) ORBConfig.GetInstance().GetConfig( SECTION_NAME );
    }

    public String getSectionWrapperName()
    {
      return ConfigConstants.CLUSTER;
    }
  }
}

#if (CLOUD)
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using Weborb.Util;
using Weborb.Cloud;

namespace Weborb.Service
  {
  public class CloudAssembliesRegistry
    {
    private const string ASSEMBLY_DIR = "bin";

    #region Properties
    private static CloudBlobContainer assemblyStorageFacility = null;    
    public static Dictionary<String, Assembly> CloudAssemblies = new Dictionary<string, Assembly>();
    public static Dictionary<String, long> AssembliesTimestamp = new Dictionary<string, long>();
    private static bool initialized;

    static void initiaize()
      {
      if ( initialized ) return;      

      var accountInfo = AzureUtil.StorageAccount;

      var containerName = ORBConstants.WEBORB_AZURE_CONTAINER;
      assemblyStorageFacility = accountInfo.CreateCloudBlobClient().GetContainerReference( containerName );

      assemblyStorageFacility.CreateIfNotExist();

      var permissions = assemblyStorageFacility.GetPermissions();
      permissions.PublicAccess = BlobContainerPublicAccessType.Container;
      assemblyStorageFacility.SetPermissions( permissions );
      initialized = true;
      }

    public static Assembly[] CloudAssembliesArray
      {
      get
        {
        refreshAssemblies();

        IEnumerator iter = CloudAssemblies.Values.GetEnumerator();

        List<Assembly> assemblies = new List<Assembly>();

        while ( iter.MoveNext() )
          assemblies.Add( (Assembly)iter.Current );

        return assemblies.ToArray();
        }
      }

    public static Assembly[] CurrentAssemblies
      {
      get
        {
        try
          {
          refreshAssemblies();

          return AssembliesRegistry.RemoveDuplicateAssemblies( AppDomain.CurrentDomain.GetAssemblies() );
          }
        catch
          {
          return AppDomain.CurrentDomain.GetAssemblies();
          }
        }
      }
    #endregion

    #region Assembly Loading API Methods
    private static void LoadAssembly( CloudBlob blob, string assemblyName, byte[] serializedAssembly )
      {
      blob.FetchAttributes();
      if ( blob.Metadata.Get( "timestamp" ) == null )
        {
        blob.Metadata[ "timestamp" ] = ORBUtil.currentTimeMillis().ToString();
        blob.SetMetadata();
        }
      var timestamp = long.Parse( blob.Metadata[ "timestamp" ] );

      if ( !CloudAssemblies.ContainsKey( assemblyName ) )
        {
        CloudAssemblies.Add( assemblyName, Assembly.Load( serializedAssembly ) );
        AssembliesTimestamp[ assemblyName ] = timestamp;
        }
      else
        {
        if ( timestamp > AssembliesTimestamp[ assemblyName ] )
          {
          CloudAssemblies[ assemblyName ] = Assembly.Load( serializedAssembly );
          AssembliesTimestamp[ assemblyName ] = timestamp;
          TypeLoader.clearTypeCache();
          }
        }      
      }
    #endregion

    #region Assembly Storage API Methods
    public static void StoreAssembly( string filePath )
      {
      StoreAssembly( new FileInfo( filePath ) );
      }
    public static void StoreAssembly( FileInfo file )
      {
      StoreAssembly( file.Name, new FileStream( file.FullName, FileMode.Open ) );
      }
    public static void StoreAssembly( string name, Stream stream )
      {
      StoreAssembly( name, new BinaryReader( stream ), Convert.ToInt32( stream.Length ) );

      stream.Close();
      }
    public static void StoreAssembly( string name, BinaryReader reader, int streamLengh )
      {
      StoreAssembly( name, reader.ReadBytes( streamLengh ) );

      reader.Close();
      }
    public static void StoreAssembly( string name, byte[] assembly )
    {
      refreshAssemblies();

      CloudBlob blob = assemblyStorageFacility.GetDirectoryReference( ASSEMBLY_DIR ).GetBlobReference( name );
      using( BlobStream stream = blob.OpenWrite() )
      {
        stream.Write( assembly, 0, assembly.Length );
      }
      LoadAssembly( blob, blob.Uri.ToString(), assembly );
    }
    #endregion

    #region Establish DLL Storage Facility API Methods
    private static void refreshAssemblies()
      {
      initiaize();
      reloadStoredCloudAssemblies( assemblyStorageFacility.GetDirectoryReference( ASSEMBLY_DIR ).ListBlobs() );
      }

    private static void reloadStoredCloudAssemblies( IEnumerable<IListBlobItem> storedAssemblies )
      {
      foreach ( IListBlobItem assembly in storedAssemblies )
        {
        var blob = AzureUtil.StorageAccount.CreateCloudBlobClient().GetBlobReference( assembly.Uri.ToString() );

        if ( assembly is CloudBlobDirectory )
          reloadStoredCloudAssemblies( ( (CloudBlobDirectory)assembly ).ListBlobs() );
        else if ( blob.Uri.ToString().EndsWith( ".dll", StringComparison.CurrentCultureIgnoreCase ) )
          LoadAssembly( blob, blob.Uri.ToString(), blob.DownloadByteArray() );
        }
      }
    #endregion
    }
  }
#endif

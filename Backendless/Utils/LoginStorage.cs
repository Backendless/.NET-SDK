using System;
using System.Collections.Generic;
using System.IO;
#if UNIVERSALW8
using Windows.Storage;
#else
using System.IO.IsolatedStorage;
#endif
using System.Text;

namespace BackendlessAPI.Utils
{
  public class LoginStorage : ILoginStorage
  {
    public LoginStorage()
    {
      myNewPrefs = LoadData();
    }

    public string ObjectId
    {
      get;
      set;
    }

    public string UserToken
    {
      get;
      set;
    }

    bool myNewPrefs;
    public bool HasData
    {
      get { return myNewPrefs; }
    }

    private bool LoadData()
    {
      #if UNIVERSALW8
      ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

      if( localSettings.Values.ContainsKey( "BackendlessUserInfo" ) )
      {
        ApplicationDataCompositeValue userInfo = (ApplicationDataCompositeValue) localSettings.Values[ "BackendlessUserInfo" ];
        this.UserToken = (String) userInfo[ "userToken" ];
        this.UserId = (String) userInfo[ "userId" ];
        return false;
      }
      else
      {
        return true;
      }

#else
      try
      {
          // Retrieve an IsolatedStorageFile for the current Domain and Assembly.
          IsolatedStorageFileStream isoStream = null;
#if !WINDOWS_PHONE && !WINDOWS_PHONE8
          IsolatedStorageFile isoFile =
            IsolatedStorageFile.GetStore( IsolatedStorageScope.User |
            IsolatedStorageScope.Assembly |
            IsolatedStorageScope.Domain,
            null,
            null );

        isoStream =
            new IsolatedStorageFileStream( "BackendlessUserInfo",
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read );
#else
          IsolatedStorageFile isoFile = null;

          try
          {
              isoFile = IsolatedStorageFile.GetUserStoreForApplication();
          }
          catch( System.Exception )
          {
              return false;
          }

         if( !isoFile.FileExists( "BackendlessUserInfo" ) )
             return false;

        try
          {
           isoStream = new IsolatedStorageFileStream( "BackendlessUserInfo",
            FileMode.Open,
            FileAccess.Read,
            isoFile );
          }
          catch( System.Exception e )
          {
              Console.WriteLine(e);
          }
#endif
        StreamReader reader = new StreamReader( isoStream );
        // Read the data.
        this.UserToken = reader.ReadLine();
        this.ObjectId = reader.ReadLine();
        reader.Close();
        isoStream.Close();
        return true;
      }
      catch( System.Exception )
      {
        // Expected exception if a file cannot be found. This indicates that we have a new user.
        return false;
      }
#endif
    }

    public void SaveData()
    {
#if UNIVERSALW8
      ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
      ApplicationDataCompositeValue userInfo = new ApplicationDataCompositeValue();

      userInfo[ "userToken" ] = this.UserToken;
      userInfo[ "userId" ] = this.UserId;
      localSettings.Values[ "BackendlessUserInfo" ] = userInfo;
#else
      try
      {
#if !WINDOWS_PHONE && !WINDOWS_PHONE8
        IsolatedStorageFile isoFile;
        isoFile = IsolatedStorageFile.GetUserStoreForDomain();

        // Open or create a writable file.
        IsolatedStorageFileStream isoStream =
            new IsolatedStorageFileStream( "BackendlessUserInfo",
            FileMode.OpenOrCreate,
            FileAccess.Write,
            isoFile ); 
#else
          IsolatedStorageFile isoFile =
    IsolatedStorageFile.GetUserStoreForApplication();

        IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream( "BackendlessUserInfo",
            FileMode.OpenOrCreate,
            FileAccess.Write,
            isoFile );
#endif

        StreamWriter writer = new StreamWriter( isoStream );
        writer.WriteLine( this.UserToken );
        writer.WriteLine( this.ObjectId );
        // StreamWriter.Close implicitly closes isoStream.
        writer.Close();
        isoFile.Dispose();
      }
      catch( IsolatedStorageException ex )
      {
        // Add code here to handle the exception.
        Console.WriteLine( ex );
      }
#endif
    }

    public void DeleteFiles()
    {
#if UNIVERSALW8
      ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
      localSettings.Values.Remove( "BackendlessUserInfo" );
#else
      try
      {
#if !WINDOWS_PHONE && !WINDOWS_PHONE8
        IsolatedStorageFile isoFile = IsolatedStorageFile.GetStore( IsolatedStorageScope.User |
            IsolatedStorageScope.Assembly |
            IsolatedStorageScope.Domain,
            typeof( System.Security.Policy.Url ),
            typeof( System.Security.Policy.Url ) );
#else
          IsolatedStorageFile isoFile =
    IsolatedStorageFile.GetUserStoreForApplication();
#endif

        //String[] dirNames = isoFile.GetDirectoryNames( "*" );
        String[] fileNames = isoFile.GetFileNames( "*" );

        // List the files currently in this Isolated Storage.
        // The list represents all users who have personal
        // preferences stored for this application.
        if( fileNames.Length > 0 )
        {
          for( int i = 0; i < fileNames.Length; ++i )
          {
            // Delete the files.
            isoFile.DeleteFile( fileNames[ i ] );
          }
          // Confirm that no files remain.
          fileNames = isoFile.GetFileNames( "*" );
        }
      }
      catch( System.Exception e )
      {
        Console.WriteLine( e.ToString() );
      }
#endif
    }
    // This method deletes directories in the specified Isolated Storage, after first 
    // deleting the files they contain. In this example, the Archive directory is deleted. 
    // There should be no other directories in this Isolated Storage.
  /*
    public void DeleteDirectories()
    {
      try
      {
#if !WINDOWS_PHONE
        IsolatedStorageFile isoFile = IsolatedStorageFile.GetStore( IsolatedStorageScope.User |
            IsolatedStorageScope.Assembly |
            IsolatedStorageScope.Domain,
            typeof( System.Security.Policy.Url ),
            typeof( System.Security.Policy.Url ) );
#else
        IsolatedStorageFile isoFile =
IsolatedStorageFile.GetUserStoreForApplication();
#endif

        isoFile.Remove();
      }
      catch( System.Exception e )
      {
        Console.WriteLine( e.ToString() );
      }
    }
   */
  }
}

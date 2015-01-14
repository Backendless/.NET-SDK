using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace BackendlessAPI.Utils
{
  public class LoginStorage
  {
    public LoginStorage()
    {
      myNewPrefs = LoadData();
    }

    public string UserId
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
    public bool NewPrefs
    {
      get { return myNewPrefs; }
    }

    private bool LoadData()
    {
      try
      {
          // Retrieve an IsolatedStorageFile for the current Domain and Assembly.
          IsolatedStorageFileStream isoStream = null;
#if !WINDOWS_PHONE
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
          IsolatedStorageFile isoFile =
              IsolatedStorageFile.GetUserStoreForApplication();

         if( !isoFile.FileExists( "BackendlessUserInfo" ) )
             return true;

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
        this.UserId = reader.ReadLine();
        reader.Close();
        isoStream.Close();
        return false;
      }
      catch( System.IO.FileNotFoundException )
      {
        // Expected exception if a file cannot be found. This indicates that we have a new user.
        return true;
      }
    }

    public void SaveData()
    {
      try
      {
#if !WINDOWS_PHONE
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
        writer.WriteLine( this.UserId );
        // StreamWriter.Close implicitly closes isoStream.
        writer.Close();
        isoFile.Dispose();
      }
      catch( IsolatedStorageException ex )
      {
        // Add code here to handle the exception.
        Console.WriteLine( ex );
      }
    }

    public void DeleteFiles()
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

    }
    // This method deletes directories in the specified Isolated Storage, after first 
    // deleting the files they contain. In this example, the Archive directory is deleted. 
    // There should be no other directories in this Isolated Storage.
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
  }
}

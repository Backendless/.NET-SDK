using System;
using System.Diagnostics;
using System.IO;
using Plugin.DeviceInfo;

namespace BackendlessAPI.Xamarin
{
  public class LocalStorage
  {
    private String path = System.Environment.GetFolderPath( System.Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create );
    private Boolean _hasData;
    private String _deviceId;

    public LocalStorage()
    {
      HasData = !String.IsNullOrEmpty( GetData() );
    }

    public Boolean HasData
    {
      get => _hasData;
      private set => _hasData = value;
    }

    public String DeviceId
    {
      get
      {
        if( HasData )
          return _deviceId;
        else
        {
          SaveData();
          return _deviceId = GetData();
        }
      }
      private set => _deviceId = String.IsNullOrEmpty( _deviceId ) ? value : _deviceId;
    }

    public void SaveData()
    {
      try
      {
        String fileName = Path.Combine( path, "BackendlessDeviceId.txt" );

        //Write
        using( var writer = System.IO.File.CreateText( fileName ) )
        {
          writer.WriteLine( CrossDeviceInfo.Current.Id );
        }

        HasData = true;
      }
      catch( System.Exception ex )
      {
        Debug.WriteLine( $"{ex.Message}" );
      }
    }


    public String GetData()
    {
      try
      {
        String fileName = Path.Combine( path, "BackendlessDeviceId.txt" );

        //Read
        using( var reader = new StreamReader( fileName, true ) )
        {
          DeviceId = reader.ReadToEnd().Trim( "\n".ToCharArray() );
        }

        return DeviceId;
      }
      catch
      {
        return null;
      }
    }

    public void DeleteFiles()
    {
      try
      {
        if( HasData )
        {
          String fileName = Path.Combine( path, "BackendlessDeviceId.txt" );

          //Remove
          System.IO.File.Delete( fileName );
          HasData = false;
        }
      }
      catch( System.Exception e )
      {
        throw new System.Exception( e.Message );
      }
    }
  }
}

using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Acquaint.Util
{
  /// <summary>
  /// This class uses the Xamarin settings plugin (Plugins.Settings) to implement cross-platform storing of settings.
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get { return CrossSettings.Current; }
    }

    public static void ResetUserConfigurableSettingsToDefaults()
    {
      DataIsSeeded = DataIsSeededDefault;
      LocalDataResetIsRequested = true;
      BackendlessServiceUrl = BackendlessServiceUrlDefault;
      ImageCacheDurationHours = ImageCacheDurationHoursDefault;
    }

    public static bool IsUsingLocalDataSource => DataPartitionPhrase == "UseLocalDataSource";

    public static event EventHandler OnDataPartitionPhraseChanged;

    /// <summary>
    /// Raises the data parition phrase changed event.
    /// </summary>
    /// <param name="e">E.</param>
    static void RaiseDataParitionPhraseChangedEvent( EventArgs e )
    {
      var handler = OnDataPartitionPhraseChanged;

      if( handler != null )
        handler( null, e );
    }

    #region user-configurable

    private const string BackendlessServiceUrlKey = "BackendlessServiceUrl_key";
    private static readonly string BackendlessServiceUrlDefault = "https://api.backendless.com";

    private const string DataPartitionPhraseKey = "DataPartitionPhrase_key";
    private static readonly string DataSeedPhraseDefault = "";

    private const string ImageCacheDurationHoursKey = "ImageCacheDurationHours_key";
    public static readonly int ImageCacheDurationHoursDefault = 1;

    public static string BackendlessServiceUrl
    {
      get { return AppSettings.GetValueOrDefault( BackendlessServiceUrlKey, BackendlessServiceUrlDefault ); }
      set { AppSettings.AddOrUpdateValue( BackendlessServiceUrlKey, value ); }
    }

    public static string DataPartitionPhrase
    {
      get { return AppSettings.GetValueOrDefault( DataPartitionPhraseKey, DataSeedPhraseDefault ); }
      set
      {
        AppSettings.AddOrUpdateValue( DataPartitionPhraseKey, value );
        RaiseDataParitionPhraseChangedEvent( null );
      }
    }

    public static int ImageCacheDurationHours
    {
      get { return AppSettings.GetValueOrDefault( ImageCacheDurationHoursKey, ImageCacheDurationHoursDefault ); }
      set { AppSettings.AddOrUpdateValue( ImageCacheDurationHoursKey, value ); }
    }

    #endregion


    #region non-user-configurable

    private const string DataIsSeededKey = "DataIsSeeded_key";
    private static readonly bool DataIsSeededDefault = false;

    private const string LocalDataResetIsRequestedKey = "LocalDataResetIsRequested_key";
    private static readonly bool LocalDataResetIsRequestedDefault = false;

    private const string ClearImageCacheIsRequestedKey = "ClearImageCacheIsRequested_key";
    private static readonly bool ClearImageCacheIsRequestedDefault = false;

    private const string HockeyAppIdKey = "HockeyAppId_key";
    private static readonly string HockeyAppIdDefault = "11111111222222223333333344444444"; // This is just a placeholder value. Replace with your real HockeyApp App ID.

    private const string BingMapsKeyKey = "BingMapsKey_key";
    private static readonly string BingMapsKeyDefault = "UW0peICp3gljJyhqQKFZ~R3XF1I5BvWmWmkD4ujytTA~AoUOqpk2nJB-Wh7wH-9S-zaG-w6sygLitXugNOqm71wx_nc6WHIt6Lb29gyTU04X";

    public static bool LocalDataResetIsRequested
    {
      get { return AppSettings.GetValueOrDefault( LocalDataResetIsRequestedKey, LocalDataResetIsRequestedDefault ); }
      set { AppSettings.AddOrUpdateValue( LocalDataResetIsRequestedKey, value ); }
    }

    public static bool ClearImageCacheIsRequested
    {
      get { return AppSettings.GetValueOrDefault( ClearImageCacheIsRequestedKey, ClearImageCacheIsRequestedDefault ); }
      set { AppSettings.AddOrUpdateValue( ClearImageCacheIsRequestedKey, value ); }
    }

    public static bool DataIsSeeded
    {
      get { return AppSettings.GetValueOrDefault( DataIsSeededKey, DataIsSeededDefault ); }
      set { AppSettings.AddOrUpdateValue( DataIsSeededKey, value ); }
    }

    public static string HockeyAppId
    {
      get { return AppSettings.GetValueOrDefault( HockeyAppIdKey, HockeyAppIdDefault ); }
      set { AppSettings.AddOrUpdateValue( HockeyAppIdKey, value ); }
    }

    public static string BingMapsKey
    {
      get { return AppSettings.GetValueOrDefault( BingMapsKeyKey, BingMapsKeyDefault ); }
      set { AppSettings.AddOrUpdateValue( BingMapsKeyKey, value ); }
    }

    public static string BackendlessAPPID
    {
      // This is just a placeholder, replace with your own application ID
      get { return "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"; }
    }

    public static string BackendlessAPIKEY
    {
      // This is just a placeholder, replace with your own .NET API Key
      get { return "ZZZZZZZZ-ZZZZ-ZZZZ-ZZZZ-ZZZZZZZZZZZZ"; }
    }

    #endregion
  }
}


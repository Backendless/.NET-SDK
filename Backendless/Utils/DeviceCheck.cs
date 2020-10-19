using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Util;

namespace BackendlessAPI.Utils
{
  internal static class DeviceCheck
  {
    private const String XAMARIN_FULLNAME = "Xamarin.Forms.Device, Xamarin.Forms.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null";

    internal static Boolean IsMobile
    {
      get => TypeLoader.LoadType( XAMARIN_FULLNAME ) != null;
    }

    internal static String GetDeviceOS()
    {
      Type currentDeviceType = TypeLoader.LoadType( XAMARIN_FULLNAME );

      if( currentDeviceType != null )
        return currentDeviceType.GetProperty( "RuntimePlatform" )
                                .GetValue( currentDeviceType, null ).ToString().ToUpper();

      return "UNKNOWN";
    }
  }
}

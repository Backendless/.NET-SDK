using System;
#if UNIVERSALW8
using Windows.Networking;
#else
using Microsoft.Phone.Notification;
#endif

namespace BackendlessAPI.Push
{
  internal class RegistrationDecorator
  {
    private RegistrationInfo _registrationInfo;

    public RegistrationDecorator( HttpNotificationChannel channel, string registrationId,
                                  DateTime registrationExpiration )
    {
      _registrationInfo = new RegistrationInfo
        {
          Channel = channel,
          RegistrationId = registrationId,
          RegistrationExpiration = registrationExpiration
        };
    }

    public bool IsRegistered()
    {
      if( _registrationInfo == null )
        return false;

      if( _registrationInfo.Channel == null || string.IsNullOrEmpty( _registrationInfo.RegistrationId ) ||
          _registrationInfo.RegistrationExpiration < DateTime.Now )
      {
        _registrationInfo = null;
        return false;
      }

      return true;
    }

    public string GetRegistrationId()
    {
      return _registrationInfo.RegistrationId;
    }

    public RegistrationInfo GetRegistrationInfo()
    {
      return _registrationInfo;
    }
  }

  internal class RegistrationInfo
  {
    public HttpNotificationChannel Channel { get; set; }
    public string RegistrationId { get; set; }
    public DateTime RegistrationExpiration { get; set; }
  }
}
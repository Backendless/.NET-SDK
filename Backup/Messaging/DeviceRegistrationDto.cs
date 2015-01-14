using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class DeviceRegistration
  {
    [SetClientClassMemberName( "id" )]
    public string Id { get; set; }

    [SetClientClassMemberName( "channels" )]
    public List<string> Channels { get; set; }

    [SetClientClassMemberName( "expiration" )]
    public DateTime? Expiration { get; set; }

    [SetClientClassMemberName( "os" )]
    public string Os { get; set; }

    [SetClientClassMemberName( "osVersion" )]
    public string OsVersion { get; set; }

    [SetClientClassMemberName( "deviceToken" )]
    public string DeviceToken { get; set; }

    [SetClientClassMemberName( "registrationId" )]
    public string RegistrationId { get; set; }

    [SetClientClassMemberName( "deviceId" )]
    public string DeviceId { get; set; }

    public void AddChannel( string channel )
    {
      if( Channels == null )
        Channels = new List<string>();

      Channels.Add( channel );
    }

    public void ClearRegistration()
    {
      Id = null;
      Channels = null;
      RegistrationId = null;
      DeviceToken = null;
    }
  }
}
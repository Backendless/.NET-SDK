﻿using System;
using Weborb.Service;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;

namespace BackendlessAPI.Messaging
{
  public class DeviceRegistration
  {
    [SetClientClassMemberName( "id" )]
    public String Id { get; set; }

    [SetClientClassMemberName( "channels" )]
    public List<String> Channels { get; set; }

    [SetClientClassMemberName( "expiration" )]
    public DateTime? Expiration { get; set; }

    [SetClientClassMemberName( "os" )]
    public String Os { get; set; }

    [SetClientClassMemberName( "osVersion" )]
    public String OsVersion { get; set; }

    [SetClientClassMemberName( "deviceToken" )]
    public String DeviceToken { get; set; }

    [SetClientClassMemberName( "registrationId" )]
    public String RegistrationId { get; set; }

    [SetClientClassMemberName( "deviceId" )]
    public String DeviceId { get; } = DeviceIdGenerator();

    public void AddChannel( String channel )
    {
      if( Channels == null )
        Channels = new List<String>();

      Channels.Add( channel );
    }

    public void ClearRegistration()
    {
      Id = null;
      Channels = null;
      RegistrationId = null;
      DeviceToken = null;
    }

    public static String DeviceIdGenerator()
    {
      Guid guid = Guid.NewGuid();
      string str = guid.ToString();
      return str;
    }
  }
}
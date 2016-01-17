using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.V3Types.Core
  {
  public interface IMessageEventListener
    {
    void messageReceived( String clientId, Object message );
    void messageSend( Object message );
    }
  }

using System;
using System.IO;
using Weborb.Message;

namespace Weborb.Protocols
  {
  public interface IMessageFactory
    {
    string GetProtocolName( Request input );
    string[] GetProtocolNames();
    bool CanParse( string contentType );
    Request Parse( Stream requestStream );
    }
  }

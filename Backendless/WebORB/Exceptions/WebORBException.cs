using System;
using Weborb.Client;

namespace Weborb.Exceptions
{
  public class WebORBException : Exception
  {
    public WebORBException( Fault fault )
    {
      Fault = fault;
    }

    public Fault Fault{ get; private set; }
  }
}
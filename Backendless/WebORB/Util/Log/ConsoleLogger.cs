using System;
using System.Threading;
using System.Text;

namespace Weborb.Util.Logging
  {
  public class ConsoleLogger : AbstractLogger
    {
    public override void fireEvent( String category, Object eventObject, DateTime timestamp )
      {
      Console.WriteLine( format( category, eventObject, timestamp ) );
      }
    }
  }
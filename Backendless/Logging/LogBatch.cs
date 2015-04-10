using System;
using System.Collections.Generic;

namespace BackendlessAPI.Logging
{
  class LogBatch
  {
    public String logLevel;
    public String logger;
    public LinkedList<LogMessage> messages;
  }
}

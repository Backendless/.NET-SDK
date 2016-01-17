using System;

namespace BackendlessAPI.Logging
{
  class LogMessage
  {
    internal LogMessage( String logger, LogLevel logLevel, DateTime timestamp, String message, String exception )
    {
      this.logger = logger;
      this.level = logLevel;
      this.timestamp = timestamp;
      this.message = message;
      this.exception = exception;
    }

    public String logger { get; set; }
    public LogLevel level { get; set; }
    public DateTime timestamp { get; set; }
    public String message { get; set; }
    public String exception { get; set; }
  }
}

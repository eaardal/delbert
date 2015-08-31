using System;

namespace Delbert.Infrastructure.Logging
{
    public class LogEntry
    {
        public string Message { get; set; }
        public Type SourceOfLogMessage { get; set; }
        public DateTime Timestamp { get; set; }
        public Severity Severity { get; set; }

        public LogEntry(string message, Type sourceOfLogMessage, DateTime timestamp, Severity severity)
        {
            Message = message;
            SourceOfLogMessage = sourceOfLogMessage;
            Timestamp = timestamp;
            Severity = severity;
        }
    }
}
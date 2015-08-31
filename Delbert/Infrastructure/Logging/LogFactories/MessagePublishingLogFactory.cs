using System;
using System.Linq;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Infrastructure.Logging.LogFactories
{
    public class MessagePublishingLogFactory : ILogFactory
    {
        private readonly IMessageBus _messageBus;

        public MessagePublishingLogFactory(IMessageBus messageBus)
        {
            if (messageBus == null) throw new ArgumentNullException(nameof(messageBus));
            _messageBus = messageBus;
        }

        public ILog LogFor(Type sourceOfLogMsg)
        {
            return new MessagePublishingLogger(sourceOfLogMsg, _messageBus);
        }

        private class MessagePublishingLogger : ILog
        {
            private readonly Type _sourceOfLogMsg;
            private readonly IMessageBus _messageBus;

            public MessagePublishingLogger(Type sourceOfLogMsg, IMessageBus messageBus)
            {
                if (sourceOfLogMsg == null) throw new ArgumentNullException(nameof(sourceOfLogMsg));
                if (messageBus == null) throw new ArgumentNullException(nameof(messageBus));
                _sourceOfLogMsg = sourceOfLogMsg;
                _messageBus = messageBus;
            }

            private void LogMessage(Severity severity, string message, params object[] args)
            {
                LogEntry logEntry;

                if (args == null || !args.Any())
                {
                    logEntry = new LogEntry(message, _sourceOfLogMsg, DateTime.Now, severity);
                }
                else
                {
                    logEntry = new LogEntry(string.Format(message, args), _sourceOfLogMsg, DateTime.Now, severity);
                }

                _messageBus.Publish(new LogEntryLogged(logEntry));
            }

            public void Debug(string message)
            {
                LogMessage(Severity.Debug, message);
            }

            public void Debug(string message, params object[] args)
            {
                LogMessage(Severity.Debug, message, args);
            }

            public void Verbose(string message)
            {
                LogMessage(Severity.Verbose, message);
            }

            public void Verbose(string message, params object[] args)
            {
                LogMessage(Severity.Verbose, message, args);
            }

            public void Info(string message)
            {
                LogMessage(Severity.Info, message);
            }

            public void Info(string message, params object[] args)
            {
                LogMessage(Severity.Info, message, args);
            }

            public void Warning(string message)
            {
                LogMessage(Severity.Warning, message);
            }

            public void Warning(string message, params object[] args)
            {
                LogMessage(Severity.Warning, message, args);
            }

            public void Error(string message)
            {
                LogMessage(Severity.Error, message);
            }

            public void Error(string message, params object[] args)
            {
                LogMessage(Severity.Error, message, args);
            }

            public void Error(string message, Exception ex, params object[] args)
            {
                var logMessage = $"{string.Format(message, args)}\n\n{ex.Message}\n\n{ex.StackTrace}";
                LogMessage(Severity.Error, logMessage);
            }

            public void Error(string message, Exception ex)
            {
                var logMessage = $"{message}\n\n{ex.Message}\n\n{ex.StackTrace}";
                LogMessage(Severity.Error, logMessage);
            }

            public void Error(Exception ex)
            {
                var logMessage = $"{ex.Message}\n\n{ex.StackTrace}}}";
                LogMessage(Severity.Error, logMessage);
            }
        }

        public class LogEntryLogged
        {
            public LogEntry LogEntry { get; private set; }

            public LogEntryLogged(LogEntry logEntry)
            {
                LogEntry = logEntry;
            }

            public LogEntryLogged(string message, Type sourceOfLogMessage, DateTime timestamp, Severity severity)
                : this(new LogEntry(message, sourceOfLogMessage, timestamp, severity))
            {
                
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Infrastructure.Logging.Contracts;

namespace Delbert.Actors
{
    public class ProcessActor : LoggingReceiveActor
    {
        private readonly IProcessAdapter _process;

        public ProcessActor(IProcessAdapter process, ILogger log) : base(log)
        {
            if (process == null) throw new ArgumentNullException(nameof(process));
            _process = process;

            Receive<StartProcessForFile>(msg => OnStartProcessForFile(msg));
        }

        private void OnStartProcessForFile(StartProcessForFile msg)
        {
            if (msg.File != null && msg.File.Exists)
            {
                _process.Start(msg.File.FullName);
            }
            else
            {
                Sender.Tell(new Failure());
            }
        }

        #region Messages

        public class StartProcessForFile
        {
            public FileInfo File { get; }

            public StartProcessForFile(FileInfo file)
            {
                File = file;
            }
        }

        #endregion
    }
}

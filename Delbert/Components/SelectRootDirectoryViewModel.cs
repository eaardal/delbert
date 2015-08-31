using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Akka.Actor;
using Akka.DI.Core;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Services;

namespace Delbert.Components
{
    class SelectRootDirectoryViewModel : ScreenViewModel, ISelectRootDirectoryViewModel
    {
        private readonly IActorSystemAdapter _actorSystem;
        private string _rootDirectoryPath;

        public SelectRootDirectoryViewModel(IIoC ioc, IActorSystemAdapter actorSystem) : base(ioc)
        {
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            _actorSystem = actorSystem;
        }

        public string RootDirectoryPath
        {
            get { return _rootDirectoryPath; }
            set
            {
                if (value == _rootDirectoryPath) return;
                _rootDirectoryPath = value;
                NotifyOfPropertyChange(() => RootDirectoryPath);
            }
        }

        public void BrowseRootDirectory()
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();
            
            var directory = dialog.SelectedPath.ToDirectoryInfo();

            var rootDirectory = _actorSystem.ActorOf<RootDirectoryActor>(ActorRegistry.RootDirectory);
            rootDirectory.Tell(new RootDirectoryActor.SetRootDirectory(directory));

            RootDirectoryPath = dialog.SelectedPath;
        }
    }
}

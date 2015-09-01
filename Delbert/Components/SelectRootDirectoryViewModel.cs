using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Akka.Actor;
using Delbert.Actors;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;

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

            MessageBus.Subscribe<NewRootDirectorySet>(OnNewRootDirectorySet);
        }

        protected override async void OnActivate()
        {
            await GetExistingRootDirectory();
        }

        private async Task GetExistingRootDirectory()
        {
            var rootDirectoryActor = _actorSystem.ActorSelection(ActorRegistry.RootDirectory);

            try
            {
                var result =
                    await
                        rootDirectoryActor.AskWithResultOf<RootDirectoryActor.GetRootDirectoryResult>(
                            new RootDirectoryActor.GetRootDirectory());

                SetRootDirectory(result.CurrentRootDirectory.FullName);
            }
            catch (Exception)
            {
                
            }
        }

        private void OnNewRootDirectorySet(NewRootDirectorySet message)
        {
            SetRootDirectory(message.RootDirectory.FullName);
        }

        private void SetRootDirectory(string rootDirectoryPath)
        {
            if (string.IsNullOrEmpty(RootDirectoryPath) || (!string.IsNullOrEmpty(rootDirectoryPath) && rootDirectoryPath != RootDirectoryPath))
            {
                RootDirectoryPath = rootDirectoryPath;
            }
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

            dialog.ShowDialog();
            
            var directory = dialog.SelectedPath.ToDirectoryInfo();

            var rootDirectory = _actorSystem.ActorSelection(ActorRegistry.RootDirectory);
            rootDirectory.Tell(new RootDirectoryActor.SetRootDirectory(directory));

            SetRootDirectory(dialog.SelectedPath);
        }
    }
}

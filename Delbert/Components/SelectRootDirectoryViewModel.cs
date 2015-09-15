using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Akka.Actor;
using Delbert.Actors;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;

namespace Delbert.Components
{
    class SelectRootDirectoryViewModel : ScreenViewModel, ISelectRootDirectoryViewModel
    {
        private readonly IRootDirectoryFacade _rootDirectory;
        private string _rootDirectoryPath;

        public SelectRootDirectoryViewModel(IIoC ioc, IRootDirectoryFacade rootDirectory) : base(ioc)
        {
            if (rootDirectory == null) throw new ArgumentNullException(nameof(rootDirectory));
            _rootDirectory = rootDirectory;

            MessageBus.Subscribe<RootDirectoryChanged>(OnRootDirectoryChanged);
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

            if (string.IsNullOrEmpty(dialog.SelectedPath)) return;

            var directory = dialog.SelectedPath.ToDirectoryInfo();

            _rootDirectory.SetRootDirectory(directory);

            SetRootDirectory(dialog.SelectedPath);
        }

        protected override async void OnActivate()
        {
            await GetExistingRootDirectory();
        }

        private async Task GetExistingRootDirectory()
        {
            var directory = await _rootDirectory.GetRootDirectory();
            
            SetRootDirectory(directory.FullName);
        }
        
        private void SetRootDirectory(string rootDirectoryPath)
        {
            if (string.IsNullOrEmpty(RootDirectoryPath) || (!string.IsNullOrEmpty(rootDirectoryPath) && rootDirectoryPath != RootDirectoryPath))
            {
                RootDirectoryPath = rootDirectoryPath;
            }
        }
        
        private void OnRootDirectoryChanged(RootDirectoryChanged message)
        {
            SetRootDirectory(message.RootDirectory.FullName);
        }
    }
}

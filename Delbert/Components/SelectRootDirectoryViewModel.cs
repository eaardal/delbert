﻿using System;
using System.Windows.Forms;
using Akka.Actor;
using Delbert.Actors;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;

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

            dialog.ShowDialog();
            
            var directory = dialog.SelectedPath.ToDirectoryInfo();

            var rootDirectory = _actorSystem.ActorSelection(ActorRegistry.RootDirectory);
            rootDirectory.Tell(new RootDirectoryActor.SetRootDirectory(directory));

            RootDirectoryPath = dialog.SelectedPath;
        }
    }
}

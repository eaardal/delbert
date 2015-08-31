﻿using System;
using Caliburn.Micro;

namespace Delbert.Shell
{
    sealed class ShellViewModel : Conductor<IScreen>, IShellViewModel
    {
        private IScreen _currentScreen;

        public ShellViewModel(IMainViewModel mainViewModel)
        {
            if (mainViewModel == null) throw new ArgumentNullException(nameof(mainViewModel));
            
            DisplayName = "Matmons";

            ActivateItem(mainViewModel);

            CurrentScreen = mainViewModel;
        }

        public IScreen CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                if (_currentScreen == value) return;
                _currentScreen = value;
                NotifyOfPropertyChange(() => CurrentScreen);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Delbert.Components;
using Delbert.Components.Notebook;
using Delbert.Components.Section;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Shell.Abstract;

namespace Delbert.Shell
{
    sealed class MainViewModel : ConductorViewModel, IMainViewModel
    {
        private IScreen _selectRootDirectory;
        private IScreen _listNotebooks;
        private IScreen _listNotebookSections;

        public MainViewModel(IIoC ioc, 
            ISelectRootDirectoryViewModel selectRootDirectoryViewModel,
            IListNotebooksViewModel listNotebooksViewModel,
            IListNotebookSectionsViewModel listNotebookSectionsViewModel) : base(ioc)
        {
            if (selectRootDirectoryViewModel == null)
                throw new ArgumentNullException(nameof(selectRootDirectoryViewModel));

            if (listNotebooksViewModel == null)
                throw new ArgumentNullException(nameof(listNotebooksViewModel));

            if (listNotebookSectionsViewModel == null)
                throw new ArgumentNullException(nameof(listNotebookSectionsViewModel));

            SelectRootDirectory = selectRootDirectoryViewModel;
            ListNotebooks = listNotebooksViewModel;
            ListNotebookSections = listNotebookSectionsViewModel;

            SetStartupState();

            MessageBus.Subscribe<NewRootDirectorySet>(OnNewRootDirectory);
        }

        private void SetStartupState()
        {
            SelectRootDirectory.Activate();
            ListNotebooks.Deactivate(true);
        }

        private void OnNewRootDirectory(NewRootDirectorySet message)
        {
            ActivateItem(ListNotebooks);
        }

        public IScreen ListNotebookSections
        {
            get { return _listNotebookSections; }
            set
            {
                if (Equals(value, _listNotebookSections)) return;
                _listNotebookSections = value;
                NotifyOfPropertyChange(() => ListNotebookSections);
            }
        }

        public IScreen ListNotebooks
        {
            get { return _listNotebooks; }
            set
            {
                if (Equals(value, _listNotebooks)) return;
                _listNotebooks = value;
                NotifyOfPropertyChange(() => ListNotebooks);
            }
        }

        public IScreen SelectRootDirectory
        {
            get { return _selectRootDirectory; }
            set
            {
                if (Equals(value, _selectRootDirectory)) return;
                _selectRootDirectory = value;
                NotifyOfPropertyChange(() => SelectRootDirectory);
            }
        }
    }
}

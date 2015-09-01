using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Delbert.Components;
using Delbert.Components.Editor;
using Delbert.Components.Notebook;
using Delbert.Components.Page;
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
        private IScreen _listSections;
        private IScreen _listPages;
        private IScreen _editor;

        public MainViewModel(IIoC ioc, 
            ISelectRootDirectoryViewModel selectRootDirectoryViewModel,
            IListNotebooksViewModel listNotebooksViewModel,
            IListSectionsViewModel listSectionsViewModel,
            IListPagesViewModel listPagesViewModel,
            IEditorViewModel editorViewModel) : base(ioc)
        {
            if (selectRootDirectoryViewModel == null)
                throw new ArgumentNullException(nameof(selectRootDirectoryViewModel));

            if (listNotebooksViewModel == null)
                throw new ArgumentNullException(nameof(listNotebooksViewModel));

            if (listSectionsViewModel == null)
                throw new ArgumentNullException(nameof(listSectionsViewModel));

            if (listPagesViewModel == null)
                throw new ArgumentNullException(nameof(listPagesViewModel));

            if (editorViewModel == null)
                throw new ArgumentNullException(nameof(editorViewModel));

            SelectRootDirectory = selectRootDirectoryViewModel;
            ListNotebooks = listNotebooksViewModel;
            ListSections = listSectionsViewModel;
            ListPages = listPagesViewModel;
            Editor = editorViewModel;

            SelectRootDirectory.Activate();

            MessageBus.Subscribe<NewRootDirectorySet>(OnNewRootDirectory);
        }
        
        private void OnNewRootDirectory(NewRootDirectorySet message)
        {
            //TODO: Hide root dir textbox?
        }

        public IScreen Editor
        {
            get { return _editor; }
            set
            {
                if (Equals(value, _editor)) return;
                _editor = value;
                NotifyOfPropertyChange(() => Editor);
            }
        }

        public IScreen ListPages
        {
            get { return _listPages; }
            set
            {
                if (Equals(value, _listPages)) return;
                _listPages = value;
                NotifyOfPropertyChange(() => ListPages);
            }
        }

        public IScreen ListSections
        {
            get { return _listSections; }
            set
            {
                if (Equals(value, _listSections)) return;
                _listSections = value;
                NotifyOfPropertyChange(() => ListSections);
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

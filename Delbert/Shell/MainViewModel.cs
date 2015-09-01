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
        private IScreen _listNotebookSections;
        private IScreen _listPages;
        private IScreen _editor;

        public MainViewModel(IIoC ioc, 
            ISelectRootDirectoryViewModel selectRootDirectoryViewModel,
            IListNotebooksViewModel listNotebooksViewModel,
            IListNotebookSectionsViewModel listNotebookSectionsViewModel,
            IListPagesViewModel listPagesViewModel,
            IEditorViewModel editorViewModel) : base(ioc)
        {
            if (selectRootDirectoryViewModel == null)
                throw new ArgumentNullException(nameof(selectRootDirectoryViewModel));

            if (listNotebooksViewModel == null)
                throw new ArgumentNullException(nameof(listNotebooksViewModel));

            if (listNotebookSectionsViewModel == null)
                throw new ArgumentNullException(nameof(listNotebookSectionsViewModel));

            if (listPagesViewModel == null)
                throw new ArgumentNullException(nameof(listPagesViewModel));

            if (editorViewModel == null)
                throw new ArgumentNullException(nameof(editorViewModel));

            SelectRootDirectory = selectRootDirectoryViewModel;
            ListNotebooks = listNotebooksViewModel;
            ListNotebookSections = listNotebookSectionsViewModel;
            ListPages = listPagesViewModel;
            Editor = editorViewModel;

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

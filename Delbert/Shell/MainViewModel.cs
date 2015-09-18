using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Delbert.Actors;
using Delbert.Actors.Facades.Abstract;
using Delbert.Components;
using Delbert.Components.Editor;
using Delbert.Components.Notebook;
using Delbert.Components.Page;
using Delbert.Components.Section;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Shell.Abstract;

namespace Delbert.Shell
{
    sealed class MainViewModel : ConductorViewModel, IMainViewModel
    {
        private readonly IRootDirectoryFacade _rootDirectory;
        private IScreen _selectRootDirectory;
        private IScreen _listNotebooks;
        private IScreen _listSections;
        private IScreen _listPages;
        private IScreen _editor;
        private IScreen _addNotebook;
        private IScreen _addSection;
        private bool _isRootDirectorySelected;
        private bool _isAnyNotebookSelected;
        private IScreen _addPage;
        private bool _isAnySectionSelected;

        public MainViewModel(IIoC ioc, 
            ISelectRootDirectoryViewModel selectRootDirectoryViewModel,
            IListNotebooksViewModel listNotebooksViewModel,
            IListSectionsViewModel listSectionsViewModel,
            IListPagesViewModel listPagesViewModel,
            IEditorViewModel editorViewModel,
            IAddNotebookViewModel addNotebookViewModel,
            IAddSectionViewModel addSectionViewModel,
            IAddPageViewModel addPageViewModel,
            IRootDirectoryFacade rootDirectory
            ) : base(ioc)
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

            if (addNotebookViewModel == null)
                throw new ArgumentNullException(nameof(addNotebookViewModel));

            if (addSectionViewModel == null)
                throw new ArgumentNullException(nameof(addSectionViewModel));

            if (addPageViewModel == null)
                throw new ArgumentNullException(nameof(addPageViewModel));

            if (rootDirectory == null)
                throw new ArgumentNullException(nameof(rootDirectory));

            _rootDirectory = rootDirectory;

            SelectRootDirectory = selectRootDirectoryViewModel;
            ListNotebooks = listNotebooksViewModel;
            ListSections = listSectionsViewModel;
            ListPages = listPagesViewModel;
            Editor = editorViewModel;
            AddNotebook = addNotebookViewModel;
            AddSection = addSectionViewModel;
            AddPage = addPageViewModel;
              
            MessageBus.Subscribe<RootDirectoryChanged>(OnNewRootDirectory);
            MessageBus.Subscribe<NotebookSelected>(OnNotebookSelected);
            MessageBus.Subscribe<SectionSelected>(OnSectionSelected);
        }

        private void OnSectionSelected(SectionSelected message)
        {
            IsAnySectionSelected = true;
        }

        private void OnNotebookSelected(NotebookSelected message)
        {
            IsAnyNotebookSelected = true;
            IsAnySectionSelected = false;
        }

        private void OnNewRootDirectory(RootDirectoryChanged message)
        {
            IsRootDirectorySelected = message.RootDirectory != null && message.RootDirectory.Exists;
        }

        protected override void OnActivate()
        {
            _rootDirectory.SetRootDirectoryFromCommandLineArgumentsIfExists();

            SelectRootDirectory.Activate();
            AddNotebook.Activate();
            ListNotebooks.Activate();
            AddSection.Activate();
            AddPage.Activate();
        }

        public bool IsAnySectionSelected
        {
            get { return _isAnySectionSelected; }
            set
            {
                if (value == _isAnySectionSelected) return;
                _isAnySectionSelected = value;
                NotifyOfPropertyChange(() => IsAnySectionSelected);
            }
        }

        public bool IsAnyNotebookSelected
        {
            get { return _isAnyNotebookSelected; }
            set
            {
                if (value == _isAnyNotebookSelected) return;
                _isAnyNotebookSelected = value;
                NotifyOfPropertyChange(() => IsAnyNotebookSelected);
            }
        }

        public bool IsRootDirectorySelected
        {
            get { return _isRootDirectorySelected; }
            set
            {
                if (value == _isRootDirectorySelected) return;
                _isRootDirectorySelected = value;
                NotifyOfPropertyChange(() => IsRootDirectorySelected);
            }
        }

        public IScreen AddPage
        {
            get { return _addPage; }
            set
            {
                if (Equals(value, _addPage)) return;
                _addPage = value;
                NotifyOfPropertyChange(() => AddPage);
            }
        }

        public IScreen AddSection
        {
            get { return _addSection; }
            set
            {
                if (Equals(value, _addSection)) return;
                _addSection = value;
                NotifyOfPropertyChange(() => AddSection);
            }
        }

        public IScreen AddNotebook
        {
            get { return _addNotebook; }
            set
            {
                if (Equals(value, _addNotebook)) return;
                _addNotebook = value;
                NotifyOfPropertyChange(() => AddNotebook);
            }
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

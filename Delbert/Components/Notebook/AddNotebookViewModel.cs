using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;

namespace Delbert.Components.Notebook
{
    public sealed class AddNotebookViewModel : ScreenViewModel, IAddNotebookViewModel
    {
        private readonly INotebookFacade _notebook;
        private readonly IRootDirectoryFacade _rootDirectory;
        private Visibility _showReadOnlyFieldsVisibility;
        private Visibility _showEditFieldsVisibility;
        private string _newNotebookName;

        public AddNotebookViewModel(INotebookFacade notebook, IRootDirectoryFacade rootDirectory, IIoC ioc) : base(ioc)
        {
            if (notebook == null) throw new ArgumentNullException(nameof(notebook));
            _notebook = notebook;
            _rootDirectory = rootDirectory;
        }

        protected override void OnActivate()
        {
            ShowReadOnly();
        }

        public Visibility ShowEditFieldsVisibility
        {
            get { return _showEditFieldsVisibility; }
            set
            {
                if (value == _showEditFieldsVisibility) return;
                _showEditFieldsVisibility = value;
                NotifyOfPropertyChange(() => ShowEditFieldsVisibility);
            }
        }

        public Visibility ShowReadOnlyFieldsVisibility
        {
            get { return _showReadOnlyFieldsVisibility; }
            set
            {
                if (value == _showReadOnlyFieldsVisibility) return;
                _showReadOnlyFieldsVisibility = value;
                NotifyOfPropertyChange(() => ShowReadOnlyFieldsVisibility);
            }
        }

        public string NewNotebookName
        {
            get { return _newNotebookName; }
            set
            {
                if (value == _newNotebookName) return;
                _newNotebookName = value;
                NotifyOfPropertyChange(() => NewNotebookName);
            }
        }

        public void CreateNew()
        {
            ShowEdit();
        }

        public async void KeyPressed(ActionExecutionContext context)
        {
            await TryCreateNewNotebook(context);
        }

        private async Task TryCreateNewNotebook(ActionExecutionContext context)
        {
            if (string.IsNullOrEmpty(NewNotebookName)) return;

            var keyArgs = context.EventArgs as KeyEventArgs;

            if (keyArgs?.Key == Key.Enter)
            {
                await CreateNotebook();

                NewNotebookName = null;

                ShowReadOnly();

                MessageBus.Publish(new NotebookCreated());
            }

            if (keyArgs?.Key == Key.Escape)
            {
                ShowReadOnly();

                NewNotebookName = null;
            }
        }

        private async Task CreateNotebook()
        {
            var rootDirectory = await _rootDirectory.GetRootDirectory();

            var newNotebookDirectory = rootDirectory.CreateSubdirectory(NewNotebookName);

            _notebook.CreateNotebook(newNotebookDirectory);
        }

        private void ShowReadOnly()
        {
            ShowReadOnlyFieldsVisibility = Visibility.Visible;
            ShowEditFieldsVisibility = Visibility.Collapsed;
        }

        private void ShowEdit()
        {
            ShowReadOnlyFieldsVisibility = Visibility.Collapsed;
            ShowEditFieldsVisibility = Visibility.Visible;
        }
    }
}

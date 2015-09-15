using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Delbert.Actors.Facades.Abstract;
using Delbert.Components.Notebook;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Section
{
    public class AddSectionViewModel : ScreenViewModel, IAddSectionViewModel
    {
        private readonly ISectionFacade _section;
        private Visibility _showReadOnlyFieldsVisibility;
        private Visibility _showEditFieldsVisibility;
        private string _newSectionName;
        private NotebookDto _selectedNotebook;

        public AddSectionViewModel(ISectionFacade section, IIoC ioc) : base(ioc)
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            _section = section;

            MessageBus.Subscribe<NotebookSelected>(OnNotebookSelected);
        }

        private void OnNotebookSelected(NotebookSelected message)
        {
            _selectedNotebook = message.Notebook;
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

        public string NewSectionName
        {
            get { return _newSectionName; }
            set
            {
                if (value == _newSectionName) return;
                _newSectionName = value;
                NotifyOfPropertyChange(() => NewSectionName);
            }
        }

        public void CreateNew()
        {
            ShowEdit();
        }

        public void KeyPressed(ActionExecutionContext context)
        {
            TryCreateNewNotebook(context);
        }

        private void TryCreateNewNotebook(ActionExecutionContext context)
        {
            if (string.IsNullOrEmpty(NewSectionName) || _selectedNotebook == null || _selectedNotebook.Directory.Exists == false) return;

            var keyArgs = context.EventArgs as KeyEventArgs;

            if (keyArgs?.Key == Key.Enter)
            {
                CreateSection();

                NewSectionName = null;

                ShowReadOnly();

                MessageBus.Publish(new SectionCreated(_selectedNotebook));
            }
        }

        private void CreateSection()
        {
            var newSectionDirectory = _selectedNotebook.Directory.CreateSubdirectory(NewSectionName);

            _section.CreateSection(newSectionDirectory);
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

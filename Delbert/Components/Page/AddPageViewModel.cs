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
using Delbert.Model;

namespace Delbert.Components.Page
{
    public class AddPageViewModel : ScreenViewModel, IAddPageViewModel
    {
        private readonly IPageFacade _page;
        private Visibility _showReadOnlyFieldsVisibility;
        private Visibility _showEditFieldsVisibility;
        private string _newPageName;
        private SectionDto _selectedSection;

        public AddPageViewModel(IPageFacade page, IIoC ioc) : base(ioc)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            _page = page;

            MessageBus.Subscribe<SectionSelected>(OnSectionSelected);
        }
        
        private void OnSectionSelected(SectionSelected message)
        {
            _selectedSection = message.Section;
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

        public string NewPageName
        {
            get { return _newPageName; }
            set
            {
                if (value == _newPageName) return;
                _newPageName = value;
                NotifyOfPropertyChange(() => NewPageName);
            }
        }

        public void CreateNew()
        {
            ShowEdit();
        }

        public void KeyPressed(ActionExecutionContext context)
        {
            TryCreateNewPage(context);
        }

        private void TryCreateNewPage(ActionExecutionContext context)
        {
            if (string.IsNullOrEmpty(NewPageName) || _selectedSection == null || _selectedSection.Directory.Exists == false) return;

            var keyArgs = context.EventArgs as KeyEventArgs;

            if (keyArgs?.Key == Key.Enter)
            {
                CreatePage();

                NewPageName = null;

                ShowReadOnly();

                MessageBus.Publish(new PageCreated(_selectedSection));
            }

            if (keyArgs?.Key == Key.Escape)
            {
                ShowReadOnly();
                NewPageName = null;
            }
        }

        private void CreatePage()
        {
            var filePath = $"{_selectedSection.Directory.FullName}\\{NewPageName}.txt";
            var newPageFile = new FileInfo(filePath);

            _page.CreatePage(newPageFile);
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

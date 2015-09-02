using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Delbert.Infrastructure.Abstract;

namespace Delbert.Components.Notebook
{
    public sealed class AddNotebookViewModel : ScreenViewModel, IAddNotebookViewModel
    {
        private Visibility _showReadOnlyFieldsVisibility;
        private Visibility _showEditFieldsVisibility;

        public AddNotebookViewModel(IIoC ioc) : base(ioc)
        {

        }

        protected override void OnActivate()
        {
            ShowReadOnlyFieldsVisibility = Visibility.Visible;
            ShowEditFieldsVisibility = Visibility.Collapsed;
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

        public void CreateNew()
        {
            ShowReadOnlyFieldsVisibility = Visibility.Collapsed;
            ShowEditFieldsVisibility = Visibility.Visible;
        }

        public void KeyPressed(object sender, object args)
        {
            
        }
    }
}

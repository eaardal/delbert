﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Delbert.Actors;
using Delbert.Actors.Facades.Abstract;
using Delbert.Infrastructure;
using Delbert.Infrastructure.Abstract;
using Delbert.Messages;
using Delbert.Model;

namespace Delbert.Components.Section
{
    class ListSectionsViewModel : ScreenViewModel, IListSectionsViewModel
    {
        private readonly INotebookFacade _notebook;

        public ListSectionsViewModel(INotebookFacade notebook, IIoC ioc) : base(ioc)
        {
            if (notebook == null) throw new ArgumentNullException(nameof(notebook));
            _notebook = notebook;

            Sections = new ItemChangeAwareObservableCollection<SectionDto>();

            MessageBus.Subscribe<NotebookSelected>(async msg => await OnNotebookSelected(msg));
            MessageBus.Subscribe<SectionCreated>(async msg => await OnSectionCreated(msg));
            MessageBus.Subscribe<PageCreated>(async msg => await OnPageCreated(msg));
        }

        private async Task OnPageCreated(PageCreated msg)
        {
            var notebooks = await _notebook.GetNotebooks();

            var selectedSection = Sections.Single(s => s.IsSelected);

            var updatedParentNotebook = notebooks.Single(n => n.Id == selectedSection.ParentNotebook.Id);

            await DoOnUiDispatcherAsync(() =>
            {
                Sections.Clear();
                updatedParentNotebook.Sections.ForEach(s =>
                {
                    if (s.Id == selectedSection.Id)
                    {
                        s.Select();
                    }
                    Sections.Add(s);
                });
            });
        }

        private async Task OnSectionCreated(SectionCreated msg)
        {
            var notebooks = await _notebook.GetNotebooks();

            var updatedParentNotebook =
                notebooks.SingleOrDefault(n => n.Directory.FullName == msg.NewSectionParentNotebook.Directory.FullName);

            await GetSections(updatedParentNotebook);
        }

        public ItemChangeAwareObservableCollection<SectionDto> Sections { get; }

        private async Task OnNotebookSelected(NotebookSelected msg)
        {
            try
            {
                await GetSections(msg.Notebook);
            }
            catch (Exception ex)
            {
                Log.Msg(this, l => l.Error(ex));
            }
        }

        private async Task GetSections(NotebookDto notebook)
        {
            await DoOnUiDispatcherAsync(() =>
            {
                Sections.Clear();
                notebook.Sections.ForEach(s => Sections.Add(s));
            });
        }

        public void SectionSelected(SectionDto section)
        {
            if (section == null)
            {
                Log.Msg(this, l => l.Warning("Selected section was null"));
                return;
            }

            Sections.ForEach(s => s.Deselect());

            section.Select();

            MessageBus.Publish(new SectionSelected(section));
        }
    }
}

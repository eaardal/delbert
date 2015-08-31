using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Delbert.Infrastructure
{
    public sealed class ItemChangeAwareObservableCollection<T> : ObservableCollection<T>
            where T : INotifyPropertyChanged
    {
        public delegate void ItemChangedEventHandler(object sender, PropertyChangedEventArgs args);

        public event ItemChangedEventHandler ItemChanged;

        public ItemChangeAwareObservableCollection()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        public ItemChangeAwareObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems)
            {
                this.Add(item);
            }
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
            base.OnCollectionChanged(args);

            ItemChanged?.Invoke(sender, e);
        }
    }
}

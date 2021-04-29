using System;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace SeeShellsV2.Utilities
{
    public class ObservableSortedList<T> : ICollection<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private List<T> data;
        private IComparer<T> comparer;

        public ObservableSortedList()
        {
            data = new List<T>();
        }

        public ObservableSortedList(IComparer<T> comparer)
        {
            data = new List<T>();
            this.comparer = comparer;
        }

        public SynchronizationContext SynchronizationContext => _synchronizationContext;

        public int Count { get => data.Count; }

        public bool IsReadOnly { get => false; }

        public IComparer<T> Comparer
        {
            get => comparer;
            set
            {
                comparer = value;
                data = data.OrderBy(t => t, comparer).ToList();

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnPropertyChanged(new PropertyChangedEventArgs("Comparer"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            int idx = data.BinarySearch(item, comparer);

            idx = idx >= 0 ? idx + 1 : ~idx;

            data.Insert(idx, item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, idx));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                int idx = data.BinarySearch(item, comparer);

                idx = idx >= 0 ? idx + 1 : ~idx;

                data.Insert(idx, item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        public void Clear()
        {
            data.Clear();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        }

        public bool Contains(T item)
        {
            return data.BinarySearch(item, comparer) >= 0;
        }

        public void CopyTo(T[] arr, int l)
        {
            foreach (T element in data)
                arr[l++] = element;
        }

        public bool Remove(T item)
        {
            int idx = data.BinarySearch(item, comparer);

            if (idx >= 0)
            {
                data.RemoveAt(idx);

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, idx));
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            }

            return idx >= 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the CollectionChanged event on the current thread
                RaiseCollectionChanged(e);
            }
            else
            {
                // Raises the CollectionChanged event on the creator thread
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(e);
            }
            else
            {
                // Raises the PropertyChanged event on the creator thread
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            CollectionChanged?.Invoke(this, (NotifyCollectionChangedEventArgs)param);
        }

        private void RaisePropertyChanged(object param)
        {
            PropertyChanged?.Invoke(this, (PropertyChangedEventArgs)param);
        }
    }
}

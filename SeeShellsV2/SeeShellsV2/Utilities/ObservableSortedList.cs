using System;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Windows;
using System.Collections.Generic;
using System.Collections.Specialized;

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

        public int Count { get => data.Count; }

        public bool IsReadOnly { get => false; }

        public IComparer<T> Comparer
        {
            get => comparer;
            set
            {
                comparer = value;
                data = data.OrderBy(t => t, comparer).ToList();

                Application.Current.Dispatcher.BeginInvoke(
                    new Action<ObservableSortedList<T>>((sender) => {
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Comparer"));
                    }),
                    this
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            int idx = data.BinarySearch(item, comparer);

            idx = idx >= 0 ? idx + 1 : ~idx;

            data.Insert(idx, item);

            Application.Current.Dispatcher.BeginInvoke(
                new Action<ObservableSortedList<T>>((sender) => {
                    sender.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, idx));
                    sender.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                }),
                this
            );
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                int idx = data.BinarySearch(item, comparer);

                idx = idx >= 0 ? idx + 1 : ~idx;

                data.Insert(idx, item);
            }

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new Action<ObservableSortedList<T>>((sender) => {
                    sender.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    sender.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                }),
                this
            );
        }

        public void Clear()
        {
            data.Clear();
            Application.Current.Dispatcher.BeginInvoke(
                new Action<ObservableSortedList<T>>((sender) => {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                }),
                this
            );
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

                Application.Current.Dispatcher.BeginInvoke(
                    new Action<ObservableSortedList<T>>((sender) => {
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, idx));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                    }),
                    this
                );
            }

            return idx >= 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

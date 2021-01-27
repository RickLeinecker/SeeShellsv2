using System.ComponentModel;
using System.Linq;
using System.Collections;
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
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Comparer"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(T item)
        {
            int idx = data.BinarySearch(item, comparer);

            idx = idx >= 0 ? idx + 1 : ~idx;

            data.Insert(idx, item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, idx));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
        }

        public void Clear()
        {
            data.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
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
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, idx));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
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

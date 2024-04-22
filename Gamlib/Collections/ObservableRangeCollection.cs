using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Gamlib.Collections
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        public ObservableRangeCollection() : base()
        {
        }

        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        public void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (notificationMode != NotifyCollectionChangedAction.Add &&
                notificationMode != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationMode));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            var startIndex = Count;

            var itemsAdded = AddArrangeCore(collection);

            if (!itemsAdded)
                return;

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Reset);

                return;
            }

            var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);

            RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Add, changedItems, startIndex);
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            var changedItems = collection is List<T> list ? list : new List<T>(collection);

            foreach (var changedItem in changedItems)
            {
                Items.Remove(changedItem);
            }


            if (changedItems.Count == 0)
                return;

            RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Remove, changedItems, 0);
        }

        public void Replace(T item) => ReplaceRange(new T[] { item });

        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CheckReentrancy();

            var previouslyEmpty = Items.Count == 0;

            Items.Clear();

            AddArrangeCore(collection);

            var currentlyEmpty = Items.Count == 0;

            if (previouslyEmpty && currentlyEmpty)
                return;

            RaiseChangeNotificationEvents(NotifyCollectionChangedAction.Reset);
        }

        private bool AddArrangeCore(IEnumerable<T> collection)
        {
            var itemAdded = false;
            foreach (var item in collection)
            {
                Items.Add(item);
                itemAdded = true;
            }

            return itemAdded;
        }

        private void RaiseChangeNotificationEvents(NotifyCollectionChangedAction action, IList changedItems = null, int startingIndex = -1)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            if (changedItems is null)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));
            else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItems, startingIndex));
        }
    }
}


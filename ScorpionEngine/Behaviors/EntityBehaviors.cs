using System.Collections;
using System.Collections.Generic;

namespace ScorpionEngine.Behaviors
{
    public class EntityBehaviors : IEnumerable, IList<IBehavior>
    {
        public List<IBehavior> _items = new List<IBehavior>();


        #region Props
        public IBehavior this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly { get; } = false;
        #endregion


        #region Public Methods
        public void Add(IBehavior item)
        {
            _items.Add(item);
        }


        public void Clear()
        {
            _items.Clear();
        }


        public bool Contains(IBehavior item)
        {
            return _items.Contains(item);
        }


        public void CopyTo(IBehavior[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }


        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }


        public int IndexOf(IBehavior item)
        {
            return _items.IndexOf(item);
        }


        public void Insert(int index, IBehavior item)
        {
            _items.Insert(index, item);
        }


        public bool Remove(IBehavior item)
        {
            return _items.Remove(item);
        }


        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }


        IEnumerator<IBehavior> IEnumerable<IBehavior>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        #endregion
    }
}

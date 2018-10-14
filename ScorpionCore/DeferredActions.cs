using System;
using System.Collections;
using System.Collections.Generic;

namespace ScorpionCore
{
    public class DeferredActions : IEnumerable, IList<Action>
    {
        private List<Action> _actions = new List<Action>();


        #region Props
        public Action this[int index]
        {
            get
            {
                return _actions[index];
            }
            set
            {
                _actions[index] = value;
            }
        }

        public int Count => _actions.Count;

        public bool IsReadOnly { get; }
        #endregion


        #region Public Methods
        public void ExecuteAll()
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                _actions[i]();

                //Destroy the action
                Remove(_actions[i]);
                i--;
            }
        }


        public void Add(Action item)
        {
            _actions.Add(item);
        }


        public void Clear()
        {
            _actions.Clear();
        }


        public bool Contains(Action item)
        {
            return _actions.Contains(item);
        }


        public void CopyTo(Action[] array, int arrayIndex)
        {
            _actions.CopyTo(array, arrayIndex);
        }


        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                yield return _actions[i];
            }
        }


        public int IndexOf(Action item)
        {
            return _actions.IndexOf(item);
        }


        public void Insert(int index, Action item)
        {
            _actions.Insert(index, item);
        }


        public bool Remove(Action item)
        {
            return _actions.Remove(item);
        }


        public void RemoveAt(int index)
        {
            _actions.RemoveAt(index);
        }


        IEnumerator<Action> IEnumerable<Action>.GetEnumerator()
        {
            return _actions.GetEnumerator();
        }
        #endregion
    }
}

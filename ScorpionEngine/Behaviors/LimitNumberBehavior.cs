using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Behaviors
{
    public class LimitNumberBehavior : IBehavior
    {
        private Func<float> _getValue;
        private Action<float> _setLimit;
        private float _limitValue;


        /// <summary>
        /// Creates a new instance of <see cref="LimitNumberBehavior"/>.
        /// </summary>
        /// <param name="getValue">Gets the value to watch that might exceed the given <paramref name="limitValue"/>.</param>
        /// <param name="setLimit">Sets the limit to the given value sent in through the parameter.</param>
        /// <param name="limitValue">The limit value.  Positive will check if greater than and negative will check less than.</param>
        public LimitNumberBehavior(Func<float> getValue, Action<float> setLimit, float limitValue)
        {
            _getValue = getValue;
            _setLimit = setLimit;
            _limitValue = limitValue;
        }


        public void Update(EngineTime engineTime)
        {
            var currentValue = _getValue();

            if(_limitValue > 0 && currentValue > _limitValue)
            {
                _setLimit(_limitValue);
            }
            else if (_limitValue < 0 && currentValue < _limitValue)
            {
                _setLimit(_limitValue);
            }
        }
    }
}

using System;

namespace ScorpionEngine.Behaviors
{
    public class LimitNumberBehavior : Behavior
    {
        private Func<float> _getValue;
        private Action<float> _setLimit;


        #region Constructors
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
            LimitValue = limitValue;
            Name = nameof(LimitNumberBehavior);
            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Props
        public float LimitValue { get; set; }
        #endregion


        #region Public Methods
        public void UpdateAction(EngineTime engineTime)
        {
            var currentValue = _getValue();

            if(LimitValue > 0 && currentValue > LimitValue)
            {
                _setLimit(LimitValue);
            }
            else if (LimitValue < 0 && currentValue < LimitValue)
            {
                _setLimit(LimitValue);
            }
        }
        #endregion
    }
}

using System;

namespace ScorpionEngine.Behaviors
{
    public class LimitNumberBehavior : Behavior
    {
        private readonly Func<float> _getValue;
        private readonly Action<float> _setLimit;


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="LimitNumberBehavior"/>.
        /// </summary>
        /// <param name="getValue">Gets the value to watch that might exceed the given <paramref name="limitValue"/>.</param>
        /// <param name="setLimit">Sets the limit to the given value sent in through the parameter.</param>
        /// <param name="limitValue">The limit value.  Positive will check if greater than and negative will check less than.</param>
        /// <param name="name">The name of the behavior.</param>
        public LimitNumberBehavior(Func<float> getValue, Action<float> setLimit, float limitValue, string name = nameof(LimitNumberBehavior))
        {
            _getValue = getValue;
            _setLimit = setLimit;
            LimitValue = limitValue;
            Name = name;
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

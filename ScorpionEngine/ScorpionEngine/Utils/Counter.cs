using System;

namespace ScorpionEngine.Utils
{
    /// <summary>
    /// Keeps counts an arbitrary number by a set amount in a set direction and triggers minimum and maximum reached events.
    /// </summary>
    public class Counter
    {
        #region Events
        /// <summary>
        /// Occurs when the count has reached its maximum.
        /// </summary>
        public event EventHandler MaxReachedWhenIncrementing;

        /// <summary>
        /// Occurs when the count has reached its minimum.
        /// </summary>
        public event EventHandler MinReachedWhenDecrementing;
        #endregion

        #region Fields
        private int _min;
        private int _max;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of Counter. The min cannot be greater then or equal to the max. If the min is higher then max or the max is less then the min, an ArgumentOutOfException will be thrown.
        /// </summary>
        /// <param name="min">The minimum setting of the counter that the MinReachedWhenDecrementing event will be invoked.</param>
        /// <param name="max">The maximum setting of the counter to inoke the MaxReachedWhenIncrementing event will be invoked.</param>
        /// <param name="countAmount">The amount to increment or decrement the counter value when the Count method is called.</param>
        /// <param name="value">The value to start the counter at.  If larger or equal then max, then value will be set to 0.</param>
        public Counter(int min, int max, int countAmount, int value = 0)
        {
            Value = value;

            //Make sure that the min is then the max
            if(min >= max)
                throw new ArgumentOutOfRangeException(@"The min cannot be greater then max and max cannot be less then min.");

            //Set the minimum
            _min = min;

            //Set the max
            _max = max;

            //Set the count amount
            CountAmount = countAmount;

            //Default the reset mode to auto
            ResetMode = ResetType.Auto;
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets the minmum amount that the counter will have to reach to invoke the MinReachedWhenDecrementing event.
        /// </summary>
        public int Min
        {
            get { return _min; }
            set
            {
                //Make sure that the min is then the max
                if (value >= _max)
                    throw new ArgumentOutOfRangeException(@"The min cannot be greater then max and max cannot be less then min.");
            }
        }

        /// <summary>
        /// Gets the maximum amount that the counter will have to reach to invoke the MaxReachedWhenIncrementing event.
        /// </summary>
        public int Max
        {
            get
            {
                return _max;
            }
            set
            {
                //Make sure that the min is then the max
                if (value >= _min)
                    throw new ArgumentOutOfRangeException(@"The min cannot be greater then max and max cannot be less then min.");
            }
        }

        /// <summary>
        /// Gets or sets the count amount.
        /// </summary>
        public int CountAmount { get; set; }

        /// <summary>
        /// Gets or sets the reset mode.  If the mode is set to manual, the Reset method is the only way to reset the value back to 0.
        /// </summary>
        public ResetType ResetMode { get; set; }

        /// <summary>
        /// Gets or sets the direction to count.
        /// </summary>
        public CountType CountDirection { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Increment or decrement the value by the CountAmount property value in the direction of the CountDirection property.
        /// </summary>
        public void Count()
        {
            //Increment or decrement the value
            switch (CountDirection)
            {
                case CountType.Increment:
                    //Count the value
                    Value += CountAmount;

                    //If the value is greater than or equal the max, invoke the MaxReachedWhenIncrementing event and set the value back to 0
                    if (Value < Max) return;

                    //Invoke the MaxReachedWhenIncrementing event
                    MaxReachedWhenIncrementing?.Invoke(this, new EventArgs());

                    //If the reset mode is set to auto, reset the value
                    if (ResetMode == ResetType.Auto && Value > Max)
                        Reset();
                    break;
                case CountType.Decrement:
                    //Count the value
                    Value -= CountAmount;

                    //If the value is less than or equal the max, invoke the MinReachedWhenDecrementing event and set the value back to 0
                    if (Value > Min) return;

                    //Invoke the MinReachedWhenDecrementing event
                    MinReachedWhenDecrementing?.Invoke(this, new EventArgs());

                    //If the reset mode is set to auto, reset the value
                    if (ResetMode == ResetType.Auto && Value < Min)
                        Reset();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Resets the value back to 0.
        /// </summary>
        public void Reset()
        {
            switch (CountDirection)
            {
                case CountType.Increment:
                    Value = Min;
                    break;
                case CountType.Decrement:
                    Value = Max;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Resets the counter to the given value.
        /// </summary>
        /// <param name="value">The value to set the counter at. If value is greater then max, then the value will just be set to the max.</param>
        public void Reset(int value)
        {
            switch (CountDirection)
            {
                case CountType.Increment:
                    //Clamp the value to the min if the incoming value is greater then min
                    Value = value < Min ? Min : value;
                    break;
                case CountType.Decrement:
                    //Clamp the value to the max if the incoming value is greater then max
                    Value = value > Max ? Max : value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}
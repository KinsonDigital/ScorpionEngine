// <copyright file="Counter.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Utils
{
    using System;

    /// <summary>
    /// Keeps counts an arbitrary number by a set amount in a set direction and triggers minimum and maximum reached events.
    /// </summary>
    public class Counter
    {
        private int min;
        private int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter"/> class.
        /// </summary>
        /// <param name="min">The minimum setting of the counter that the MinReachedWhenDecrementing event will be invoked.</param>
        /// <param name="max">The maximum setting of the counter to invoke the MaxReachedWhenIncrementing event will be invoked.</param>
        /// <param name="countAmount">The amount to increment or decrement the counter value when the Count method is called.</param>
        /// <param name="value">The value to start the counter at.  If larger or equal then max, then value will be set to 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the min is higher then max or the max is less then the min, an ArgumentOutOfException will be thrown.</exception>
        public Counter(int min, int max, int countAmount, int value = 0)
        {
            Value = value;

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Parameter must be less than the max.");
            }

            this.min = min;
            this.max = max;

            // Set the count amount
            CountAmount = countAmount;
        }

        /// <summary>
        /// Occurs when the count has reached its maximum.
        /// </summary>
        public event EventHandler MaxReachedWhenIncrementing;

        /// <summary>
        /// Occurs when the count has reached its minimum.
        /// </summary>
        public event EventHandler MinReachedWhenDecrementing;

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets or sets the minimum amount that the counter will have to reach to invoke the <see cref="MinReachedWhenDecrementing"/> event.
        /// </summary>
        public int Min
        {
            get => this.min;
            set
            {
                if (value > this.max)
                {
                    throw new Exception($"The min value of {value} cannot be greater than max value of {this.max}.");
                }

                this.min = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum amount that the counter will have to reach to invoke the <see cref="MaxReachedWhenIncrementing"/> event.
        /// </summary>
        public int Max
        {
            get => this.max;
            set
            {
                // Make sure that the min is then the max
                if (value < this.min)
                {
                    throw new Exception($"The max value of {value} cannot be less than min value of {this.min}.");
                }

                this.max = value;
            }
        }

        /// <summary>
        /// Gets or sets the count amount.
        /// </summary>
        public int CountAmount { get; set; }

        /// <summary>
        /// Gets or sets the reset mode.  If the mode is set to manual, you must manually reset the counter using the <see cref="Reset"/>() method.
        /// </summary>
        public ResetType ResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Gets or sets the direction to count. Directions are either counting up or down.
        /// </summary>
        public CountType CountDirection { get; set; }

        /// <summary>
        /// Increment or decrement the value by the <see cref="CountAmount"/> property value in the direction
        /// of the <see cref="CountDirection"/> property value.
        /// </summary>
        public void Count()
        {
            // Increment or decrement the value
            switch (CountDirection)
            {
                case CountType.Increment:
                    // Count the value
                    Value += CountAmount;

                    // If the value is greater than or equal the max, invoke the MaxReachedWhenIncrementing event and set the value back to 0
                    if (Value < Max)
                    {
                        return;
                    }

                    // Invoke the MaxReachedWhenIncrementing event
                    MaxReachedWhenIncrementing?.Invoke(this, new EventArgs());

                    // If the reset mode is set to auto, reset the value
                    if (ResetMode == ResetType.Auto && Value > Max)
                    {
                        Reset();
                    }

                    break;
                case CountType.Decrement:
                    // Count the value
                    Value -= CountAmount;

                    // If the value is less than or equal the max,
                    // invoke the MinReachedWhenDecrementing event and set the value back to 0
                    if (Value > Min)
                    {
                        return;
                    }

                    // Invoke the MinReachedWhenDecrementing event
                    MinReachedWhenDecrementing?.Invoke(this, new EventArgs());

                    // If the reset mode is set to auto, reset the value
                    if (ResetMode == ResetType.Auto && Value < Min)
                    {
                        Reset();
                    }

                    break;
                default:
                    throw new Exception($"The {nameof(CountDirection)} is set to an invalid enumeration value.");
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
                    throw new Exception($"The {nameof(CountDirection)} is set to an invalid enumeration value.");
            }
        }
    }
}

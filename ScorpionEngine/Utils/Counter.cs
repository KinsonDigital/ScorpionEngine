// <copyright file="Counter.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Utils
{
    using System;

    /// <summary>
    /// Increments or decrements a value and invokes events if the value has reached
    /// a particular limit.
    /// </summary>
    public class Counter : ICounter
    {
        private int min = 1;
        private int max = 10;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? MaxReachedWhenIncrementing;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? MinReachedWhenDecrementing;

        /// <inheritdoc/>
        public int Value { get; internal set; }

        /// <inheritdoc/>
        public int IncrementAmount { get; set; } = 1;

        /// <inheritdoc/>
        public int DecrementAmount { get; set; } = 1;

        /// <inheritdoc/>
        public int Min
        {
            get => this.min;
            set => this.min = value > this.max ? this.max : value;
        }

        /// <inheritdoc/>
        public int Max
        {
            get => this.max;
            set => this.max = value < this.min ? this.min : value;
        }

        /// <inheritdoc/>
        public CountType CountDirection { get; set; }

        /// <inheritdoc/>
        public ResetType ResetMode { get; set; } = ResetType.Auto;

        /// <inheritdoc/>
        public void Count()
        {
            // Increment or decrement the value
            switch (CountDirection)
            {
                case CountType.Increment:
                    // Count the value
                    Value += IncrementAmount;

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
                    Value -= DecrementAmount;

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

        /// <inheritdoc/>
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

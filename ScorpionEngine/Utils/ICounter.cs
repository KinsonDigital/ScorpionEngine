// <copyright file="ICounter.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Utils
{
    using System;

    /// <summary>
    /// Increments or decrements a value and invokes events if the value has reached
    /// a particular limit.
    /// </summary>
    public interface ICounter
    {
        /// <summary>
        /// Occurs when the count has reached its maximum.
        /// </summary>
        event EventHandler<EventArgs>? MaxReachedWhenIncrementing;

        /// <summary>
        /// Occurs when the count has reached its minimum.
        /// </summary>
        event EventHandler<EventArgs>? MinReachedWhenDecrementing;

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        int Value { get; }

        /// <summary>
        /// Gets or sets the amount to increment by.
        /// </summary>
        int IncrementAmount { get; set; }

        /// <summary>
        /// Gets or sets the amount ot decrement by.
        /// </summary>
        int DecrementAmount { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount that the counter will have to reach to invoke the <see cref="MaxReachedWhenIncrementing"/> event.
        /// </summary>
        int Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum amount that the counter will have to reach to invoke the <see cref="MinReachedWhenDecrementing"/> event.
        /// </summary>
        int Min { get; set; }

        /// <summary>
        /// Gets or sets the direction to count.
        /// </summary>
        CountType CountDirection { get; set; }

        /// <summary>
        /// Gets or sets the reset mode.  If the mode is set to manual, you must manually reset the counter using the <see cref="Reset"/>() method.
        /// </summary>
        ResetType ResetMode { get; set; }

        /// <summary>
        /// Increment or decrement the <see cref="Value"/> by the <see cref="IncrementAmount"/>
        /// or <see cref="DecrementAmount"/> properties in the direction set by the
        /// <see cref="CountDirection"/> property value.
        /// </summary>
        void Count();

        /// <summary>
        /// Resets the <see cref="Value"/> back to 0.
        /// </summary>
        void Reset();
    }
}

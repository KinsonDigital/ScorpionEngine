// <copyright file="LimitNumberBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;
    using Raptor;

    /// <summary>
    /// Limits a number to a set positive or negative value.
    /// </summary>
    public class LimitNumberBehavior : Behavior
    {
        private readonly Func<float> _getValue;
        private readonly Action<float> _setLimit;

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

        /// <summary>
        /// Gets or sets the number to limit the number to.
        /// </summary>
        public float LimitValue { get; set; }

        /// <summary>
        /// Then method to run that creates the number limiting behavior.  Executed when the <see cref="Behavior.Update(EngineTime)"/>
        /// is executed.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        private void UpdateAction(EngineTime engineTime)
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
    }
}

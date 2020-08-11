﻿// <copyright file="Behavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;
    using Raptor;

    /// <summary>
    /// Represents a custom set of behavior to execute.
    /// </summary>
    public abstract class Behavior : IBehavior
    {
        #region Private Fields
        private Action<EngineTime> _behaviorAction;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the <see cref="Behavior"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Enables or disables the <see cref="Behavior"/>.  Default value is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the behavior set by the <see cref="SetUpdateAction(Action{EngineTime})"/> method.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public void Update(EngineTime engineTime)
        {
            if (_behaviorAction == null || !Enabled)
                return;

            _behaviorAction(engineTime);
        }


        /// <summary>
        /// Sets the action that sets the behavior of this object.
        /// </summary>
        /// <param name="action">The behavior that will be executed when the <see cref="Update(EngineTime)"/> is invoked.</param>
        protected void SetUpdateAction(Action<EngineTime> action) => _behaviorAction = action;
        #endregion
    }
}
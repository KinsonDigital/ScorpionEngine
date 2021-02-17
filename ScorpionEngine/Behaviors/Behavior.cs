// <copyright file="Behavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;

    /// <summary>
    /// Represents a custom set of behavior to execute.
    /// </summary>
    public abstract class Behavior : IBehavior
    {
        private Action<GameTime> behaviorAction;

        /// <summary>
        /// Gets or sets the name of the <see cref="Behavior"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Enables or disables the <see cref="Behavior"/>.  Default value is true.
        /// </summary>
        public bool Enabled { get; set; } = true;
        public Guid ID { get; set; }

        /// <summary>
        /// Updates the behavior set by the <see cref="SetUpdateAction(Action{GameTime})"/> method.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public void Update(GameTime gameTime)
        {
            if (this.behaviorAction == null || !Enabled)
            {
                return;
            }

            this.behaviorAction(gameTime);
        }

        /// <summary>
        /// Sets the action that sets the behavior of this object.
        /// </summary>
        /// <param name="action">The behavior that will be executed when the <see cref="Update(GameTime)"/> is invoked.</param>
        protected void SetUpdateAction(Action<GameTime> action) => this.behaviorAction = action;
    }
}

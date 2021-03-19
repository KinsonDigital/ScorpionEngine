// <copyright file="KeyboardMovementBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Entities;
    using Raptor.Input;

    /// <summary>
    /// Moves a <see cref="Entity"/> forward in the direction it is facing with added rotation
    /// using the keyboard.
    /// </summary>
    /// <typeparam name="TEntity">The type of <see cref="Entity"/> to move.</typeparam>
    public class KeyboardMovementBehavior : IEntityBehavior
    {
        private readonly IGameInput<KeyCode, KeyboardState> keyboard;
        private readonly Func<GameTime, Vector2, Vector2> calcPosition;
        private KeyboardState currentKeyboardState;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMovementBehavior"/> class.
        /// </summary>
        /// <param name="keyboard">Manages keyboard input.</param>
        /// <param name="entity">The entity to add the behavior to.</param>
        /// <param name="calcPosition">Delegate used to calculate the new position of the entity.</param>
        public KeyboardMovementBehavior(
            IGameInput<KeyCode, KeyboardState> keyboard,
            IEntity entity,
            Func<GameTime, Vector2, Vector2> calcPosition)
        {
            this.keyboard = keyboard;

            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "The parameter must not be null.");
            }

            if (calcPosition is null)
            {
                throw new ArgumentNullException(nameof(calcPosition), "The parameter must not be null.");
            }

            Entity = entity;

            this.calcPosition = calcPosition;
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="Entity"/> to the left.
        /// </summary>
        public KeyCode MoveKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Entity"/> is moving.
        /// </summary>
        public bool IsMoving { get; private set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; } = true;

        /// <inheritdoc/>
        public Guid ID { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public IEntity Entity { get; private set; }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            this.currentKeyboardState = this.keyboard.GetState();

            IsMoving = this.currentKeyboardState.IsKeyDown(MoveKey);

            if (IsMoving)
            {
                Entity.Position = this.calcPosition(gameTime, Entity.Position);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true"/> to dispose of managed resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;
        }
    }
}

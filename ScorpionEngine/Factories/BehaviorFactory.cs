// <copyright file="BehaviorFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Factories
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using Raptor.Input;

    /// <summary>
    /// Creates entity behaviors.
    /// </summary>
    public static class BehaviorFactory
    {
        /// <summary>
        /// Creates a new entity behavior of the implementation type <see cref="KeyboardMovementBehavior"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to attach the behavior to.</typeparam>
        /// <param name="entity">The <see cref="IEntity"/> to attache the behavior to.</param>
        /// <param name="moveKey">The keyboard key to move the entity.</param>
        /// <param name="calcPosition">
        ///     The delegate used to calculate the movement of the entity when the
        ///     <see cref="KeyboardMovementBehavior.MoveKey"/> is in the down position.
        /// </param>
        /// <returns>The behavior of type <see cref="IEntityBehavior"/>.</returns>
        public static IEntityBehavior CreateKeyboardMovement<TEntity>(
            TEntity entity,
            KeyCode moveKey,
            Func<GameTime, Vector2, Vector2> calcPosition)
                where TEntity : class, IEntity
                    => new KeyboardMovementBehavior(new Keyboard(), entity, calcPosition)
                    {
                        MoveKey = moveKey,
                    };
    }
}

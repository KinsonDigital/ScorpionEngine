// <copyright file="IUpdatableObject.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    /// <summary>
    /// Provides the ability to update an item.
    /// </summary>
    public interface IUpdatableObject
    {
        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="gameTime">The time that has passed in the game.</param>
        void Update(GameTime gameTime);
    }
}

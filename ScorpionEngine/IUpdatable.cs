// <copyright file="IUpdatable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using Raptor;

namespace KDScorpionEngine
{
    /// <summary>
    /// Makes an object an updatable for the game engine.
    /// </summary>
    public interface IUpdatable
    {
        void Update(EngineTime engineTime);
    }
}

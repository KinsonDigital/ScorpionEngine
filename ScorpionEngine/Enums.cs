// <copyright file="Enums.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    /// <summary>
    /// Determines the type of the key behavior.
    /// </summary>
    public enum KeyBehaviorType
    {
        /// <summary>
        /// The key press event will continuously fire when the key is pressed down.
        /// </summary>
        KeyDownContinuous = 1,

        /// <summary>
        /// The key press event will be invoked only once on key press.
        /// </summary>
        OnceOnDown = 2,

        /// <summary>
        /// The key release event will be invoked only once on key release.
        /// </summary>
        OnceOnRelease = 3,

        /// <summary>
        /// The key press event will fire after a set time delay.
        /// </summary>
        OnKeyDownTimeDelay = 4,

        /// <summary>
        /// The key release event will fire after a set time delay.
        /// </summary>
        OnKeyReleaseTimeDelay = 5,

        /// <summary>
        /// The key release event will fire after any key has been released.
        /// </summary>
        OnAnyKeyPress = 6
    }

    /// <summary>
    /// Represents different directions of movement.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Up movement.
        /// </summary>
        Up = 1,

        /// <summary>
        /// Down movement.
        /// </summary>
        Down = 2,

        /// <summary>
        /// Left movement.
        /// </summary>
        Left = 3,

        /// <summary>
        /// Right movement.
        /// </summary>
        Right = 4,

        /// <summary>
        /// Up left movement.
        /// </summary>
        UpLeft = 5,

        /// <summary>
        /// Up right movement.
        /// </summary>
        UpRight = 6,

        /// <summary>
        /// Down left movement.
        /// </summary>
        DownLeft = 7,

        /// <summary>
        /// Down right movement.
        /// </summary>
        DownRight = 8,
    }

    /// <summary>
    /// Represents entities that are the given descriptions.
    /// </summary>
    public enum EntitiesThatAre 
    {
        /// <summary>
        /// Visible entities.
        /// </summary>
        Visible = 1,

        /// <summary>
        /// Hidden entities.
        /// </summary>
        Hidden = 2,

        /// <summary>
        /// Any kind of entity.
        /// </summary>
        Anything = 3
    }

    /// <summary>
    /// Represents two different ways to reset something.
    /// </summary>
    public enum ResetType
    {
        /// <summary>
        /// Represents a manual reset.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// Represents an automatic reset.
        /// </summary>
        Auto = 2
    }

    /// <summary>
    /// Represents the direction to count.
    /// </summary>
    public enum CountType
    {
        /// <summary>
        /// Count up towards the positive direction.
        /// </summary>
        Increment,

        /// <summary>
        /// Count down towards the negative direction.
        /// </summary>
        Decrement
    }
}

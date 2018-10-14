namespace ScorpionEngine
{
    /// <summary>
    /// Represents different ways that an <see cref="IScene"/> should run.
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// This makes an <see cref="IScene"/> run continously.  Used for standard game running through frames.
        /// </summary>
        Continuous = 1,

        /// <summary>
        /// This gives fine control to run the game a set amount of frames at a time.
        /// </summary>
        FrameStack = 2
    }

    /// <summary>
    /// The different types of origins to set.
    /// </summary>
    public enum OriginType
    {
        /// <summary>
        /// Origin is in the center of the texture.
        /// </summary>
        Center = 1,
        /// <summary>
        /// Origin is at the top left corner of the texture.
        /// </summary>
        TopLeft = 2,
        /// <summary>
        /// Origin is at the top right corner of the texture.
        /// </summary>
        TopRight = 3,
        /// <summary>
        /// Origin is at the bottom left corner of the texture.
        /// </summary>
        BottomLeft = 4,
        /// <summary>
        /// Origin is at the bottom right corner of the texture.
        /// </summary>
        BottomRight = 5
    }

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
        OnceOnPress = 2,
        /// <summary>
        /// The key release event will be invoked only once on key release.
        /// </summary>
        OnceOnRelease = 3,
        /// <summary>
        /// The key press event will fire after a set time delay.
        /// </summary>
        OnKeyPressedTimeDelay = 4,
        /// <summary>
        /// The key release event will fire after a set time delay.
        /// </summary>
        OnKeyReleaseTimeDelay = 5,
        /// <summary>
        /// The key release event will fire after any key has been released.
        /// </summary>
        OnAnyKeyPress = 6,
        /// <summary>
        /// The key release event will fire after any key has been pressed.
        /// </summary>
        OnAnyKeyRelease = 7
    }

    /// <summary>
    /// The source that the graphics of a GameObject will be drawn from.
    /// </summary>
    public enum GraphicContentSource
    {
        /// <summary>
        /// A standard texture where the entire texture is used in the game object.
        /// </summary>
        Standard = 1,
        /// <summary>
        /// A texture atlas where only part of the texture is used that is loaded in the engine class.
        /// </summary>
        Atlas = 2
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
    /// Represents the 2 different ways an entity can rotate.
    /// </summary>
    public enum RotationDirection 
    {
        /// <summary>
        /// Rotates clockwise.
        /// </summary>
        Clockwise = 1,
        /// <summary>
        /// Rotates counter clockwise.
        /// </summary>
        CounterClockwise  = 2
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

using ScorpionCore;

using ScorpionEngine.Input;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Represents a moveable game object that can be controlled via keyboard or mouse input.
    /// </summary>
    public class ControllableObject : MovableObject
    {
        #region Fields
        private KeyBehavior _stopMovementOnKeyRelease;//Will fire when any key is released
        private KeyBehavior _stopRotationOnKeyRelease;//Will fire when any key is released
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of an game object.
        /// </summary>
        /// <param name="textureName">The textureName of the game object.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public ControllableObject(Vector[] polyVertices, IKeyboard keyboard)
            : base(polyVertices)
        {
            CreateKeyBehaviors(keyboard);
        }


        /// <summary>
        /// Creates a new instance of an game object.
        /// </summary>
        /// <param name="textureName">The textureName of the game object.</param>
        /// <param name="location">Sets the location of the game object in the game world.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public ControllableObject(Vector[] polyVertices, Vector location, IKeyboard keyboard)
            : base(polyVertices, location)
        {
            CreateKeyBehaviors(keyboard);
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the move right key behavior.
        /// </summary>
        public KeyBehavior MoveRightKey { get; set; }

        /// <summary>
        /// Gets or sets the move right key behavior.
        /// </summary>
        public KeyBehavior MoveLeftKey { get; set; }

        /// <summary>
        /// Gets or sets the move right key behavior.
        /// </summary>
        public KeyBehavior MoveUpKey { get; set; }

        /// <summary>
        /// Gets or sets the move right key behavior.
        /// </summary>
        public KeyBehavior MoveDownKey { get; set; }

        /// <summary>
        /// Gets or sets the rotate clock wise key behavior.
        /// </summary>
        public KeyBehavior RotateCwKey { get; set; }

        /// <summary>
        /// Gets or sets the rotate counter clock wise key behavior.
        /// </summary>
        public KeyBehavior RotateCcwKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the controllable object should stop movement on key release.
        /// </summary>
        public bool StopMovementOnKeyRelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the controllable object should stop rotating on key release.
        /// </summary>
        public bool StopRotationOnKeyRelease { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Enables all movement of the game object with keyboard.
        /// </summary>
        public void EnableKeyboardMovement()
        {
            MoveLeftKey.Enabled = true;
            MoveRightKey.Enabled = true;
            MoveUpKey.Enabled = true;
            MoveDownKey.Enabled = true;
        }


        /// <summary>
        /// Disables all movement of the game object with keyboard.
        /// </summary>
        public void DisableKeyboardMovement()
        {
            MoveLeftKey.Enabled = false;
            MoveRightKey.Enabled = false;
            MoveUpKey.Enabled = false;
            MoveDownKey.Enabled = false;
        }


        /// <summary>
        /// Enables rotation of the game object using the set rotation key.
        /// </summary>
        public void EnableKeyboardRotation()
        {
            RotateCwKey.Enabled = true;
            RotateCcwKey.Enabled = true;
        }


        /// <summary>
        /// Disables rotation of the game object using the set rotation key.
        /// </summary>
        public void DisableKeyboardRotation()
        {
            RotateCwKey.Enabled = false;
            RotateCcwKey.Enabled = false;
        }
        #endregion


        #region Game Loop Methods
        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="engineTime">The time elapsed since last frame.</param>
        public override void OnUpdate(IEngineTiming engineTime)
        {
            _engineTime = engineTime;

            //Update all of the key behaviors
            MoveRightKey.Update(engineTime);
            MoveLeftKey.Update(engineTime);
            MoveUpKey.Update(engineTime);
            MoveDownKey.Update(engineTime);
            RotateCwKey.Update(engineTime);
            RotateCcwKey.Update(engineTime);
            _stopRotationOnKeyRelease.Update(engineTime);
            _stopMovementOnKeyRelease.Update(engineTime);

            base.OnUpdate(engineTime);
        }
        #endregion


        #region Events
        /// <summary>
        /// Will stop any movement on key release if the StopRotationOnKeyRelease setting is enabled.
        /// </summary>
        private void StopRotationOnKeyRelease_KeyUpEvent(object sender, KeyEventArgs e)
        {
            //If the stop movement on key release setting is enabled, stop the movement of the object
            if (StopRotationOnKeyRelease) StopRotation();
        }


        /// <summary>
        /// Will stop any movement on key release if the StopMovementOnKeyRelease setting is enabled.
        /// </summary>
        private void StopMovementOnKeyReleaseKeyUpEvent(object sender, KeyEventArgs e)
        {
            //If the stop movement on key release setting is enabled, stop the movement of the object
            if (StopMovementOnKeyRelease) StopMovement();
        }


        /// <summary>
        /// Rotates the game object clock wise.
        /// </summary>
        private void RotateCWKeyDownEvent(object sender, KeyEventArgs e)
        {
            RotateCW();
        }


        /// <summary>
        /// Rotates the game object counter clock wise.
        /// </summary>
        private void RotateCCWKeyDownEvent(object sender, KeyEventArgs e)
        {
            RotateCCW();
        }


        /// <summary>
        /// Moves the character to the right on the screen.
        /// </summary>
        private void MoveRightKeyDownEvent(object sender, KeyEventArgs e)
        {
            MoveRight();
        }


        /// <summary>
        /// Moves the character left on the screen.
        /// </summary>
        private void MoveLeftKeyDownEvent(object sender, KeyEventArgs e)
        {
            MoveLeft();
        }


        /// <summary>
        /// Moves the character up on the screen.
        /// </summary>
        private void MoveUpKeyDownEvent(object sender, KeyEventArgs e)
        {
            MoveUp();
        }


        /// <summary>
        /// Moves the character down on the screen.
        /// </summary>
        private void MoveDownKeyDownEvent(object sender, KeyEventArgs e)
        {
            MoveDown();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Creates all of the key behaviors.
        /// </summary>
        private void CreateKeyBehaviors(IKeyboard keyboard)
        {
            MoveRightKey = new KeyBehavior(keyboard, InputKeys.Right);
            MoveLeftKey = new KeyBehavior(keyboard, InputKeys.Left);
            MoveUpKey = new KeyBehavior(keyboard, InputKeys.Up);
            MoveDownKey = new KeyBehavior(keyboard, InputKeys.Down);
            RotateCwKey = new KeyBehavior(keyboard, InputKeys.D);
            RotateCcwKey = new KeyBehavior(keyboard, InputKeys.A);

            _stopMovementOnKeyRelease = new KeyBehavior(keyboard, true)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyRelease
            };

            _stopRotationOnKeyRelease = new KeyBehavior(keyboard, true)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyRelease
            };

            //Register the movement key down events.
            MoveRightKey.KeyDownEvent += MoveRightKeyDownEvent;
            MoveLeftKey.KeyDownEvent += MoveLeftKeyDownEvent;
            MoveUpKey.KeyDownEvent += MoveUpKeyDownEvent;
            MoveDownKey.KeyDownEvent += MoveDownKeyDownEvent;

            //Register the rotate key down events.
            RotateCwKey.KeyDownEvent += RotateCWKeyDownEvent;
            RotateCcwKey.KeyDownEvent += RotateCCWKeyDownEvent;

            //Register the any key release key behavior. This will be used for the 
            //stop movement and stop rotation functionality if enabled
            _stopMovementOnKeyRelease.KeyUpEvent += StopMovementOnKeyReleaseKeyUpEvent;
            _stopRotationOnKeyRelease.KeyUpEvent += StopRotationOnKeyRelease_KeyUpEvent;
        }
        #endregion
    }
}
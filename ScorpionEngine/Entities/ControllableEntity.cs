using ScorpionEngine.Behaviors;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Entities
{
    /// <summary>
    /// Represents a moveable game object that can be controlled via keyboard or mouse input.
    /// </summary>
    public class ControllableEntity : DynamicEntity
    {
        #region Fields
        private MovementBehavior _movementBehavior;
        private KeyBehavior _stopMovementOnKeyRelease;//Will fire when any key is released
        private KeyBehavior _stopRotationOnKeyRelease;//Will fire when any key is released
        #endregion


        #region Constructors
        public ControllableEntity(Texture texture, Vector position,bool isStaticBody = false) : base(texture, position, isStaticBody)
        {
            SetupBehaviors();
        }


        public ControllableEntity(Vector[] polyVertices, Vector position,bool isStaticBody = false) : base(polyVertices, position, isStaticBody)
        {
            SetupBehaviors();
        }


        public ControllableEntity(Texture texture, Vector[] polyVertices, Vector position,bool isStaticBody = false) : base(texture, polyVertices, position, isStaticBody)
        {
            SetupBehaviors();
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
        #endregion


        #region Game Loop Methods
        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="engineTime">The time elapsed since last frame.</param>
        public override void Update(EngineTime engineTime)
        {
            _engineTime = engineTime;

            base.Update(engineTime);
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
        #endregion


        #region Private Methods
        /// <summary>
        /// Creates all of the key behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            //_movementBehavior = new MovementBehavior(this, Speed);
            //Behaviors.Add(_movementBehavior);

            //TODO: Look into removing this and adding this to the movement behavior instead
            //_stopMovementOnKeyRelease = new KeyBehavior(true)
            //{
            //    BehaviorType = KeyBehaviorType.OnAnyKeyRelease
            //};

            //TODO: Look into removing this and adding this to the movement behavior instead
            //_stopRotationOnKeyRelease = new KeyBehavior(true)
            //{
            //    BehaviorType = KeyBehaviorType.OnAnyKeyRelease
            //};


            //Register the any key release key behavior. This will be used for the 
            //stop movement and stop rotation functionality if enabled
            //TODO: Look into removing this and adding this to the movement behavior instead
            _stopMovementOnKeyRelease.KeyUpEvent += StopMovementOnKeyReleaseKeyUpEvent;
            //TODO: Look into removing this and adding this to the movement behavior instead
            _stopRotationOnKeyRelease.KeyUpEvent += StopRotationOnKeyRelease_KeyUpEvent;
        }
        #endregion
    }
}
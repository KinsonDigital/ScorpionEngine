using System;
using System.Collections.Generic;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;
using ScorpionEngine.Utils;

namespace ScorpionEngine.Entities
{
    /// <summary>
    /// Represents a game object that can be moved around the screen.  
    /// This is just a game object with moving capability added on to it.
    /// </summary>
    public class DynamicEntity : Entity
    {
        #region Fields
        private Vector _facingDirection = new Vector(0, -1);
        private readonly Dictionary<Direction, bool> _linearMovementLocks = new Dictionary<Direction, bool>();//Holds the lock states for the 8 linear movements.  True means locked.
        private LimitNumberBehavior _moveRightVelocityMaxBehavior;
        private LimitNumberBehavior _moveLeftVelocityMaxBehavior;
        private LimitNumberBehavior _moveDownVelocityMaxBehavior;
        private LimitNumberBehavior _moveUpVelocityMaxBehavior;
        private LimitNumberBehavior _rotateCWVelocityMaxBehavior;
        private LimitNumberBehavior _rotateCCWVelocityMaxBehavior;
        private bool _stopMovement;
        private float _preInitAngle;
        private float _preInitLinearDeceleration;
        private float _preInitAngularDeceleration;
        private float _maxLinearSpeed;
        #endregion


        #region Constructors
        public DynamicEntity()
        {
            SetupMaxLinearBehaviors(10f);
            SetupMaxRotationBehaviors(10f);
        }


        public DynamicEntity(float friction = 0.2f) : base(friction: friction)
        {
            SetupMaxLinearBehaviors(40f);
            SetupMaxRotationBehaviors(40f);
        }


        public DynamicEntity(Vector position, float friction = 0.2f) : base(position, friction)
        {
            SetupMaxLinearBehaviors(40f);
            SetupMaxRotationBehaviors(40f);
        }


        public DynamicEntity(Texture texture, Vector position, float friction = 0.2f) : base(texture, position, friction)
        {
            SetupMaxLinearBehaviors(40f);
            SetupMaxRotationBehaviors(40f);
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="vertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public DynamicEntity(Vector[] vertices, Vector position, float friction = 0.2f) : base(vertices, position, friction)
        {
            SetupMaxLinearBehaviors(40f);
            SetupMaxRotationBehaviors(40f);
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="position">The location to draw the object.</param>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public DynamicEntity(Texture texture, Vector[] polyVertices, Vector position, float friction = 0.2f) : base(texture, polyVertices, position, friction)
        {
            SetupMaxLinearBehaviors(40f);
            SetupMaxRotationBehaviors(40f);
        }
        #endregion


        #region Properties
        public bool IsMoving
        {
            get
            {
                return Body.LinearVelocity != Vector.Zero || Body.AngularVelocity != 0;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if rotation should be locked.
        /// </summary>
        public bool RotationEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the angle in degrees.
        /// </summary>
        public float Angle
        {
            get => IsInitialized ? Body.Angle : _preInitAngle;
            set
            {
                if (IsInitialized)
                {
                    Body.Angle = value;
                }
                else
                {
                    _preInitAngle = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum linear movement speed. Positive and
        /// negative values behave the same.
        /// </summary>
        public float MaxLinearSpeed
        {
            get => _maxLinearSpeed;
            set
            {
                _maxLinearSpeed = value;
                SetBehaviorLimits(_maxLinearSpeed);
            }
        }

        public float MaxRotationSpeed
        {
            get => _rotateCWVelocityMaxBehavior.LimitValue;
            set
            {
                _rotateCWVelocityMaxBehavior.LimitValue = value.ForcePositive();
                _rotateCCWVelocityMaxBehavior.LimitValue = value.ForceNegative();
            }
        }

        /// <summary>
        /// Gets or sets the rotational speed of the object in degrees.
        /// </summary>
        public float RotateSpeed { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the speed in the X direction. Negative values will move the
        /// <see cref="DynamicEntity"/> left. Positive values move the <see cref="DynamicEntity"/>
        /// right.
        /// </summary>
        public float SpeedX { get; set; } = 0.25f;

        /// <summary>
        /// Gets or sets the speed in the Y direction. Negative values will move the
        /// <see cref="DynamicEntity"/> up. Positive values move the <see cref="DynamicEntity"/>
        /// down.
        /// </summary>
        public float SpeedY { get; set; } = 0.25f;

        /// <summary>
        /// Gets or sets the linear deceleration of the object. DEFAULT: 1
        /// </summary>
        public float LinearDeceleration
        {
            get => IsInitialized ? Body.InternalPhysicsBody.LinearDeceleration : _preInitLinearDeceleration;
            set
            {
                if (IsInitialized)
                {
                    Body.InternalPhysicsBody.LinearDeceleration = value;
                }
                else
                {
                    _preInitLinearDeceleration = value;
                }
            }
        }

        public float AngularDeceleration
        {
            get => IsInitialized ? Body.InternalPhysicsBody.AngularDeceleration : _preInitAngularDeceleration;
            set
            {
                if(IsInitialized)
                {
                    Body.InternalPhysicsBody.AngularDeceleration = value;
                }
                else
                {
                    _preInitAngularDeceleration = value;
                }
            }
        }
        #endregion


        #region Public Method
        /// <summary>
        /// Initializes the <see cref="DynamicEntity"/>.
        /// </summary>
        public override void Initialize()
        {
            if (!IsInitialized)
            {
                base.Initialize();

                Angle = _preInitAngle;
                LinearDeceleration = _preInitLinearDeceleration;
                AngularDeceleration = _preInitAngularDeceleration;
            }
        }


        /// <summary>
        /// Updates the moveable object.
        /// </summary>
        /// <param name="engineTime"></param>
        public override void Update(EngineTime engineTime)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            _facingDirection = Tools.RotateAround(new Vector(0, -1), Vector.Zero, Body.InternalPhysicsBody.Angle);

            ProcessMovementStop();

            base.Update(engineTime);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right using the <see cref="SpeedX"/> value.
        /// Behaves the same no matter if <see cref="SpeedX"/> is positive or negative.
        /// </summary>
        public void MoveRight()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForcePositive(), 0);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveRight(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed.ForcePositive(), 0);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left using the <see cref="SpeedX"/> value.
        /// Behaves the same no matter if <see cref="SpeedX"/> is positive or negative.
        /// </summary>
        public void MoveLeft()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForceNegative(), 0);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveLeft(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed.ForceNegative(), 0);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up using the <see cref="SpeedY"/> value.
        /// Behaves the same no matter if <see cref="SpeedY"/> is positive or negative.
        /// </summary>
        public void MoveUp()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, SpeedY.ForceNegative());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveUp(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, speed.ForceNegative());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the <see cref="SpeedY"/> value.
        /// Behaves the same no matter if <see cref="SpeedY"/> is positive or negative.
        /// </summary>
        public void MoveDown()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, SpeedY.ForcePositive());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveDown(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, speed);
        }


        public void MoveUpRight()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForcePositive(), SpeedY.ForceNegative());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveUpRight(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed, speed.ForceNegative());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the left.
        /// </summary>
        public void MoveUpLeft()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForceNegative(), SpeedY.ForceNegative());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveUpLeft(float speed)
        {
            speed = speed.ForceNegative();
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed, speed);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the right.
        /// </summary>
        public void MoveDownRight()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForcePositive(), SpeedY.ForcePositive());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveDownRight(float speed)
        {
            speed = speed.ForcePositive();
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed, speed);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the left.
        /// </summary>
        public void MoveDownLeft()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForceNegative(), SpeedY.ForcePositive());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveDownLeft(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed.ForceNegative(), speed.ForcePositive());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> based on the current <see cref="SpeedX"/>
        /// and <see cref="SpeedY"/> values. Positive and negative values of <see cref="SpeedX"/> and <see cref="SpeedY"/>
        /// determines the direction of travel on that axis.
        /// </summary>
        public void MoveAtSetSpeed()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX, SpeedY);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> at the currently set angle using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveAtSetAngle(float speed)
        {
            var directionToMove = new Vector(_facingDirection.X, _facingDirection.Y) * speed.ForcePositive();

            Body.InternalPhysicsBody.ApplyLinearImpulse(directionToMove.X, directionToMove.Y);
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise using the <see cref="RotateSpeed"/> value.
        /// Behaves the same no matter if the <see cref="RotateSpeed"/> value is positive or negative.
        /// </summary>
        public void RotateCW()
        {
            if(RotationEnabled)
                Body.InternalPhysicsBody.ApplyAngularImpulse(RotateSpeed.ForcePositive());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise using the given speed.
        /// </summary>
        /// <param name="speed">The speed to rotate the <see cref="DynamicEntity"/>. Positive and negative values behave the same.</param>
        public void RotateCW(float speed)
        {
            if (RotationEnabled)
                Body.InternalPhysicsBody.ApplyAngularImpulse(speed.ForcePositive());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise using the <see cref="RotateSpeed"/> value.
        /// Behaves the same no matter if the <see cref="RotateSpeed"/> value is positive or negative.
        /// </summary>
        public void RotateCCW()
        {
            if (RotationEnabled)
                Body.InternalPhysicsBody.ApplyAngularImpulse(RotateSpeed.ForceNegative());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise using the given speed.
        /// </summary>
        /// <param name="speed">The speed to rotate the <see cref="DynamicEntity"/>. Positive and negative values behave the same.</param>
        public void RotateCCW(float speed)
        {
            if (RotationEnabled)
                Body.InternalPhysicsBody.ApplyAngularImpulse(speed.ForceNegative());
        }


        /// <summary>
        /// Stops the movement of the object.
        /// </summary>
        public void StopMovement()
        {
            _stopMovement = true;
        }


        /// <summary>
        /// Stops the rotation of the object.
        /// </summary>
        public void StopRotation()
        {
            Body.InternalPhysicsBody.AngularVelocity = 0;
        }


        /// <summary>
        /// Starts process of stopping the movement of the <see cref="DynamicEntity"/> if it has been flagged to stop.
        /// </summary>
        private void ProcessMovementStop()
        {
            if (_stopMovement)
            {
                //If the body is still moving in the Y direction
                if (Body.InternalPhysicsBody.LinearVelocityY != 0)
                {
                    Body.InternalPhysicsBody.ApplyLinearImpulse(Body.InternalPhysicsBody.LinearVelocityX, Body.InternalPhysicsBody.LinearVelocityY * -1);
                }

                //If the body is still moving in the X direction
                if (Body.InternalPhysicsBody.LinearVelocityX != 0)
                {
                    Body.InternalPhysicsBody.ApplyLinearImpulse(Body.InternalPhysicsBody.LinearVelocityX * -1, Body.InternalPhysicsBody.LinearVelocityY);
                }

                //If the body has stopped moving, set the flag back to false
                if (Body.InternalPhysicsBody.LinearVelocityX == 0 && Body.InternalPhysicsBody.LinearVelocityY == 0 &&
                    Body.AngularVelocity == 0)
                    _stopMovement = false;
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up all of the max linear behaviors.
        /// </summary>
        /// <param name="maxSpeed">The maximum speed of the directions of movement.
        /// Must be a positive number. Negative numbers will be treated as positive</param>
        private void SetupMaxLinearBehaviors(float maxSpeed)
        {
            //Force the speed to a positive number if it is negative
            maxSpeed = maxSpeed.ForcePositive();

            _moveUpVelocityMaxBehavior = new LimitNumberBehavior(GetLinearYValue, SetLinearYValue, maxSpeed * -1, nameof(_moveUpVelocityMaxBehavior));
            _moveDownVelocityMaxBehavior = new LimitNumberBehavior(GetLinearYValue, SetLinearYValue, maxSpeed, nameof(_moveDownVelocityMaxBehavior));

            _moveLeftVelocityMaxBehavior = new LimitNumberBehavior(GetLinearXValue, SetLinearXValue, maxSpeed * -1, nameof(_moveLeftVelocityMaxBehavior));
            _moveRightVelocityMaxBehavior = new LimitNumberBehavior(GetLinearXValue, SetLinearXValue, maxSpeed, nameof(_moveRightVelocityMaxBehavior));

            Behaviors.Add(_moveUpVelocityMaxBehavior);
            Behaviors.Add(_moveDownVelocityMaxBehavior);
            Behaviors.Add(_moveRightVelocityMaxBehavior);
            Behaviors.Add(_moveLeftVelocityMaxBehavior);
        }


        /// <summary>
        /// Sets up all of the max rotataion behaviors.
        /// </summary>
        private void SetupMaxRotationBehaviors(float maxSpeed)
        {
            _rotateCWVelocityMaxBehavior = new LimitNumberBehavior(GetAngularValue, SetAngularValue, maxSpeed);
            _rotateCCWVelocityMaxBehavior = new LimitNumberBehavior(GetAngularValue, SetAngularValue, maxSpeed * -1);

            Behaviors.Add(_rotateCWVelocityMaxBehavior);
            Behaviors.Add(_rotateCCWVelocityMaxBehavior);
        }


        private void SetBehaviorLimits(float maxLinearSpeed)
        {
            _moveUpVelocityMaxBehavior.LimitValue = maxLinearSpeed.ForceNegative();
            _moveDownVelocityMaxBehavior.LimitValue = maxLinearSpeed.ForcePositive();
            _moveLeftVelocityMaxBehavior.LimitValue = maxLinearSpeed.ForceNegative();
            _moveRightVelocityMaxBehavior.LimitValue = maxLinearSpeed.ForcePositive();
        }


        /// <summary>
        /// Gets the current linear velocity of the X component
        /// </summary>
        /// <returns></returns>
        private float GetLinearXValue()
        {
            return Body.LinearVelocity.X;
        }


        /// <summary>
        /// Gets the current linear velocity of the Y component
        /// </summary>
        /// <returns></returns>
        private float GetLinearYValue()
        {
            return Body.LinearVelocity.Y;
        }


        /// <summary>
        /// Sets X value of the linear velocity
        /// </summary>
        /// <returns></returns>
        private void SetLinearXValue(float value)
        {
            Body.LinearVelocity = new Vector(value, Body.LinearVelocity.Y);
        }


        /// <summary>
        /// Sets Y value of the linear velocity
        /// </summary>
        /// <returns></returns>
        private void SetLinearYValue(float value)
        {
            if (Body.LinearVelocity.Y != value)
                Body.LinearVelocity = new Vector(Body.LinearVelocity.X, value);
        }


        private float GetAngularValue()
        {
            return Body.AngularVelocity;
        }


        private void SetAngularValue(float value)
        {
            Body.AngularVelocity = value;
        }
        #endregion
    }
}
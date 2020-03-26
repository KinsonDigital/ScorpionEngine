using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Exceptions;
using Raptor;
using Raptor.Graphics;
using Raptor.Plugins;

namespace KDScorpionEngine.Entities
{
    /// <summary>
    /// Represents a game object that can be moved around the screen.  
    /// This is just an <see cref="Entity"/> with the capability of being moved.
    /// </summary>
    public class DynamicEntity : Entity
    {
        #region Private Fields
        private Vector _facingDirection = new Vector(0, -1);
        private LimitNumberBehavior _moveRightVelocityMaxBehavior;
        private LimitNumberBehavior _moveLeftVelocityMaxBehavior;
        private LimitNumberBehavior _moveDownVelocityMaxBehavior;
        private LimitNumberBehavior _moveUpVelocityMaxBehavior;
        private LimitNumberBehavior _rotateCWVelocityMaxBehavior;
        private LimitNumberBehavior _rotateCCWVelocityMaxBehavior;
        private float _preInitAngle;
        private float _preInitLinearDeceleration;
        private float _preInitAngularDeceleration;
        private float _maxLinearSpeed;
        private const float DEFAULT_MAX_SPEED = 40f;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="body">The mocked physics body to inject.</param>
        internal DynamicEntity(IPhysicsBody body) : base(body)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }


        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        public DynamicEntity()
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }


        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        public DynamicEntity(float friction = 0.2f) : base(friction: friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }


        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="position">The position of where to render the <see cref="DynamicEntity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        public DynamicEntity(Vector position, float friction = 0.2f) : base(position, friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }


        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="texture">The texture of the entity to render.</param>
        /// <param name="position">The position of where to render the <see cref="DynamicEntity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        public DynamicEntity(Texture texture, Vector position, float friction = 0.2f) : base(texture, position, friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }


        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="DynamicEntity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="DynamicEntity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        public DynamicEntity(Vector[] vertices, Vector position, float friction = 0.2f) : base(vertices, position, friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }


        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="texture">The texture of the entity to render.</param>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="DynamicEntity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="DynamicEntity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        public DynamicEntity(Texture texture, Vector[] polyVertices, Vector position, float friction = 0.2f) : base(texture, polyVertices, position, friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets a value indicating if the <see cref="DynamicEntity"/> is moving or sitting still.
        /// </summary>
        public bool IsMoving => Body.LinearVelocity != Vector.Zero || Body.AngularVelocity != 0;

        /// <summary>
        /// Gets or sets a value indicating if rotation should be locked in place.
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
        /// Gets or sets the maximum linear movement speed of the <see cref="DynamicEntity"/>. Positive and
        /// negative values behave the same.
        /// Example: 5 and -5 would set the maximum speed to the same but only dictate the direction
        /// that the max speed is applied.
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

        /// <summary>
        /// Gets or sets the maximum rotation speed of the <see cref="DynamicEntity"/>.
        /// </summary>
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
        /// Gets or sets the rotational speed of the <see cref="DynamicEntity"/> in degrees.
        /// </summary>
        public float RotateSpeed { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the speed in the X direction. Negative values will move the
        /// <see cref="DynamicEntity"/> to the left. Positive values will move the <see cref="DynamicEntity"/>
        /// to the right.
        /// </summary>
        public float SpeedX { get; set; } = 0.25f;

        /// <summary>
        /// Gets or sets the speed in the Y direction. Negative values will move the
        /// <see cref="DynamicEntity"/> up. Positive values move the <see cref="DynamicEntity"/>
        /// down.
        /// </summary>
        public float SpeedY { get; set; } = 0.25f;

        /// <summary>
        /// Gets or sets the linear deceleration of the <see cref="DynamicEntity"/>.
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

        /// <summary>
        /// Gets or sets the angular deceleration of the <see cref="DynamicEntity"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="DynamicEntity"/> is in the process of stopping.
        /// </summary>
        public bool IsEntityStopping { get; set; }
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
        /// Updates the <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public override void Update(EngineTime engineTime)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            _facingDirection = new Vector(0, -1).RotateAround(Vector.Zero, Body.InternalPhysicsBody.Angle);

            ProcessMovementStop();

            base.Update(engineTime);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right using the <see cref="SpeedX"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveRight()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX.ForcePositive(), 0, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveRight(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(speed.ForcePositive(), 0, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left using the <see cref="SpeedX"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveLeft()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX.ForceNegative(), 0, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveLeft(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(speed.ForceNegative(), 0, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up using the <see cref="SpeedY"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveUp()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(0, SpeedY.ForceNegative(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveUp(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(0, speed.ForceNegative(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the <see cref="SpeedY"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveDown()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(0, SpeedY.ForcePositive(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveDown(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(0, speed, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the right using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveUpRight()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX.ForcePositive(), SpeedY.ForceNegative(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveUpRight(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(speed, speed.ForceNegative(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the left using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveUpLeft()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX.ForceNegative(), SpeedY.ForceNegative(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveUpLeft(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            speed = speed.ForceNegative();
            Body.InternalPhysicsBody.ApplyForce(speed, speed, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the right using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveDownRight()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX.ForcePositive(), SpeedY.ForcePositive(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveDownRight(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            speed = speed.ForcePositive();
            Body.InternalPhysicsBody.ApplyForce(speed, speed, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the left using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveDownLeft()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX.ForceNegative(), SpeedY.ForcePositive(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveDownLeft(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(speed.ForceNegative(), speed.ForcePositive(), Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> based on the current <see cref="SpeedX"/>
        /// and <see cref="SpeedY"/> property values. Positive and negative values of <see cref="SpeedX"/> and <see cref="SpeedY"/>
        /// determines the direction of travel on that axis.
        /// </summary>
        public void MoveAtSetSpeed()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.ApplyForce(SpeedX, SpeedY, Position.X, Position.Y);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> at the currently set angle using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveAtSetAngle(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            var directionToMove = new Vector(_facingDirection.X, _facingDirection.Y) * speed.ForcePositive();

            Body.InternalPhysicsBody.ApplyForce(directionToMove.X, directionToMove.Y, Position.X, Position.Y);
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise using the <see cref="RotateSpeed"/> property value.
        /// NOTE: Positive and negative values behave the same.
        /// </summary>
        public void RotateCW() => RotateCW(RotateSpeed);


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise using the given speed.
        /// </summary>
        /// <param name="speed">The speed to rotate the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void RotateCW(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            if (RotationEnabled)
                Body.InternalPhysicsBody.ApplyAngularImpulse(speed.ForcePositive());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise using the <see cref="RotateSpeed"/> property value.
        /// NOTE: Positive and negative values behave the same.
        /// </summary>
        public void RotateCCW() => RotateCCW(RotateSpeed);


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise using the given speed.
        /// </summary>
        /// <param name="speed">The speed to rotate the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void RotateCCW(float speed)
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            if (RotationEnabled)
                Body.InternalPhysicsBody.ApplyAngularImpulse(speed.ForceNegative());
        }


        /// <summary>
        /// Stops the movement of the <see cref="DynamicEntity"/>.
        /// </summary>
        public void StopMovement() => IsEntityStopping = true;


        /// <summary>
        /// Stops the rotation of the <see cref="DynamicEntity"/>.
        /// </summary>
        public void StopRotation()
        {
            if (Body == null)
                throw new EntityNotInitializedException();

            Body.InternalPhysicsBody.AngularVelocity = 0;
        }


        /// <summary>
        /// Starts process of stopping the movement of the <see cref="DynamicEntity"/> if it has been flagged to stop.
        /// </summary>
        private void ProcessMovementStop()
        {
            if (IsEntityStopping)
            {
                //If the body is still moving in the Y direction
                if (Body.InternalPhysicsBody.LinearVelocityY != 0)
                {
                    Body.InternalPhysicsBody.ApplyForce(Body.InternalPhysicsBody.LinearVelocityX, Body.InternalPhysicsBody.LinearVelocityY * -1, Position.X, Position.Y);
                }

                //If the body is still moving in the X direction
                if (Body.InternalPhysicsBody.LinearVelocityX != 0)
                {
                    Body.InternalPhysicsBody.ApplyForce(Body.InternalPhysicsBody.LinearVelocityX * -1, Body.InternalPhysicsBody.LinearVelocityY, Position.X, Position.Y);
                }

                //If the body is still rotating
                if (Body.InternalPhysicsBody.AngularVelocity != 0)
                    Body.InternalPhysicsBody.ApplyAngularImpulse(Body.InternalPhysicsBody.AngularVelocity * -1);

                //If the body has stopped moving, set the flag back to false
                if (Body.InternalPhysicsBody.LinearVelocityX == 0 && Body.InternalPhysicsBody.LinearVelocityY == 0 &&
                    Body.AngularVelocity == 0)
                    IsEntityStopping = false;
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up all of the max linear behaviors.
        /// </summary>
        /// <param name="maxSpeed">The maximum speed of the directions of movement.  A positive or negative speed value will have the same effect.</param>
        private void SetupMaxLinearBehaviors(float maxSpeed)
        {
            //Force the speed to a positive number if it is negative
            maxSpeed = maxSpeed.ForcePositive();

            _moveUpVelocityMaxBehavior = new LimitNumberBehavior(GetLinearYValue, SetLinearYValue, maxSpeed * -1, nameof(_moveUpVelocityMaxBehavior))
            {
                Name = $"{nameof(DynamicEntity)}.{nameof(LimitNumberBehavior)} => {"{ "}{nameof(_moveUpVelocityMaxBehavior)}{" }"}"
            };
            _moveDownVelocityMaxBehavior = new LimitNumberBehavior(GetLinearYValue, SetLinearYValue, maxSpeed, nameof(_moveDownVelocityMaxBehavior))
            {
                Name = $"{nameof(DynamicEntity)}.{nameof(LimitNumberBehavior)} => {"{ "}{nameof(_moveDownVelocityMaxBehavior)}{" }"}"
            };
            _moveRightVelocityMaxBehavior = new LimitNumberBehavior(GetLinearXValue, SetLinearXValue, maxSpeed, nameof(_moveRightVelocityMaxBehavior))
            {
                Name = $"{nameof(DynamicEntity)}.{nameof(LimitNumberBehavior)} => {"{ "}{nameof(_moveRightVelocityMaxBehavior)}{" }"}"
            };
            _moveLeftVelocityMaxBehavior = new LimitNumberBehavior(GetLinearXValue, SetLinearXValue, maxSpeed * -1, nameof(_moveLeftVelocityMaxBehavior))
            {
                Name = $"{nameof(DynamicEntity)}.{nameof(LimitNumberBehavior)} => {"{ "}{nameof(_moveLeftVelocityMaxBehavior)}{" }"}"
            };

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
            _rotateCWVelocityMaxBehavior = new LimitNumberBehavior(GetAngularVelocityValue, SetAngularValue, maxSpeed)
            {
                Name = $"{nameof(DynamicEntity)}.{nameof(LimitNumberBehavior)} => {"{ "}{nameof(_rotateCWVelocityMaxBehavior)}{" }"}"
            };

            _rotateCCWVelocityMaxBehavior = new LimitNumberBehavior(GetAngularVelocityValue, SetAngularValue, maxSpeed.ForceNegative())
            {
                Name = $"{nameof(DynamicEntity)}.{nameof(LimitNumberBehavior)} => {"{ "}{nameof(_rotateCCWVelocityMaxBehavior)}{" }"}"
            };

            Behaviors.Add(_rotateCWVelocityMaxBehavior);
            Behaviors.Add(_rotateCCWVelocityMaxBehavior);
        }


        /// <summary>
        /// Sets the limits of the number limit behaviors.
        /// </summary>
        /// <param name="maxLinearSpeed">The maximum linear speed.  A positive or negative speed value will have the same effect.</param>
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
        private float GetLinearXValue() => Body.LinearVelocity.X;


        /// <summary>
        /// Gets the current linear velocity of the Y component
        /// </summary>
        /// <returns></returns>
        private float GetLinearYValue() => Body.LinearVelocity.Y;


        /// <summary>
        /// Sets X value of the linear velocity
        /// </summary>
        /// <returns></returns>
        private void SetLinearXValue(float value) => Body.LinearVelocity = new Vector(value, Body.LinearVelocity.Y);


        /// <summary>
        /// Sets Y value of the linear velocity
        /// </summary>
        /// <returns></returns>
        private void SetLinearYValue(float value)
        {
            if (Body.LinearVelocity.Y != value)
                Body.LinearVelocity = new Vector(Body.LinearVelocity.X, value);
        }


        /// <summary>
        /// Gets the angular velocity value.
        /// </summary>
        /// <returns></returns>
        private float GetAngularVelocityValue() => Body.AngularVelocity;


        /// <summary>
        /// Sets the angular velocity to the given value.
        /// </summary>
        /// <param name="value">The value to set the angular velocity to.</param>
        private void SetAngularValue(float value) => Body.AngularVelocity = value;
        #endregion
    }
}
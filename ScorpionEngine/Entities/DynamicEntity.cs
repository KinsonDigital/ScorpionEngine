using System.Collections.Generic;
using ScorpionEngine.Behaviors;
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
        #endregion


        #region Constructors
        public DynamicEntity(Texture texture, Vector position, bool isStaticBody = false) : base(texture, position, isStaticBody)
        {
            SetupMaxLinearBehaviors(1f);
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="vertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public DynamicEntity(Vector[] vertices, Vector position, bool isStaticBody = false) : base(vertices, position, isStaticBody)
        {
            SetupMaxLinearBehaviors(1f);
            AngularDeceleration = 0.5f;
            LinearDeceleration = 0.6f;
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="location">The location to draw the object.</param>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public DynamicEntity(Texture texture, Vector[] polyVertices, Vector location, bool isStaticBody = false) : base(texture, polyVertices, location, isStaticBody)
        {
            SetupMaxLinearBehaviors(1f);
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets a value indicating if the object will also point towards its destination point if it is set to follow.
        /// </summary>
        public bool PointToDestinationOnFollow { get; set; }

        /// <summary>
        /// Gets or sets the desination point of the object to follow if the IsFollowing property is set to true.
        /// </summary>
        public Vector DestinationPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if rotation should be locked.
        /// </summary>
        //TODO: Get this property/feature working
        public bool RotationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if acceleration will be enabled or disabled for rotation.  When disabled, any rotation of the object
        /// will result in the object instantly moving at the RotateSpeed setting instead of a gradual increase to top speed.
        /// </summary>
        public bool RotationalAccelerationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if acceleration will be enabled or disabled for linear movement.  When disabled, any movement of the object
        /// will result in the object instantly moving at the LinearMovementSpeed setting instead of a gradual increase to top speed.
        /// </summary>
        public bool LinearAccelerationEnabled { get; set; }

        /// <summary>
        /// Gets the general direction that the game object is moving.
        /// </summary>
        public Direction MovementDirection { get; } = Direction.Right;

        /// <summary>
        /// Gets or sets the angle in degrees.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the maximum linear movement speed. Positive and
        /// negative values behave the same.
        /// </summary>
        public float MaxLinearSpeed
        {
            get => _moveRightVelocityMaxBehavior.LimitValue;
            set
            {
                if (_moveRightVelocityMaxBehavior == null)
                {
                    SetupMaxLinearBehaviors(value.ForcePositive());
                }
                else
                {
                    _moveLeftVelocityMaxBehavior.LimitValue = value.ForceNegative();
                    _moveRightVelocityMaxBehavior.LimitValue = value.ForcePositive();
                    _moveUpVelocityMaxBehavior.LimitValue = value.ForceNegative();
                    _moveDownVelocityMaxBehavior.LimitValue = value.ForcePositive();
                }
            }
        }

        public float MaxAngularSpeed
        {
            get => _rotateCWVelocityMaxBehavior.LimitValue;
            set
            {
                if (_rotateCWVelocityMaxBehavior == null)
                {
                    SetupMaxRotationBehaviors(value.ForcePositive());
                }
                else
                {
                    _rotateCWVelocityMaxBehavior.LimitValue = value.ForcePositive();
                    _rotateCCWVelocityMaxBehavior.LimitValue = value.ForceNegative();
                }
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
        /// Gets or sets the impulseType to auto rotate if AutoRotate is set to true.
        /// If auto rotate is set to true, manual rotation via keyboard input and RotateCW and RotateCCW will be disabled.
        /// </summary>
        public RotationDirection AutoRotateDirection { get; set; } = RotationDirection.Clockwise;

        /// <summary>
        /// Gets or sets the impulseType that the entity will automatically move.
        /// </summary>
        public Direction AutoDirection { get; set; } = Direction.Up;

        /// <summary>
        /// Gets or sets the linear deceleration of the object. DEFAULT: 1
        /// </summary>
        public float LinearDeceleration
        {
            get => Body.InternalPhysicsBody.LinearDeceleration;
            set => Body.InternalPhysicsBody.LinearDeceleration = value;
        }

        public float AngularDeceleration
        {
            get => Body.InternalPhysicsBody.AngularDeceleration;
            set => Body.InternalPhysicsBody.AngularDeceleration = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if the entity should rotate automatically at the set velocity.
        /// If auto rotate is set to true, manual rotation via keyboard input and RotateCW and RotateCCW will be disabled.
        /// </summary>
        public bool AutoRotate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if auto move is on.  Moves the entity at the set velocity a the given impulseType that autoDirection is set to.
        /// </summary>
        public bool AutoMove { get; set; }
        #endregion


        #region Public Method
        /// <summary>
        /// Updates the moveable object.
        /// </summary>
        /// <param name="engineTime"></param>
        public override void OnUpdate(EngineTime engineTime)
        {
            _facingDirection = Tools.RotateAround(new Vector(0, -1), Vector.Zero, Body.InternalPhysicsBody.Angle.ToRadians());

            ProcessMovementStop();

            base.OnUpdate(engineTime);
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
        /// Moves the <see cref="DynamicEntity"/> to the right using the <see cref="SpeedX"/> value.
        /// Behaves the same no matter if <see cref="SpeedX"/> is positive or negative.
        /// </summary>
        public void MoveRight()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForcePositive(), 0);
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
        /// Moves the <see cref="DynamicEntity"/> to the left using the <see cref="SpeedX"/> value.
        /// Behaves the same no matter if <see cref="SpeedX"/> is positive or negative.
        /// </summary>
        public void MoveLeft()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(SpeedX.ForceNegative(), 0);
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
        /// Moves the <see cref="DynamicEntity"/> up using the <see cref="SpeedY"/> value.
        /// Behaves the same no matter if <see cref="SpeedY"/> is positive or negative.
        /// </summary>
        public void MoveUp()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, SpeedY.ForceNegative());
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveDown(float speed)
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, speed);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the <see cref="SpeedY"/> value.
        /// Behaves the same no matter if <see cref="SpeedY"/> is positive or negative.
        /// </summary>
        public void MoveDown()
        {
            Body.InternalPhysicsBody.ApplyLinearImpulse(0, SpeedY.ForcePositive());
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
        /// Moves the <see cref="DynamicEntity"/> up and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>. Positive and negative numbers behave the same.</param>
        public void MoveUpLeft(float speed)
        {
            speed = speed.ForceNegative();
            Body.InternalPhysicsBody.ApplyLinearImpulse(speed, speed);
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
            Body.InternalPhysicsBody.ApplyAngularImpulse(RotateSpeed.ForcePositive());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise using the given speed.
        /// </summary>
        /// <param name="speed">The speed to rotate the <see cref="DynamicEntity"/>. Positive and negative values behave the same.</param>
        public void RotateCW(float speed)
        {
            Body.InternalPhysicsBody.ApplyAngularImpulse(speed.ForcePositive());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise using the <see cref="RotateSpeed"/> value.
        /// Behaves the same no matter if the <see cref="RotateSpeed"/> value is positive or negative.
        /// </summary>
        public void RotateCCW()
        {
            Body.InternalPhysicsBody.ApplyAngularImpulse(RotateSpeed.ForceNegative());
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise using the given speed.
        /// </summary>
        /// <param name="speed">The speed to rotate the <see cref="DynamicEntity"/>. Positive and negative values behave the same.</param>
        public void RotateCCW(float speed)
        {
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
                if (Body.InternalPhysicsBody.LinearVelocityX == 0 && Body.InternalPhysicsBody.LinearVelocityY == 0)
                    _stopMovement = false;
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up all of the max linear behaviors.
        /// </summary>
        private void SetupMaxLinearBehaviors(float maxSpeed)
        {
            _moveRightVelocityMaxBehavior = new LimitNumberBehavior(GetXLinearValue, SetXLinearValue, maxSpeed);
            _moveLeftVelocityMaxBehavior = new LimitNumberBehavior(GetXLinearValue, SetXLinearValue, maxSpeed * -1);

            _moveDownVelocityMaxBehavior = new LimitNumberBehavior(GetYLinearValue, SetYLinearValue, maxSpeed);
            _moveUpVelocityMaxBehavior = new LimitNumberBehavior(GetYLinearValue, SetYLinearValue, maxSpeed * -1);

            Behaviors.Add(_moveRightVelocityMaxBehavior);
            Behaviors.Add(_moveLeftVelocityMaxBehavior);
            Behaviors.Add(_moveDownVelocityMaxBehavior);
            Behaviors.Add(_moveUpVelocityMaxBehavior);
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


        /// <summary>
        /// Gets the current linear velocity of the X component
        /// </summary>
        /// <returns></returns>
        private float GetXLinearValue()
        {
            return Body.LinearVelocity.X;
        }


        /// <summary>
        /// Sets X value of the linear velocity
        /// </summary>
        /// <returns></returns>
        private void SetXLinearValue(float value)
        {
            Body.LinearVelocity = new Vector(value, Body.LinearVelocity.Y);
        }


        /// <summary>
        /// Gets the current linear velocity of the Y component
        /// </summary>
        /// <returns></returns>
        private float GetYLinearValue()
        {
            return Body.LinearVelocity.Y;
        }


        /// <summary>
        /// Sets Y value of the linear velocity
        /// </summary>
        /// <returns></returns>
        private void SetYLinearValue(float value)
        {
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
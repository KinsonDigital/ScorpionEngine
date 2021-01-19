// <copyright file="DynamicEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using Raptor.Graphics;

    /// <summary>
    /// Represents a game object that can be moved around the screen.
    /// This is just an <see cref="Entity"/> with the capability of being moved.
    /// </summary>
    public class DynamicEntity : Entity
    {
        private LimitNumberBehavior moveRightVelocityMaxBehavior;
        private LimitNumberBehavior moveLeftVelocityMaxBehavior;
        private LimitNumberBehavior moveDownVelocityMaxBehavior;
        private LimitNumberBehavior moveUpVelocityMaxBehavior;
        private LimitNumberBehavior rotateCWVelocityMaxBehavior;
        private LimitNumberBehavior rotateCCWVelocityMaxBehavior;
        private float preInitAngle;
        private float preInitLinearDeceleration;
        private float preInitAngularDeceleration;
        private float maxLinearSpeed;
        private const float DEFAULT_MAX_SPEED = 40f;

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
        public DynamicEntity(float friction = 0.2f)
            : base(friction: friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }

        /// <summary>
        /// Creates a new instance of <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="position">The position of where to render the <see cref="DynamicEntity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        public DynamicEntity(Vector2 position, float friction = 0.2f)
            : base(position, friction)
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
        public DynamicEntity(Texture texture, Vector2 position, float friction = 0.2f)
            : base(texture, position, friction)
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
        public DynamicEntity(Vector2[] vertices, Vector2 position, float friction = 0.2f)
            : base(vertices, position, friction)
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
        public DynamicEntity(Texture texture, Vector2[] polyVertices, Vector2 position, float friction = 0.2f)
            : base(texture, polyVertices, position, friction)
        {
            SetupMaxLinearBehaviors(DEFAULT_MAX_SPEED);
            SetupMaxRotationBehaviors(DEFAULT_MAX_SPEED);
        }

        /// <summary>
        /// Gets a value indicating if the <see cref="DynamicEntity"/> is moving or sitting still.
        /// </summary>
        public bool IsMoving => throw new NotImplementedException();

        /// <summary>
        /// Gets or sets a value indicating if rotation should be locked in place.
        /// </summary>
        public bool RotationEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the angle in degrees.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the maximum linear movement speed of the <see cref="DynamicEntity"/>. Positive and
        /// negative values behave the same.
        /// Example: 5 and -5 would set the maximum speed to the same but only dictate the direction
        /// that the max speed is applied.
        /// </summary>
        public float MaxLinearSpeed
        {
            get => this.maxLinearSpeed;
            set
            {
                this.maxLinearSpeed = value;
                SetBehaviorLimits(this.maxLinearSpeed);
            }
        }

        /// <summary>
        /// Gets or sets the maximum rotation speed of the <see cref="DynamicEntity"/>.
        /// </summary>
        public float MaxRotationSpeed
        {
            get => this.rotateCWVelocityMaxBehavior.LimitValue;
            set
            {
                this.rotateCWVelocityMaxBehavior.LimitValue = value.ToPositive();
                this.rotateCCWVelocityMaxBehavior.LimitValue = value.ToNegative();
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
        /// Gets or sets a value indicating if the <see cref="DynamicEntity"/> is in the process of stopping.
        /// </summary>
        public bool IsEntityStopping { get; set; }

        /// <summary>
        /// Updates the <see cref="DynamicEntity"/>.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public override void Update(GameTime gameTime)
        {
            ProcessMovementStop();

            base.Update(gameTime);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right using the <see cref="SpeedX"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveRight()
        {
            // TODO: Get this working
            ////Body.ApplyForce(SpeedX.ForcePositive(), 0, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveRight(float speed)
        {
            // TODO: Get this working
            ////Body.ApplyForce(speed.ForcePositive(), 0, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left using the <see cref="SpeedX"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveLeft()
        {
            // TODO: Get this working
            // Body.ApplyForce(SpeedX.ForceNegative(), 0, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveLeft(float speed)
        {
            // TODO: Get this working
            // Body.ApplyForce(speed.ForceNegative(), 0, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up using the <see cref="SpeedY"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveUp()
        {
            // TODO: Get this working
            // Body.ApplyForce(0, SpeedY.ForceNegative(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveUp(float speed)
        {
            // TODO: Get this working
            // Body.ApplyForce(0, speed.ForceNegative(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the <see cref="SpeedY"/> property value.
        /// NOTE: A positive or negative value will have the same effect.
        /// </summary>
        public void MoveDown()
        {
            // TODO: Get this working
            // Body.ApplyForce(0, SpeedY.ForcePositive(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveDown(float speed)
        {
            // TODO: Get this working
            // Body.ApplyForce(0, speed, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the right using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveUpRight()
        {
            // TODO: Get this working
            // Body.ApplyForce(SpeedX.ForcePositive(), SpeedY.ForceNegative(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveUpRight(float speed)
        {
            // TODO: Get this working
            // Body.ApplyForce(speed, speed.ForceNegative(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the left using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveUpLeft()
        {
            // TODO: Get this working
            // Body.ApplyForce(SpeedX.ForceNegative(), SpeedY.ForceNegative(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveUpLeft(float speed)
        {
            // TODO: Get this working
            speed = speed.ToNegative();
            // Body.ApplyForce(speed, speed, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the right using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveDownRight()
        {
            // TODO: Get this working
            // Body.ApplyForce(SpeedX.ForcePositive(), SpeedY.ForcePositive(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the right using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveDownRight(float speed)
        {
            // TODO: Get this working
            speed = speed.ToPositive();
            // Body.ApplyForce(speed, speed, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the left using the <see cref="SpeedX"/> and <see cref="SpeedY"/> property values.
        /// NOTE: Positive or negative values will have the same effect.
        /// </summary>
        public void MoveDownLeft()
        {
            // TODO: Get this working
            // Body.ApplyForce(SpeedX.ForceNegative(), SpeedY.ForcePositive(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down and to the left using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveDownLeft(float speed)
        {
            // Body.ApplyForce(speed.ForceNegative(), speed.ForcePositive(), Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> based on the current <see cref="SpeedX"/>
        /// and <see cref="SpeedY"/> property values. Positive and negative values of <see cref="SpeedX"/> and <see cref="SpeedY"/>
        /// determines the direction of travel on that axis.
        /// </summary>
        public void MoveAtSetSpeed()
        {
            // TODO: Get this working
            // Body.ApplyForce(SpeedX, SpeedY, Position.X, Position.Y);
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> at the currently set angle using the given speed.
        /// </summary>
        /// <param name="speed">The speed to move the <see cref="DynamicEntity"/>.  A positive or negative speed value will have the same effect.</param>
        public void MoveAtSetAngle(float speed)
        {
            // TODO: Get this working
            // Body.ApplyForce(directionToMove.X, directionToMove.Y, Position.X, Position.Y);
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
            // TODO: Get this working
            // if (RotationEnabled)
            //    Body.ApplyAngularImpulse(speed.ForcePositive());
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
            // TODO: Get this working with unit test
            // if (RotationEnabled)
            //    Body.ApplyAngularImpulse(speed.ForceNegative());
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
        }

        /// <summary>
        /// Starts process of stopping the movement of the <see cref="DynamicEntity"/> if it has been flagged to stop.
        /// </summary>
        private void ProcessMovementStop()
        {
        }

        /// <summary>
        /// Sets up all of the max linear behaviors.
        /// </summary>
        /// <param name="maxSpeed">The maximum speed of the directions of movement.  A positive or negative speed value will have the same effect.</param>
        private void SetupMaxLinearBehaviors(float maxSpeed)
        {
        }

        /// <summary>
        /// Sets up all of the max rotataion behaviors.
        /// </summary>
        private void SetupMaxRotationBehaviors(float maxSpeed)
        {
        }

        /// <summary>
        /// Sets the limits of the number limit behaviors.
        /// </summary>
        /// <param name="maxLinearSpeed">The maximum linear speed.  A positive or negative speed value will have the same effect.</param>
        private void SetBehaviorLimits(float maxLinearSpeed)
        {
            this.moveUpVelocityMaxBehavior.LimitValue = maxLinearSpeed.ToNegative();
            this.moveDownVelocityMaxBehavior.LimitValue = maxLinearSpeed.ToPositive();
            this.moveLeftVelocityMaxBehavior.LimitValue = maxLinearSpeed.ToNegative();
            this.moveRightVelocityMaxBehavior.LimitValue = maxLinearSpeed.ToPositive();
        }
    }
}

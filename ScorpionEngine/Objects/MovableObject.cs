using System;
using System.Collections.Generic;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Content;

using ScorpionEngine.Events;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;
using ScorpionEngine.Utils;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Represents a game object that can be moved around the screen.  
    /// This is just a game object with moving capability added on to it.
    /// </summary>
    public class MovableObject : GameObject
    {
        #region Events
        /// <summary>
        /// Occurs every time the entity moves.
        /// </summary>
        public event EventHandler<OnMovedEventArgs> OnMove;
        #endregion


        #region Fields
        private float _linearAcceleration = 0.1f;//The acceleration of any linear movement
        private readonly Dictionary<Direction, bool> _linearMovementLocks = new Dictionary<Direction, bool>();//Holds the lock states for the 8 linear movements.  True means locked.
        #endregion


        #region Constructors
        public MovableObject(Vector position) : base(position)
        {

        }


        public MovableObject(Texture texture, Vector position) : base(texture, position)
        {

        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="vertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(Vector[] vertices, Vector position) : base(vertices, position)
        {
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="location">The location to draw the object.</param>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(Texture texture, Vector[] polyVertices, Vector location) : base(texture, polyVertices, location)
        {
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
        /// Gets or sets the speed for non linear movement.
        /// </summary>
        public float Speed { get; set; } = 5f;

        /// <summary>
        /// Gets or sets the rotational speed of the object in degrees.
        /// </summary>
        public float RotateSpeed { get; set; } = 1f;

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
        /// Gets or sets the acceleration for linear movement. Greater the number, the faster the object will accelerate up to max speed.
        /// </summary>
        public float LinearAcceleration
        {
            get
            {
                //Multiply the acceleration by 10 to make the number larger.
                //The value outgoing is set to a larger number to make it less confusing for the user.
                //This is so the user can use 1 instead of the value 0.1.
                return _linearAcceleration * 10f;
            }
            set
            {
                //Divide the incoming value by 10 to turn it back to a floating point number.
                //The value incoming is set to a larger number to make it less confusing for the user.
                //This is so the user can use 1 instead of the value 0.1.
                _linearAcceleration = value / 10f;
            }
        }

        /// <summary>
        /// Gets or sets the linear deceleration of the object. DEFAULT: 1
        /// </summary>
        public float LinearDeceleration { get; set; }

        /// <summary>
        /// Gets the speed on the x axis.
        /// </summary>
        public float CurrentSpeedX
        {
            get
            {
                //TODO: set the speed on the x axis
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the speed on the y axis.
        /// </summary>
        public float CurrentSpeedY
        {
            get
            {
                //TODO: set the speed on the x axis
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the speed at which the object moves when following the destination point.
        /// </summary>
        public float MaxFollowSpeed { get; set; } = 1f;

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
        public override void OnUpdate(IEngineTiming engineTime)
        {
            base.OnUpdate(engineTime);
        }


        /// <summary>
        /// Moves the game object to the right.
        /// </summary>
        public void MoveRight()
        {

        }


        /// <summary>
        /// Moves the game object to the left.
        /// </summary>
        public void MoveLeft()
        {

        }


        /// <summary>
        /// Moves the game object up.
        /// </summary>
        public void MoveUp()
        {
        }


        /// <summary>
        /// Moves the game object down.
        /// </summary>
        public void MoveDown()
        {
        }


        /// <summary>
        /// Moves the game object up and to the right.
        /// </summary>
        public void MoveUpRight()
        {
        }


        /// <summary>
        /// Moves the game object up and to the left.
        /// </summary>
        public void MoveUpLeft()
        {
        }


        /// <summary>
        /// Moves the game object down and to the right.
        /// </summary>
        public void MoveDownRight()
        {
        }


        /// <summary>
        /// Moves the game object down and to the left.
        /// </summary>
        public void MoveDownLeft()
        {
        } 


        /// <summary>
        /// Moves the entity at the currently set angle/impulseType.
        /// </summary>
        public void MoveAtSetAngle()
        {
        }


        /// <summary>
        /// Rotates the game object clockwise.
        /// </summary>
        public void RotateCW()
        {

        }


        /// <summary>
        /// Rotates the game object counter clockwise.
        /// </summary>
        public void RotateCCW()
        {

        }


        /// <summary>
        /// Stops the movement of the object.
        /// </summary>
        public void StopMovement()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Stops the rotation of the object.
        /// </summary>
        public void StopRotation()
        {
            //TODO: Stop rotation of the object
            throw new NotImplementedException();
        }
        #endregion
    }
}
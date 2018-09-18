using System;
using System.Collections.Generic;
using ScorpionEngine.Core;
using ScorpionEngine.Events;
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
        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(string textureName, Vector[] polyVertices = null) : base(textureName)
        {
            //Initialize the rest of the movable object
            InitializeObj(Vector.Zero, textureName, polyVertices);
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="location">The location to draw the object.</param>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(Vector location, string textureName, Vector[] polyVertices = null)
            : base(location, textureName, polyVertices)
        {
        }


        /// <summary>
        /// Creates a new instance of MovableObject.
        /// </summary>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="location">The location to draw the MovableObject.</param>
        /// <param name="speed">The velocity that the MovableObject moves at.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(Vector location, Vector speed, string textureName, Vector[] polyVertices = null)
            : base(location, textureName, polyVertices)
        {
            throw new NotImplementedException();
            //Initialize the rest of the movable object
//            InitializeObj(location, textureName);//NOT NEEDED - DELETE
        }


        /// <summary>
        /// Loads a texture atlas for the movable object to use.
        /// </summary>
        /// <param name="textureAtlasName">The name of the texture atlas to load.</param>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <param name="subTextureID">The name of the sub texture in the atlas texture to render.</param> 
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(string textureAtlasName, string atlasDataName, string subTextureID, Vector[] polyVertices = null) 
            : base(textureAtlasName, atlasDataName, subTextureID, polyVertices)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Creates a new instance of an movable object.
        /// </summary>
        /// <param name="textureAtlasName">The name of the texture atlas to load.</param>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <param name="subTextureID">The name of the sub texture in the atlas texture to render.</param>
        /// <param name="location">Sets the location of the movable object in the game world.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(Vector location, string textureAtlasName, string atlasDataName, string subTextureID, Vector[] polyVertices = null)
            : base(location, textureAtlasName, atlasDataName, subTextureID, polyVertices)
        {
            throw new NotImplementedException();
            //Initialize the atlas
            //            InitializeAtlas(textureAtlasName, atlasDataName, subTextureID);//NOT NEEDED - DELETE

            //Initialize the rest of the movable object
            //            InitializeObj(location, textureAtlasName);//NOT NEEDED - DELETE
        }


        /// <summary>
        /// Creates a new instance of an movable object.
        /// </summary>
        /// <param name="textureAtlasName">The name of the texture atlas to load.</param>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <param name="subTextureID">The name of the sub texture in the atlas texture to render.</param>
        /// <param name="location">Sets the location of the movable object in the game world.</param>
        /// <param name="velocity">Sets the velocity of the movable object.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public MovableObject(Vector location, Vector velocity, string textureAtlasName, string atlasDataName, string subTextureID, Vector[] polyVertices = null)
            : base(location, textureAtlasName, atlasDataName, subTextureID, polyVertices)
        {
            throw new NotFiniteNumberException();
            //Initialize the atlas
//            InitializeAtlas(textureAtlasName, atlasDataName, subTextureID);//NOT NEEDED - DELETE

            //Initialize the rest of the movable object
            //            InitializeObj(location, textureAtlasName);//NOT NEEDED - DELETE
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets a value indicating if the object will follow its set destination point.
        /// NOTE: If true, the object will ignore all other move commands and only concentrate on moving to its DestinationPoint.
        /// </summary>
        public bool IsFollowing { get; set; }

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
        /// Moves the game object to the right.
        /// </summary>
        public void MoveRight()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            var previousAngle = Angle;

            Angle = 90;
            MoveAtSetAngle();
            Angle = previousAngle;
        }


        /// <summary>
        /// Moves the game object to the left.
        /// </summary>
        public void MoveLeft()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            var previousAngle = Angle;

            Angle = 270;
            MoveAtSetAngle();
            Angle = previousAngle;
        }


        /// <summary>
        /// Moves the game object up.
        /// </summary>
        public void MoveUp()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            var previousAngle = Angle;

            Angle = 0;
            MoveAtSetAngle();
            Angle = previousAngle;
        }


        /// <summary>
        /// Moves the game object down.
        /// </summary>
        public void MoveDown()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            var previousAngle = Angle;

            Angle = 180;
            MoveAtSetAngle();
            Angle = previousAngle;
        }


        /// <summary>
        /// Moves the game object up and to the right.
        /// </summary>
        public void MoveUpRight()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            MoveUp();
            MoveRight();
        }


        /// <summary>
        /// Moves the game object up and to the left.
        /// </summary>
        public void MoveUpLeft()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            MoveUp();
            MoveLeft();
        }


        /// <summary>
        /// Moves the game object down and to the right.
        /// </summary>
        public void MoveDownRight()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            MoveDown();
            MoveRight();
        }


        /// <summary>
        /// Moves the game object down and to the left.
        /// </summary>
        public void MoveDownLeft()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            MoveDown();
            MoveLeft();
        } 


        /// <summary>
        /// Moves the entity at the currently set angle/impulseType.
        /// </summary>
        public void MoveAtSetAngle()
        {
            var x = Position.X + Speed * (float)Math.Sin(GameMath.ToRadians(Angle));
            var y = Position.Y - Speed * (float)Math.Cos(GameMath.ToRadians(Angle));

            SetPosition(new Vector(x, y));
        }


        /// <summary>
        /// Rotates the game object clockwise.
        /// </summary>
        public void RotateCW()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            Angle += RotateSpeed;
        }


        /// <summary>
        /// Rotates the game object counter clockwise.
        /// </summary>
        public void RotateCCW()
        {
            //Ignore if IsFollowing is true
            if (IsFollowing) return;

            Angle -= RotateSpeed;
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


        /// <summary>
        /// Locks linear movement in the given direction.
        /// </summary>
        /// <param name="direction">The direction to lock.</param>
        public void LockLinearMovement(Direction direction)
        {
            _linearMovementLocks[direction] = true;
        }


        /// <summary>
        /// Unlocks the linear movement in the given direction.
        /// </summary>
        /// <param name="direction">The direction to unlock.</param>
        public void UnlockLinearMovement(Direction direction)
        {
            _linearMovementLocks[direction] = false;
        }


        /// <summary>
        /// Locks all linear movement.
        /// </summary>
        public void LockAllLinearMovement()
        {
            //Lock all linear movements.
            foreach (var item in _linearMovementLocks)
            {
                _linearMovementLocks[item.Key] = true;
            }
        }


        /// <summary>
        /// Unlocks all linear movement.
        /// </summary>
        public void UnlockAllLinearMovement()
        {
            //Lock all linear movements.
            foreach (var item in _linearMovementLocks)
            {
                _linearMovementLocks[item.Key] = false;
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Follows the set destination point.
        /// </summary>
        private void Follow()
        {
            //TODO: Get this feature working
        }
        #endregion


        #region Game Loop Methods
        /// <summary>
        /// Updates the moveable object.
        /// </summary>
        /// <param name="engineTime"></param>
        public override void OnUpdate(IEngineTiming engineTime)
        {
            #region Auto Rotation
            //TODO: Needs testing
            if (AutoRotate)
            {
                AutoRotate = false; //Disable the auto rotate so that the rotate method can be used

                switch (AutoRotateDirection)
                {
                    case RotationDirection.Clockwise:
                        RotateCW();
                        break;

                    case RotationDirection.CounterClockwise:
                        RotateCCW();
                        break;
                }

                AutoRotate = true; //Reenable auto rotate so that rotate methods cannot be used again
            }
            #endregion

            #region AutoMovement
            //If automove is true and the object is NOT set to follow the desitnation point
            if (AutoMove && !IsFollowing)
            {
                throw new NotImplementedException();
                switch (AutoDirection)
                {
                    case Direction.Up:
                        break;
                    case Direction.Down:
                        break;
                    case Direction.Left:
                        break;
                    case Direction.Right:
                        break;
                    case Direction.UpLeft:
                        break;
                    case Direction.UpRight:
                        break;
                    case Direction.DownLeft:
                        break;
                    case Direction.DownRight:
                        break;
                }
            }
            #endregion

            #region Is Follow Code
            //If set to follow
            if (IsFollowing)
            {
                Follow();
            }
            #endregion

            base.OnUpdate(engineTime);
        }
        #endregion


        #region Overridden Methods
        /// <summary>
        /// Initialize the MoveableObject.
        /// </summary>
        /// <param name="location">The location to draw the object.</param>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular.</param>
        protected override void InitializeObj(Vector location, string textureName = "", Vector[] polyVertices = null)
        {
            base.InitializeObj(location, textureName, polyVertices);

            LinearDeceleration = 1f; //Set the default linear deceleration

            var totalDirections = Enum.GetValues(typeof (Direction)).Length;

            //Add all of the linear locks with a default value of false/unlocked
            for (var i = Tools.GetEnumMin(typeof(Direction)); i <= totalDirections; i++)
            {
                _linearMovementLocks.Add((Direction)i, false);
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ScorpionEngine;
using ScorpionEngine.Objects;
using ScorpionEngine.Input;
using ScorpionEngine.Utils;
using SysStopWatch = System.Diagnostics.Stopwatch;

namespace ScorpTestGame
{
    /// <summary>
    /// Level 1 of the space shooter game.
    /// </summary>
    public class Level1 : World
    {
        #region Fields
        private bool _moveRock = false;

        private readonly ControllableObject _ship;
        private readonly MovableObject _enemy;
        private readonly GameObject _leftWall;
        private readonly GameObject _rightWall;
        private readonly GameObject _topWall;
        private readonly GameObject _bottomWall;

        private readonly ControllableObject _rock;
        private readonly ScorpionEngine.Input.KeyboardInput _keyboardInput;
        private readonly MouseInput _mouseInput;

        private Tweener _tweener = new Tweener();
        private SysStopWatch _timer = new SysStopWatch();
        private bool _useTimer = true;
        #endregion

        #region Constructors
        /// <summary>
        /// Setup Level 1.
        /// </summary>
        /// <param name="gravity">The gravity in the world.</param>
        public Level1(Vector gravity) : base(new Vector(0, 0f))
        {
            DebugDrawEnabled = true;
            Engine.MouseVisible = true;

            _keyboardInput = new ScorpionEngine.Input.KeyboardInput();
            _mouseInput = new MouseInput();
            _mouseInput.OnLeftButtonDown += _mouseInput_OnLeftButtonDown;
            _mouseInput.OnLeftButtonReleased += _mouseInput_OnLeftButtonReleased;
            var shipVerts = new Vector[3];

            shipVerts[0] = new Vector(20.5f, 0f);
            shipVerts[1] = new Vector(41f,41f);
            shipVerts[2] = new Vector(0f,41f);

            shipVerts = Tools.ConvertForOrigin(shipVerts);

            //Create the ship vertices
            _ship = new ControllableObject(new Vector(100, 400), "Ship", shipVerts)
            {
                MaxFollowSpeed = 10,
                MoveAtAngleSpeed = 5f,
                RotateSpeed = 100,
                LinearAcceleration = 1f,
                LinearDeceleration = 0.8f,
                RotationalAccelerationEnabled = true,
                LinearAccelerationEnabled = true,
                RotationEnabled = true,
                DestinationPoint = new Vector(300,0)
            };

            _ship.SetAllMaxLinearMovementSpeeds(40);

            var rockVerts = new Vector[8];

            rockVerts[0] = new Vector(10,0);
            rockVerts[1] = new Vector(23,0);
            rockVerts[2] = new Vector(34,11);
            rockVerts[3] = new Vector(34,25);
            rockVerts[4] = new Vector(22,37);
            rockVerts[5] = new Vector(11,37);
            rockVerts[6] = new Vector(0,26);
            rockVerts[7] = new Vector(0,10);

            rockVerts = Tools.ConvertForOrigin(rockVerts);

            _rock = new ControllableObject(new Vector(250, 200), "Rock", rockVerts)
            {
                RotateSpeed = 1
            };

            #region Create The Walls
            var verts = new Vector[4];

            verts[0] = new Vector(Engine.WindowWidth / 2f * -1, -10);
            verts[1] = new Vector(Engine.WindowWidth / 2f, -10);
            verts[2] = new Vector(Engine.WindowWidth / 2f, 10);
            verts[3] = new Vector(Engine.WindowWidth / 2f * -1, 10);

            _bottomWall = new GameObject(new Vector(Engine.WindowWidth / 2f,Engine.WindowHeight), "BottomWall", verts);
            #endregion

            //Add both objects to the world
            AddGameObj(_ship);
            AddGameObj(_rock);
            AddGameObj(_bottomWall);
//            AddGameObj(_enemy);
        }
        #endregion

        #region Event Methods
        private void _mouseInput_OnLeftButtonDown(object sender, ScorpionEngine.Input.MouseEventArgs e)
        {
            _ship.IsFollowing = true;
        }

        private void _mouseInput_OnLeftButtonReleased(object sender, ScorpionEngine.Input.MouseEventArgs e)
        {
            _ship.IsFollowing = false;
        }
        #endregion

        #region Game Loop Methods
        public override void OnUpdate(EngineTime engineTime)
        {
            _keyboardInput.UpdateCurrentState();
            _mouseInput.UpdateCurrentState();

            #region Move At Set Angle Code
            //            if (_keyboardInput.IsKeyDown(InputKeys.Right))
            //            {
            //                _ship.RotateCW();
            //            }
            //
            //            if (_keyboardInput.IsKeyDown(InputKeys.Left))
            //            {
            //                _ship.RotateCCW();
            //            }
            //
            //            if (_keyboardInput.IsKeyDown(InputKeys.Up))
            //            {
            //                _ship.MoveAtSetAngle();
            //            }
            #endregion

            _ship.DestinationPoint = _mouseInput.Position;

            #region Keyboard Code
            if (_keyboardInput.IsKeyPressed(InputKeys.OemTilde))
            {
                DebugDrawEnabled = !DebugDrawEnabled;
                _moveRock = !_moveRock;
            }

            if (_keyboardInput.IsKeyDown(InputKeys.LeftShift))
            {
                if (_keyboardInput.IsKeyDown(InputKeys.Right))
                {
                    _ship.RotateCW();
                }
                else if (_keyboardInput.IsKeyDown(InputKeys.Left))
                {
                    _ship.RotateCCW();
                }
            }
            else
            {
                if (_keyboardInput.IsKeyDown(InputKeys.Right))
                {
                    if(_useTimer) _timer.Start();

                    _ship.MoveRight();
                }

                if (_keyboardInput.IsKeyDown(InputKeys.Left))
                {
                    _ship.MoveLeft();
                }

                if (_keyboardInput.IsKeyDown(InputKeys.Up))
                {
                    _ship.MoveUp();
                }

                if (_keyboardInput.IsKeyDown(InputKeys.Down))
                {
                    _ship.MoveDown();
                }
            }
            #endregion

            if (_ship.Position.X > Engine.WindowWidth)
            {
                _timer.Stop();
                _useTimer = false;
                Engine.WindowTitle = $"Time Completed: {_timer.Elapsed.TotalSeconds}";
            }

            _keyboardInput.UpdatePreviousState();
            _mouseInput.UpdatePreviousState();

            base.OnUpdate(engineTime);
        }
        #endregion
    }
}

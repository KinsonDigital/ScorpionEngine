using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Objects;
using ScorpionEngine.Physics;
using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpTestGame
{
    public class Level1 : GameScene
    {
        private DynamicEntity _ship;
        private Keyboard _keyboard;
        private Mouse _mouse;
        private GameText _message;


        public Level1() : base(new Vector(0f, 0f))
        {
            
        }


        public override void Initialize()
        {
            _keyboard = new Keyboard();

            var fallingRectVertices = new Vector[3]
            {
                new Vector(-0, -21),
                new Vector(21, 21),
                new Vector(-21, 21)
            };

            _ship = new DynamicEntity(fallingRectVertices, new Vector(330, 200))
            {
                DebugDrawEnabled = true,
                MaxLinearSpeed = 0.5f,
                MaxAngularSpeed = 0.25f,
                AngularDeceleration = 0.25f
            };

            PhysicsWorld.AddEntity(_ship);
            AddEntity(_ship);


            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _ship.Texture = contentLoader.LoadTexture("Ship");
            _message = contentLoader.LoadText("MyGameText");
            _message.Text = "Hello World";

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            ProcessKeys();

            _message.Text = $"Ship Location: X: {_ship.Position.X} - {_ship.Position.Y}";

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _ship.Render(renderer);
            renderer.Render(_message, 100, 100, new GameColor(255, 255, 255, 255));

            base.Render(renderer);
        }


        public override void UnloadContent(ContentLoader contentLoader)
        {
            throw new NotImplementedException();
        }


        #region Private Methods
        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            if (_keyboard.IsKeyDown(InputKeys.Up))
            {
                _ship.MoveUp(4);
            }

            if (_keyboard.IsKeyDown(InputKeys.Down))
            {
                _ship.MoveDown(4);
            }

            if (_keyboard.IsKeyDown(InputKeys.Right))
            {
                _ship.MoveRight(4);
            }

            if (_keyboard.IsKeyDown(InputKeys.Left))
            {
                _ship.MoveLeft(4);
            }

            if (_keyboard.IsKeyDown(InputKeys.D))
            {
                _ship.RotateCW(0.25f);
            }

            if (_keyboard.IsKeyDown(InputKeys.A))
            {
                _ship.RotateCCW(0.25f);
            }

            if (_keyboard.IsKeyDown(InputKeys.End))
            {
                _ship.StopMovement();
            }

            _keyboard.UpdatePreviousState();
        }
        #endregion
    }
}

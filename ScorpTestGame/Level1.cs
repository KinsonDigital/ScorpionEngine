using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Entities;
using ScorpionEngine.Physics;
using ScorpionEngine.Scene;
using ScorpionEngine.UI;
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
        private UIText _shipLocation;


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

            _shipLocation = new UIText();
            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _ship.Texture = contentLoader.LoadTexture("Ship");

            var shipLocationLabelText = contentLoader.LoadText("MyGameText");
            shipLocationLabelText.Text = "Location";
            shipLocationLabelText.Color = new GameColor(255, 255, 255, 255);

            var shipLocationValueText = contentLoader.LoadText("MyGameText");
            shipLocationValueText.Text = "NA";
            shipLocationValueText.Color = new GameColor(255, 255, 255, 255);

            _shipLocation.LabelText = shipLocationLabelText;
            _shipLocation.ValueText = shipLocationValueText;

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            ProcessKeys();

            _shipLocation.SetValueText($"X: {_ship.Position.X} - Y: {_ship.Position.Y}");

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _ship.Render(renderer);

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

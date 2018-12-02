using ScorpionEngine.Scene;
using ScorpionEngine.UI;
using System;
using ScorpionCore.Graphics;
using ScorpionCore.Content;
using ScorpionCore;
using ScorpionCore.Input;

namespace ScorpTestGame.Scenes
{
    public class Level1 : GameScene
    {
        private PlayerShip _ship;
        private Keyboard _keyboard;
        private Mouse _mouse;
        private UIText _shipLocation;


        public Level1() : base(new Vector(0f, 0f))
        {
            
        }


        public override void Initialize()
        {
            _keyboard = new Keyboard();

            _ship = new PlayerShip();

            _ship.Initialize();
            _shipLocation = new UIText();

            AddEntity(_ship);

            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _ship.LoadContent(contentLoader);

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

            _shipLocation.SetValueText($"X: {Math.Round(_ship.Position.X, 2)} - Y: {Math.Round(_ship.Position.Y, 2)}");

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _ship.Render(renderer);

            _shipLocation.Render(renderer);

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

            //if (_keyboard.IsKeyDown(InputKeys.Up))
            //{
            //    _myEntity.MoveUp(1f);
            //}

            //if (_keyboard.IsKeyDown(InputKeys.Down))
            //{
            //    _ship.MoveDown(4);
            //}

            //if (_keyboard.IsKeyDown(InputKeys.Right))
            //{
            //    _ship.MoveRight(4);
            //}

            //if (_keyboard.IsKeyDown(InputKeys.Left))
            //{
            //    _ship.MoveLeft(4);
            //}

            //if (_keyboard.IsKeyDown(InputKeys.D))
            //{
            //    _ship.RotateCW(0.25f);
            //}

            //if (_keyboard.IsKeyDown(InputKeys.A))
            //{
            //    _ship.RotateCCW(0.25f);
            //}

            //if (_keyboard.IsKeyDown(InputKeys.End))
            //{
            //    _ship.StopMovement();
            //}

            _keyboard.UpdatePreviousState();
        }
        #endregion
    }
}

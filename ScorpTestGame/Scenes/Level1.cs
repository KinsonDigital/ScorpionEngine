using KDScorpionEngine.Scene;
using System;
using KDScorpionCore.Graphics;
using KDScorpionCore.Content;
using KDScorpionCore;
using KDScorpionUI;
using KDScorpionEngine.Input;
using KDScorpionEngine.Graphics;

namespace ScorpTestGame.Scenes
{
    public class Level1 : GameScene
    {
        private PlayerShip _ship;
        private Keyboard _keyboard;
        private readonly Mouse _mouse = new Mouse();
        private UIText _shipPosition;
        private UIText _mousePosition;


        public Level1() : base(new Vector(0f, 0f))
        {
            
        }


        public override void Initialize()
        {
            _keyboard = new Keyboard();

            _ship = new PlayerShip();

            _ship.Initialize();
            _shipPosition = new UIText();
            _mousePosition = new UIText()
            {
                Position = new Vector(0, 20)
            };

            AddEntity(_ship);

            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _ship.LoadContent(contentLoader);

            var shipPositionLabelText = contentLoader.LoadText("MyGameText");
            shipPositionLabelText.Text = "Ship Pos";
            shipPositionLabelText.Color = new GameColor(255, 255, 228, 132);

            var shipPositionValueText = contentLoader.LoadText("MyGameText");
            shipPositionValueText.Text = "NA";
            shipPositionValueText.Color = new GameColor(255, 255, 255, 255);

            _shipPosition.LabelText = shipPositionLabelText;
            _shipPosition.ValueText = shipPositionValueText;


            var mousePositionLabelText = contentLoader.LoadText("MyGameText");
            mousePositionLabelText.Text = "Mouse Pos";
            mousePositionLabelText.Color = new GameColor(255, 255, 228, 132);

            var mousePositionValueText = contentLoader.LoadText("MyGameText");
            mousePositionValueText.Text = "NA";
            mousePositionValueText.Color = new GameColor(255, 255, 255, 255);

            _mousePosition.LabelText = mousePositionLabelText;
            _mousePosition.ValueText = mousePositionValueText;

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            ProcessKeys();
            _mouse.UpdateCurrentState();

            _shipPosition.SetValueText($"X: {Math.Round(_ship.Position.X, 2)} - Y: {Math.Round(_ship.Position.Y, 2)}");
            _mousePosition.SetValueText($"X: {_mouse.X} - Y: {_mouse.Y}");

            _mouse.UpdatePreviousState();
            base.Update(engineTime);
        }


        public override void Render(GameRenderer renderer)
        {
            _shipPosition.Render(renderer);
            _mousePosition.Render(renderer);

            base.Render(renderer);
        }


        public override void UnloadContent(ContentLoader contentLoader) => throw new NotImplementedException();


        #region Private Methods
        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            _keyboard.UpdatePreviousState();
        }
        #endregion
    }
}

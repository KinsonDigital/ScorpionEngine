// <copyright file="Level1.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame.Scenes
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;
    using Raptor.UI;

    /// <summary>
    /// Level 1 scene.
    /// </summary>
    public class Level1 : GameScene
    {
        private readonly Mouse mouse = new Mouse();
        private PlayerShip ship;
        private Keyboard keyboard;
        private UIText shipPosition;
        private UIText mousePosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Level1"/> class.
        /// </summary>
        public Level1()
            : base(new Vector2(0f, 0f))
        {
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        public override void Initialize()
        {
            this.keyboard = new Keyboard();

            this.ship = new PlayerShip();

            this.ship.Initialize();
            this.shipPosition = new UIText();
            this.mousePosition = new UIText()
            {
                Position = new Vector2(0, 20),
            };

            AddEntity(this.ship);

            base.Initialize();
        }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <param name="contentLoader">Loads the content items.</param>
        public override void LoadContent(ContentLoader contentLoader)
        {
            if (contentLoader is null)
                throw new ArgumentNullException(nameof(contentLoader), "The parameter must not be null.");

            this.ship.LoadContent(contentLoader);

            var shipPositionLabelText = contentLoader.LoadText("MyGameText");
            shipPositionLabelText.Text = "Ship Pos";
            shipPositionLabelText.Color = new GameColor(255, 255, 228, 132);

            var shipPositionValueText = contentLoader.LoadText("MyGameText");
            shipPositionValueText.Text = "NA";
            shipPositionValueText.Color = new GameColor(255, 255, 255, 255);

            this.shipPosition.LabelText = shipPositionLabelText;
            this.shipPosition.ValueText = shipPositionValueText;

            var mousePositionLabelText = contentLoader.LoadText("MyGameText");
            mousePositionLabelText.Text = "Mouse Pos";
            mousePositionLabelText.Color = new GameColor(255, 255, 228, 132);

            var mousePositionValueText = contentLoader.LoadText("MyGameText");
            mousePositionValueText.Text = "NA";
            mousePositionValueText.Color = new GameColor(255, 255, 255, 255);

            this.mousePosition.LabelText = mousePositionLabelText;
            this.mousePosition.ValueText = mousePositionValueText;

            base.LoadContent(contentLoader);
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="engineTime">Updates the scene objects.</param>
        public override void Update(EngineTime engineTime)
        {
            ProcessKeys();
            this.mouse.UpdateCurrentState();

            this.shipPosition.SetValueText($"X: {Math.Round(this.ship.Position.X, 2)} - Y: {Math.Round(this.ship.Position.Y, 2)}");
            this.mousePosition.SetValueText($"X: {this.mouse.X} - Y: {this.mouse.Y}");

            this.mouse.UpdatePreviousState();
            base.Update(engineTime);
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <param name="renderer">Renders the graphics in the scene.</param>
        public override void Render(GameRenderer renderer)
        {
            this.shipPosition.Render(renderer);
            this.mousePosition.Render(renderer);

            base.Render(renderer);
        }

        /// <summary>
        /// Unloads the content from the scene.
        /// </summary>
        /// <param name="contentLoader">Used to unload content.</param>
        public override void UnloadContent(ContentLoader contentLoader) => throw new NotImplementedException();

        private void ProcessKeys()
        {
            this.keyboard.UpdateCurrentState();

            this.keyboard.UpdatePreviousState();
        }
    }
}

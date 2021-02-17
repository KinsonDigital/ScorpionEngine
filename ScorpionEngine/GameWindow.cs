// <copyright file="GameWindow.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using KDScorpionEngine.Graphics;
    using Raptor;
    using Raptor.Desktop;

    public class GameWindow : Window
    {
        private readonly Renderer renderer;
        private readonly GameTime gameTime;

        public GameWindow(IWindow window)
            : base(window)
        {
            this.renderer = new Renderer(window.Width, window.Height);
            this.gameTime = new GameTime();
        }

        public Action? InitAction { get; set; }

        public Action? LoadAction { get; set; }

        public Action<GameTime>? UpdateAction { get; set; }

        public Action<Renderer>? RenderAction { get; set; }

        public override void OnLoad()
        {
            InitAction?.Invoke();
            LoadAction?.Invoke();
            base.OnLoad();
        }

        public override void OnUpdate(FrameTime frameTime)
        {
            this.gameTime.UpdateTotalGameTime(frameTime.ElapsedTime.Milliseconds);

            UpdateAction?.Invoke(this.gameTime);
            base.OnUpdate(frameTime);
        }

        public override void OnDraw(FrameTime frameTime)
        {
            RenderAction?.Invoke(this.renderer);
            base.OnDraw(frameTime);
        }
    }
}

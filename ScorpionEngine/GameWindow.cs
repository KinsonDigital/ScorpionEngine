// <copyright file="GameWindow.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using KDScorpionEngine.Graphics;
    using Raptor;
    using Raptor.Desktop;
    using Raptor.Graphics;

    /// <summary>
    /// A game window where the game interaction and rendering occurs.
    /// </summary>
    public class GameWindow : Window
    {
        private readonly Renderer renderer;
        private readonly GameTime gameTime;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        /// <param name="window">The internal window implementation.</param>
        /// <param name="spriteBatch">Used to render sprites.</param>
        public GameWindow(IWindow window, ISpriteBatch spriteBatch)
            : base(window)
        {
            this.renderer = new Renderer(spriteBatch, window.Width, window.Height);
            this.gameTime = new GameTime();
        }

        /// <summary>
        /// Gets or sets the delegate that will be invoked once during initialization of the window.
        /// </summary>
        public Action? InitAction { get; set; }

        /// <summary>
        /// Gets or sets the delegate that will be invoked once during the loading of the window.
        /// </summary>
        public Action? LoadAction { get; set; }

        /// <summary>
        /// Gets or sets the delegate that will be invoked on an interval to update game logic.
        /// </summary>
        public Action<GameTime>? UpdateAction { get; set; }

        /// <summary>
        /// Gets or sets the delegate that will be invoked on an intervale to render to the window.
        /// </summary>
        /// <remarks>This always occurs after the <see cref="UpdateAction"/>.</remarks>
        public Action<Renderer>? RenderAction { get; set; }

        /// <inheritdoc/>
        public override void OnLoad()
        {
            InitAction?.Invoke();
            LoadAction?.Invoke();
            base.OnLoad();
        }

        /// <inheritdoc/>
        public override void OnUpdate(FrameTime frameTime)
        {
            this.gameTime.UpdateTotalGameTime(frameTime.ElapsedTime.Milliseconds);

            UpdateAction?.Invoke(this.gameTime);
            base.OnUpdate(frameTime);
        }

        /// <inheritdoc/>
        public override void OnDraw(FrameTime frameTime)
        {
            RenderAction?.Invoke(this.renderer);
            base.OnDraw(frameTime);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                base.Dispose(disposing);
            }

            this.isDisposed = true;
        }
    }
}

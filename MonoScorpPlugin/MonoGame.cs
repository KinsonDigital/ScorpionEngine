﻿using KDScorpionCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using KDScorpionCore.Plugins;

namespace MonoScorpPlugin
{
    /// <summary>
    /// The internal implementation of a <see cref="Game"/> object from MonoGame.
    /// This provides the functionality required to use MonoGame internally for this plugin
    /// implementation.
    /// </summary>
    internal class MonoGame : Game, IEngineEvents
    {
        #region Private Fields
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        #endregion


        #region Events
        /// <summary>
        /// Occurs once every frame before the OnDraw event before the OnDraw event is invoked.
        /// </summary>
        public event EventHandler<OnUpdateEventArgs> OnUpdate;

        /// <summary>
        /// Occurs once every frame after the OnUpdate event has been been invoked.
        /// </summary>
        public event EventHandler<OnRenderEventArgs> OnRender;

        /// <summary>
        /// Occurs one time during game initialization. This event is fired before the OnLoadContent event is fired. Add initialization code here.
        /// </summary>
        public event EventHandler OnInitialize;

        /// <summary>
        /// Occurs one time during game intialization after the OnInit event is fired.
        /// </summary>
        public event EventHandler OnLoadContent;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MonoGame"/>.
        /// </summary>
        public MonoGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            base.Content.RootDirectory = "Content";
            Content = base.Content;

            Renderer = new MonoRenderer();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the renderer implementation to render the <see cref="Texture2D"/>s and
        /// <see cref="SpriteFont"/>s to the graphics surface.
        /// </summary>
        public IRenderer Renderer { get; set; }

        /// <summary>
        /// Gets or sets the mono engine time implementation to pass into the public events of this class.
        /// </summary>
        public MonoEngineTime EngineTime { get; set; }

        /// <summary>
        /// Gets the custom content manager to load and unload content.
        /// </summary>
        public new static ContentManager Content { get; private set; }

        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public int WindowWidth
        {
            get => _graphicsDeviceManager.PreferredBackBufferWidth;
            set => _graphicsDeviceManager.PreferredBackBufferWidth = value;
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public int WindowHeight
        {
            get => _graphicsDeviceManager.PreferredBackBufferHeight;
            set => _graphicsDeviceManager.PreferredBackBufferHeight = value;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the <see cref="Game"/> engine.
        /// </summary>
        public void Start() => Run(GameRunBehavior.Synchronous);

        /// <summary>
        /// Sets the frames per second to the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to use.</param>
        public void SetFPS(float value) => TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / value);
        #endregion


        #region MonoGame Methods
        /// <summary>
        /// Fires the <see cref="OnInitialize"/> event.  Runs game initialization code.
        /// </summary>
        protected override void Initialize()
        {
            _graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphicsDeviceManager.ApplyChanges();

            OnInitialize?.Invoke(this, new EventArgs());

            base.Initialize();
        }


        /// <summary>
        /// Fires the <see cref="OnLoadContent"/> event.  Runs content loading code.
        /// </summary>
        protected override void LoadContent()
        {
            OnLoadContent?.Invoke(this, new EventArgs());

            base.LoadContent();
        }


        /// <summary>
        /// Fires the <see cref="OnUpdate"/> event to update game logic.
        /// </summary>
        /// <param name="gameTime">The current game time information.</param>
        protected override void Update(GameTime gameTime)
        {
            EngineTime = new MonoEngineTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);

            OnUpdate?.Invoke(this, new OnUpdateEventArgs(EngineTime));

            base.Update(gameTime);
        }


        /// <summary>
        /// Fires the <see cref="OnRender"/> event to render the graphics to the graphics surface.
        /// </summary>
        /// <param name="gameTime">The current game time information.</param>
        protected override void Draw(GameTime gameTime)
        {
            var engineTime = new MonoEngineTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);

            OnRender?.Invoke(this, new OnRenderEventArgs(Renderer));

            base.Draw(gameTime);
        }
        #endregion
    }
}
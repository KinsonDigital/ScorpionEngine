using ScorpionCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using ScorpionCore.Plugins;
using ScorpionCore.Graphics;

namespace MonoScorpPlugin
{
    internal class MonoGame : Game, IEngineEvents
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

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
        public MonoGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            base.Content.RootDirectory = "Content";
            Content = base.Content;

            Renderer = new MonoRenderer();
        }
        #endregion


        #region Props
        public IRenderer Renderer { get; set; }

        public MonoEngineTime EngineTime { get; set; }

        public new static ContentManager Content { get; private set; }

        public static GraphicsDevice MonoGraphicsDevice { get; set; }

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
        public void Start()
        {
            Run(GameRunBehavior.Synchronous);
        }


        public void SetFPS(float value)
        {
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / value);
        }
        #endregion


        #region MonoGame Methods
        /// <summary>
        /// Fires the OnInit event.  Runs game initialization code.
        /// </summary>
        protected override void Initialize()
        {
            _graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphicsDeviceManager.ApplyChanges();

            OnInitialize?.Invoke(this, new EventArgs());

            base.Initialize();
        }


        /// <summary>
        /// Fires the OnLoadContent event.  Runs content loading code.
        /// </summary>
        protected override void LoadContent()
        {
            OnLoadContent?.Invoke(this, new EventArgs());

            base.LoadContent();
        }


        /// <summary>
        /// Fires the OnUpdate event to update game logic.
        /// </summary>
        /// <param name="gameTime">The current game time information.</param>
        protected override void Update(GameTime gameTime)
        {
            EngineTime = new MonoEngineTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);

            OnUpdate?.Invoke(this, new OnUpdateEventArgs(EngineTime));

            base.Update(gameTime);
        }


        /// <summary>
        /// Fires the OnDraw event to render the graphics of the game.
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
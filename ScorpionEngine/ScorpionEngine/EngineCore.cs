using Microsoft.Xna.Framework;
using System;
using System.Runtime;
using ScorpionEngine.EventArguments;

namespace ScorpionEngine
{
    /// <summary>
    /// The core of the engine. Drives all of the game loop methods.
    /// </summary>
    public class EngineCore : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        #region Events
        /// <summary>
        /// Occurs once every frame before the OnDraw event before the OnDraw event is invoked.
        /// </summary>
        public event EventHandler<OnUpdateDrawEventArgs> OnUpdate;

        /// <summary>
        /// Occurs once every frame after the OnUpdate event has been been invoked.
        /// </summary>
        public event EventHandler<OnUpdateDrawEventArgs> OnDraw;

        /// <summary>
        /// Occurs one time during game initialization. This event is fired before the OnLoadContent event is fired. Add initialization code here.
        /// </summary>
        public event EventHandler OnInit;

        /// <summary>
        /// Occurs one time during game intialization after the OnInit event is fired.
        /// </summary>
        public event EventHandler OnLoadContent;
        #endregion

        public EngineCore() : base()
        {
        }

        #region One Time Run Methods
        /// <summary>
        /// Fires the OnInit event.  Runs game initialization code.
        /// </summary>
        protected override void Initialize()
        {
            OnInit?.Invoke(this, new EventArgs());

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
        #endregion

        #region Game Loops Methods
        /// <summary>
        /// Fires the OnUpdate event to update game logic.
        /// </summary>
        /// <param name="gameTime">The current game time information.</param>
        protected override void Update(GameTime gameTime)
        {
            var engineTime = new EngineTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);

            OnUpdate?.Invoke(this, new OnUpdateDrawEventArgs(engineTime));

            base.Update(gameTime);
        }

        /// <summary>
        /// Fires the OnDraw event to render the graphics of the game.
        /// </summary>
        /// <param name="gameTime">The current game time information.</param>
        protected override void Draw(GameTime gameTime)
        {
            var engineTime = new EngineTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);

            OnDraw?.Invoke(this, new OnUpdateDrawEventArgs(engineTime));

            base.Draw(gameTime);
        }
        #endregion
    }
}
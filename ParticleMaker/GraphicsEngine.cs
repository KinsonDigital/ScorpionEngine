using KDParticleEngine;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace ParticleMaker
{
    public class GraphicsEngine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IntPtr _windowsHandle;
        private Control _window;
        private ParticleEngine _particleEngine;
        private ContentLoader _contentLoader;
        private ParticleRenderer _particleRenderer;
        private Renderer _renderer;
        private ParticleTextureLoader _particleTextureLoader;
        private bool _shuttingDown = false;


        #region Constructors
        public GraphicsEngine(IntPtr windowHandle, ParticleEngine particleEngine)
        {
            _particleEngine = particleEngine;

            _windowsHandle = windowHandle;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreparingDeviceSettings += _graphics_PreparingDeviceSettings;
        }
        #endregion


        #region Public Methods
        public void Stop()
        {
            _shuttingDown = true;

            Exit();
        }
        #endregion


        #region Event Methods
        private void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _windowsHandle;
        }
        #endregion


        #region Protected Methods
        protected override void Initialize()
        {
            Window.Position = new Point(-30000, Window.Position.Y);
            _window = Control.FromHandle(Window.Handle);

            _particleTextureLoader = new ParticleTextureLoader(_graphics.GraphicsDevice);
            _contentLoader = new ContentLoader(_particleTextureLoader);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _particleRenderer = new ParticleRenderer(_spriteBatch);
            _renderer = new Renderer(_particleRenderer);

            var starTexture = _contentLoader.LoadTexture("Star");

            _particleEngine.AddTexture(starTexture);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            if (_shuttingDown)
                return;

            if (_window.Visible)
                _window.Hide();

            var engineTime = new EngineTime() { ElapsedEngineTime = gameTime.ElapsedGameTime };

            _particleEngine.Update(engineTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            if (_shuttingDown)
                return;

            GraphicsDevice.Clear(new Color(40, 40, 40, 255));

            _spriteBatch.Begin();

            _spriteBatch.DrawRectangle(new Rectangle(100, 150, 100, 100), Color.CornflowerBlue);

            _particleEngine.Render(_renderer);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticleMaker
{
    public class GraphicsEngine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IntPtr _windowsHandle;
        private Control _window;


        public GraphicsEngine(IntPtr windowHandle)
        {
            _windowsHandle = windowHandle;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreparingDeviceSettings += _graphics_PreparingDeviceSettings;
            Content.RootDirectory = "Content";
        }


        private void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _windowsHandle;
        }


        protected override void Initialize()
        {
            Window.Position = new Point(-3000, Window.Position.Y);
            _window = Control.FromHandle(Window.Handle);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            if (_window.Visible)
                _window.Hide();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 40, 40, 255));

            _spriteBatch.Begin();

            _spriteBatch.DrawRectangle(new Rectangle(100, 150, 100, 100), Color.CornflowerBlue);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}

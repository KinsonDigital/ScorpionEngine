using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionCore.Plugins;

namespace MonoScorpPlugin
{
    public class MonoEngineCore : IEngineCore
    {
        private MonoGame _monoGame;
        private IRenderer _renderer;

        public event EventHandler<OnUpdateEventArgs> OnUpdate;
        public event EventHandler<EventArgs> OnRender;
        public event EventHandler OnInitialize;
        public event EventHandler OnLoadContent;


        public MonoEngineCore()
        {
            _monoGame = new MonoGame();

            //Load the injecter plugin
            _renderer = PluginLoader.GetPluginByType<IRenderer>();

            _monoGame.OnInitialize += _monoGame_OnInitialize;
            _monoGame.OnLoadContent += _monoGame_OnLoadContent;
            _monoGame.OnUpdate += _monoGame_OnUpdate;
            _monoGame.OnRender += _monoGame_OnRender;
        }


        #region Props
        public int WindowWidth
        {
            get => 1;
            set
            {
                //TODO: Look into if this should change the window width
            }
        }

        public int WindowHeight
        {
            get => 2;
            set
            {
                //TODO: Look into if this should change the window width
            }
        }

        public IRenderer Renderer
        {
            get => _renderer;
            set { throw new Exception("This property setter has been purposly setup to not be used."); }
        }
        #endregion


        #region Public Methods
        public void Start()
        {
            _monoGame.Start();
        }


        public void Stop()
        {
            _monoGame.Dispose();
            _monoGame.Exit();
        }


        public void SetFPS(float value)
        {
            _monoGame.SetFPS(value);
        }


        public void Dispose()
        {
        }
        #endregion


        #region Private Methods
        private void _monoGame_OnInitialize(object sender, EventArgs e)
        {
            //Inject the graphics device into the renderer
            _renderer.InjectData(_monoGame.GraphicsDevice);

            OnInitialize?.Invoke(sender, e);
        }


        //TODO: Create an OnLoadContentEventArgs and use here.
        //This new class will contain the content manager
        private void _monoGame_OnLoadContent(object sender, EventArgs e)
        {
            OnLoadContent?.Invoke(sender, e);
        }


        private void _monoGame_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            OnUpdate?.Invoke(sender, e);
        }


        private void _monoGame_OnRender(object sender, EventArgs e)
        {
            OnRender?.Invoke(sender, e);
        }

        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }

        public T GetData<T>() where T : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

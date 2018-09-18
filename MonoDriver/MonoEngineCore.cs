using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using ScorpionEngine.Events;
using ScorpionEngine.Scene;

namespace MonoDriver
{
    public class MonoEngineCore : IEngineCore
    {
        private MonoCoreDriver _coreDriver;

        public event EventHandler<OnUpdateEventArgs> OnUpdate;
        public event EventHandler<OnRenderEventArgs<MonoRenderer, Texture2D>> OnRender;
        public event EventHandler OnInitialize;
        public event EventHandler OnLoadContent;

        public MonoEngineCore(IScene scene)
        {
            Scene = scene;
            _coreDriver = new MonoCoreDriver();

            _coreDriver.OnInitialize += _coreDriver_OnInitialize;
            _coreDriver.OnLoadContent += _coreDriver_OnLoadContent;
            _coreDriver.OnUpdate += _coreDriver_OnUpdate;
            _coreDriver.OnRender += _coreDriver_OnRender;
        }


        #region Props
        public IScene Scene { get; private set; }

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

        public IContentLoader Content { get; set; }
        #endregion


        #region Public Methods
        public void SetScene(IScene scene)
        {
            Scene = scene;
        }


        public void Start()
        {
            _coreDriver.Start();
        }


        public void SetFPS(float value)
        {
            _coreDriver.SetFPS(value);
        }


        public void Dispose()
        {
        }
        #endregion


        #region Private Methods
        private void _coreDriver_OnInitialize(object sender, EventArgs e)
        {
            Scene.Initialize();
            OnInitialize?.Invoke(sender, e);
        }


        //TODO: Create an OnLoadContentEventArgs and use here.
        //This new class will contain the content manager
        private void _coreDriver_OnLoadContent(object sender, EventArgs e)
        {
            Scene.LoadContent(Content);
            OnLoadContent?.Invoke(sender, e);
        }


        private void _coreDriver_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            Scene.Update(e.EngineTime);

            OnUpdate?.Invoke(sender, e);
        }


        private void _coreDriver_OnRender(object sender, OnRenderEventArgs<MonoRenderer, Texture2D> e)
        {
            OnRender?.Invoke(sender, e);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoScorpPlugin
{
    public class MonoEngineCore : IEngineCore
    {
        private MonoGame _monoGame;

        public event EventHandler<OnUpdateEventArgs> OnUpdate;
        public event EventHandler<EventArgs> OnRender;
        public event EventHandler OnInitialize;
        public event EventHandler OnLoadContent;


        public MonoEngineCore()
        {
            _monoGame = new MonoGame();

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
        #endregion


        #region Public Methods
        public void Start()
        {
            _monoGame.Start();
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
        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Plugins
{
    /// <summary>
    /// Provides the core of a game engine.
    /// </summary>
    public interface IEngineCore : IDisposable, IEngineEvents, IPlugin
    {
        #region Props
        int WindowWidth { get; set; }

        int WindowHeight { get; set; }

        IRenderer Renderer { get; set; }
        #endregion


        #region Methods
        void Start();

        void Stop();

        void SetFPS(float value);

        //TODO: Add stop method here
        #endregion
    }
}
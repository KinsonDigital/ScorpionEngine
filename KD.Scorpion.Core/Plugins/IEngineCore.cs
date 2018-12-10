using System;

namespace KDScorpionCore.Plugins
{
    /// <summary>
    /// Provides the core of a game engine.
    /// </summary>
    public interface IEngineCore : IDisposable, IEngineEvents, IPlugin
    {
        //TODO: Add docs to this interface
        #region Props
        int WindowWidth { get; set; }

        int WindowHeight { get; set; }

        IRenderer Renderer { get; set; }
        #endregion


        #region Methods
        void Start();

        void Stop();

        void SetFPS(float value);

        bool IsRunning();
        //TODO: Add stop method here
        #endregion
    }
}

using ScorpionEngine.Content;
using ScorpionEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Core
{
    /// <summary>
    /// Provides the core of a game engine.
    /// </summary>
    public interface IEngineCore : IDisposable
    {
        #region Props
        int WindowWidth { get; set; }

        int WindowHeight { get; set; }

        IContentLoader Content { get; set; }
        #endregion


        #region Methods
        void Start();

        void SetFPS(float value);

        //TODO: Add stop method here
        #endregion
    }
}

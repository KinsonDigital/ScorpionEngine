using ScorpionCore.Plugins;
using System;

namespace ScorpionEngine.Events
{
    public class OnRenderEventArgs : EventArgs
    {
        #region Constructor
        /// <summary>
        /// Creates a new instance of OnEntityMovedEventArgs.
        /// </summary>
        /// <param name="renderer">The renderer drawing the graphics.</param>
        public OnRenderEventArgs(IRenderer renderer)
        {
            Renderer = renderer;
        }
        #endregion


        public IRenderer Renderer { get; set; }
    }
}

using ScorpionEngine.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Events
{
    public class OnRenderEventArgs<T> : EventArgs //where T : IRenderer
    {
        #region Constructor
        /// <summary>
        /// Creates a new instance of OnEntityMovedEventArgs.
        /// </summary>
        /// <param name="renderer">The renderer drawing the graphics.</param>
        public OnRenderEventArgs(T renderer)
        {
            Renderer = renderer;
        }
        #endregion


        public T Renderer { get; set; }
    }
}

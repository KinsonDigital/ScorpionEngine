using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    /// <summary>
    /// Provides functionality for game content to be rendered to the screen.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Renders things to the screen.
        /// </summary>
        /// <param name="renderer">The rederer to use for rendering.</param>
        void Render(IRenderer renderer);
    }
}

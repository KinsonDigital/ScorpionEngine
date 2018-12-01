using ScorpionCore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionUI
{
    public interface IBorder : IUpdatable, IRenderable
    {
        #region Props
        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        GameColor Color { get; set; }

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or ses the height of the border.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        int Thickness { get; set; }
        #endregion
    }
}

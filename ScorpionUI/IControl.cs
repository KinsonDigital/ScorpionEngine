using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionUI
{
    /// <summary>
    /// A user interface object that can be updated and rendered to the screen.
    /// </summary>
    interface IControl : IUpdatable, IRenderable
    {
        #region Props
        /// <summary>
        /// Gets or sets the position of the <see cref="IControl"/> on the screen.
        /// </summary>
        Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="IControl"/>.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the <see cref="IControl"/>.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IBorder"/> of the control.
        /// </summary>
        IBorder Border { get; set; }
        #endregion
    }
}

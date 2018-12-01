using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using ScorpionCore.Graphics;

namespace ScorpionUI
{
    public class Border : IBorder
    {
        #region Props
        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        public GameColor Color { get; set; }

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or ses the height of the border.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        public int Thickness { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the <see cref="Border"/>.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the engine since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
        }


        /// <summary>
        /// Renders the <see cref="Border"/> to the screen.
        /// </summary>
        /// <param name="renderer">Renders the object the screen.</param>
        public void Render(Renderer renderer)
        {
        }
        #endregion
    }
}

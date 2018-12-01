﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using ScorpionCore.Graphics;

namespace ScorpionUI
{
    public class Button : IControl
    {
        #region Props
        /// <summary>
        /// Gets or sets the position of the <see cref="Button"/> on the screen.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="Button"/>.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the <see cref="Button"/>.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the texture of the button.
        /// </summary>
        public Texture Texture { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Border"/> of the control.
        /// </summary>
        internal IBorder Border { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the <see cref="Button"/>.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the engine since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
        }


        /// <summary>
        /// Renders the <see cref="Button"/> to the screen.
        /// </summary>
        /// <param name="renderer">Renders the <see cref="Button"/>.</param>
        public void Render(Renderer renderer)
        {

        }
        #endregion
    }
}

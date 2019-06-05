﻿using KDScorpionCore.Plugins;

namespace KDScorpionCore
{
    /// <summary>
    /// Text that can be rendered to the screen.
    /// </summary>
    public interface IText : IPlugin
    {
        #region Props
        /// <summary>
        /// Gets the width of the text.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the text.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        byte[] Color { get; set; }
        #endregion
    }
}

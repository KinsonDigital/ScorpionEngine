﻿using ScorpionCore;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Represents the state of the mouse.
    /// </summary>
    public struct MouseInputState
    {
        #region Props
        /// <summary>
        /// Gets or sets the down state of the left mouse button.
        /// </summary>
        public bool LeftButtonDown { get; set; }

        /// <summary>
        /// Gets or sets the down state of the middle mouse button.
        /// </summary>
        public bool MiddleButtonDown { get; set; }

        /// <summary>
        /// Gets or sets the down state of the right mouse button.
        /// </summary>
        public bool RightButtonDown { get; set; }

        /// <summary>
        /// Gets or sets the X position of the mouse.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of the mouse.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the position of the mouse.
        /// </summary>
        public Vector Position
        {
            get
            {
                return new Vector(X, Y);
            }
            set
            {
                X = (int)value.X;
                Y = (int) value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the position value of the mouse scroll wheel.
        /// </summary>
        public int ScrollWheelValue { get; set; }
        #endregion
    }
}

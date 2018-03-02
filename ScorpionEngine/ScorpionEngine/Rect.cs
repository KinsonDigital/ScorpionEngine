using Microsoft.Xna.Framework;

namespace ScorpionEngine
{
    /// <summary>
    /// Creates a new rectangle.
    /// </summary>
    public struct Rect
    {
        #region Fields
        private Rectangle _rectangle;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Rectangle.
        /// </summary>
        /// <param name="x">The x-component of the rectangle.</param>
        /// <param name="y">The y-component of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rect(int x, int y, int width, int height)
        {
            _rectangle = new Rectangle(x, y, width, height);
        }

        #endregion

        #region Properties
       /// <summary>
       /// Gets an empty version of a Rect instance.
       /// </summary>
        public static Rect Empty
        {
            get
            {
                return new Rect(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Gets the x-component of the rectangle.
        /// </summary>
        public int X
        {
            get
            {
                return _rectangle.X;
            }
            set
            {
                _rectangle.X = value;
            }
        }

        /// <summary>
        /// Gets the y-component of the rectangle.
        /// </summary>
        public int Y
        {
            get
            {
                return _rectangle.Y;
            }
            set
            {
                _rectangle.Y = value;
            }
        }

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int Width
        {
            get
            {
                return _rectangle.Width;
            }
            set
            {
                _rectangle.Width = value;
            }
        }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int Height
        {
            get
            {
                return _rectangle.Height;
            }
            set
            {
                _rectangle.Height = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether this Rectangle contains the given x,y components.
        /// </summary>
        /// <returns></returns>
        /// <param name="x">The x-component to check for.</param>
        /// <param name="y">The y-component to check for.</param>
        public bool Contains(int x, int y)
        {
            return _rectangle.Contains(x, y);
        }

        /// <summary>
        /// Determines whether this Rectangle entirely contains the given Rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangel to check.</param>
        /// <returns></returns>
        public bool Contains(Rect rectangle)
        {
            return _rectangle.Contains(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));
        }

        /// <summary>
        /// Determines whether this Rectangle entirly contains the given vector.
        /// </summary>
        /// <param name="vector">The vector to check for.</param>
        /// <returns></returns>
        public bool Contains(Vector vector)
        {
            return _rectangle.Contains(vector.X, vector.Y);
        }

        #endregion

        #region Overloaded Methods
        /// <summary>
        /// Converts a xna rectangle to a scorpion engine rectangle.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Rect(Rectangle value)
        {
            return new Rect(value.X, value.Y, value.Width, value.Height);
        }
        #endregion
    }
}
using ScorpionCore;
using ScorpionEngine.Physics;

namespace ScorpionEngine
{
    /// <summary>
    /// Creates a new rectangle.
    /// </summary>
    public struct Rect
    {
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
            X = x;
            Y = y;
            Width = width;
            Height = height;
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
        public int X { get; set; }

        /// <summary>
        /// Gets the y-component of the rectangle.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int Height { get; set; }

        public int Top => Y;

        public int Right => X + Width;

        public int Bottom => Y + Height;

        public int Left => X;
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
            return x >= X && x <= Right && y >= Y && y <= Bottom;
        }


        /// <summary>
        /// Determines whether this Rectangle entirely contains the given Rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangel to check.</param>
        /// <returns></returns>
        public bool Contains(Rect rectangle)
        {
            return Contains(rectangle.Left, rectangle.Top) ||
                    Contains(rectangle.Right, rectangle.Top) ||
                    Contains(rectangle.Right, rectangle.Bottom) ||
                    Contains(rectangle.Left, rectangle.Bottom);
        }


        /// <summary>
        /// Determines whether this Rectangle entirly contains the given vector.
        /// </summary>
        /// <param name="vector">The vector to check for.</param>
        /// <returns></returns>
        public bool Contains(Vector vector)
        {
            return Contains((int)vector.X, (int)vector.Y);
        }
        #endregion
    }
}
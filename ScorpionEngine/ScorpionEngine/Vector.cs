using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace ScorpionEngine
{
    /// <summary>
    /// Represents a location of a game object on the game window.
    /// </summary>
    public struct Vector
    {
        #region Fields
        private Vector2 _vector;
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of Position.
        /// </summary>
        /// <param name="x">The x-component of the location.</param>
        /// <param name="y">The y-component of the location.</param>
        public Vector(float x, float y)
        {
            _vector = new Vector2(x, y);
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the x-component of the vector.
        /// </summary>
        public float X
        {
            get
            {
                return _vector.X;
            }
            set
            {
                _vector.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the y-component of the vector.
        /// </summary>
        public float Y
        {
            get
            {
                return _vector.Y;
            }
            set
            {
                _vector.Y = value;
            }
        }

        /// <summary>
        /// Gets a vector with both x and y components set to zero.
        /// </summary>
        public static Vector Zero
        {
            get
            {
                return new Vector(0, 0);
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns the current vector into a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        /// <returns>Unit Vector</returns>
        public void Normalize()
        {
            var num = 1f / (float)Math.Sqrt((double)X * X + Y * (double)Y);
            X *= num;
            Y *= num;
        }
        #endregion


        #region Overloaded Methods
        /// <summary>
        /// Returns the string interpretation of this Vector.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _vector.X.ToString(CultureInfo.InvariantCulture) + "," + _vector.Y.ToString(CultureInfo.InvariantCulture);
        }
        #endregion


        #region Overloaded Operators
        #region Overloaded Operators
        /// <summary>
        /// Subtracts the 2 given vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns></returns>
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        /// <summary>
        /// Adds the 2 given vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns></returns>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        /// <summary>
        /// Adds the given <paramref name="scalar"/> to the X and Y componentes of the given <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector to add the scalar to.</param>
        /// <param name="scalar">The scalar to add the vector to.</param>
        /// <returns></returns>
        public static Vector operator +(Vector vector, float scalar)
        {
            return new Vector(vector.X + scalar, vector.Y + scalar);
        }

        /// <summary>
        /// Multiplies the 2 given vectors, returning the Dot product.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns></returns>
        public static float operator *(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// <summary>
        /// Multiplies the given vector by the given scalar value.
        /// </summary>
        /// <param name="vector">The vector to multiply by the scalar value.</param>
        /// <param name="scalar">The scalar value to multiply by the vector.</param>
        /// <returns></returns>
        public static Vector operator *(Vector vector, float scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }

        /// <summary>
        /// Multiplies the given vector by the given scalar value.
        /// </summary>
        /// <param name="scalar">The scalar value to multiply by the vector.</param>
        /// <param name="vector">The vector to multiply by the scalar value.</param>
        /// <returns></returns>
        public static Vector operator *(float mult, Vector v)
        {
            return new Vector(v.X * mult, v.Y * mult);
        }

        /// <summary>
        /// Returns a value indicating if the 2 given <see cref="vector"/>s have the same component values.
        /// </summary>
        /// <param name="vector1">The first vector in the comparison.</param>
        /// <param name="vector2">The second vector in the comparison.</param>
        /// <returns></returns>
        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return vector1.X == vector2.X && vector1.Y == vector2.Y;
        }

        /// <summary>
        /// Returns a value indicating if the 2 given <see cref="vector"/>s have different component values.
        /// </summary>
        /// <param name="vector1">The first vector in the comparison.</param>
        /// <param name="vector2">The second vector in the comparison.</param>
        /// <returns></returns>
        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }
        #endregion

        #endregion


        #region Static Methods
        /// <summary>
        /// Performs vector addition on value1 and value2/>.
        /// </summary>
        /// <param name="value1">The vector to add to value2.</param>
        /// <param name="value2">The vector to add to value1.</param>
        /// <returns>
        /// The result of the vector addition.
        /// </returns>
        public static Vector2 Add(Vector2 value1, Vector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }


        /// <summary>
        /// Returns a new vector of value1 subtracted by value1.
        /// </summary>
        /// <param name="value1">The first value in the subtraction.</param>
        /// <param name="value2">The second value in the subtraction.</param>
        /// <returns>
        /// The result of the vector subtraction.
        /// </returns>
        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }


        /// <summary>
        /// Returns a scalar result of the value1 vector multiplied by value2.  The result is a Dot Product.
        /// </summary>
        /// <param name="value1">The vector to be scaled..</param>
        /// <param name="scaleFactor">The value to use to scale the vector.</param>
        /// <returns>The dot product scalar.</returns>
        public static Vector Multiply(Vector value1, float scaleFactor)
        {
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;

            return value1;
        }


        /// <summary>
        /// Returns a vector of value1 multiplied by value2.  The result is a Cross Product.
        /// </summary>
        /// <param name="value1">The first vector to multiply by the second vector.</param>
        /// <param name="value2">The second vector to multiply by the first vector.</param>
        /// <returns>The cross product vector.</returns>
        public static Vector Multiply(Vector value1, Vector value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }


        /// <summary>
        /// Divides the components of the vector by a scalar.
        /// </summary>
        /// <param name="value1">The vector to be divided by value2.</param>
        /// <param name="value2">The vector to divide into value1.</param>
        /// <returns>
        /// The result of dividing a vector by a vector.
        /// </returns>
        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }


        /// <summary>
        /// Divides the components of the vector by a scalar.
        /// </summary>
        /// <param name="value1">The vector used to divide by the divider.</param>
        /// <param name="divider">Divisor scalar.</param>
        /// <returns>
        /// The result of dividing a vector by a scalar.
        /// </returns>
        public static Vector Divide(Vector value1, float divider)
        {
            var num = 1f / divider;
            value1.X *= num;
            value1.Y *= num;
            return value1;
        }


        /// <summary>
        /// Returns the given vector as a unit vector. The result is a vector one unit in length pointing in the same direction as the original vector.
        /// </summary>
        /// <returns>Unit Vector</returns>
        public static Vector Normalize(Vector value)
        {
            var num = 1f / (float)Math.Sqrt(value.X * (double)value.X + (double)value.Y * value.Y);
            value.X *= num;
            value.Y *= num;

            return value;
        }
        #endregion
    }
}
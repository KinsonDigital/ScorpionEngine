// <copyright file="TextureTypeException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when an invalid texture type is used.
    /// </summary>
    public class TextureTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureTypeException"/> class.
        /// </summary>
        public TextureTypeException()
            : base("The texture type is invalid.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureTypeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public TextureTypeException(string message)
            : base(message)
        {
        }
    }
}

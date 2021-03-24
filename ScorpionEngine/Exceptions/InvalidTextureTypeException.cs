// <copyright file="InvalidTextureTypeException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when an invalid texture type is used.
    /// </summary>
    public class InvalidTextureTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTextureTypeException"/> class.
        /// </summary>
        public InvalidTextureTypeException()
            : base("The texture type is invalid.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTextureTypeException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidTextureTypeException(string message)
            : base(message)
        {
        }
    }
}

// <copyright file="NameNotFoundException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when a name does not exist.
    /// </summary>
    public class NameNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="NameNotFoundException"/>.
        /// </summary>
        public NameNotFoundException() : base("A scene with that name does not exist.")
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="NameNotFoundException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public NameNotFoundException(string message): base(message)
        {
        }
    }
}

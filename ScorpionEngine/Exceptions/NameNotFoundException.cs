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
        /// Initializes a new instance of the <see cref="NameNotFoundException"/> class.
        /// </summary>
        public NameNotFoundException()
            : base("A scene with that name does not exist.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public NameNotFoundException(string message)
            : base(message)
        {
        }
    }
}

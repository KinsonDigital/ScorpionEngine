﻿// <copyright file="IdNotFoundException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown if an ID has not been found.
    /// </summary>
    public class IdNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdNotFoundException"/> class.
        /// </summary>
        public IdNotFoundException()
            : base("The ID has not been found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdNotFoundException"/> class.
        /// </summary>
        /// <param name="sceneId">The ID that has not been found.</param>
        public IdNotFoundException(int sceneId)
            : base($"An ID with the number '{sceneId}' has not been found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public IdNotFoundException(string message)
            : base(message)
        {
        }
    }
}

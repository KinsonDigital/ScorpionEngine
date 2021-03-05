// <copyright file="InputFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Factories
{
    using KDScorpionEngine.Input;
    using KDScorpionEngine.Utils;
    using Raptor.Input;

    /// <summary>
    /// Creates input related objects for managing keyboard and mouse input.
    /// </summary>
    public static class InputFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="KeyboardWatcher"/>.
        /// </summary>
        /// <param name="enabled">True to enable the watcher by default.</param>
        /// <returns>The keyboard watcher.</returns>
        public static KeyboardWatcher CreateKeyboardWatcher(bool enabled = true)
            => new KeyboardWatcher(
                enabled,
                new Keyboard(),
                new StopWatch(),
                new StopWatch());

        /// <summary>
        /// Creates a new instance of <see cref="MouseWatcher"/>.
        /// </summary>
        /// <param name="enabled">True to enable the watcher by default.</param>
        /// <returns>The keyboard watcher.</returns>
        public static MouseWatcher CreateMouseWatcher(bool enabled = true)
            => new MouseWatcher(
                enabled,
                new Mouse(),
                new StopWatch(),
                new StopWatch());
    }
}

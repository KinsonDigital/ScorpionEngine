// <copyright file="InputFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Factories
{
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Input;

    /// <summary>
    /// Creates input related objects for managing keyboard and mouse input.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class InputFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="KeyboardWatcher"/>.
        /// </summary>
        /// <param name="enabled">
        ///     <see langword="true"/> to enable the watcher by default.
        /// </param>
        /// <returns>The keyboard watcher.</returns>
        public static KeyboardWatcher CreateKeyboardWatcher(bool enabled = true)
        {
            var instance = IoC.Container.GetInstance<KeyboardWatcher>();
            instance.Enabled = enabled;

            return instance;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MouseWatcher"/>.
        /// </summary>
        /// <param name="enabled">
        ///     <see langword="true"/> to enable the watcher by default.
        /// </param>
        /// <returns>The keyboard watcher.</returns>
        public static MouseWatcher CreateMouseWatcher(bool enabled = true)
        {
            var instance = IoC.Container.GetInstance<MouseWatcher>();
            instance.Enabled = enabled;

            return instance;
        }
    }
}

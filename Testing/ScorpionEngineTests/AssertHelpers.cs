// <copyright file="AssertExt.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;

    /// <summary>
    /// Provides extensions for the <see cref="Assert"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AssertHelpers
    {
        /// <summary>
        /// Asserts if an <see cref="Exception"/> was not thrown.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Exception"/>.</typeparam>
        /// <param name="action">The action to catch the exception against.</param>
        public static void DoesNotThrow<T>(Action action)
            where T : Exception
        {
            try
            {
                if (action is null)
                {
                    return;
                }

                action();
            }
            catch (T)
            {
                Assert.True(false, $"Expected the exception {typeof(T).Name} to not be thrown.");
            }
        }

        /// <summary>
        /// Asserts if an action does not throw a null reference exception.
        /// </summary>
        /// <param name="action">The action to catch the exception against.</param>
        public static void DoesNotThrowNullReference(Action action)
        {
            try
            {
                if (action is null)
                {
                    return;
                }

                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NullReferenceException))
                {
                    Assert.True(false, $"Expected not to raise a {nameof(NullReferenceException)} exception.");
                }
                else
                {
                    Assert.True(true);
                }
            }
        }

        /// <summary>
        /// Verifies that the exact exception is thrown (and not a derived exception type) and that
        /// the exception message matches the given <paramref name="expectedMessage"/>.
        /// </summary>
        /// <typeparam name="T">The type of exception that the test is verifying.</typeparam>
        /// <param name="testCode">The code that will be throwing the expected exception.</param>
        /// <param name="expectedMessage">The expected message of the exception.</param>
        public static void ThrowsWithMessage<T>(Action testCode, string expectedMessage)
            where T : Exception
        {
            Assert.Equal(expectedMessage, Assert.Throws<T>(testCode).Message);
        }

        /// <summary>
        /// Verifies that an event with the exact event args is not raised.
        /// </summary>
        /// <typeparam name="T">The type of the event arguments to expect</typeparam>
        /// <param name="attach">Code to attach the event handler.</param>
        /// <param name="detach">Code to detatch the event handler.</param>
        /// <param name="testCode">A delegate to the code to be tested.</param>
        public static void DoesNotRaise<T>(Action<EventHandler<T>> attach, Action<EventHandler<T>> detach, Action testCode)
            where T : EventArgs
        {
            try
            {
                Assert.Raises(attach, detach, testCode);

                Assert.Equal("No event was raised", "An event was raised.");
            }
            catch (Exception ex)
            {
                Assert.Equal("(No event was raised)\r\nEventArgs\r\n(No event was raised)", ex.Message);
            }
        }
    }
}

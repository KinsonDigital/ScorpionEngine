using Xunit;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngineTests
{
    /// <summary>
    /// Provides extensions for the <see cref="Assert"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AssertExt
    {
        /// <summary>
        /// Asserts if an <see cref="Exception"/> was not thrown.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Exception"/>.</typeparam>
        /// <param name="action">The action to catch the exception against.</param>
        public static void DoesNotThrow<T>(Action action) where T : Exception
        {
            try
            {
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
    }
}


using System;


namespace ScorpionEngine.Exceptions
{
    /// <summary>
    /// Thrown when a name does not exist.
    /// </summary>
    public class NameNotFoundException : Exception
    {
        #region Constructors
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
        #endregion
    }
}

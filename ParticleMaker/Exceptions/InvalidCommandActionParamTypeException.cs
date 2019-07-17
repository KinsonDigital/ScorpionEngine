using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when the incoming parameter type of a command
    /// action method is of the incorrect type.
    /// </summary>
    public class InvalidCommandActionParamTypeException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="InvalidCommandActionParamTypeException"/>.
        /// </summary>
        /// <param name="commandActionMethodName">The name of the command action method name of where the exception occured.</param>
        /// <param name="paramName">The name of the command action method parameter that is of the incorrect type.</param>
        public InvalidCommandActionParamTypeException(string commandActionMethodName, string paramName) : base($"The {commandActionMethodName} method parameter '{paramName}' not a valid type.") { }
        #endregion
    }
}

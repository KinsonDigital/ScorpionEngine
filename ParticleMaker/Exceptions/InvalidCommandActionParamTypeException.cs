using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Holds exception information for exceptions that occur when the incoming parameter type of a command
    /// action method is of the incorrect type.
    /// </summary>
    public class InvalidCommandActionParamTypeException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidCommandActionParamTypeException"/>.
        /// </summary>
        /// <param name="commandActionMethodName">The name of the command action method name of where the exception occured.</param>
        /// <param name="paramName">The name of the command action method parameter that is the incorrect type.</param>
        public InvalidCommandActionParamTypeException(string commandActionMethodName, string paramName) : base($"The {commandActionMethodName} method parameter '{paramName}' not a valid type.")
        {
        }
    }
}

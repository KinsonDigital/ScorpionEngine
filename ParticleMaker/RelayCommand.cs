using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace ParticleMaker
{
    /// <summary>
    /// Provides a command with the ability to set the execute and can exectue
    /// code to be invoked as delegates.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Public Events
        /// <summary>
        /// Invoked when the can execute status has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;
        #endregion


        #region Fields
        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteAction;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RelayCommand()
        {
            //This is here to satisfy a paramaterless constructor for design time data in xaml files.
        }


        /// <summary>
        /// Creates a new instance of <see cref="RelayCommand"/>
        /// </summary>
        /// <param name="execute">The action to execute when execution is attempted.</param>
        /// <param name="canExecute">Returns a value indicating if the action can be executed.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _executeAction = execute;
            _canExecuteAction = canExecute;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if the <see cref="RelayCommand"/> can execute its action.
        /// </summary>
        /// <param name="parameter">The incoming data.</param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => _canExecuteAction(parameter);


        /// <summary>
        /// Executes the set action.
        /// </summary>
        /// <param name="parameter">The incoming data to use in the action.</param>
        public void Execute(object parameter) => _executeAction(parameter);
        #endregion
    }
}

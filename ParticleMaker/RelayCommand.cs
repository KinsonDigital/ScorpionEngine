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
        /// Occurs when the can execute status has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;
        #endregion


        #region Fields
        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteAction;
        private bool _ignoreCanExecute;
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


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the can execute functionality should be ignored.
        /// If set to true, the command will always execute.
        /// </summary>
        public bool IgnoreCanExecute
        {
            get => _ignoreCanExecute;
            set
            {
                var originalValue = _ignoreCanExecute;

                _ignoreCanExecute = value;

                if (originalValue != value)
                    CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if the <see cref="RelayCommand"/> can execute its action.
        /// </summary>
        /// <param name="parameter">The incoming data.</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_ignoreCanExecute)
                return true;


            return _canExecuteAction(parameter);
        }


        /// <summary>
        /// Executes the set action.
        /// </summary>
        /// <param name="parameter">The incoming data to use in the action.</param>
        public void Execute(object parameter) => _executeAction(parameter);
        #endregion
    }
}

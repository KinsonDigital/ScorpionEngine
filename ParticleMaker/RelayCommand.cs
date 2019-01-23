using System;
using System.Windows.Input;

namespace ParticleMaker
{
    public class RelayCommand : ICommand
    {
        #region Public Events
        /// <summary>
        /// Invoked when the can execute status has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;
        #endregion


        #region Fields
        private Action _executeAction;
        private Func<bool> _canExecuteAction;
        #endregion


        #region Constructors
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _executeAction = execute;
            _canExecuteAction = canExecute;
        }
        #endregion


        #region Public Methods
        public bool CanExecute(object parameter)
        {
            return _canExecuteAction();
        }


        /// <summary>
        /// Executes the set action.
        /// </summary>
        /// <param name="parameter">The data to send.</param>
        public void Execute(object parameter)
        {
            _executeAction();
        }
        #endregion
    }
}

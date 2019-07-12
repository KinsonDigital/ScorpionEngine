using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParticleMaker.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        #region Public Events
        /// <summary>
        /// Occurs when a property's value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Public Methods
        /// <summary>
        /// Notifies the binding system that a property value has changed.
        /// </summary>
        /// <param name="propName">The name of the prop to signify that changed.</param>
        public void NotifyPropChange([CallerMemberName]string propName = "")
        {
            if (!string.IsNullOrEmpty(propName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        /// <summary>
        /// Notifies the binding system that a property value has changed
        /// for all of the given <paramref name="propNames"/>.
        /// </summary>
        /// <param name="propNames">The list of property names of the properties to notify a change on.</param>
        public void NotifyAllPropChanges(string[] propNames)
        {
            foreach (var name in propNames)
            {
                NotifyPropChange(name);
            }
        }
        #endregion
    }
}

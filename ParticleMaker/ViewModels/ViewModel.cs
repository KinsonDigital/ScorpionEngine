using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ParticleMaker.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        /// <summary>
        /// Notifies the binding system that a property value has changed.
        /// </summary>
        /// <param name="propName"></param>
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public void NotifyAllPropChanges(string[] propNames)
        {
            foreach (var name in propNames)
            {
                NotifyPropChange(name);
            }
        }
    }
}

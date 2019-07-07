using ParticleMaker.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker.Tests.ViewModels
{
    /// <summary>
    /// Used for faking out the ViewModel abstract class for testing.
    /// </summary>
    public class ViewModelFake : ViewModel
    {
        #region Fields
        private int _testPropA;
        private int _testPropB;
        #endregion


        #region Props
        public int TestPropA
        {
            [ExcludeFromCodeCoverage]
            get => _testPropA;
            set
            {
                _testPropA = value;

                NotifyPropChange();
            }
        }

        public int TestPropB
        {
            [ExcludeFromCodeCoverage]
            get => _testPropB;
            set
            {
                _testPropB = value;

                NotifyPropChange();
            }
        }
        #endregion
    }
}

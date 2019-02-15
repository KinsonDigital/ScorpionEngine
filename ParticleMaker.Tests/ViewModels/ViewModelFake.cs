using ParticleMaker.ViewModels;

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
            get => _testPropA;
            set
            {
                _testPropA = value;

                NotifyPropChange();
            }
        }

        public int TestPropB
        {
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

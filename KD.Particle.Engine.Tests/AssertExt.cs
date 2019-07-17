using Xunit;

namespace KDParticleEngineTests
{
    /// <summary>
    /// Provides extensions to the <see cref="Assert"/> class.
    /// </summary>
    public static class AssertExt
    {
        #region Public Methods
        public static void WithinRange(int value, int min, int max)
        {
            if (value >= min && value <= max)
                return;

            Assert.True(false, $"Incorrect Value: {value}\nMin Value: {min}\nMax Value: {max}");
        }


        public static void WithinRange(float value, float min, float max)
        {
            if (value >= min && value <= max)
                return;

            Assert.True(false, $"Incorrect Value: {value}\nMin Value: {min}\nMax Value: {max}");
        }


        public static void WithinRange(double value, double min, double max)
        {
            if (value >= min && value <= max)
                return;

            Assert.True(false, $"Incorrect Value: {value}\nMin Value: {min}\nMax Value: {max}");
        }
        #endregion
    }
}

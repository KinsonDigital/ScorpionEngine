using KDParticleEngine.Services;
using Xunit;
using Xunit.Abstractions;

namespace KDParticleEngineTests.Services
{
    public class RandomizerServiceTests
    {
        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(1, 4, true)]
        [InlineData(1, 8, true)]
        [InlineData(1, 16, true)]
        public void GetValue_WhenInvokingWithIntValues_ReturnsWithinRange(int minValue, int maxValue, bool expected)
        {
            //Arrange
            var randomizer = new RandomizerService();

            for (int i = 0; i < 1000; i++)
            {
                //Act
                var result = randomizer.GetValue(minValue, maxValue);

                //Assert
                AssertExt.WithinRange(result, minValue, maxValue);
            }
        }


        [Theory]
        [InlineData(1.001, 2.001, true)]
        [InlineData(1.001, 4.001, true)]
        [InlineData(1.001, 8.001, true)]
        [InlineData(1.001, 16.001, true)]
        public void GetValue_WhenInvokingWithFloatValues_ReturnsWithinRange(float minValue, float maxValue, bool expected)
        {
            //Arrange
            var randomizer = new RandomizerService();

            for (int i = 0; i < 100000; i++)
            {
                //Act
                var result = randomizer.GetValue(minValue, maxValue);

                //Assert
                AssertExt.WithinRange(result, minValue, maxValue);
            }
        }
    }
}

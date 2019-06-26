using System;

namespace KDParticleEngine.Services
{ 
    /// <summary>
    /// Provides methods for randomizing numbers.
    /// </summary>
    public class RandomizerService : IRandomizerService
    {
        #region Fields
        private readonly Random _random;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RandomizerService"/>.
        /// </summary>
        public RandomizerService()
        {
            _random = new Random();
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <returns></returns>
        public bool FlipCoin() => _random.NextDouble() <= 0.5f;


        /// <summary>
        /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
        /// A random value will be chosen between the min and max values no matter which value is less than 
        /// or greater than the other.
        /// </summary>
        /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
        /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
        /// <returns></returns>
        public float GetValue(float minValue, float maxValue)
        {
            var minValueAsInt = (int)((minValue + 0.001f) * 1000);
            var maxValueAsInt = (int)((maxValue + 0.001f) * 1000);

            if (minValueAsInt > maxValueAsInt)
            {
                return _random.Next(maxValueAsInt, minValueAsInt) / 1000f;
            }
            else
            {
                return _random.Next(minValueAsInt, maxValueAsInt) / 1000f;
            }
        }


        /// <summary>
        /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
        /// A random value will be chosen between the min and max values no matter which value is less than 
        /// or greater than the other.
        /// </summary>
        /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
        /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
        /// <returns></returns>
        public double GetValue(double minValue, double maxValue) =>
            GetValue((float)(minValue + 0.001), (float)(maxValue + 0.001));


        /// <summary>
        /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
        /// A random value will be chosen between the min and max values no matter which value is less than 
        /// or greater than the other.
        /// </summary>
        /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
        /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
        /// <returns></returns>
        public int GetValue(int minValue, int maxValue)
        {
            return minValue > maxValue ?
                _random.Next(maxValue, minValue + 1) :
                _random.Next(minValue, maxValue + 1);
        }
        #endregion
    }
}

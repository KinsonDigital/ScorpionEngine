namespace KDParticleEngine.Services
{
    /// <summary>
    /// Provides methods for randomizing numbers.
    /// </summary>
    public interface IRandomizerService
    {
        #region Methods
        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <returns></returns>
        bool FlipCoin();


        /// <summary>
        /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
        /// A random value will be chosen between the min and max values no matter which value is less than 
        /// or greater than the other.
        /// </summary>
        /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
        /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
        /// <returns></returns>
        double GetValue(double minValue, double maxValue);


        /// <summary>
        /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
        /// A random value will be chosen between the min and max values no matter which value is less than 
        /// or greater than the other.
        /// </summary>
        /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
        /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
        /// <returns></returns>
        float GetValue(float minValue, float maxValue);


        /// <summary>
        /// Gets a random number between the given <paramref name="minValue"/> and <paramref name="maxValue"/>s.
        /// A random value will be chosen between the min and max values no matter which value is less than 
        /// or greater than the other.
        /// </summary>
        /// <param name="minValue">The inclusive minimum value of the range to randomly choose from.</param>
        /// <param name="maxValue">The inclusive maximum value of the range to randomly choose from.</param>
        /// <returns></returns>
        int GetValue(int minValue, int maxValue);
        #endregion
    }
}
namespace MonoScorpPlugin
{
    public static class ExtensionMethods
    {
        private const float PI = 3.1415926535897931f;

        /// <summary>
        /// Converts the given radians to degrees.
        /// </summary>
        /// <param name="radians">The radians to convert.</param>
        /// <returns></returns>
        public static float ToDegrees(this float radians)
        {
            return radians * 180.0f / PI;
        }


        /// <summary>
        /// Converts the given degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns></returns>
        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }
    }
}

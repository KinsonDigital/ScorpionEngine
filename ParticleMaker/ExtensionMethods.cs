namespace ParticleMaker
{
    public static class ExtensionMethods
    {
        private const float PI = 3.1415926535897931f;


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

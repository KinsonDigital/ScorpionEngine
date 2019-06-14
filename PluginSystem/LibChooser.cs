namespace PluginSystem
{
    /// <summary>
    /// Chooses the name of the engine and physics libraries based on the chosen build platform.
    /// </summary>
    internal static class LibChooser
    {
        #region Props
        /// <summary>
        /// Gets the name of the Scorpion Core plugin library.
        /// </summary>
        public static string EnginePluginLibraryName
        {
            get
            {
#if SDL
                return "SDLScorpPlugin";
#elif MONOGAME
                return "MonoScorpPlugin";
#endif
            }
        }

        /// <summary>
        /// Gets the name of the Velcro Physics plugin library.
        /// </summary>
        public static string PhysicsPLuginLibraryName => "VelcroPhysicsPlugin";
        #endregion
    }
}

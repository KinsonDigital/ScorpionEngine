using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PluginSystem
{
    /// <summary>
    /// Chooses the name of the engine and physics libraries based on the chosen build platform.
    /// </summary>
    internal static class LibChooser
    {
        #region Private Fields
        private static string _enginePluginLibraryName;
        #endregion


        #region Props
        /// <summary>
        /// Gets the name of the Scorpion Core plugin library.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static string EnginePluginLibraryName
        {
            get
            {
                SetLibNameAsSDL();
                SetLibNameAsMonoGame();

                return _enginePluginLibraryName;
            }
        }

        /// <summary>
        /// Gets the name of the Velcro Physics plugin library.
        /// </summary>
        public static string PhysicsPLuginLibraryName => "VelcroPhysicsPlugin";
        #endregion


        #region Private Methods
        [Conditional("SDL")]
        private static void SetLibNameAsSDL() => _enginePluginLibraryName = "SDLScorpPlugin";


        [Conditional("MONOGAME")]
        private static void SetLibNameAsMonoGame() => _enginePluginLibraryName = "MonoScorpPlugin";
        #endregion
    }
}

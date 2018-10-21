using ScorpionCore.Plugins;

namespace ScorpionCore
{
    internal static class PluginSystem
    {
        #region Props
        public static PluginLibrary EnginePlugins { get; private set; }

        public static PluginLibrary PhysicsPlugins { get; private set; }
        #endregion


        #region Public Methods
        public static void LoadPlugins()
        {
            EnginePlugins = new PluginLibrary("MonoScorpPlugin");
            PhysicsPlugins = new PluginLibrary("VelcroPhysicsPlugin");
        }
        #endregion
    }
}

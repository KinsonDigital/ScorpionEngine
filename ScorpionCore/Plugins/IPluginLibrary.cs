namespace ScorpionCore.Plugins
{
    public interface IPluginLibrary
    {
        string Name { get; set; }


        #region Public Methods
        T LoadPlugin<T>() where T : class, IPlugin;


        T LoadPlugin<T>(params object[] paramItems) where T : class, IPlugin;
        #endregion
    }
}
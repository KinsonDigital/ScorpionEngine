using System;

namespace KDScorpionCore
{
    public interface IEngineEvents
    {
        /// Occurs once every frame before the OnDraw event before the OnDraw event is invoked.
        /// </summary>
        event EventHandler<OnUpdateEventArgs> OnUpdate;

        /// <summary>
        /// Occurs once every frame after the OnUpdate event has been been invoked.
        /// </summary>
        event EventHandler<OnRenderEventArgs> OnRender;

        /// <summary>
        /// Occurs one time during game initialization. This event is fired before the OnLoadContent event is fired. Add initialization code here.
        /// </summary>
        event EventHandler OnInitialize;

        /// <summary>
        /// Occurs one time during game intialization after the OnInit event is fired.
        /// </summary>
        event EventHandler OnLoadContent;
    }
}

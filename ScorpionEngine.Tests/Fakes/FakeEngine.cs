using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;

namespace ScorpionEngine.Tests.Fakes
{
    public class FakeEngine : Engine
    {
        public FakeEngine(IPluginLibrary enginePluginLib, IPluginLibrary physicsPluginLib) : base(enginePluginLib, physicsPluginLib)
        {
        }


        #region Props
        public bool InitInvoked { get; set; }

        public bool LoadContentInvoked { get; set; }

        public bool UpdateInvoked { get; set; }

        public bool RenderInvoked { get; set; }
        #endregion


        #region Public Methods
        public override void Init()
        {
            InitInvoked = true;
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            LoadContentInvoked = true;
        }


        public override void Update(EngineTime engineTime)
        {
            UpdateInvoked = true;
        }


        public override void Render(Renderer renderer)
        {
            RenderInvoked = true;
        }
        #endregion
    }
}

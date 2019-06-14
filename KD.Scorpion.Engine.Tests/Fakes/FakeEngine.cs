using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionEngine;
using KDScorpionEngine.Graphics;
using PluginSystem;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeEngine : Engine
    {
        public FakeEngine(IPluginLibrary enginePluginLib) : base(enginePluginLib)
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


        public override void Render(GameRenderer renderer)
        {
            RenderInvoked = true;
        }
        #endregion
    }
}

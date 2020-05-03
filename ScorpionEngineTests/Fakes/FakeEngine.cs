using KDScorpionEngine;
using KDScorpionEngine.Graphics;
using Moq;
using Raptor;
using Raptor.Content;
using Raptor.Plugins;

namespace KDScorpionEngineTests.Fakes
{
    /// <summary>
    /// Provides a fake implementation of the <see cref="Engine"/> class.
    /// </summary>
    public class FakeEngine : Engine
    {
        private readonly IEngineCore _fakeEngineCore;


        public FakeEngine(IContentLoader contentLoader, IEngineCore engineCore, IKeyboard keyboard) : base(contentLoader, engineCore, keyboard)
        {
            _fakeEngineCore = engineCore;

            engineCore.OnInitialize += (sender, e) => Init();
            engineCore.OnLoadContent += (sender, e) => LoadContent(ContentLoader);
            engineCore.OnUpdate += (sender, e) => Update(new EngineTime());
            engineCore.OnRender += (sender, e) => Render(new GameRenderer(new Mock<IRenderer>().Object, new Mock<IDebugDraw>().Object));
        }


        #region Props
        public bool InitInvoked { get; set; }

        public bool LoadContentInvoked { get; set; }

        public bool UpdateInvoked { get; set; }

        public bool RenderInvoked { get; set; }
        #endregion


        #region Public Methods
        public new void Start()
        {
            _fakeEngineCore.StartEngine();
        }


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

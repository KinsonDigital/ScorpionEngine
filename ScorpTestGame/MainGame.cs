using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Scene;
using ScorpTestGame.Scenes;

namespace ScorpTestGame
{
    /// <summary>
    /// The main game.
    /// </summary>
    public class MainGame : Engine
    {
        private Level1 _level1;
        private ParticleTestingScene _particleScene;


        #region Constructors
        /// <summary>
        /// Creates a new space shooter game engine.
        /// </summary>
        public MainGame()
        {
            //Do not set the world in here.  The world has to be set in the OnInit() method so that way
            //the graphics device has been created.  The graphics device will not be created until the
            //engine has started up.
        }
        #endregion


        #region Public Methods
        public override void Init()
        {
            _level1 = new Level1();
            _particleScene = new ParticleTestingScene();

            SceneManager.Add(_level1);
            SceneManager.Add(_particleScene);

            SceneManager.SetCurrentScene(0);

            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            base.Render(renderer);
        }
        #endregion
    }
}
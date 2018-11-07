using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Scene;

namespace ScorpTestGame
{
    /// <summary>
    /// The engine of the game.
    /// </summary>
    public class TestGame : Engine
    {
        private Level1 _level1;


        /// <summary>
        /// Creates a new space shooter game engine.
        /// </summary>
        public TestGame()
        {
            //Do not set the world in here.  The world has to be set in the OnInit() method so that way
            //the graphics device has been created.  The graphics device will not be created until the
            //engine has started up.
        }


        public override void Init()
        {
            _level1 = new Level1();
            SceneManager.AddScene(_level1);

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

        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
using System;
using ScorpionEngine;

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
        public TestGame() : base()
        {
            //Do not set the world in here.  The world has to be set in the OnInit() method so that way
            //the graphics device has been created.  The graphics device will not be created until the
            //engine has started up.
        }

        public override void OnInit()
        {
            _level1 = new Level1(new Vector(0, 1f));
            SetWorld(_level1);

            base.OnInit();
        }
    }
}
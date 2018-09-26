using System;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Objects;
using ScorpionEngine.Physics;
using ScorpionEngine.Scene;

namespace ScorpTestGame
{
    /// <summary>
    /// The engine of the game.
    /// </summary>
    public class TestGame : Engine
    {
        private Keyboard _keyboard;
        private Mouse _mouse;
        private SceneManager _sceneManager;
        private Texture _rectTexture;
        private MovableObject _rectObject;
        private int _x = 200;
        private int _y = 200;


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
            _keyboard = new Keyboard();
            _mouse = new Mouse();

            _rectObject = new MovableObject();
            _rectObject.Position = new Vector(200, 200);

            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _rectTexture = contentLoader.LoadTexture("GrayRectangle");

            _rectObject.Texture = _rectTexture;

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            _keyboard.UpdateCurrentState();
            _mouse.UpdateCurrentState();

            ProcessKeys();

            _keyboard.UpdatePreviousState();
            _mouse.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            renderer.Clear(100, 149, 237, 255);

            _rectObject.Render(renderer);

            base.Render(renderer);
        }


        private void ProcessKeys()
        {
            if (_keyboard.IsKeyDown(InputKeys.Right))
            {
            }

            if (_keyboard.IsKeyDown(InputKeys.Left))
            {
            }

            if (_mouse.IsButtonPressed(InputButton.LeftButton))
            {
            }

            if (_mouse.IsButtonPressed(InputButton.RightButton))
            {
            }

            if (_keyboard.IsKeyPressed(InputKeys.End))
            {
                _rectObject.Visible = !_rectObject.Visible;
            }
        }


        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private Texture _fallingRectTexture;
        private DynamicEntity _fallingRect;
        private Texture _platformTexture;
        private DynamicEntity _platformRect;
        private TextObject _fps;
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
            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _fps = new TextObject("Hello World", Color.Black, Color.Black, new Vector(300, 300));
            _fallingRectTexture = contentLoader.LoadTexture("GreenRectangle");

            var fallingRectVertices = new Vector[4]
            {
                new Vector(-50, -50),
                new Vector(50, -50),
                new Vector(50, 50),
                new Vector(-50, 50)
            };

            _fallingRect = new DynamicEntity(_fallingRectTexture, fallingRectVertices, new Vector(200, 200));

            _platformTexture = contentLoader.LoadTexture("LongRectangle");

            var platformVertices = new Vector[4]
            {
                new Vector(-100, -100),
                new Vector(100, -100),
                new Vector(100, 100),
                new Vector(-100, 100)
            };

            _platformRect = new DynamicEntity(_platformTexture, fallingRectVertices, new Vector(200, 400), true);

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
            renderer.Clear(50, 50, 50, 255);

            _fallingRect.Render(renderer);
            _platformRect.Render(renderer);

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
                _fallingRect.Visible = !_fallingRect.Visible;
            }
        }


        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
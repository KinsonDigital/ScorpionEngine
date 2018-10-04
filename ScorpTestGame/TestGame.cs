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
        private Texture _rectTexture;
        private DynamicEntity _fallingObject;
        private GameObject _platformObject;
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

            //TODO: Need to create a regular game engine object and get its internal physics body wired up and working

            

            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _fps = new TextObject("Hello World", Color.Black, Color.Black, new Vector(300, 300));
            _rectTexture = contentLoader.LoadTexture("GreenRectangle");

            var vertices = new Vector[4]
            {
                new Vector(0, 0),
                new Vector(50, 0),
                new Vector(50, 50),
                new Vector(0, 50)
            };

            _fallingObject = new DynamicEntity(_rectTexture, vertices, new Vector(200, 200));

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

            _fallingObject.Render(renderer);

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
                _fallingObject.Visible = !_fallingObject.Visible;
            }
        }


        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
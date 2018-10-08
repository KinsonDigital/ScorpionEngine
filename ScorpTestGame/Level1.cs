using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Objects;
using ScorpionEngine.Physics;
using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpTestGame
{
    public class Level1 : GameScene
    {
        private DynamicEntity _fallingRect;
        private DynamicEntity _platformRect;
        private Texture _fallingRectTexture;
        private Texture _platformTexture;
        private Keyboard _keyboard;
        private Mouse _mouse;


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _fallingRectTexture = contentLoader.LoadTexture("GreenRectangle");

            var fallingRectVertices = new Vector[4]
            {
                new Vector(-50, -25),
                new Vector(50, -25),
                new Vector(50, 25),
                new Vector(-50, 25)
            };

            _fallingRect = new DynamicEntity(_fallingRectTexture, fallingRectVertices, new Vector(330, 200));
            _fallingRect.DebugDrawEnabled = true;

            _platformTexture = contentLoader.LoadTexture("LongRectangle");

            var platformVertices = new Vector[4]
            {
                new Vector(-100, -12.5f),
                new Vector(100, -12.5f),
                new Vector(100, 12.5f),
                new Vector(-100, 12.5f)
            };

            _platformRect = new DynamicEntity(_platformTexture, platformVertices, new Vector(200, 400), true);
            _platformRect.DebugDrawEnabled = true;

            ContentLoaded = true;

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _fallingRect.Render(renderer);
            _platformRect.Render(renderer);

            base.Render(renderer);
        }


        public override void UnloadContent(ContentLoader contentLoader)
        {
            throw new NotImplementedException();
        }


        #region Private Methods

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
        #endregion
    }
}

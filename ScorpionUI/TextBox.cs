using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionCore.Input;

namespace ScorpionUI
{
    //Provides the ability to enter text into a box.
    public class TextBox : IControl
    {
        private Keyboard _keyboard = new Keyboard();
        private GameText _text;
        private GameText _character;
        private Vector _textPosition;
        private int _characterPosition;
        private int _cursorElapsedMilliseconds;
        private bool _cursorVisible;


        #region Props
        public Vector Position { get; set; }

        public int Width => Background.Width;

        public int Height => Background.Height;

        public Texture Background { get; set; }

        public string FontName { get; set; }
        #endregion


        #region Public Methods
        public void Initialize()
        {
        }


        public void LoadContent(ContentLoader contentLoader)
        {
            _text = contentLoader.LoadText(FontName);
        }


        public void Update(EngineTime engineTime)
        {
            ProcessKeys();

            _cursorElapsedMilliseconds += engineTime.ElapsedEngineTime.Milliseconds;

            if (_cursorElapsedMilliseconds >= 500)
            {
                _cursorElapsedMilliseconds = 0;
                _cursorVisible = !_cursorVisible;
            }

            _text.Text = "Hello";
        }


        public void Render(Renderer renderer)
        {
            renderer.Render(Background, Position);

            //Update the X position of the cursor
            _textPosition = new Vector((Position.X - Width / 2f) + 5f, Position.Y - _text.Height / 2f);

            var cursorPosition = new Vector(_textPosition.X - 2, _textPosition.Y);

            //Render the text inside of the textbox
            renderer.Render(_text, _textPosition, new GameColor(0, 0, 0, 255));

            //Render the blinking cursor
            var lineStart = new Vector(_textPosition.X, _textPosition.Y);
            var lineStop = new Vector(_textPosition.X, _textPosition.Y + _text.Height);

            if (_cursorVisible)
                renderer.Line(lineStart, lineStop, new GameColor(255, 0, 0, 255));//TODO: Change to black when finished with testing
        }
        #endregion


        #region Private Methods
        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            if (_keyboard.AreAnyKeysPressed())
            {
                var pressedKeys = _keyboard.GetCurrentPressedKeys();

                var isLetterDown = _keyboard.IsKeyDown(InputKeys.F);
            }

            _keyboard.UpdatePreviousState();
        }


        private void CalcCursorXPosition()
        {
        }
        #endregion
    }
}

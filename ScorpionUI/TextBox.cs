using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionCore.Input;

namespace ScorpionUI
{
    /// <summary>
    /// Provides the ability to enter text into a box.
    /// </summary>
    public class TextBox : IControl
    {
        private Keyboard _keyboard = new Keyboard();
        private GameText _text;
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


        #region Constructors
        public TextBox()
        {
        }
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
        }


        public void Render(Renderer renderer)
        {
            renderer.Render(Background, Position);

            var _cursorX = CalcCursorXPos();

            //Update the X position of the text
            _textPosition = new Vector((Position.X - Width / 2f) + 5f, Position.Y - _text.Height / 2f);

            var cursorPositionX = _textPosition.X - 1 + _cursorX;

            //Render the text inside of the textbox
            if (!string.IsNullOrEmpty(_text.Text))
                renderer.Render(_text, _textPosition, new GameColor(0, 0, 0, 255));

            //Render the blinking cursor
            var lineStart = new Vector(cursorPositionX, Position.Y - (Background.Height / 2) + 3);
            var lineStop = new Vector(cursorPositionX, Position.Y + (Background.Height / 2) - 3);

            if (_cursorVisible)
                renderer.Line(lineStart, lineStop, new GameColor(255, 0, 0, 255));//TODO: Change to black when finished with testing
        }
        #endregion


        #region Private Methods
        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            if (_keyboard.IsKeyPressed(InputKeys.Right))
            {
                _characterPosition = _characterPosition > _text.Text.Length - 1 ?
                    _characterPosition :
                    _characterPosition + 1;

                _keyboard.UpdatePreviousState();
                return;
            }

            if (_keyboard.IsKeyPressed(InputKeys.Left))
            {
                _characterPosition = _characterPosition <= 0 ?
                    _characterPosition :
                    _characterPosition - 1;

                _keyboard.UpdatePreviousState();
                return;
            }

            var isShiftDown = _keyboard.IsKeyDown(InputKeys.LeftShift) || _keyboard.IsKeyDown(InputKeys.RightShift);

            ///////TODO: Remmove after debugging
            //////////////////////////////
            
            //The delete keys. This is the standard one and the numpad one
            if(_keyboard.IsDeleteKeyPressed())
            {
                _text.Text = _text.Text.Remove(_characterPosition, 1);
            }

            if (_keyboard.IsKeyPressed(InputKeys.Back) && _characterPosition > 0)
            {
                _characterPosition -= 1;
                _text.Text = _text.Text.Remove(_characterPosition, 1);
            }

            
            //If a letter is pressed, add it to the textbox
            if (_keyboard.IsLetterPressed(out InputKeys letter))
            {
                var letterText = "";

                if (letter == InputKeys.Space)
                {
                    letterText = " ";
                }
                else
                {
                    letterText = isShiftDown || _keyboard.CapsLockOn ?
                        letter.ToString() :
                        letter.ToString().ToLower();
                }

                _text.Text = _text.Text.Insert(_characterPosition, letterText);
                _characterPosition += 1;
            }

            //If a number was pressed on the keyboard
            if (_keyboard.IsNumberPressed(out InputKeys number))
            {
                var character = _keyboard.KeyToChar(number).ToString();

                _text.Text = _text.Text.Insert(_characterPosition, character);

                _characterPosition += 1;
            }

            //If a symbol was press on the keyboard
            if (_keyboard.IsSymbolPressed(out InputKeys symbol))
            {
                var character = _keyboard.KeyToChar(symbol).ToString();
                
                _text.Text = _text.Text.Insert(_characterPosition, character);

                _characterPosition += 1;
            }

            _keyboard.UpdatePreviousState();
        }


        private int CalcCursorXPos()
        {
            var textTemp = _text.Text;

            //Update the text that is from the first letter up to the cursor position
            _text.Text = _text.Text.Substring(0, _characterPosition);

            var result = _text.Width;

            _text.Text = textTemp;


            return result;
        }


        private string GetCharactersToFitTextboxWidth()
        {
            return "";
        }
        #endregion
    }
}

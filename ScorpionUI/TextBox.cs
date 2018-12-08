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
        private string _deferredText;
        private Vector _textPosition;
        private int _characterPosition;
        private int _cursorElapsedMilliseconds;
        private bool _cursorVisible;
        private const int LEFT_MARGIN = 5;
        private const int RIGHT_MARGIN = 5;
        private int _rightSide;
        private int _leftSide;
        private DeferredActions _deferredActions = new DeferredActions();


        #region Props
        public Vector Position { get; set; }

        public int Width => Background.Width;

        public int Height => Background.Height;

        public Texture Background { get; set; }

        public string FontName { get; set; }

        public string Text
        {
            get
            {
                return _text == null ? _deferredText : _text.Text;
            }
            set
            {
                if(_text is null)
                {
                    _deferredActions.Add(() =>
                    {
                        _text.Text = value;
                    });

                    _deferredText = value;
                }
                else
                {
                    _text.Text = value;
                }
            }
        }
        #endregion


        #region Constructors
        public TextBox()
        {
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Sets the position of the cursor in the textbox.
        /// </summary>
        /// <param name="position">The position of the cursor. Anything outside of the text will be set to the min or max.</param>
        public void SetCursorPosition(int position)
        {
            position = position < 0 ? 0 : position;
            position = position > Text.Length ? Text.Length : position;
            _characterPosition = position;
        }


        public void SetCursorToEnd()
        {
            _characterPosition = Text.Length;
        }


        public void Initialize()
        {
        }


        public void LoadContent(ContentLoader contentLoader)
        {
            _text = contentLoader.LoadText(FontName);
            _deferredActions.ExecuteAll();
        }


        public void Update(EngineTime engineTime)
        {
            var halfWidth = Width / 2;

            _leftSide = (int)Position.X - halfWidth + LEFT_MARGIN;
            _rightSide = (int)Position.X + halfWidth - RIGHT_MARGIN;

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

            //Update the X position of the text
            _textPosition = new Vector(_leftSide, Position.Y - _text.Height / 2f);

            var cursorPositionX = _leftSide + CalcCursorXPos();

            //Render the text inside of the textbox
            if (!string.IsNullOrEmpty(Text))
            {
                if (_text.Width <= _rightSide - _leftSide)
                {
                    renderer.Render(_text, _textPosition, new GameColor(0, 0, 0, 255));
                }
                else
                {
                    var tempText = _text.Text;

                    Text = GetCharactersToFitTextArea();
                    renderer.Render(_text, _textPosition, new GameColor(0, 0, 0, 255));
                    Text = tempText;
                }
            }

            //DEBUGGING
            //Render the margins for visual debugging
            var leftMarginStart = new Vector(_leftSide, Position.Y - 50);
            var leftMarginStop = new Vector(_leftSide, Position.Y + 50);
            renderer.Line(leftMarginStart, leftMarginStop, new GameColor(0, 255, 0, 255));

            var rightMarginStart = new Vector(_rightSide, Position.Y - 50);
            var rightMarginStop = new Vector(_rightSide, Position.Y + 50);
            renderer.Line(rightMarginStart, rightMarginStop, new GameColor(0, 255, 0, 255));
            ///////////

            //Render the blinking cursor
            var lineStart = new Vector(cursorPositionX, Position.Y - (Background.Height / 2) + 3);
            var lineStop = new Vector(cursorPositionX, Position.Y + (Background.Height / 2) - 3);

            lineStart.X = lineStart.X > _rightSide ? _rightSide : lineStart.X;
            lineStop.X = lineStop.X > _rightSide ? _rightSide : lineStop.X;

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


        private string GetCharactersToFitTextArea()
        {
            var textTemp = _text.Text;
            var textAreaWidth = _rightSide - _leftSide;

            _text.Text = string.Empty;

            for (int i = textTemp.Length - 1; i >= 0; i--)
            {
                _text.Text = _text.Text.Insert(0, textTemp[i].ToString());

                //If the text is currently too wide to fit, remove one character
                if (_text.Width > textAreaWidth)
                {
                    _text.Text = _text.Text.Substring(1, _text.Text.Length - 1);
                    break;
                }
            }


            var result = _text.Text;

            //Set text back to original value
            _text.Text = textTemp;


            return result;
        }


        private static string Reverse(string value)
        {
            var result = string.Empty;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                result += value[i];
            }


            return result;
        }
        #endregion
    }
}

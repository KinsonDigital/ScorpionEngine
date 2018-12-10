using System;
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
        private string _leftText = string.Empty;
        private string _rightText = string.Empty;
        private GameText _visibleText;
        private static GameText _textRuler;//Used for measuring text with and height
        private string _allText;
        private int _visibleTextCharPosition;
        private int _charPosDelta;
        private Vector _textPosition;
        private int _characterPosition;
        private int _cursorElapsedMilliseconds;
        private bool _cursorVisible;
        private const int LEFT_MARGIN = 5;
        private const int RIGHT_MARGIN = 5;
        private int _rightSide;
        private int _leftSide;
        private int _lastDirectionOfTravel = 0;
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
                return _allText;
            }
            set
            {
                _allText = value;
            }
        }
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
            _visibleText = contentLoader.LoadText(FontName);
            _textRuler = contentLoader.LoadText(FontName);

            _deferredActions.ExecuteAll();
        }


        public void Update(EngineTime engineTime)
        {
            UpdateSideLocations();

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
            _textPosition = new Vector(_leftSide, Position.Y - _visibleText.Height / 2f);

            //Render the text inside of the textbox
            _visibleText.Text = ClipText(_allText);

            renderer.Render(_visibleText, _textPosition, new GameColor(0, 0, 0, 255));

            //Render the end to cover any text that has passed the end of the render area
            var textureLeft = Position.X - (Width / 2);

            var topLeftCorner = new Vector(Position.X - Width / 2, Position.Y - Height / 2);

            var areaWidth = Width - (_rightSide - topLeftCorner.X);

            var coverArea = new Rect(Width - areaWidth, 0, areaWidth, Height);
            var coverPosition = new Vector(454, 250);// new Vector(_rightSide, Position.Y);

            renderer.RenderTextureArea(Background, coverArea, coverPosition);

            var cursorPositionX = _leftSide + CalcCursorXPos();

            //DEBUGGING
            //Render the dot at the right side line
            renderer.FillCircle(new Vector(_rightSide, Position.Y - Height / 2), 5, new GameColor(125, 125, 0, 255));

            //Render the margins for visual debugging
            var leftMarginStart = new Vector(_leftSide, Position.Y - 50);
            var leftMarginStop = new Vector(_leftSide, Position.Y + 50);
            renderer.Line(leftMarginStart, leftMarginStop, new GameColor(0, 255, 0, 255));

            var rightMarginStart = new Vector(_rightSide, Position.Y - 50);
            var rightMarginStop = new Vector(_rightSide, Position.Y + 50);
            renderer.Line(rightMarginStart, rightMarginStop, new GameColor(0, 255, 0, 255));
            ///////////

            //Render the blinking cursor
            var lineStart = CalcCursorStart();// new Vector(cursorPositionX, Position.Y - (Background.Height / 2) + 3);
            var lineStop = CalcCursorStop();// new Vector(cursorPositionX, Position.Y + (Background.Height / 2) - 3);

            lineStart.X = lineStart.X > _rightSide ? _rightSide : lineStart.X;
            lineStop.X = lineStop.X > _rightSide ? _rightSide : lineStop.X;

            if (_cursorVisible)
                renderer.Line(lineStart, lineStop, new GameColor(255, 0, 0, 255));//TODO: Change to black when finished with testing
        }
        #endregion


        #region Private Methods
        private void UpdateSideLocations()
        {
            var halfWidth = Width / 2;

            _leftSide = (int)Position.X - halfWidth + LEFT_MARGIN;
            _rightSide = (int)Position.X + halfWidth - RIGHT_MARGIN;
        }


        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            if (_keyboard.IsKeyPressed(InputKeys.Right))
            {
                _lastDirectionOfTravel = 1;

                _characterPosition = _characterPosition > _allText.Length - 1 ?
                    _characterPosition :
                    _characterPosition + 1;

                _visibleTextCharPosition = _visibleTextCharPosition > _visibleText.Text.Length - 1 ?
                    _visibleTextCharPosition :
                    _visibleTextCharPosition + 1;

                _charPosDelta = Math.Abs(_characterPosition - _visibleTextCharPosition);

                _keyboard.UpdatePreviousState();
                return;
            }

            if (_keyboard.IsKeyPressed(InputKeys.Left))
            {
                _lastDirectionOfTravel = -1;

                _characterPosition = _characterPosition <= 0 ?
                    _characterPosition :
                    _characterPosition - 1;

                _visibleTextCharPosition = _visibleTextCharPosition == 0 ?
                    _visibleTextCharPosition :
                    _visibleTextCharPosition - 1;

                _charPosDelta = Math.Abs(_characterPosition - _visibleTextCharPosition);

                _keyboard.UpdatePreviousState();
                return;
            }

            var isShiftDown = _keyboard.IsKeyDown(InputKeys.LeftShift) || _keyboard.IsKeyDown(InputKeys.RightShift);

            if (!string.IsNullOrEmpty(_visibleText.Text))
            {
                //The delete keys. This is the standard one and the numpad one
                if(_keyboard.IsDeleteKeyPressed())
                {
                    _visibleText.Text = _visibleText.Text.Remove(_characterPosition, 1);
                }

                if (_keyboard.IsKeyPressed(InputKeys.Back) && _characterPosition > 0)
                {
                    RemoveCharacterUsingBackspace();
                    //_visibleText.Text = _visibleText.Text.Remove(_characterPosition, 1);
                }
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

                _visibleText.Text = _visibleText.Text.Insert(_characterPosition, letterText);
                _characterPosition += 1;
            }

            //If a number was pressed on the keyboard
            if (_keyboard.IsNumberPressed(out InputKeys number))
            {
                var character = _keyboard.KeyToChar(number).ToString();

                _visibleText.Text = _visibleText.Text.Insert(_characterPosition, character);

                _characterPosition += 1;
            }

            //If a symbol was press on the keyboard
            if (_keyboard.IsSymbolPressed(out InputKeys symbol))
            {
                var character = _keyboard.KeyToChar(symbol).ToString();
                
                _visibleText.Text = _visibleText.Text.Insert(_characterPosition, character);

                _characterPosition += 1;
            }

            _keyboard.UpdatePreviousState();
        }


        private Vector CalcCursorStart()
        {
            var cursorPositionX = _leftSide + CalcCursorXPos();


            return new Vector(cursorPositionX, Position.Y - (Background.Height / 2) + 3);
        }


        private Vector CalcCursorStop()
        {
            var cursorPositionX = _leftSide + CalcCursorXPos();


            return new Vector(cursorPositionX, Position.Y + (Background.Height / 2) - 3);
        }


        private void RemoveCharacterUsingBackspace()
        {
            var isTextClipped = IsTextClipped();

            var visibleTextIndex = _allText.IndexOf(_visibleText.Text);
            var charToRemoveIndex = visibleTextIndex + _characterPosition - 1;

            _allText = _allText.Remove(charToRemoveIndex, 1);
        }


        private int CalcCursorXPos()
        {
            _textRuler.Text = string.Empty;

            //Update the text that is from the first letter up to the cursor position
            _textRuler.Text = _allText.Substring(_charPosDelta, Math.Abs(_characterPosition - _charPosDelta));

            var result = _textRuler.Width;

            _textRuler.Text = string.Empty;

            return result;
        }


        private string ClipText(string text)
        {
            _textRuler.Text = string.Empty;

            var textAreaWidth = _rightSide - _leftSide;

            var startIndex = _charPosDelta == 0 ?
                0 :
                _charPosDelta + _lastDirectionOfTravel;

            for (int i = startIndex; i < text.Length; i++)
            {
                _textRuler.Text += _allText[i].ToString();

                //If the text is currently too wide to fit, remove one character
                if (_textRuler.Width > textAreaWidth)
                {
                    _textRuler.Text = _textRuler.Text.Substring(0, _textRuler.Text.Length - 1);
                    break;
                }
            }


            var result = _textRuler.Text;

            _textRuler.Text = string.Empty;


            return result;
        }


        private bool IsPositionInLeftSection()
        {
            var allText = $"{_leftText}{_visibleText.Text}{_rightText}";

            return _characterPosition <= _leftText.Length - 1;
        }


        private bool IsPositionInCenterSection()
        {
            var combinedSections = $"{_leftText}{_visibleText.Text}";

            return _characterPosition >= _leftText.Length - 1 && _characterPosition <= combinedSections.Length - 1;
        }


        private bool IsTextClipped()
        {
            return _allText != _visibleText.Text;
        }
        #endregion
    }
}

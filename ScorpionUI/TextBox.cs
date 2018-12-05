using System;
using System.Collections.Generic;
using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionCore.Input;

namespace ScorpionUI
{
    //Provides the ability to enter text into a box.
    public class TextBox : IControl
    {
        private readonly InputKeys[] _letters = new []
        {
            InputKeys.A, InputKeys.B, InputKeys.C, InputKeys.D, InputKeys.E,
            InputKeys.F, InputKeys.G, InputKeys.H, InputKeys.I, InputKeys.J,
            InputKeys.K,InputKeys.L, InputKeys.M, InputKeys.N, InputKeys.O,
            InputKeys.P, InputKeys.Q, InputKeys.R, InputKeys.S, InputKeys.T,
            InputKeys.U, InputKeys.V, InputKeys.W, InputKeys.X, InputKeys.Y,
            InputKeys.Z
        };
        //Create list of symbol key codes when NOT using any shift key modifiers
        private static InputKeys[] _noShiftSymbols = new[]
        {
            InputKeys.OemSemicolon, InputKeys.OemPlus, InputKeys.OemComma,
            InputKeys.OemMinus, InputKeys.OemPeriod, InputKeys.OemQuestion,
            InputKeys.OemTilde, InputKeys.OemOpenBrackets, InputKeys.OemPipe,
            InputKeys.OemCloseBrackets, InputKeys.OemQuotes, InputKeys.Decimal,
            InputKeys.Divide, InputKeys.Multiply, InputKeys.Subtract, InputKeys.Add
        };
        //Create list of symbol key codes when PRESSING any of the shift key modifiers
        private static readonly InputKeys[] _withShiftSymbols = new[]
        {
            InputKeys.OemPlus, InputKeys.OemComma, InputKeys.OemMinus, InputKeys.OemPeriod,
            InputKeys.OemQuestion, InputKeys.OemTilde, InputKeys.OemPipe, InputKeys.OemOpenBrackets,
            InputKeys.OemCloseBrackets, InputKeys.OemQuotes, InputKeys.OemSemicolon,
            InputKeys.D0, InputKeys.D1, InputKeys.D2, InputKeys.D3, InputKeys.D4,
            InputKeys.D5, InputKeys.D6, InputKeys.D7, InputKeys.D8, InputKeys.D9,
            InputKeys.Divide, InputKeys.Multiply, InputKeys.Subtract, InputKeys.Add
        };
        //All of the text items to be shown when while NOT pressing
        //the left or right shift modifier keys
        private static Dictionary<InputKeys, string> _noShiftModifierSymbolTextItems = new Dictionary<InputKeys, string>()
        {
            { InputKeys.OemPlus, "=" },
            { InputKeys.OemComma, "," },
            { InputKeys.OemMinus, "-" },
            { InputKeys.OemPeriod, "." },
            { InputKeys.OemQuestion, "/" },
            { InputKeys.OemTilde, "`" },
            { InputKeys.OemPipe, "\\" },
            { InputKeys.OemOpenBrackets, "[" },
            { InputKeys.OemCloseBrackets, "]" },
            { InputKeys.OemQuotes, "'" },
            { InputKeys.OemSemicolon, ";" },
            { InputKeys.Decimal, "." },
            { InputKeys.Divide, "/" },
            { InputKeys.Multiply, "*" },
            { InputKeys.Subtract, "-" },
            { InputKeys.Add, "+" }
        };
        //All of the text items to be shown when while pressing
        //the left or right shift modifier keys
        private static Dictionary<InputKeys, string> _withShiftModifierSymbolTextItems = new Dictionary<InputKeys, string>()
        {
            { InputKeys.OemPlus, "+" },
            { InputKeys.OemComma, "<" },
            { InputKeys.OemMinus, "_" },
            { InputKeys.OemPeriod, ">" },
            { InputKeys.OemQuestion, "?" },
            { InputKeys.OemTilde, "~" },
            { InputKeys.OemPipe, "|" },
            { InputKeys.OemOpenBrackets, "{" },
            { InputKeys.OemCloseBrackets, "}" },
            { InputKeys.OemQuotes, "\"" },
            { InputKeys.OemSemicolon, ":" },
            { InputKeys.D1, "!" },
            { InputKeys.D2, "@" },
            { InputKeys.D3, "#" },
            { InputKeys.D4, "$" },
            { InputKeys.D5, "%" },
            { InputKeys.D6, "^" },
            { InputKeys.D7, "&" },
            { InputKeys.D8, "*" },
            { InputKeys.D9, "(" },
            { InputKeys.D0, ")" },
            { InputKeys.Divide, "/" },
            { InputKeys.Multiply, "*" },
            { InputKeys.Subtract, "-" },
            { InputKeys.Add, "+" }
        };
        private Keyboard _keyboard = new Keyboard();
        private GameText _text;
        private GameText _upToCursorText;
        private Vector _textPosition;
        private int _characterPosition;
        private int _cursorElapsedMilliseconds;
        private bool _cursorVisible;
        private byte _red;


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
            _upToCursorText = contentLoader.LoadText(FontName);
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
                renderer.Render(_text, _textPosition, new GameColor(_red, 0, 0, 255));

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

            //The delete keys. This is the standard one and the numpad one
            if(_keyboard.IsDeleteKeyPressed())
            //if (_keyboard.IsKeyPressed(InputKeys.Delete) && _characterPosition < _text.Text.Length)
            {
                _text.Text = _text.Text.Remove(_characterPosition, 1);
            }

            if (_keyboard.IsKeyPressed(InputKeys.Back) && _characterPosition > 0)
            {
                _characterPosition -= 1;
                _text.Text = _text.Text.Remove(_characterPosition, 1);
            }

            var isShiftDown = _keyboard.IsKeyDown(InputKeys.LeftShift) || _keyboard.IsKeyDown(InputKeys.RightShift);
            
            //If a letter is pressed, add it to the textbox
            if (IsLetterPressed(out InputKeys letter))
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

            var keys = new List<InputKeys>();
            for (int i = (int)InputKeys.D0; i <= (int)InputKeys.D9; i++)
            {
                keys.Add((InputKeys)i);
            }

            ///////DEBUGGING
            if(_keyboard.IsAnyKeyDown(keys.ToArray()))
            {
                _red = 255;
            }
            
            if(_keyboard.IsKeyPressed(InputKeys.Space))
            {
                _red = 0;
            }

            if (_keyboard.AreAnyKeysDown())
            {
                var downKeys = _keyboard.GetCurrentPressedKeys();

                if(downKeys[0] == InputKeys.D0)
                { }
            }
            /////////////////

            //If a symbol was press on the keyboard
            if (IsSymbolPressed(isShiftDown, out InputKeys symbol))
            {
                var symbolText = isShiftDown ? _withShiftModifierSymbolTextItems[symbol] : _noShiftModifierSymbolTextItems[symbol];
                
                _text.Text = _text.Text.Insert(_characterPosition, symbolText);

                _characterPosition += 1;
            }

            _keyboard.UpdatePreviousState();
        }


        private int CalcCursorXPos()
        {
            //TODO: Remove when finished debugging
            var subString = _text.Text.Substring(0, _characterPosition);

            //Update the text that is from the first letter up to the cursor position
            _upToCursorText.Text = _text.Text.Substring(0, _characterPosition);

            return _upToCursorText.Width;
        }


        private bool IsLetterPressed(out InputKeys letter)
        {
            for (int i = 0; i < _letters.Length; i++)
            {
                if(_keyboard.IsKeyPressed((InputKeys)_letters[i]))
                {
                    letter = (InputKeys)_letters[i];
                    return true;
                }
            }

            letter = InputKeys.None;


            return false;
        }


        private bool IsSymbolPressed(bool shiftDown, out InputKeys symbol)
        {
            if(shiftDown)
            {
                //Check if any symbols have been pressed while pressing the 
                //left or right shift modifier keys
                for (int i = 0; i < _withShiftSymbols.Length; i++)
                {
                    if (_keyboard.IsKeyPressed(_withShiftSymbols[i]))
                    {
                        symbol = _withShiftSymbols[i];
                        return true;
                    }
                }
            }
            else
            {
                //Check if any symbols have been pressed while NOT pressing the 
                //left or right shift modifier keys
                for (int i = 0; i < _noShiftSymbols.Length; i++)
                {
                    if (_keyboard.IsKeyPressed(_noShiftSymbols[i]))
                    {
                        symbol = _noShiftSymbols[i];
                        return true;
                    }
                }
            }

            symbol = InputKeys.None;


            return false;
        }
        #endregion
    }
}

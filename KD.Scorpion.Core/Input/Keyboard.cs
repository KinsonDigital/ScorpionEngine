using KDScorpionCore.Plugins;
using System.Collections.Generic;
using System.Linq;

namespace KDScorpionCore.Input
{
    /// <summary>
    /// Tracks the state of the keys on keyboard.
    /// </summary>
    public class Keyboard
    {
        #region Fields
        private readonly InputKeys[] _lettersKeys = new[]
        {
            InputKeys.A, InputKeys.B, InputKeys.C, InputKeys.D, InputKeys.E,
            InputKeys.F, InputKeys.G, InputKeys.H, InputKeys.I, InputKeys.J,
            InputKeys.K,InputKeys.L, InputKeys.M, InputKeys.N, InputKeys.O,
            InputKeys.P, InputKeys.Q, InputKeys.R, InputKeys.S, InputKeys.T,
            InputKeys.U, InputKeys.V, InputKeys.W, InputKeys.X, InputKeys.Y,
            InputKeys.Z, InputKeys.Space
        };

        private static readonly InputKeys[] _numbersKeys = new[]
        {
            InputKeys.D0, InputKeys.D1, InputKeys.D2,
            InputKeys.D3, InputKeys.D4, InputKeys.D5,
            InputKeys.D6, InputKeys.D7, InputKeys.D8,
            InputKeys.D9, InputKeys.NumPad0, InputKeys.NumPad0,
            InputKeys.NumPad0, InputKeys.NumPad1, InputKeys.NumPad2,
            InputKeys.NumPad3, InputKeys.NumPad4, InputKeys.NumPad5,
            InputKeys.NumPad6, InputKeys.NumPad7, InputKeys.NumPad8,
            InputKeys.NumPad9,
        };

        private static InputKeys[] _symbolKeys = new[]
        {
            InputKeys.OemSemicolon, InputKeys.OemPlus, InputKeys.OemComma,
            InputKeys.OemMinus, InputKeys.OemPeriod, InputKeys.OemQuestion,
            InputKeys.OemTilde, InputKeys.OemOpenBrackets, InputKeys.OemPipe,
            InputKeys.OemCloseBrackets, InputKeys.OemQuotes, InputKeys.Decimal,
            InputKeys.Divide, InputKeys.Multiply, InputKeys.Subtract, InputKeys.Add
        };

        private static Dictionary<InputKeys, string> _noShiftModifierSymbolTextItems = new Dictionary<InputKeys, string>()
        {
            { InputKeys.OemPlus, "=" }, { InputKeys.OemComma, "," }, { InputKeys.OemMinus, "-" },
            { InputKeys.OemPeriod, "." }, { InputKeys.OemQuestion, "/" }, { InputKeys.OemTilde, "`" },
            { InputKeys.OemPipe, "\\" }, { InputKeys.OemOpenBrackets, "[" }, { InputKeys.OemCloseBrackets, "]" },
            { InputKeys.OemQuotes, "'" }, { InputKeys.OemSemicolon, ";" }, { InputKeys.Decimal, "." },
            { InputKeys.Divide, "/" }, { InputKeys.Multiply, "*" }, { InputKeys.Subtract, "-" },
            { InputKeys.Add, "+" }
        };

        private static Dictionary<InputKeys, string> _withShiftModifierSymbolTextItems = new Dictionary<InputKeys, string>()
        {
            { InputKeys.OemPlus, "+" }, { InputKeys.OemComma, "<" }, { InputKeys.OemMinus, "_" },
            { InputKeys.OemPeriod, ">" }, { InputKeys.OemQuestion, "?" }, { InputKeys.OemTilde, "~" },
            { InputKeys.OemPipe, "|" }, { InputKeys.OemOpenBrackets, "{" }, { InputKeys.OemCloseBrackets, "}" },
            { InputKeys.OemQuotes, "\"" }, { InputKeys.OemSemicolon, ":" }, { InputKeys.D1, "!" },
            { InputKeys.D2, "@" }, { InputKeys.D3, "#" }, { InputKeys.D4, "$" }, { InputKeys.D5, "%" },
            { InputKeys.D6, "^" }, { InputKeys.D7, "&" }, { InputKeys.D8, "*" }, { InputKeys.D9, "(" },
            { InputKeys.D0, ")" }, { InputKeys.Divide, "/" }, { InputKeys.Multiply, "*" }, { InputKeys.Subtract, "-" },
            { InputKeys.Add, "+" }
        };
        #endregion


        #region Constructors
        internal Keyboard(IKeyboard keyboard)
        {
            InternalKeyboard = keyboard;
        }


        /// <summary>
        /// Creates a new instance of <see cref="Keyboard"/> for tracking keyboard events.
        /// </summary>
        public Keyboard()
        {
            InternalKeyboard = PluginSystem.EnginePlugins.LoadPlugin<IKeyboard>();
        }
        #endregion


        #region Props
        /// <summary>
        /// The internal keyboard plugin implementation.
        /// </summary>
        internal IKeyboard InternalKeyboard { get; }

        /// <summary>
        /// Gets a value indicating if the caps lock key is on.
        /// </summary>
        public bool CapsLockOn => InternalKeyboard.CapsLockOn;

        /// <summary>
        /// Gets a value indicating if the numlock key is on.
        /// </summary>
        public bool NumLockOn => InternalKeyboard.NumLockOn;
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns all of the currently pressed keys on the keyboard.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetCurrentPressedKeys()
        {
            return (from k in InternalKeyboard.GetCurrentPressedKeys()
                    select (InputKeys)k).ToArray();
        }


        /// <summary>
        /// Returns all of the previously pressed keys from the last frame.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetPreviousPressedKeys()
        {
            return (from k in InternalKeyboard.GetPreviousPressedKeys()
                    select (InputKeys)k).ToArray();
        }


        /// <summary>
        /// Gets a value indicating if any keys are in the down position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysDown()
        {
            return InternalKeyboard.AreAnyKeysDown();
        }


        /// <summary>
        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysPressed()
        {
            return InternalKeyboard.AreAnyKeysPressed();
        }


        /// <summary>
        /// Returns a value indicating if any of the given key codes are being held down.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        public bool IsAnyKeyDown(InputKeys[] keys)
        {
            var keyCodes = new List<int>();

            foreach (var key in keys)
            {
                keyCodes.Add((int)key);
            }


            return InternalKeyboard.IsAnyKeyDown(keyCodes.ToArray());
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(InputKeys key)
        {
            return InternalKeyboard.IsKeyDown((int)key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(InputKeys key)
        {
            return InternalKeyboard.IsKeyUp((int)key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(InputKeys key)
        {
            return InternalKeyboard.IsKeyPressed((int)key);
        }


        /// <summary>
        /// Returns a value indicating if any letter key is pressed.
        /// </summary>
        /// <param name="letterKey">The letter key that was pressed if found.</param>
        /// <returns></returns>
        public bool IsLetterPressed(out InputKeys letterKey)
        {
            for (int i = 0; i < _lettersKeys.Length; i++)
            {
                if (InternalKeyboard.IsKeyPressed((int)_lettersKeys[i]))
                {
                    letterKey = _lettersKeys[i];
                    return true;
                }
            }

            letterKey = InputKeys.None;


            return false;
        }


        /// <summary>
        /// Returns a value indicating if any number key is pressed.
        /// </summary>
        /// <param name="symbolKey">The number key that was pressed if found.</param>
        /// <returns></returns>
        public bool IsNumberPressed(out InputKeys numberKey)
        {
            if (IsAnyShiftKeyDown())
            {
                numberKey = InputKeys.None;
                return false;
            }

            for (int i = 0; i < _numbersKeys.Length; i++)
            {
                if (InternalKeyboard.IsKeyPressed((int)_numbersKeys[i]))
                {
                    numberKey = _numbersKeys[i];

                    return true;
                }
            }

            numberKey = InputKeys.None;


            return false;
        }


        /// <summary>
        /// Returns a value indicating if any symbol key is pressed.
        /// </summary>
        /// <param name="symbolKey">The symbok key that was pressed if found.</param>
        /// <returns></returns>
        public bool IsSymbolPressed(out InputKeys symbolKey)
        {
            if (IsAnyShiftKeyDown())
            {
                for (int i = 0; i < _numbersKeys.Length; i++)
                {
                    if (InternalKeyboard.IsKeyPressed((int)_numbersKeys[i]))
                    {
                        symbolKey = _numbersKeys[i];
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _symbolKeys.Length; i++)
                {
                    if (InternalKeyboard.IsKeyPressed((int)_symbolKeys[i]))
                    {
                        symbolKey = _symbolKeys[i];
                        return true;
                    }
                }
            }

            symbolKey = InputKeys.None;


            return false;
        }


        /// <summary>
        /// Returns the character equivalent of the given key if it was
        /// a letter, number or symbol key.  Tilde(~) will be returned if not
        /// a letter, number or symbol.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public char KeyToChar(InputKeys key)
        {
            if (IsAnyShiftKeyDown())
            {
                if (_lettersKeys.Contains(key))
                {
                    return key.ToString()[0];
                }
                else if (_symbolKeys.Contains(key))
                {
                    return _withShiftModifierSymbolTextItems[key][0];
                }
                else if (_numbersKeys.Contains(key))
                {
                    var keyString = key.ToString();

                    return keyString[keyString.Length - 1];
                }
            }
            else
            {
                if(_lettersKeys.Contains(key))
                {
                    return key.ToString().ToLower()[0];
                }
                else if(_symbolKeys.Contains(key))
                {
                    return _noShiftModifierSymbolTextItems[key][0];
                }
                else if(_numbersKeys.Contains(key))
                {
                    var keyString = key.ToString();

                    return keyString[keyString.Length - 1];
                }
            }


            return '~';
        }


        /// <summary>
        /// Returns a value indicating if any of the shift keys are being pressed down.
        /// </summary>
        /// <returns></returns>
        public bool IsAnyShiftKeyDown()
        {
            return InternalKeyboard.IsKeyDown((int)InputKeys.LeftShift) || InternalKeyboard.IsKeyDown((int)InputKeys.RightShift);
        }


        /// <summary>
        /// Returns a value indicating if the any of the delete keys ahve been fully pressed.
        /// </summary>
        /// <returns></returns>
        public bool IsDeleteKeyPressed()
        {
            return IsKeyPressed(InputKeys.Delete) || (IsAnyShiftKeyDown() && IsKeyPressed(InputKeys.Decimal));
        }


        /// <summary>
        /// Returns a value indicating if the backspace key has been fully pressed.
        /// </summary>
        /// <returns></returns>
        public bool IsBackspaceKeyPressed()
        {
            return IsKeyPressed(InputKeys.Back);
        }


        /// <summary>
        /// Returns a value indicating if a letter on the keyboard was pressed.
        /// </summary>
        /// <returns></returns>
        public bool WasLetterPressed()
        {
            return InternalKeyboard.WasLetterPressed();
        }


        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState()
        {
            InternalKeyboard.UpdateCurrentState();
        }


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        public void UpdatePreviousState()
        {
            InternalKeyboard.UpdatePreviousState();
        }
        #endregion
    }
}

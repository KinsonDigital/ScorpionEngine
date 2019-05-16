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
        private readonly KeyCodes[] _lettersKeys = new[]
        {
            KeyCodes.A, KeyCodes.B, KeyCodes.C, KeyCodes.D, KeyCodes.E,
            KeyCodes.F, KeyCodes.G, KeyCodes.H, KeyCodes.I, KeyCodes.J,
            KeyCodes.K,KeyCodes.L, KeyCodes.M, KeyCodes.N, KeyCodes.O,
            KeyCodes.P, KeyCodes.Q, KeyCodes.R, KeyCodes.S, KeyCodes.T,
            KeyCodes.U, KeyCodes.V, KeyCodes.W, KeyCodes.X, KeyCodes.Y,
            KeyCodes.Z, KeyCodes.Space
        };

        private static readonly KeyCodes[] _numbersKeys = new[]
        {
            KeyCodes.D0, KeyCodes.D1, KeyCodes.D2,
            KeyCodes.D3, KeyCodes.D4, KeyCodes.D5,
            KeyCodes.D6, KeyCodes.D7, KeyCodes.D8,
            KeyCodes.D9, KeyCodes.NumPad0, KeyCodes.NumPad0,
            KeyCodes.NumPad0, KeyCodes.NumPad1, KeyCodes.NumPad2,
            KeyCodes.NumPad3, KeyCodes.NumPad4, KeyCodes.NumPad5,
            KeyCodes.NumPad6, KeyCodes.NumPad7, KeyCodes.NumPad8,
            KeyCodes.NumPad9,
        };

        private static KeyCodes[] _symbolKeys = new[]
        {
            KeyCodes.OemSemicolon, KeyCodes.OemPlus, KeyCodes.OemComma,
            KeyCodes.OemMinus, KeyCodes.OemPeriod, KeyCodes.OemQuestion,
            KeyCodes.OemTilde, KeyCodes.OemOpenBrackets, KeyCodes.OemPipe,
            KeyCodes.OemCloseBrackets, KeyCodes.OemQuotes, KeyCodes.Decimal,
            KeyCodes.Divide, KeyCodes.Multiply, KeyCodes.Subtract, KeyCodes.Add
        };

        private static Dictionary<KeyCodes, string> _noShiftModifierSymbolTextItems = new Dictionary<KeyCodes, string>()
        {
            { KeyCodes.OemPlus, "=" }, { KeyCodes.OemComma, "," }, { KeyCodes.OemMinus, "-" },
            { KeyCodes.OemPeriod, "." }, { KeyCodes.OemQuestion, "/" }, { KeyCodes.OemTilde, "`" },
            { KeyCodes.OemPipe, "\\" }, { KeyCodes.OemOpenBrackets, "[" }, { KeyCodes.OemCloseBrackets, "]" },
            { KeyCodes.OemQuotes, "'" }, { KeyCodes.OemSemicolon, ";" }, { KeyCodes.Decimal, "." },
            { KeyCodes.Divide, "/" }, { KeyCodes.Multiply, "*" }, { KeyCodes.Subtract, "-" },
            { KeyCodes.Add, "+" }
        };

        private static Dictionary<KeyCodes, string> _withShiftModifierSymbolTextItems = new Dictionary<KeyCodes, string>()
        {
            { KeyCodes.OemPlus, "+" }, { KeyCodes.OemComma, "<" }, { KeyCodes.OemMinus, "_" },
            { KeyCodes.OemPeriod, ">" }, { KeyCodes.OemQuestion, "?" }, { KeyCodes.OemTilde, "~" },
            { KeyCodes.OemPipe, "|" }, { KeyCodes.OemOpenBrackets, "{" }, { KeyCodes.OemCloseBrackets, "}" },
            { KeyCodes.OemQuotes, "\"" }, { KeyCodes.OemSemicolon, ":" }, { KeyCodes.D1, "!" },
            { KeyCodes.D2, "@" }, { KeyCodes.D3, "#" }, { KeyCodes.D4, "$" }, { KeyCodes.D5, "%" },
            { KeyCodes.D6, "^" }, { KeyCodes.D7, "&" }, { KeyCodes.D8, "*" }, { KeyCodes.D9, "(" },
            { KeyCodes.D0, ")" }, { KeyCodes.Divide, "/" }, { KeyCodes.Multiply, "*" }, { KeyCodes.Subtract, "-" },
            { KeyCodes.Add, "+" }
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
        public KeyCodes[] GetCurrentPressedKeys()
        {
            return (from k in InternalKeyboard.GetCurrentPressedKeys()
                    select (KeyCodes)k).ToArray();
        }


        /// <summary>
        /// Returns all of the previously pressed keys from the last frame.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetPreviousPressedKeys()
        {
            return (from k in InternalKeyboard.GetPreviousPressedKeys()
                    select (KeyCodes)k).ToArray();
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
        /// Returns a value indicating if any of the given key codes are being held down.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        public bool IsAnyKeyDown(KeyCodes[] keys)
        {
            var keyCodes = new List<int>();

            foreach (var key in keys)
            {
                keyCodes.Add((int)key);
            }


            return InternalKeyboard.IsAnyKeyDown(keys);
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(KeyCodes key)
        {
            return InternalKeyboard.IsKeyDown(key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(KeyCodes key)
        {
            return InternalKeyboard.IsKeyUp((int)key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(KeyCodes key)
        {
            return InternalKeyboard.IsKeyPressed((int)key);
        }


        /// <summary>
        /// Returns a value indicating if any letter key is pressed.
        /// </summary>
        /// <param name="letterKey">The letter key that was pressed if found.</param>
        /// <returns></returns>
        public bool IsLetterPressed(out KeyCodes letterKey)
        {
            for (int i = 0; i < _lettersKeys.Length; i++)
            {
                if (InternalKeyboard.IsKeyPressed((int)_lettersKeys[i]))
                {
                    letterKey = _lettersKeys[i];
                    return true;
                }
            }

            letterKey = KeyCodes.None;


            return false;
        }


        /// <summary>
        /// Returns a value indicating if any number key is pressed.
        /// </summary>
        /// <param name="symbolKey">The number key that was pressed if found.</param>
        /// <returns></returns>
        public bool IsNumberPressed(out KeyCodes numberKey)
        {
            if (IsAnyShiftKeyDown())
            {
                numberKey = KeyCodes.None;
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

            numberKey = KeyCodes.None;


            return false;
        }


        /// <summary>
        /// Returns a value indicating if any symbol key is pressed.
        /// </summary>
        /// <param name="symbolKey">The symbok key that was pressed if found.</param>
        /// <returns></returns>
        public bool IsSymbolPressed(out KeyCodes symbolKey)
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

            symbolKey = KeyCodes.None;


            return false;
        }


        /// <summary>
        /// Returns the character equivalent of the given key if it was
        /// a letter, number or symbol key.  Tilde(~) will be returned if not
        /// a letter, number or symbol.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public char KeyToChar(KeyCodes key)
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
            return InternalKeyboard.IsKeyDown(KeyCodes.LeftShift) || InternalKeyboard.IsKeyDown(KeyCodes.RightShift);
        }


        /// <summary>
        /// Returns a value indicating if the any of the delete keys ahve been fully pressed.
        /// </summary>
        /// <returns></returns>
        public bool IsDeleteKeyPressed()
        {
            return IsKeyPressed(KeyCodes.Delete) || (IsAnyShiftKeyDown() && IsKeyPressed(KeyCodes.Decimal));
        }


        /// <summary>
        /// Returns a value indicating if the backspace key has been fully pressed.
        /// </summary>
        /// <returns></returns>
        public bool IsBackspaceKeyPressed()
        {
            return IsKeyPressed(KeyCodes.Back);
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

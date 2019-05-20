using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using SDL2;

namespace SDLScorpPlugin
{
    /// <summary>
    /// Used to check the state of the keyboard using the SDL
    /// crossplatform library.
    /// </summary>
    public class SDLKeyboard : IKeyboard
    {
        #region Private Vars
        /// <summary>
        /// Holds a list of all the keys and there current state this frame.
        /// </summary>
        private readonly static Dictionary<SDL.SDL_Keycode, bool> _currentStateKeys = new Dictionary<SDL.SDL_Keycode, bool>();
        //new Dictionary<SDL.SDL_Keycode, bool>((from k in KeyboardKeyMapper.SDLToStandardMappings select new KeyValuePair<SDL.SDL_Keycode, bool>(k.Key, false)).ToArray());

        /// <summary>
        /// Holds a list of all the keys and there state the previous frame.
        /// </summary>
        private readonly static Dictionary<SDL.SDL_Keycode, bool> _prevStateKeys = new Dictionary<SDL.SDL_Keycode, bool>();
            //new Dictionary<SDL.SDL_Keycode, bool>((from k in KeyboardKeyMapper.SDLToStandardMappings select new KeyValuePair<SDL.SDL_Keycode, bool>(k.Key, false)).ToArray());
        #endregion


        #region Props
        /// <summary>
        /// Gets a value indicating if the caps lock key is on.
        /// </summary>
        public bool CapsLockOn => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_CAPS) == SDL.SDL_Keymod.KMOD_CAPS;

        /// <summary>
        /// Gets a value indicating if the numlock key is on.
        /// </summary>
        public bool NumLockOn => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_NUM) == SDL.SDL_Keymod.KMOD_NUM;

        /// <summary>
        /// Gets a value indicating if the left shift key is being pressed.
        /// </summary>
        public bool IsLeftShiftDown => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_LSHIFT) == SDL.SDL_Keymod.KMOD_LSHIFT;

        /// <summary>
        /// Gets a value indicating if the right shift key is being pressed.
        /// </summary>
        public bool IsRightShiftDown => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_RSHIFT) == SDL.SDL_Keymod.KMOD_RSHIFT;

        /// <summary>
        /// Gets a value indicating if the left control key is being pressed down.
        /// </summary>
        public bool IsLeftCtrlDown => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_LCTRL) == SDL.SDL_Keymod.KMOD_LCTRL;

        /// <summary>
        /// Gets a value indicating if the right control key is being pressed down.
        /// </summary>
        public bool IsRightCtrlDown => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_RCTRL) == SDL.SDL_Keymod.KMOD_RCTRL;

        /// <summary>
        /// Gets a value indicating if the left alt key is being pressed down.
        /// </summary>
        public bool IsLeftAltDown => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_LALT) == SDL.SDL_Keymod.KMOD_LALT;

        /// <summary>
        /// Gets a value indicating if the right alt key is being pressed down.
        /// </summary>
        public bool IsRightAltDown => (SDL.SDL_GetModState() & SDL.SDL_Keymod.KMOD_RALT) == SDL.SDL_Keymod.KMOD_RALT;
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if any keys are in the down position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysDown() => _currentStateKeys.Any(k => k.Value);


        /// <summary>
        /// Returns all of the currently pressed keys of the keyboard for the current frame.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetCurrentPressedKeys()
        {
            return (from k in _currentStateKeys
                    where k.Value
                    select KeyboardKeyMapper.ToStandardKeyCode(k.Key)).ToArray();
        }


        /// <summary>
        /// Returns all of the previously pressed keys of the keyborad from the last frame.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetPreviousPressedKeys()
        {
            return (from k in _prevStateKeys
                    where k.Value
                    select KeyboardKeyMapper.ToStandardKeyCode(k.Key)).ToArray();
        }


        /// <summary>
        /// Returns a value indicating if any of the given key codes are being held down.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        public bool IsAnyKeyDown(KeyCodes[] keys)
        {
            var downKeys = (from k in _currentStateKeys where k.Value select k).ToArray();


            return keys.Any(k => downKeys.Any(dk => dk.Key == KeyboardKeyMapper.ToSDLKeyCode((KeyCodes)k)));
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(KeyCodes key) => _currentStateKeys[KeyboardKeyMapper.ToSDLKeyCode(key)];


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(KeyCodes key) => !IsKeyDown(key);


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(KeyCodes key) => _currentStateKeys[KeyboardKeyMapper.ToSDLKeyCode(key)] == false &&
            _prevStateKeys[KeyboardKeyMapper.ToSDLKeyCode(key)] == true;


        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState()
        {
            while (SDL.SDL_PollEvent(out var e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    _currentStateKeys[e.key.keysym.sym] = true;
                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    _currentStateKeys[e.key.keysym.sym] = false;
                }
            }
        }


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        public void UpdatePreviousState()
        {
            foreach (var currentKey in _currentStateKeys)
            {
                _prevStateKeys[currentKey.Key] = currentKey.Value;
            }
        }

        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }

        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

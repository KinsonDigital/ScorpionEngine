using KDScorpionCore.Plugins;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLScorpPlugin
{
    public class SDLMouse : IMouse
    {
        #region Private Fields
        private static bool _currentLeftButtonState;
        private static bool _currentRightButtonState;
        private static bool _currentMiddleButtonState;
        private static bool _prevLeftButtonState;
        private static bool _prevRightButtonState;
        private static bool _prevMiddleButtonState;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the X position of the mouse in the game window.
        /// </summary>
        public int X
        {
            get => (int)SDLEngineCore.MousePosition.X;
            set => SetPosition(value, Y);
        }

        /// <summary>
        /// Gets or sets the Y position of the mouse in the game window.
        /// </summary>
        public int Y
        {
            get => (int)SDLEngineCore.MousePosition.Y;
            set => SetPosition(X, value);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns true if the given input is in the down position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonDown(int input)
        {
            //Return the down state of the given mouse input
            switch (input)
            {
                case 1://Left button
                    return _currentLeftButtonState;
                case 2://Right button
                    return _currentRightButtonState;
                case 3://Middle button
                    return _currentMiddleButtonState;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }


        /// <summary>
        /// Returns true if the given input is in the up position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonUp(int input) => !IsButtonDown(input);


        /// <summary>
        /// Returns true if the given mouse input has been released from the down position.
        /// </summary>
        /// <param name="input">The mouse input to check for.</param>
        /// <returns></returns>
        public bool IsButtonPressed(int input)
        {
            //Return the pressed state of the given mouse input
            switch (input)
            {
                case 1://Left button
                    return !_currentLeftButtonState && _prevLeftButtonState;
                case 2://Right button
                    return !_currentRightButtonState && _prevRightButtonState;
                case 3://Middle button
                    return !_currentMiddleButtonState && _prevMiddleButtonState;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }


        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="x">The horizontal position to set the mouse to over the game window.</param>
        /// <param name="y">The vertical position to set the mouse to over the game window.</param>
        public void SetPosition(int x, int y) => SDL.SDL_WarpMouseInWindow(SDLEngineCore.WindowPtr, x, y);


        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        public void UpdateCurrentState()
        {
            _currentLeftButtonState = SDLEngineCore.CurrentLeftMouseButtonState;
            _currentRightButtonState = SDLEngineCore.CurrentRightMouseButtonState;
            _currentMiddleButtonState = SDLEngineCore.CurrentMiddleMouseButtonState;
        }


        /// <summary>
        /// Update the previous state of the mouse.
        /// </summary>
        public void UpdatePreviousState()
        {
            _prevLeftButtonState = _currentLeftButtonState;
            _prevMiddleButtonState = _currentMiddleButtonState;
            _prevRightButtonState = _currentRightButtonState;
        }


        /// <summary>
        /// Injects <see cref="SDLMouse"/> related data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Gets <see cref="SDLMouse"/> related data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();
        #endregion
    }
}

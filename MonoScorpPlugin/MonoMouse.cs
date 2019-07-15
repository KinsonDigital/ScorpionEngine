using System;
using Microsoft.Xna.Framework.Input;
using XNAMouse = Microsoft.Xna.Framework.Input.Mouse;
using KDScorpionCore.Plugins;
using KDScorpionCore.Input;

namespace MonoScorpPlugin
{
    public class MonoMouse : IMouse
    {
        #region Private Fields
        private MouseState _currentState;//The current state of the mouse
        private MouseState _previousState;//The previous state of the mouse on the last frame
        #endregion


        #region Props
        /// <summary>
        /// Gets sets the X position of the mouse.
        /// </summary>
        public int X
        {
            get => _currentState.X;
            set => SetPosition(value, _currentState.Y);
        }

        /// <summary>
        /// Gets sets the Y position of the mouse.
        /// </summary>
        public int Y
        {
            get => _currentState.Y;
            set => SetPosition(_currentState.X, value);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        public void UpdateCurrentState() => _currentState = XNAMouse.GetState();


        /// <summary>
        /// Update the previous state of the mouse.
        /// </summary>
        public void UpdatePreviousState() => _previousState = _currentState;


        /// <summary>
        /// Sets the position of the mouse using the given <paramref name="x"/> and <paramref name="y"/> values.
        /// </summary>
        /// <param name="x">The horizontal X position to set the mouse in the game window.</param>
        /// <param name="y">The vertical Y position to set the mouse in the game window.</param>
        public void SetPosition(int x, int y) => XNAMouse.SetPosition(x, y);


        /// <summary>
        /// Returns true if the given input is in the down position.
        /// </summary>
        /// <param name="input">The input button to check.</param>
        /// <returns></returns>
        public bool IsButtonDown(InputButton input)
        {
            //Return the down state of the given mouse input
            switch (input)
            {
                case InputButton.LeftButton:
                    return _currentState.LeftButton == ButtonState.Pressed;
                case InputButton.RightButton:
                    return _currentState.RightButton == ButtonState.Pressed;
                case InputButton.MiddleButton:
                    return _currentState.MiddleButton == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }


        /// <summary>
        /// Returns true if the given <paramref name="inputButton"/> is in the up position.
        /// </summary>
        /// <param name="inputButton">The input button to check.</param>
        /// <returns></returns>
        public bool IsButtonUp(InputButton input) => !IsButtonDown(input);


        /// <summary>
        /// Returns true if the given mouse <paramref name="inputButton"/> has been released from the down position.
        /// </summary>
        /// <param name="inputButton">The input button to check.</param>
        /// <returns></returns>
        public bool IsButtonPressed(InputButton input)
        {
            //Check the given input to see if it has been released from the down position
            switch (input)
            {
                case InputButton.LeftButton:
                    return _currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed;
                case InputButton.RightButton:
                    return _currentState.RightButton == ButtonState.Released && _previousState.RightButton == ButtonState.Pressed;
                case InputButton.MiddleButton:
                    return _currentState.MiddleButton == ButtonState.Released && _previousState.MiddleButton == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();
        #endregion
    }
}

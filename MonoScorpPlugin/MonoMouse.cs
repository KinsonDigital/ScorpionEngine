using System;
using ScorpionCore;
using Microsoft.Xna.Framework.Input;
using ScorpionCore.Plugins;

namespace MonoScorpPlugin
{
    public class MonoMouse : IMouse
    {
        #region Fields
        private MouseState _currentState;//The current state of the mouse
        private MouseState _previousState;//The previous state of the mouse on the last frame
        #endregion


        #region Props
        /// <summary>
        /// Gets the X position of the mouse.
        /// </summary>
        public int X => _currentState.X;

        /// <summary>
        /// Gets the Y position of the mouse.
        /// </summary>
        public int Y => _currentState.Y;
        #endregion


        #region Protected Methods
        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        public void UpdateCurrentState()
        {
            //Get the state of the mouse and save it as the current state
            _currentState = Mouse.GetState();
        }


        /// <summary>
        /// Update the previous state of the mouse.
        /// </summary>
        public void UpdatePreviousState()
        {
            //Update the previous state
            _previousState = _currentState;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="x">The horizontal position to set the mouse to over the game window.</param>
        /// <param name="y">The vertical position to set the mouse to over the game window.</param>
        public void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }


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
                    return _currentState.LeftButton == ButtonState.Pressed;
                case 2://Right button
                    return _currentState.RightButton == ButtonState.Pressed;
                case 3://Middle button
                    return _currentState.MiddleButton == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }


        /// <summary>
        /// Returns true if the given input is in the up position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonUp(int input)
        {
            //Return the up state of the given mouse input
            switch (input)
            {
                case 1://Left button
                    return _currentState.LeftButton == ButtonState.Released;
                case 2://Right button
                    return _currentState.RightButton == ButtonState.Released;
                case 3://Middle button
                    return _currentState.MiddleButton == ButtonState.Released;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }


        /// <summary>
        /// Returns true if the given mouse input has been released from the down position.
        /// </summary>
        /// <param name="input">The mouse input to check for.</param>
        /// <returns></returns>
        public bool IsButtonPressed(int input)
        {
            //Check the given input to see if it has been released from the down position
            switch (input)
            {
                case 1://Left button
                    return _currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed;
                case 2://Right button
                    return _currentState.RightButton == ButtonState.Released && _previousState.RightButton == ButtonState.Pressed;
                case 3://Middle button
                    return _currentState.MiddleButton == ButtonState.Released && _previousState.MiddleButton == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
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

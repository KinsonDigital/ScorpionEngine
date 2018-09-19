using System;
using Microsoft.Xna.Framework.Input;
using ScorpionEngine;
using ScorpionEngine.Input;

namespace MonoEngineDriver
{
    public class MouseInput : IMouse
    {
        #region Events
        /// <summary>
        /// Occurs when the left mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnLeftButtonDown;

        /// <summary>
        /// Occurs when the left mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnLeftButtonReleased;

        /// <summary>
        /// Occurs when the right mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnRightButtonDown;

        /// <summary>
        /// Occurs when the right mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnRightButtonReleased;

        /// <summary>
        /// Occurs when the middle mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnMiddleButtonDown;

        /// <summary>
        /// Occurs when the middle mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnMiddleButtonReleased;
        #endregion


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

        /// <summary>
        /// Gets the position of the mouse.
        /// </summary>
        public Vector Position => new Vector(X, Y);
        #endregion


        #region Protected Methods
        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        public void UpdateCurrentState()
        {
            //Get the state of the mouse and save it as the current state
            _currentState = Mouse.GetState();

            #region Left Mouse Button
            //If the left mouse button has been pressed down
            if (_currentState.LeftButton == ButtonState.Pressed)
            {
                //Invoke the OnLeftButtonDown event and send the current state of the mouse
                OnLeftButtonDown?.Invoke(this, new MouseEventArgs(Tools.ToMouseInputState(_currentState)));
            }

            //If the left mouse button has been released
            if (_currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed)
            {
                OnLeftButtonReleased?.Invoke(this, new MouseEventArgs(Tools.ToMouseInputState(_previousState)));
            }
            #endregion

            #region Right Mouse Button
            //If the right mouse button has been pressed down
            if (_currentState.RightButton == ButtonState.Pressed)
            {
                //Invoke the OnRightButtonDown event and send the current state of the mouse
                OnRightButtonDown?.Invoke(this, new MouseEventArgs(Tools.ToMouseInputState(_currentState)));
            }

            //If the right mouse button has been released
            if (_currentState.RightButton == ButtonState.Released && _previousState.RightButton == ButtonState.Pressed)
            {
                OnRightButtonReleased?.Invoke(this, new MouseEventArgs(Tools.ToMouseInputState(_previousState)));
            }
            #endregion

            #region Middle Mouse Button
            //If the middle mouse button has been pressed down
            if (_currentState.MiddleButton == ButtonState.Pressed)
            {
                //Invoke the OnMiddleButtonDown event and send the current state of the mouse
                OnMiddleButtonDown?.Invoke(this, new MouseEventArgs(Tools.ToMouseInputState(_currentState)));
            }

            //If the middle mouse button has been released
            if (_currentState.MiddleButton == ButtonState.Released && _previousState.MiddleButton == ButtonState.Pressed)
            {
                OnMiddleButtonReleased?.Invoke(this, new MouseEventArgs(Tools.ToMouseInputState(_previousState)));
            }
            #endregion

            //            else if ()
            //            {
            //                //If any keys have been released, invoke the OnKeyUp event

            //                //Invoke the OnKeyUp event and send the list of keys that are pressed down
            //                OnRightButtonDown?.Invoke(this, new MouseEventArgs());
            //            }
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
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="position">The position to set the mouse to over the game window.</param>
        public void SetPosition(Vector position)
        {
            SetPosition((int)position.X, (int)position.Y);
        }


        /// <summary>
        /// Returns true if the given input is in the down position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
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
        /// Returns true if the given input is in the up position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonUp(InputButton input)
        {
            //Return the up state of the given mouse input
            switch (input)
            {
                case InputButton.LeftButton:
                    return _currentState.LeftButton == ButtonState.Released;
                case InputButton.RightButton:
                    return _currentState.RightButton == ButtonState.Released;
                case InputButton.MiddleButton:
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
        #endregion
    }
}

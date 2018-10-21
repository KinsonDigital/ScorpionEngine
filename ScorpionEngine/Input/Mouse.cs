using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Tracks the state of the mouse.
    /// </summary>
    public class Mouse : IMouseEvents
    {
        private IMouse _internalMouse;


        #region Events
        /// <summary>
        /// Occurs when the left mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnLeftButtonDown;

        /// <summary>
        /// Occurs when the left mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnLeftButtonReleased;

        /// <summary>
        /// Occurs when the right mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnRightButtonDown;

        /// <summary>
        /// Occurs when the right mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnRightButtonReleased;

        /// <summary>
        /// Occurs when the middle mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnMiddleButtonDown;

        /// <summary>
        /// Occurs when the middle mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnMiddleButtonReleased;
        #endregion


        /// <summary>
        /// Creates a new instance of <see cref="Mouse"/>.
        /// </summary>
        public Mouse()
        {
            _internalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
        }


        /// <summary>
        /// Returns true if the given input is in the down position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonDown(InputButton input)
        {
            return _internalMouse.IsButtonDown((int)input);
        }


        /// <summary>
        /// Returns true if the given input is in the up position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonUp(InputButton input)
        {
            return _internalMouse.IsButtonUp((int)input);
        }


        /// <summary>
        /// Returns true if the given mouse input has been released from the down position.
        /// </summary>
        /// <param name="input">The mouse input to check for.</param>
        /// <returns></returns>
        public bool IsButtonPressed(InputButton input)
        {
            return _internalMouse.IsButtonPressed((int)input);
        }


        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="x">The horizontal position to set the mouse to over the game window.</param>
        /// <param name="y">The vertical position to set the mouse to over the game window.</param>
        public void SetPosition(int x, int y)
        {
            _internalMouse.SetPosition(x, y);
        }


        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="position">The position to set the mouse to over the game window.</param>
        public void SetPosition(Vector position)
        {
            _internalMouse.SetPosition((int)position.X, (int)position.Y);
        }


        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        public void UpdateCurrentState()
        {
            _internalMouse.UpdateCurrentState();

            #region Left Mouse Button
            //If the left mouse button has been pressed down
            if (_internalMouse.IsButtonDown((int)InputButton.LeftButton))
            {
                //Invoke the OnLeftButtonDown event and send the current state of the mouse
                OnLeftButtonDown?.Invoke(this, new EventArgs());
            }

            //If the left mouse button has been released
            if (_internalMouse.IsButtonUp((int)InputButton.LeftButton) && _internalMouse.IsButtonDown((int)InputButton.LeftButton))
            {
                OnLeftButtonReleased?.Invoke(this, new EventArgs());
            }
            #endregion

            #region Right Mouse Button
            //If the right mouse button has been pressed down
            if (_internalMouse.IsButtonDown((int)InputButton.RightButton))
            {
                //Invoke the OnRightButtonDown event and send the current state of the mouse
                OnRightButtonDown?.Invoke(this, new EventArgs());
            }

            //If the right mouse button has been released
            if (_internalMouse.IsButtonUp((int)InputButton.RightButton) && _internalMouse.IsButtonDown((int)InputButton.RightButton))
            {
                OnRightButtonReleased?.Invoke(this, new EventArgs());
            }
            #endregion

            #region Middle Mouse Button
            //If the middle mouse button has been pressed down
            if (_internalMouse.IsButtonDown((int)InputButton.MiddleButton))
            {
                //Invoke the OnMiddleButtonDown event and send the current state of the mouse
                OnMiddleButtonDown?.Invoke(this, new EventArgs());
            }

            //If the middle mouse button has been released
            if (_internalMouse.IsButtonUp((int)InputButton.MiddleButton) && _internalMouse.IsButtonDown((int)InputButton.MiddleButton))
            {
                OnMiddleButtonReleased?.Invoke(this, new EventArgs());
            }
            #endregion
        }


        /// <summary>
        /// Update the previous state of the mouse.
        /// </summary>
        public void UpdatePreviousState()
        {
            _internalMouse.UpdatePreviousState();
        }
    }
}

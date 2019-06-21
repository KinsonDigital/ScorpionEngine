using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using System;

namespace KDScorpionEngine.Input
{
    /// <summary>
    /// Tracks the state of the mouse.
    /// </summary>
    public class Mouse
    {
        #region Events
        /// <summary>
        /// Occurs when the left mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnLeftButtonDown;

        /// <summary>
        /// Occurs when the left mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnLeftButtonPressed;

        /// <summary>
        /// Occurs when the right mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnRightButtonDown;

        /// <summary>
        /// Occurs when the right mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnRightButtonPressed;

        /// <summary>
        /// Occurs when the middle mouse button has been pressed to the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnMiddleButtonDown;

        /// <summary>
        /// Occurs when the middle mouse button has been released from the down position.
        /// </summary>
        public event EventHandler<MouseEventArgs> OnMiddleButtonPressed;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Mouse"/>.
        /// ONLY USED FOR TESTING!!
        /// </summary>
        /// <param name="mouse">The mouse plugin to use.</param>
        internal Mouse(IMouse mouse) => InternalMouse = mouse;


        /// <summary>
        /// Creates a new instance of <see cref="Mouse"/>.
        /// </summary>
        public Mouse() => InternalMouse = EnginePluginSystem.Plugins.EnginePlugins.LoadPlugin<IMouse>();
        #endregion


        #region Props
        internal IMouse InternalMouse { get; }


        /// <summary>
        /// Gets sets the X position of the mouse in the game window.
        /// </summary>
        public int X
        {
            get => InternalMouse.X;
            set => InternalMouse.X = value;
        }


        /// <summary>
        /// Gets sets the Y position of the mouse in the game window.
        /// </summary>
        public int Y
        {
            get => InternalMouse.Y;
            set => InternalMouse.Y = value;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns true if the given input is in the down position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonDown(InputButton input)
        {
            return InternalMouse.IsButtonDown((int)input);
        }


        /// <summary>
        /// Returns true if the given input is in the up position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        public bool IsButtonUp(InputButton input)
        {
            return InternalMouse.IsButtonUp((int)input);
        }


        /// <summary>
        /// Returns true if the given mouse input has been released from the down position.
        /// </summary>
        /// <param name="input">The mouse input to check for.</param>
        /// <returns></returns>
        public bool IsButtonPressed(InputButton input)
        {
            return InternalMouse.IsButtonPressed((int)input);
        }


        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="x">The horizontal position to set the mouse to over the game window.</param>
        /// <param name="y">The vertical position to set the mouse to over the game window.</param>
        public void SetPosition(int x, int y)
        {
            InternalMouse.SetPosition(x, y);
        }


        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="position">The position to set the mouse to over the game window.</param>
        public void SetPosition(Vector position)
        {
            InternalMouse.SetPosition((int)position.X, (int)position.Y);
        }


        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        public void UpdateCurrentState()
        {
            InternalMouse.UpdateCurrentState();

            #region Left Mouse Button
            //If the left mouse button has been pressed down
            if (InternalMouse.IsButtonDown((int)InputButton.LeftButton))
            {
                //Invoke the OnLeftButtonDown event and send the current state of the mouse
                OnLeftButtonDown?.Invoke(this, new MouseEventArgs(new MouseInputState()
                {
                    LeftButtonDown = true,
                    X = InternalMouse.X,
                    Y = InternalMouse.Y
                }));
            }

            //If the left mouse button has been pressed
            if (InternalMouse.IsButtonPressed((int)InputButton.LeftButton))
            {
                OnLeftButtonPressed?.Invoke(this, new MouseEventArgs(new MouseInputState()
                {
                    X = InternalMouse.X,
                    Y = InternalMouse.Y
                }));
            }
            #endregion

            #region Right Mouse Button
            //If the right mouse button has been pressed down
            if (InternalMouse.IsButtonDown((int)InputButton.RightButton))
            {
                //Invoke the OnRightButtonDown event and send the current state of the mouse
                OnRightButtonDown?.Invoke(this, new MouseEventArgs(new MouseInputState()
                {
                    RightButtonDown = true,
                    X = InternalMouse.X,
                    Y = InternalMouse.Y
                }));
            }

            //If the right mouse button has been pressed
            if (InternalMouse.IsButtonPressed((int)InputButton.RightButton))
            {
                OnRightButtonPressed?.Invoke(this, new MouseEventArgs(new MouseInputState()
                {
                    X = InternalMouse.X,
                    Y = InternalMouse.Y
                }));
            }
            #endregion

            #region Middle Mouse Button
            //If the middle mouse button has been pressed down
            if (InternalMouse.IsButtonDown((int)InputButton.MiddleButton))
            {
                //Invoke the OnMiddleButtonDown event and send the current state of the mouse
                OnMiddleButtonDown?.Invoke(this, new MouseEventArgs(new MouseInputState()
                {
                    MiddleButtonDown = true,
                    X = InternalMouse.X,
                    Y = InternalMouse.Y
                }));
            }

            //If the middle mouse button has been pressed
            if (InternalMouse.IsButtonPressed((int)InputButton.MiddleButton))
            {
                OnMiddleButtonPressed?.Invoke(this, new MouseEventArgs(new MouseInputState()
                {
                    X = InternalMouse.X,
                    Y = InternalMouse.Y
                }));
            }
            #endregion
        }


        /// <summary>
        /// Update the previous state of the mouse.
        /// </summary>
        public void UpdatePreviousState()
        {
            InternalMouse.UpdatePreviousState();
        }
        #endregion
    }
}

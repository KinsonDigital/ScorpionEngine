namespace KDScorpionCore.Plugins
{
    public interface IMouse : IPlugin
    {
        #region Props
        /// <summary>
        /// Gets sets the X position of the mouse in the game window.
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Gets sets the Y position of the mouse in the game window.
        /// </summary>
        int Y { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Sets the position of the mouse.
        /// </summary>
        /// <param name="x">The horizontal position to set the mouse to over the game window.</param>
        /// <param name="y">The vertical position to set the mouse to over the game window.</param>
        void SetPosition(int x, int y);


        /// <summary>
        /// Returns true if the given input is in the down position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        bool IsButtonDown(int input);


        /// <summary>
        /// Returns true if the given input is in the up position.
        /// </summary>
        /// <param name="input">The input to check for.</param>
        /// <returns></returns>
        bool IsButtonUp(int input);


        /// <summary>
        /// Returns true if the given mouse input has been released from the down position.
        /// </summary>
        /// <param name="input">The mouse input to check for.</param>
        /// <returns></returns>
        bool IsButtonPressed(int input);


        /// <summary>
        /// Update the current state of the mouse.
        /// </summary>
        void UpdateCurrentState();


        /// <summary>
        /// Update the previous state of the mouse.
        /// </summary>
        void UpdatePreviousState();
        #endregion
    }
}

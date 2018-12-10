namespace KDScorpionCore.Plugins
{
    public interface IMouse : IPlugin
    {
        #region Props
        int X { get; set; }

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


        //TODO: Add method docs
        void UpdateCurrentState();


        void UpdatePreviousState();
        #endregion
    }
}

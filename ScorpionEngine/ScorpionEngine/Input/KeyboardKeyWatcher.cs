using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    public class KeyboardKeyWatcher
    {
        #region Events
        /// <summary>
        /// Occurs when the key has reached its hit count.
        /// </summary>
        public event EventHandler OnKeyHitCountReached;
        #endregion

        #region Fields
        private readonly KeyboardInput _keyboardInput;
        private int _currentHits;//The current amount of times that the key has been hit
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of KeyboardKeyWatcher.
        /// </summary>
        public KeyboardKeyWatcher(int hitCountMax, InputKeys key)
        {
            HitCountMax = hitCountMax;
            Key = key;
            _keyboardInput = new KeyboardInput();
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets or sets the maximum amount that the hit counter will count up to before the OnKeyHitCountReached event will be fired.
        /// </summary>
        public int HitCountMax { get; set; }

        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public InputKeys Key { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Update the key watcher.
        /// </summary>
        public void Update()
        {
            //Update the keyboard input which keeps the state of the keyboard up to date
            _keyboardInput.UpdateCurrentState();

            if (_keyboardInput.IsKeyPressed(Key))
            {
                //If the max is reached, invoke the OnKeyHitCountReached event and reset it back to 0
                if (_currentHits == HitCountMax - 1)
                {
                    OnKeyHitCountReached?.Invoke(this, new EventArgs());

                    //Reset the current hits back to 0
                    _currentHits = 0;
                }
                else
                {
                    _currentHits += 1;//Increment the current hit count
                }
            }

            _keyboardInput.UpdatePreviousState();
        }
        #endregion
    }
}
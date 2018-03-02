using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Watches a keyboard key and invokes an event when the key is pressed a set amount of times.
    /// </summary>
    public class KeyHitCountWatcher : KeyboardWatcher
    {
        #region Fields
        /// <summary>
        /// Occurs when the key has reached its hit count.
        /// </summary>
        public event EventHandler OnKeyHitCountReached;
        public int _currentHitCount;//The current amount of times the key has been hit.
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of KeyboardKeyWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        public KeyHitCountWatcher(int hitCountMax, InputKeys key)
        {
            HitCountMax = hitCountMax;
            Key = key;
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets or sets the maximum amount that the hit counter will count up to before the OnButtonHitCountReached event will be fired.
        /// </summary>
        public int HitCountMax { get; set; }

        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public InputKeys Key { get; set; }

        /// <summary>
        /// Gets the current amount of times the key has been hit.
        /// </summary>
        public int CurrentHitCount => _currentHitCount;

        /// <summary>
        /// Gets the current percentage of hits the key has been hit.
        /// </summary>
        public int CurrentHitCountPercentage => (int)((CurrentHitCount / (float)HitCountMax) * 100f);
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Update the watcher state.
        /// </summary>
        /// <param name="engineTime">The engine time info.</param>
        public override void Update(EngineTime engineTime)
        {
            //Update the keyboard input which keeps the state of the keyboard up to date
            UpdateBegin();

            if (IsKeyPressed(Key))
            {
                //If the max is reached, invoke the OnButtonHitCountReached event and reset it back to 0
                if (CurrentHitCount == HitCountMax)
                {
                    OnKeyHitCountReached?.Invoke(this, new EventArgs());

                    //Reset the current hits back to 0
                    _currentHitCount = 1;
                }
                else
                {
                    _currentHitCount += 1;//Increment the current hit count
                }
            }

            UpdateEnd();
        }
        #endregion
    }
}

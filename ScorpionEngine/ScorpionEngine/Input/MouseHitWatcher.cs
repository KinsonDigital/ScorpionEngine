using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Watches a mouse button and invokes an event when the button is pressed a set amount of times.
    /// </summary>
    public class MouseHitWatcher : MouseWatcher
    {
        #region Fields
        /// <summary>
        /// Occurs when the button has reached its hit count.
        /// </summary>
        public event EventHandler OnButtonHitCountReached;
        public int _currentHitCount;//The current amount of times the key has been hit.
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of MouseHitCountWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the button will be hit before invoking an event.</param>
        /// <param name="button">The button to watch.</param>
        public MouseHitWatcher(int hitCountMax, MouseInputType button)
        {
            HitCountMax = hitCountMax;
            Button = button;
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets or sets the maximum amount that the hit counter will count up to before the OnButtonHitCountReached event will be fired.
        /// </summary>
        public int HitCountMax { get; set; }

        /// <summary>
        /// Gets or sets the button to watch.
        /// </summary>
        public MouseInputType Button { get; set; }

        /// <summary>
        /// Gets the current amount of times the button has been hit.
        /// </summary>
        public int CurrentHitCount => _currentHitCount;

        /// <summary>
        /// Gets the current percentage of hits the button has been hit.
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
            //Start the mouse update process
            UpdateBegin();

            if (IsButtonPressed(Button))
            {
                //If the max is reached, invoke the OnButtonHitCountReached event and reset it back to 0
                if (_currentHitCount == HitCountMax)
                {
                    OnButtonHitCountReached?.Invoke(this, new EventArgs());

                    //Reset the current hits back to 0
                    _currentHitCount = 1;
                }
                else
                {
                    _currentHitCount += 1;//Increment the current hit count
                }
            }

            UpdateEnd();//Finish the mouse update
        }
        #endregion
    }
}

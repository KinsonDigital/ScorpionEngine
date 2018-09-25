using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    /// <summary>
    /// Represents the different events of the <see cref="Plugins.IMouse"/>.
    /// </summary>
    public interface IMouseEvents
    {
        /// <summary>
        /// Occurs when the left mouse button has been pressed to the down position.
        /// </summary>
        event EventHandler<EventArgs> OnLeftButtonDown;

        /// <summary>
        /// Occurs when the left mouse button has been released from the down position.
        /// </summary>
        event EventHandler<EventArgs> OnLeftButtonReleased;

        /// <summary>
        /// Occurs when the right mouse button has been pressed to the down position.
        /// </summary>
        event EventHandler<EventArgs> OnRightButtonDown;

        /// <summary>
        /// Occurs when the right mouse button has been released from the down position.
        /// </summary>
        event EventHandler<EventArgs> OnRightButtonReleased;

        /// <summary>
        /// Occurs when the middle mouse button has been pressed to the down position.
        /// </summary>
        event EventHandler<EventArgs> OnMiddleButtonDown;

        /// <summary>
        /// Occurs when the middle mouse button has been released from the down position.
        /// </summary>
        event EventHandler<EventArgs> OnMiddleButtonReleased;
    }
}

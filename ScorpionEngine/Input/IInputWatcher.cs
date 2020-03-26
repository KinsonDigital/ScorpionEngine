﻿using System;

namespace KDScorpionEngine.Input
{
    /// <summary>
    /// Watches input for various events and behaviors such is how many times the input has been pressed,
    /// how long it has been held down or how long it has been released.  Various events will be triggered when
    /// these behaviours occur.
    /// </summary>
    public interface IInputWatcher
    {
        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the <see cref="IInputWatcher"/> is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets current amount of times that the set input has been hit.
        /// </summary>
        int CurrentHitCount { get; }

        /// <summary>
        /// Gets the current hit percentage that the input has been hit out of the total number of maximum tims to be hit.
        /// </summary>
        int CurrentHitCountPercentage { get; }

        /// <summary>
        /// Gets or sets the reset mode that the watcher will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the amount of time the input is in the down position.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        ResetType DownElapsedResetMode { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of times that the set input should be hit before 
        /// invoking the <see cref="OnInputHitCountReached"/> event is reached.
        /// </summary>
        int HitCountMax { get; set; }

        /// <summary>
        /// Gets or sets the reset mode that the watcher's hit count will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the hit count.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        ResetType HitCountResetMode { get; set; }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the input has been held in the down position.
        /// </summary>
        int InputDownElapsedMS { get; }

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the input has been held in the down position.
        /// </summary>
        float InputDownElapsedSeconds { get; }

        /// <summary>
        /// The amount of time in milliseconds that the input should be held down before invoking the <see cref="OnInputDownTimeOut"/> event.
        /// </summary>
        int InputDownTimeOut { get; set; }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the input has been released and is in the up position.
        /// </summary>
        int InputReleasedElapsedMS { get; }

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the input has been released and is in the up position.
        /// </summary>
        float InputReleasedElapsedSeconds { get; }

        /// <summary>
        /// The amount of time in milliseconds that the input should be released to the up position
        /// after being released from the down position before invoking the <see cref="OnInputReleasedTimeOut"/> event.
        /// </summary>
        int InputReleasedTimeout { get; set; }

        /// <summary>
        /// Gets or sets the reset mode that the watcher's input released functionality will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the input being released.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        ResetType ReleasedElapsedResetMode { get; set; }
        #endregion


        #region Event Handlers
        /// <summary>
        /// Invoked when the combo input setup has been pressed.
        /// </summary>
        event EventHandler OnInputComboPressed;

        /// <summary>
        /// Invoked when the set input has been held in the down position for a set amount of time.
        /// </summary>
        event EventHandler OnInputDownTimeOut;

        /// <summary>
        /// Invoked when the set input has been hit a set amount of times.
        /// </summary>
        event EventHandler OnInputHitCountReached;

        /// <summary>
        /// Invoked when the set input has been released from the down position for a set amount of time.
        /// </summary>
        event EventHandler OnInputReleasedTimeOut;
        #endregion
    }
}
using System;

namespace ScorpionEngine.Input
{
    public interface IInputWatcher
    {
        #region Props
        int CurrentHitCount { get; }

        int CurrentHitCountPercentage { get; }

        ResetType DownElapsedResetMode { get; set; }

        bool Enabled { get; set; }

        int HitCountMax { get; set; }

        ResetType HitCountResetMode { get; set; }

        int InputDownElapsedMS { get; }

        float InputDownElapsedSeconds { get; }

        int InputDownTimeOut { get; set; }

        int InputReleasedElapsedMS { get; }

        float InputReleasedElapsedSeconds { get; }

        int InputReleasedTimeout { get; set; }

        ResetType ReleasedElapsedResetMode { get; set; }
        #endregion


        #region Event Handlers
        event EventHandler OnInputComboPressed;

        event EventHandler OnInputDownTimeOut;

        event EventHandler OnInputHitCountReached;

        event EventHandler OnInputReleasedTimeOut;
        #endregion
    }
}
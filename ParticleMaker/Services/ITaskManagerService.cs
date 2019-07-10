using System;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides ability to manage an asynchrounous task.
    /// </summary>
    public interface ITaskManagerService : IDisposable
    {
        #region Props
        /// <summary>
        /// Gets a value indicating if the task is currently running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Returns a value indicating if the task is in the process of being cancelled.
        /// </summary>
        bool CancelPending { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Starts the task and executes the given <paramref name="taskAction"/>
        /// </summary>
        /// <param name="taskAction">The work to be performed by the task.</param>
        void Start(Action taskAction);


        /// <summary>
        /// Cancels the task.
        /// </summary>
        void Cancel();
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides the ability to manage an asynchrounous task.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TaskManagerService : ITaskManagerService
    {
        #region Privte Fields
        private Task _loopTask;
        private CancellationTokenSource _tokenSrc;
        #endregion


        #region Props
        /// <summary>
        /// Gets a value indicating if the task is currently running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_loopTask != null && _loopTask.Status == TaskStatus.Running)
                    return true;

                if (_tokenSrc != null && !_tokenSrc.IsCancellationRequested)
                    return true;


                return false;
            }
        }

        /// <summary>
        /// Returns a value indicating if the task is in the process of being cancelled.
        /// </summary>
        public bool CancelPending => _tokenSrc != null & _tokenSrc.IsCancellationRequested;
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the task and executes the given <paramref name="taskAction"/>
        /// </summary>
        /// <param name="taskAction">The work to be performed by the task.</param>
        public void Start(Action taskAction)
        {
            _tokenSrc = new CancellationTokenSource();
            _loopTask = new Task(taskAction, _tokenSrc.Token);
            _loopTask.Start();
        }


        /// <summary>
        /// Cancels the task.
        /// </summary>
        public void Cancel() => _tokenSrc?.Cancel();


        /// <summary>
        /// Disposes of the <see cref="TaskManagerService"/>.
        /// </summary>
        public void Dispose()
        {
            _tokenSrc?.Cancel();
            _tokenSrc?.Dispose();
            _tokenSrc = null;

            _loopTask?.Dispose();
            _loopTask = null;
        }
        #endregion
    }
}

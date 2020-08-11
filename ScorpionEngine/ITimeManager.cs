// <copyright file="ITimeManager.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using Raptor;

    /// <summary>
    /// Provides functionality for managing time and state of a system.
    /// </summary>
    public interface ITimeManager : IUpdatable
    {
        #region Props
        /// <summary>
        /// Gets or sets the amount of elapsed time in milliseconds that the current frame has ran.
        /// </summary>
        int ElapsedFrameTime { get; set; }

        /// <summary>
        /// Gets or sets the total frames that have elapsed.
        /// </summary>
        uint ElapsedFramesForStack { get; set; }

        /// <summary>
        /// Gets or sets the amount of frames to run per frame stack.
        /// </summary>
        uint FramesPerStack { get; set; }

        /// <summary> 
        /// Gets or sets the time in milliseconds that each frame should take. 
        /// NOTE: This is restricted to the incoming game engine frame time. If this time is less then the  
        /// engine updating this manager, then this will now work.
        /// </summary>
        int FrameTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the system is paused.
        /// </summary>
        bool Paused { get; set; }

        /// <summary>
        /// Gets or sets the total number of frames ran.
        /// </summary>
        int TotalFramesRan { get; set; }

        /// <summary>
        /// Gets or sets the mode that the system runs in.
        /// </summary>
        RunMode Mode { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Plays the scene.
        /// </summary>
        void Play();


        /// <summary>
        /// Pauses the scene.
        /// </summary>
        void Pause();

        /// <summary>
        /// Runs a complete stack of frames set by the <see cref="ITimeManager"/>.
        /// This will only work if the <see cref="Mode"/> property is set to <see cref="RunMode.FrameStack"/>.
        /// </summary>
        void RunFrameStack();


        /// <summary>
        /// Runs a set amount of frames given by the <paramref name="frames"/> param and pauses after.
        /// This will only work if the <see cref="Mode"/> property is set to <see cref="RunMode.FrameStack"/>.
        /// </summary>
        /// <param name="frames">The number of frames to run.</param>
        void RunFrames(uint frames);
        #endregion
    }
}

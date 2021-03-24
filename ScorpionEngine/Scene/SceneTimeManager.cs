// <copyright file="SceneTimeManager.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using System;
    using KDScorpionEngine.Events;

    /// <summary>
    /// Manages how a scene is updated and give the ability for the scene to be paused.
    /// </summary>
    public class SceneTimeManager : ITimeManager
    {
        private Action? frameStackCallback;

        /// <inheritdoc/>
        public event EventHandler<FrameStackFinishedEventArgs>? FrameStackFinished;

        /// <inheritdoc/>
        public int ElapsedFrameTime { get; set; }

        /// <inheritdoc/>
        public uint ElapsedFramesForStack { get; set; } = 1;

        /// <inheritdoc/>
        public uint FramesPerStack { get; set; } = 50;

        /// <inheritdoc/>
        public int FrameTime { get; set; } = 16;

        /// <inheritdoc/>
        public bool Paused { get; private set; }

        /// <inheritdoc/>
        public int TotalFramesRan { get; set; }

        /// <inheritdoc/>
        public SceneRunMode Mode { get; set; } = SceneRunMode.Continuous;

        /// <summary>
        /// Updates the <see cref="SceneTimeManager"/>.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            // Continous mode
            if (Mode == SceneRunMode.Continuous)
            {
                TotalFramesRan += 1;
            }
            else if (Mode == SceneRunMode.FrameStack) // Step mode
            {
                // If the current stack of frames are being ran
                if (!Paused)
                {
                    ElapsedFrameTime += gameTime.CurrentFrameElapsed;

                    // If the elapsed time frame has passed
                    if (ElapsedFrameTime >= FrameTime)
                    {
                        ElapsedFrameTime = 0; // Reset the elapsed time back to 0
                        ElapsedFramesForStack += 1; // Update the total number of frames that has elapsed for this frame stack
                        TotalFramesRan += 1; // Update the total number of frames that have passed

                        // If the required number of frames for this frame stack have elapsed
                        if (ElapsedFramesForStack > FramesPerStack)
                        {
                            ElapsedFramesForStack = 0;
                            Paused = true;

                            // Invoke the frame stack callback
                            this.frameStackCallback?.Invoke();

                            // Invoke the frame stack finished event
                            FrameStackFinished?.Invoke(this, new FrameStackFinishedEventArgs((int)FramesPerStack));
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Play() => Paused = false;

        /// <inheritdoc/>
        public void Pause() => Paused = true;

        /// <inheritdoc/>
        public void RunFrameStack() => Paused = Mode == SceneRunMode.FrameStack ? false : Paused;

        /// <inheritdoc/>
        public void RunFrames(uint frames)
        {
            // If the mode is not in frame stack mode or if the callback is not null.
            // If the callback is not null, that means a previous call is still running.
            if (Mode != SceneRunMode.FrameStack || this.frameStackCallback != null)
            {
                return;
            }

            Paused = false;

            // Save the currently set frames ran per stack
            var oldFramesPerStack = FramesPerStack;

            // Temporarily set the frames per stack to the requested frames
            FramesPerStack = frames;

            // Set the frame stack callback
            // This will be invoked once the frames are finished running
            this.frameStackCallback = () =>
            {
                // Set the frames per stack back to what it was before the RunFrames() method was called
                FramesPerStack = oldFramesPerStack;

                // Destroy the callback so it is not called anymore.
                this.frameStackCallback = null;
            };
        }
    }
}

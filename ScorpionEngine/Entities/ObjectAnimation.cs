// <copyright file="ObjectAnimation.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Raptor;

    /// <summary>
    /// Control animation at a particular frames per second.
    /// </summary>
    // TODO: Get this working and setup unit tests.  Wait for when you create a game for testing
    [ExcludeFromCodeCoverage]
    public class ObjectAnimation
    {
        private int fps = 10; // The frames per second that the animation will run at
        private int elapsedTime; // The amount of time elapsed since the last animation frame was changed
        private int currentFrame; // The current frame of the animation
        private readonly List<Rect> frames = new List<Rect>(); // The bounds of all the frames of the animation

        /// <summary>
        /// Creates a new instance of <see cref="ObjectAnimation"/>.
        /// </summary>
        /// <param name="frameBounds">The bounds data for the animation.</param>
        public ObjectAnimation(List<Rect> frameBounds) => this.frames = frameBounds;

        /// <summary>
        /// Gets or sets the frames per second of the animation.
        /// </summary>
        public int FPS
        {
            get => this.fps;
            set
            {
                // Make sure that the incoming value stays at a minimum of 1
                value = value <= 0 ? 1 : value;

                this.fps = value;
            }
        }

        /// <summary>
        /// Gets or sets the direction of the animation.
        /// </summary>
        public AnimationDirection Direction { get; set; } = AnimationDirection.Forward;

        /// <summary>
        /// Gets the state of the animation.
        /// </summary>
        public AnimationState State { get; private set; } = AnimationState.Stopped;

        /// <summary>
        /// Gets the frame bounds of the current frame.
        /// </summary>
        public Rect CurrentFrameBounds => this.frames[this.currentFrame];

        /// <summary>
        /// Gets or sets a value indicating if the animation loops.
        /// </summary>
        public bool Looping { get; set; } = true;

        /// <summary>
        /// Plays the animation.
        /// </summary>
        public void Play() => State = AnimationState.Running;

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Pause() => State = AnimationState.Paused;

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void Stop()
        {
            State = AnimationState.Stopped;
            this.currentFrame = 0; // Set the current frame back to the first frame
        }

        /// <summary>
        /// Updates the animation.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public void Update(IEngineTiming engineTime)
        {
            switch (State)
            {
                case AnimationState.Running:
                    // Update the elapsed time since the last time the engine loop was called
                    this.elapsedTime += engineTime.ElapsedEngineTime.Milliseconds;

                    // If the amount of time has passed for the next frame of the animation to be shown
                    if (this.elapsedTime >= 1000 / this.fps)
                    {
                        this.elapsedTime = 0;

                        // If the animation is running foward or backward
                        switch (Direction)
                        {
                            case AnimationDirection.Forward:
                                // If the current frame is NOT the last frame
                                if (this.currentFrame < this.frames.Count - 1)
                                {
                                    this.currentFrame += 1;
                                }
                                else if (this.currentFrame >= this.frames.Count - 1 && Looping) // At the last frame, move back to the first frame
                                {
                                    this.currentFrame = 0;
                                }
                                break;
                            case AnimationDirection.Backward:
                                // If the current frame is NOT the last frame
                                if (this.currentFrame > 0)
                                {
                                    this.currentFrame -= 1;
                                }
                                else if (this.currentFrame <= 0 && Looping) // At the last frame, move back to the first frame
                                {
                                    this.currentFrame = this.frames.Count - 1;
                                }
                                break;
                        }
                    }
                    break;
                case AnimationState.Stopped:
                    break;
            }
        }
    }
}
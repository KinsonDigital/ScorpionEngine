// <copyright file="Animator.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System;
    using System.Drawing;

    public class Animator : IAnimator
    {
        private int frameIndex;
        private float elapsedFrameTime;
        private float fps = 30f;
        private float fpsMilliseconds = 32f;

        public AnimateState CurrentState { get; set; }

        public AnimateDirection Direction { get; set; }

        public bool IsLooping { get; set; } = true;

        public float FPS
        {
            get => this.fps;
            set
            {
                this.fps = value;
                this.fpsMilliseconds = 1000f / value;
            }
        }

        public Rectangle[] Frames { get; set; }

        public Rectangle CurrentFrameBounds
        {
            get
            {
                var frame = Frames[this.frameIndex];

                return frame;
            }
        }

        public void NextFrame()
        {
            if (CurrentState == AnimateState.Paused)
            {
                return;
            }

            switch (Direction)
            {
                case AnimateDirection.Forward:
                    this.frameIndex += 1;
                    break;
                case AnimateDirection.Reverse:
                    this.frameIndex -= 1;
                    break;
                default:
                    throw new Exception($"Unknown '{nameof(AnimateDirection)}' value of '{Direction}'");
            }

            ProcessIndexRange();
        }

        public void PreviousFrame()
        {
            if (CurrentState == AnimateState.Paused)
            {
                return;
            }

            switch (Direction)
            {
                case AnimateDirection.Forward:
                    this.frameIndex -= 1;
                    break;
                case AnimateDirection.Reverse:
                    this.frameIndex += 1;
                    break;
                default:
                    throw new Exception($"Unknown '{nameof(AnimateDirection)}' value of '{Direction}'");
            }

            ProcessIndexRange();
        }

        // Ignores direction entirlely. Add this note to the remarks of the code docs
        public void SetFrame(int frameIndex)
        {
            ProcessIndexRange();
            this.frameIndex = frameIndex;
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentState == AnimateState.Paused)
            {
                return;
            }

            this.elapsedFrameTime += gameTime.CurrentFrameElapsed;

            if (this.elapsedFrameTime >= this.fpsMilliseconds)
            {
                this.elapsedFrameTime = 0;

                switch (Direction)
                {
                    case AnimateDirection.Forward:
                        this.frameIndex += 1;
                        break;
                    case AnimateDirection.Reverse:
                        this.frameIndex -= 1;
                        break;
                    default:
                        throw new Exception($"Unknown '{nameof(AnimateDirection)}' value of '{Direction}'");
                }

                ProcessIndexRange();
            }
        }

        private void ProcessIndexRange()
        {
            switch (Direction)
            {
                case AnimateDirection.Forward:
                    if (this.frameIndex > Frames.Length - 1)
                    {
                        this.frameIndex = IsLooping ? 0 : this.frameIndex;
                    }

                    break;
                case AnimateDirection.Reverse:
                    if (this.frameIndex < 0)
                    {
                        this.frameIndex = IsLooping ? Frames.Length - 1 : this.frameIndex;
                    }

                    break;
                default:
                    throw new Exception($"Unknown '{nameof(AnimateDirection)}' value of '{Direction}'");
            }
        }
    }
}

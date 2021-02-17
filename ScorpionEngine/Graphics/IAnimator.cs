// <copyright file="IAnimator.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System.Drawing;

    public interface IAnimator : IUpdatableObject
    {
        AnimateState CurrentState { get; set; }

        AnimateDirection Direction { get; set; }

        bool IsLooping { get; set; }

        float FPS { get; set; }

        Rectangle[] Frames { get; set; }

        Rectangle CurrentFrameBounds { get; }

        void NextFrame();

        void PreviousFrame();

        void SetFrame(int frameIndex);
    }
}

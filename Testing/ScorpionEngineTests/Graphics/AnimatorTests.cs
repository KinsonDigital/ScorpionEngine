// <copyright file="AnimatorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Graphics
{
    using System;
    using System.Drawing;
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="Animator"/> class.
    /// </summary>
    public class AnimatorTests
    {
        #region Prop Tests
        [Fact]
        public void CurrentState_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var animator = new Animator();

            // Act
            animator.CurrentState = AnimateState.Paused;

            // Assert
            Assert.Equal(AnimateState.Paused, animator.CurrentState);
        }

        [Fact]
        public void Direction_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var animator = new Animator();

            // Act
            animator.Direction = AnimateDirection.Reverse;

            // Assert
            Assert.Equal(AnimateDirection.Reverse, animator.Direction);
        }

        [Fact]
        public void IsLopping_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var animator = new Animator();

            // Act
            animator.IsLooping = false;

            // Assert
            Assert.False(animator.IsLooping);
        }

        [Fact]
        public void FPS_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var animator = new Animator();

            // Act
            animator.FPS = 35;

            // Assert
            Assert.Equal(35, animator.FPS);
        }

        [Fact]
        public void Frames_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var expected = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            };
            var animator = new Animator();

            // Act
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            }.ToReadOnlyCollection();

            // Assert
            Assert.Equal(expected, animator.Frames);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void NextFrame_WhenPaused_DoesNotMoveToTheNextFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.CurrentState = AnimateState.Paused;

            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            }.ToReadOnlyCollection();

            // Act
            animator.NextFrame();

            // Assert
            Assert.Equal(new Rectangle(11, 22, 33, 44), animator.CurrentFrameBounds);
        }

        [Fact]
        public void NextFrame_WithNoFrames_ThrowsException()
        {
            // Arrange
            var animator = new Animator();

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<Exception>(() =>
            {
                animator.NextFrame();
            }, "No frames exist in the animation.");
        }

        [Fact]
        public void NextFrame_WithLoopingTurnedOn_MovesToNextFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.IsLooping = true;
            animator.Frames = new Rectangle[]
                {
                    new Rectangle(11, 22, 33, 44),
                    new Rectangle(111, 222, 333, 444),
                    new Rectangle(1111, 2222, 3333, 4444),
                }.ToReadOnlyCollection();

            // Act
            animator.NextFrame();
            animator.NextFrame();
            animator.NextFrame();

            // Assert
            Assert.Equal(new Rectangle(11, 22, 33, 44), animator.CurrentFrameBounds);
        }

        [Fact]
        public void NextFrame_WithLoopingTurnedOff_DoesNotMoveToFirstFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.IsLooping = false;
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            }.ToReadOnlyCollection();

            // Act
            animator.NextFrame();
            animator.NextFrame();
            animator.NextFrame();
            animator.NextFrame();

            // Assert
            Assert.Equal(new Rectangle(111, 222, 333, 444), animator.CurrentFrameBounds);
        }

        [Fact]
        public void PreviousFrame_WhenPaused_DoesNotMoveToThePreviousFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.CurrentState = AnimateState.Paused;

            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            }.ToReadOnlyCollection();

            // Act
            animator.PreviousFrame();

            // Assert
            Assert.Equal(new Rectangle(11, 22, 33, 44), animator.CurrentFrameBounds);
        }

        [Fact]
        public void PreviousFrame_WithNoFrames_ThrowsException()
        {
            // Arrange
            var animator = new Animator();

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<Exception>(() =>
            {
                animator.PreviousFrame();
            }, "No frames exist in the animation.");
        }

        [Fact]
        public void PreviousFrame_WithLoopingTurnedOn_MovesToTheLastFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.IsLooping = true;
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            }.ToReadOnlyCollection();

            // Act
            animator.PreviousFrame();

            // Assert
            Assert.Equal(new Rectangle(111, 222, 333, 444), animator.CurrentFrameBounds);
        }

        [Fact]
        public void PreviousFrame_WithLoopingTurnedOff_DoesNotMoveToLastFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.IsLooping = false;
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
            }.ToReadOnlyCollection();

            // Act
            animator.PreviousFrame();

            // Assert
            Assert.Equal(new Rectangle(11, 22, 33, 44), animator.CurrentFrameBounds);
        }

        [Fact]
        public void SetFrame_WhenInvoked_SetsFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
                new Rectangle(1111, 2222, 3333, 4444),
            }.ToReadOnlyCollection();

            // Act
            animator.SetFrame(1);

            // Assert
            Assert.Equal(new Rectangle(111, 222, 333, 444), animator.CurrentFrameBounds);
        }

        [Fact]
        public void Update_WhenPaused_MovesToCorrectFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.CurrentState = AnimateState.Paused;
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
                new Rectangle(1111, 2222, 3333, 4444),
            }.ToReadOnlyCollection();

            // Act
            animator.Update(new GameTime());

            // Assert
            Assert.Equal(new Rectangle(11, 22, 33, 44), animator.CurrentFrameBounds);
        }

        [Fact]
        public void Update_WithNullFrames_ThrowsException()
        {
            // Arrange
            var animator = new Animator();
            animator.CurrentState = AnimateState.Playing;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<Exception>(() =>
            {
                animator.Update(new GameTime());
            }, "No frames exist in the animation.");
        }

        [Fact]
        public void Update_WhenMovingForwardAndLooping_MovesToCorrectFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.FPS = 30;
            animator.Direction = AnimateDirection.Forward;
            animator.CurrentState = AnimateState.Playing;

            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
                new Rectangle(1111, 2222, 3333, 4444),
            }.ToReadOnlyCollection();
            var gameTime = new GameTime();
            gameTime.AddTime(34);

            // Act
            animator.Update(gameTime);
            animator.Update(gameTime);
            animator.Update(gameTime);

            // Assert
            Assert.Equal(new Rectangle(11, 22, 33, 44), animator.CurrentFrameBounds);
        }

        [Fact]
        public void Update_WhenMovingInReverseAndLooping_MovesToCorrectFrame()
        {
            // Arrange
            var animator = new Animator();
            animator.FPS = 30;
            animator.Direction = AnimateDirection.Reverse;
            animator.CurrentState = AnimateState.Playing;

            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
                new Rectangle(111, 222, 333, 444),
                new Rectangle(1111, 2222, 3333, 4444),
            }.ToReadOnlyCollection();
            var gameTime = new GameTime();
            gameTime.AddTime(34);

            // Act
            animator.Update(gameTime);

            // Assert
            Assert.Equal(new Rectangle(1111, 2222, 3333, 4444), animator.CurrentFrameBounds);
        }

        [Fact]
        public void Update_WithUnknownDirection_ThrowsException()
        {
            // Arrange
            var animator = new Animator();
            animator.Direction = (AnimateDirection)1234;
            animator.CurrentState = AnimateState.Playing;
            animator.Frames = new Rectangle[]
            {
                new Rectangle(11, 22, 33, 44),
            }.ToReadOnlyCollection();

            var gameTime = new GameTime();
            gameTime.AddTime(34);

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<Exception>(() =>
            {
                animator.Update(gameTime);
            }, "Unknown 'AnimateDirection' value of '1234'.");
        }
        #endregion
    }
}

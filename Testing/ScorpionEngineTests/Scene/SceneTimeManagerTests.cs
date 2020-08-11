namespace KDScorpionEngineTests.Scene
{
    using System;
    using KDScorpionEngine.Scene;
    using Raptor;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="SceneTimeManager"/> class.
    /// </summary>
    public class SceneTimeManagerTests
    {
        #region Prop Tests
        [Fact]
        public void ElapsedFrameTime_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager();
            var expected = 123;

            // Act
            manager.ElapsedFrameTime = 123;
            var actual = manager.ElapsedFrameTime;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ElapsedFramesForStack_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager();

            // Act
            manager.ElapsedFramesForStack = 123;
            var actual = manager.ElapsedFramesForStack;

            // Assert
            Assert.Equal(123u, actual);
        }

        [Fact]
        public void FrameTime_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager();
            var expected = 123;

            // Act
            manager.FrameTime = 123;
            var actual = manager.FrameTime;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Paused_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager();
            var expected = true;

            // Act
            manager.Pause();
            var actual = manager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TotalFramesRan_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager();
            var expected = 123;

            // Act
            manager.TotalFramesRan = 123;
            var actual = manager.TotalFramesRan;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mode_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager();
            var expected = RunMode.FrameStack;

            // Act
            manager.Mode = RunMode.FrameStack;
            var actual = manager.Mode;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokingInContinuousMode_IncrementsTotalFramsRan()
        {
            // Arrange
            var manager = new SceneTimeManager();
            var expected = 2;

            // Act
            manager.Update(new EngineTime());
            manager.Update(new EngineTime());
            var actual = manager.TotalFramesRan;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_UpdatesElapsedFrameTimeProp()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 32
            };
            var expected = 16;

            // Act
            manager.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = manager.ElapsedFrameTime;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_ResetsElapsedFrameTimeProp()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16
            };
            var expected = 0;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 9) };

            // Act
            manager.Update(engineTime);
            manager.Update(engineTime);
            var actual = manager.ElapsedFrameTime;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_IncrementsElapsedFramesPerStackProp()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 9) };

            // Act
            manager.Update(engineTime);
            manager.Update(engineTime);
            var actual = manager.ElapsedFramesForStack;

            // Assert
            Assert.Equal(2u, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_IncrementsTotalFramesRanProp()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16
            };
            var expected = 1;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 9) };

            // Act
            manager.Update(engineTime);
            manager.Update(engineTime);
            var actual = manager.TotalFramesRan;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_ResetElapsedFramesForStackPropToZero()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16,
                FramesPerStack = 10
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 9) };

            // Act
            for (var i = 0; i < 20; i++)
            {
                manager.Update(engineTime);
            }

            var actual = manager.ElapsedFramesForStack;

            // Assert
            Assert.Equal(0u, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_SetPausedToTrue()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16,
                FramesPerStack = 10
            };
            var expected = true;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 9) };

            // Act
            for (var i = 0; i < 20; i++)
            {
                manager.Update(engineTime);
            }

            var actual = manager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingInFrameStackMode_InvokesFrameStackFinishedEvent()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16,
                FramesPerStack = 10
            };
            var actualEventInvoked = false;
            var expectedEventInvoked = true;

            manager.FrameStackFinished += (sender, e) => actualEventInvoked = true;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 9) };

            // Act
            for (var i = 0; i < 20; i++)
            {
                manager.Update(engineTime);
            }

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }

        [Fact]
        public void RunFrameStack_WhenInvokingInFrameStackMode_PauseSetToFalse()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack
            };
            manager.Pause();
            var expected = false;

            // Act
            manager.RunFrameStack();
            var actual = manager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RunFrames_WhenInvokingInContinuousMode_PauseStaysSetToTrue()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.Continuous,
            };
            manager.Pause();
            var expected = true;

            // Act
            manager.RunFrames(2);
            var actual = manager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RunFrames_WhenInvokingWithInternalCallbackNotNull_SetsFramesPerStackToCorrectValue()
        {
            // Arrange
            var manager = new SceneTimeManager()
            {
                Mode = RunMode.FrameStack,
                FrameTime = 16,
                FramesPerStack = 10
            };
            manager.Pause();
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            // Act
            manager.RunFrames(1);
            manager.Update(engineTime);
            var actual = manager.FramesPerStack;

            // Assert
            Assert.Equal(10u, actual);
        }
        #endregion
    }
}

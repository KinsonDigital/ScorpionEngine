using NUnit.Framework;
using KDScorpionCore;
using ScorpionEngine.Utils;
using System;


namespace ScorpionEngine.Tests.Utils
{
    public class StopWatchTests
    {
        #region Method Tests
        [Test]
        public void Start_WhenStarting_SetsRunningToTrue()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = true;

            //Act
            stopWatch.Start();
            var actual = stopWatch.Running;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Stop_WhenStoping_SetsRunningToFalse()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = false;

            //Act
            stopWatch.Stop();
            var actual = stopWatch.Running;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Reset_WhenReseting_SetElapsedToZeroAndRunningToFalse()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 500) };
            var expectedRunning = false;
            var expectedElapsedMS = 0;

            //Act
            stopWatch.Start();
            stopWatch.Update(engineTime);
            stopWatch.Reset();
            var actualRunning = stopWatch.Running;
            var actualElapsedMS = stopWatch.ElapsedMS;

            //Assert
            Assert.AreEqual(expectedElapsedMS, actualElapsedMS);
            Assert.AreEqual(expectedRunning, actualRunning);
        }


        [Test]
        public void Update_WhenInvokingWithResetTypeAsAuto_ResetStopWatch()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var engineTime = new EngineTime()
            {
                ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 1001)
            };
            var expectedElapsedMS = 0;
            var expectedRunning = false;

            //Act
            stopWatch.Start();
            stopWatch.Update(engineTime);
            var actualElapsedMS = stopWatch.ElapsedMS;
            var actualRunning = stopWatch.Running;

            //Assert
            Assert.AreEqual(expectedElapsedMS, actualElapsedMS);
            Assert.AreEqual(expectedRunning, actualRunning);
        }


        [Test]
        public void Update_WhenInvoking_OnTimeElapsedEventInvoked()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var stopWatch = new StopWatch(2500);
            stopWatch.OnTimeElapsed += (sender, e) => actual = true;
            var engineTime = new EngineTime()
            {
                ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 2500),
            };


            //Act
            stopWatch.Start();
            stopWatch.Update(engineTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void ResetMode_WhenSettingValue_ValueIsSetCorrectly()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = ResetType.Manual;

            //Act
            stopWatch.ResetMode = ResetType.Manual;
            var actual = stopWatch.ResetMode;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TimeOut_WhenUsingNegativeValue_SetsValueToZero()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = 1;

            //Act
            stopWatch.TimeOut = -100;
            var actual = stopWatch.TimeOut;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TimeOut_WhenUsingPositiveValue_SetsValueToIncomingValue()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = 100;

            //Act
            stopWatch.TimeOut = 100;
            var actual = stopWatch.TimeOut;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ElapsedSeconds_WhenSettingValue_ConvertsSecondsToMilliseconds()
        {
            //Arrange
            var stopWatch = new StopWatch(2500);
            var engineTime = new EngineTime()
            {
                ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 750)
            };
            var expected = 0.75f;//Seconds

            //Act
            stopWatch.Start();
            stopWatch.Update(engineTime);
            var actual = stopWatch.ElapsedSeconds;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

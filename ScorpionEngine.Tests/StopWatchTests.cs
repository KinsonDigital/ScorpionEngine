using ScorpionEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class StopWatchTests
    {
        [Fact]
        public void Start_WhenStarting_SetsRunningToTrue()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = true;

            //Act
            stopWatch.Start();
            var actual = stopWatch.Running;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Stop_WhenStoping_SetsRunningToFalse()
        {
            //Arrange
            var stopWatch = new StopWatch(1000);
            var expected = false;

            //Act
            stopWatch.Stop();
            var actual = stopWatch.Running;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expectedElapsedMS, actualElapsedMS);
            Assert.Equal(expectedRunning, actualRunning);
        }
    }
}

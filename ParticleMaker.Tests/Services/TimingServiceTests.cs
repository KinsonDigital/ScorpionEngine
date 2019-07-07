using ParticleMaker.Services;
using System.Collections.Generic;
using Xunit;

namespace ParticleMaker.Tests.Services
{
    public class TimingServiceTests
    {
        #region Prop Tests
        [Fact]
        public void FrameTimings_WhenInstanceIsCreated_IsNotNull()
        {
            //Arrange
            var timing = new TimingService();

            //Assert
            Assert.NotNull(timing.FrameTimings);
        }


        [Fact]
        public void FrameTimings_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var timing = new TimingService();

            //Act
            timing.FrameTimings = new Queue<double>(new double[]
            {
                11.11,
                22.22
            });

            //Assert
            Assert.NotNull(timing.FrameTimings);
        }


        [Fact]
        public void Elapsed_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var timing = new TimingService()
            {
                WaitTime = 250
            };

            //Act
            timing.Start();
            timing.Wait();
            timing.Stop();

            //Assert
            Assert.InRange(timing.Elapsed.TotalMilliseconds, 249, 251);
        }


        [Fact]
        public void WaitTime_WhenGettingValue_ReturnsCorrectDefaultValue()
        {
            //Arrange
            var timing = new TimingService();

            //Assert
            Assert.Equal(1000, timing.WaitTime);
        }


        [Fact]
        public void IsPaused_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var timing = new TimingService();

            timing.Start();

            //Arrange
            Assert.False(timing.IsPaused);
        }


        [Fact]
        public void TotalFrameTimes_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var timing = new TimingService();

            //Act
            timing.TotalFrameTimes = 123;

            //Arrange
            Assert.Equal(123, timing.TotalFrameTimes);
        }


        [Fact]
        public void TotalFrameTimes_WhenGettingValue_ReturnsCorrectDefaultValue()
        {
            //Arrange
            var timing = new TimingService();

            //Act & Arrange
            Assert.Equal(100, timing.TotalFrameTimes);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Record_WhenInvoking_CorrectlySetsFPSValue()
        {
            //Arrange
            var timing = new TimingService();
            timing.WaitTime = 250;

            //Act
            timing.Start();
            timing.Wait();
            timing.Record();
            timing.Stop();

            //Arrange
            Assert.InRange(timing.FPS, 3.95f, 4.05f);
        }


        [Fact]
        public void Record_WhenInvokingWithMaxedFrameTimings_DoesNotGoPastMaxTimings()
        {
            //Arrange
            var timing = new TimingService();
            timing.TotalFrameTimes = 2;

            //Act
            timing.Record();
            timing.Record();
            timing.Record();

            //Arrange
            Assert.Equal(2, timing.FrameTimings.Count);
        }
        #endregion
    }
}

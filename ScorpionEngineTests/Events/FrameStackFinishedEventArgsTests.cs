using Xunit;
using KDScorpionEngine.Events;

namespace KDScorpionEngineTests.Events
{
    /// <summary>
    /// Unit tests to test the <see cref="FrameStackFinishedEventArgs"/> class.
    /// </summary>
    public class FrameStackFinishedEventArgsTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetTotalFramesRanProp()
        {
            //Arrange
            var eventArgs = new FrameStackFinishedEventArgs(123);
            var expected = 123;

            //Act
            var actual = eventArgs.TotalFramesRan;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void TotalFramesRan_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new FrameStackFinishedEventArgs(0);
            var expected = 123;

            //Act
            eventArgs.TotalFramesRan = 123;
            var actual = eventArgs.TotalFramesRan;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

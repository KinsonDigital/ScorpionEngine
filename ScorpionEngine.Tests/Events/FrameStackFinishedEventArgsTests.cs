using NUnit.Framework;
using ScorpionEngine.Events;

namespace ScorpionEngine.Tests.Events
{
    [TestFixture]
    public class FrameStackFinishedEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetTotalFramesRanProp()
        {
            //Arrange
            var eventArgs = new FrameStackFinishedEventArgs(123);
            var expected = 123;

            //Act
            var actual = eventArgs.TotalFramesRan;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void TotalFramesRan_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new FrameStackFinishedEventArgs(0);
            var expected = 123;

            //Act
            eventArgs.TotalFramesRan = 123;
            var actual = eventArgs.TotalFramesRan;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

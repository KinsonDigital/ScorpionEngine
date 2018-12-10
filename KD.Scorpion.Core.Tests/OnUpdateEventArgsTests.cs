using Moq;
using NUnit.Framework;

namespace KDScorpionCore.Tests
{
    [TestFixture]
    public class OnUpdateEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsRendererProp()
        {
            //Arrange
            var mockRenderer = new Mock<IEngineTiming>();
            var expected = mockRenderer.Object;
            var eventArgs = new OnUpdateEventArgs(mockRenderer.Object);

            //Act
            var actual = eventArgs.EngineTime;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Renderer_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockRendererForCtor = new Mock<IEngineTiming>();
            var eventArgs = new OnUpdateEventArgs(mockRendererForCtor.Object);
            var mockRendererForProp = new Mock<IEngineTiming>();
            var expected = mockRendererForProp.Object;

            //Act
            eventArgs.EngineTime = mockRendererForProp.Object;
            var actual = eventArgs.EngineTime;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Input;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class MouseTests
    {
        [Fact]
        public void Ctor_WhenInvoking_InvokesInternalLoadPluginMethod()
        {
            //Arrange
            var mockPluginLibrary = new Mock<IPluginLibrary>();
            PluginSystem.LoadEnginePluginLibrary(mockPluginLibrary.Object);

            //Act
            var mouse = new Mouse();

            //Assert
            mockPluginLibrary.Verify(m => m.LoadPlugin<IMouse>(), Times.Once());
        }


        [Fact]
        public void Ctor_WhenInvoking_ProperlySetsInternalMouse()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();

            //Act
            var mouse = new Mouse(mockMouse.Object);

            //Assert
            Assert.NotNull(mouse.InternalMouse);
        }


        [Fact]
        public void IsButtonDown_WhenInvoking_InvokesInternalIsButtonDownMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.IsButtonDown(It.IsAny<InputButton>());

            //Assert
            mockMouse.Verify(m => m.IsButtonDown(It.IsAny<int>()), Times.Once());
        }
    }
}

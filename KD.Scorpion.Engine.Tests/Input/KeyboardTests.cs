using Moq;
using Xunit;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Input;

namespace KDScorpionEngineTests.Input
{
    public class KeyboardTests
    {
        #region Method Tests
        [Fact]
        public void GetCurrentPressedKeys_WhenInvoking_ReturnsCorrectPressedKeys()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var pressedKeys = new KeyCodes[] { KeyCodes.Left, KeyCodes.Z };

            mockKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(pressedKeys);

            var keyboard = new Keyboard(mockKeyboard.Object);
            var expected = new KeyCodes[] { KeyCodes.Left, KeyCodes.Z };

            //Act
            var actual = keyboard.GetCurrentPressedKeys();

            //Assert
            Assert.Equal(expected, actual);
            mockKeyboard.Verify(m => m.GetCurrentPressedKeys(), Times.Once());
        }


        [Fact]
        public void GetPreviousPressedKeys_WhenInvoking_ReturnsCorrectPressedKeys()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var pressedKeys = new KeyCodes[] { KeyCodes.Down, KeyCodes.U };
            mockKeyboard.Setup(m => m.GetPreviousPressedKeys()).Returns(pressedKeys);

            var keyboard = new Keyboard(mockKeyboard.Object);
            var expected = new KeyCodes[] { KeyCodes.Down, KeyCodes.U };

            //Act
            var actual = keyboard.GetPreviousPressedKeys();

            //Assert
            Assert.Equal(expected, actual);
            mockKeyboard.Verify(m => m.GetPreviousPressedKeys(), Times.Once());
        }


        [Fact]
        public void IsKeyDown_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyDown(It.IsAny<KeyCodes>())).Returns(true);
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.IsKeyDown(KeyCodes.A);

            //Assert
            mockKeyboard.Verify(m => m.IsKeyDown(It.IsAny<KeyCodes>()), Times.Once());
        }


        [Fact]
        public void IsKeyUp_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyUp(It.IsAny<KeyCodes>())).Returns(true);
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.IsKeyUp(KeyCodes.A);

            //Assert
            mockKeyboard.Verify(m => m.IsKeyUp(It.IsAny<KeyCodes>()), Times.Once());
        }


        [Fact]
        public void IsKeyPressed_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyPressed(It.IsAny<KeyCodes>())).Returns(true);
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.IsKeyPressed(KeyCodes.A);

            //Assert
            mockKeyboard.Verify(m => m.IsKeyPressed(It.IsAny<KeyCodes>()), Times.Once());
        }


        [Fact]
        public void UpdateCurrentState_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            keyboard.UpdateCurrentState();

            //Assert
            mockKeyboard.Verify(m => m.UpdateCurrentState(), Times.Once());
        }


        [Fact]
        public void UpdatePreviousState_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            keyboard.UpdatePreviousState();

            //Assert
            mockKeyboard.Verify(m => m.UpdatePreviousState(), Times.Once());
        }
        #endregion
    }
}

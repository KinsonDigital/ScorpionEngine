using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Input;
using System;
using System.Linq;
using Xunit;

namespace ScorpionEngine.Tests.Input
{
    public class KeyboardTests  : IDisposable
    {
        #region Method Tests
        [Fact]
        public void Ctor_WhenInvoking_InvokesPluginLibraryLoadPluginMethod()
        {
            //Arrange
            var mockPluginLibrary = new Mock<IPluginLibrary>();
            PluginSystem.LoadEnginePluginLibrary(mockPluginLibrary.Object);

            //Act
            var keyboard = new Keyboard();

            //Assert
            mockPluginLibrary.Verify(m => m.LoadPlugin<IKeyboard>(), Times.Once());
        }


        [Fact]
        public void GetCurrentPressedKeys_WhenInvoking_ReturnsCorrectPressedKeys()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var pressedKeys = (from k in new InputKeys[] { InputKeys.Left, InputKeys.Z }
                               select (int)k).ToArray();
            mockKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(pressedKeys);

            var keyboard = new Keyboard(mockKeyboard.Object);
            var expected = new InputKeys[] { InputKeys.Left, InputKeys.Z };

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
            var pressedKeys = (from k in new InputKeys[] { InputKeys.Down, InputKeys.U }
                               select (int)k).ToArray();
            mockKeyboard.Setup(m => m.GetPreviousPressedKeys()).Returns(pressedKeys);

            var keyboard = new Keyboard(mockKeyboard.Object);
            var expected = new InputKeys[] { InputKeys.Down, InputKeys.U };

            //Act
            var actual = keyboard.GetPreviousPressedKeys();

            //Assert
            Assert.Equal(expected, actual);
            mockKeyboard.Verify(m => m.GetPreviousPressedKeys(), Times.Once());
        }


        [Fact]
        public void AreAnyKeysPressed_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.AreAnyKeysPressed();

            //Assert
            mockKeyboard.Verify(m => m.AreAnyKeysPressed(), Times.Once());
        }


        [Fact]
        public void IsKeyDown_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyDown(It.IsAny<int>())).Returns(true);
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.IsKeyDown(InputKeys.A);

            //Assert
            mockKeyboard.Verify(m => m.IsKeyDown(It.IsAny<int>()), Times.Once());
        }


        [Fact]
        public void IsKeyUp_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyUp(It.IsAny<int>())).Returns(true);
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.IsKeyUp(InputKeys.A);

            //Assert
            mockKeyboard.Verify(m => m.IsKeyUp(It.IsAny<int>()), Times.Once());
        }


        [Fact]
        public void IsKeyPressed_WhenInvoking_InternalMethodInvoked()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyPressed(It.IsAny<int>())).Returns(true);
            var keyboard = new Keyboard(mockKeyboard.Object);

            //Act
            var actual = keyboard.IsKeyPressed(InputKeys.A);

            //Assert
            mockKeyboard.Verify(m => m.IsKeyPressed(It.IsAny<int>()), Times.Once());
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


        public void Dispose()
        {
            PluginSystem.ClearPlugins();
        }
        #endregion
    }
}

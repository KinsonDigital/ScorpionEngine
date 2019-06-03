using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Input;
using Moq;
using NUnit.Framework;
using PluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDScorpionEngineTests.Input
{
    public class MouseTests
    {
        #region Prop Tests
        [Test]
        public void X_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.SetupGet(m => m.X).Returns(1234);

            var mouse = new Mouse(mockMouse.Object);

            var expected = 1234;

            //Act
            var actual = mouse.X;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Y_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.SetupGet(m => m.Y).Returns(5678);

            var mouse = new Mouse(mockMouse.Object);

            var expected = 5678;

            //Act
            var actual = mouse.Y;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Ctor_WhenInvoked_InvokesCreateMouseMethod()
        {
            //Arrange
            var mockPluginFactory = new Mock<IPluginFactory>();

            Plugins.LoadPluginFactory(mockPluginFactory.Object);

            //Act
            var mouse = new Mouse();

            //Assert
            mockPluginFactory.Verify(m => m.CreateMouse(), Times.Once());
        }


        [Test]
        public void Ctor_WhenInvoked_ProperlySetsInternalMouse()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();

            //Act
            var mouse = new Mouse(mockMouse.Object);

            //Assert
            Assert.NotNull(mouse.InternalMouse);
        }


        [Test]
        public void IsButtonDown_WhenInvoked_InvokesInternalIsButtonDownMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.IsButtonDown(It.IsAny<InputButton>());

            //Assert
            mockMouse.Verify(m => m.IsButtonDown(It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void IsButtonUp_WhenInvoked_InvokesInternalIsButtonUpMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.IsButtonUp(It.IsAny<InputButton>());

            //Assert
            mockMouse.Verify(m => m.IsButtonUp(It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void IsButtonPressed_WhenInvoked_InvokesInternalIsButtonPressedMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.IsButtonPressed(It.IsAny<InputButton>());

            //Assert
            mockMouse.Verify(m => m.IsButtonPressed(It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void SetPosition_WhenInvokedUsingIntParams_InvokesInternalSetPositionMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.SetPosition(It.IsAny<int>(), It.IsAny<int>());

            //Assert
            mockMouse.Verify(m => m.SetPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void SetPosition_WhenInvokedUsingVectorParam_InvokesInternalSetPositionMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.SetPosition(It.IsAny<Vector>());

            //Assert
            mockMouse.Verify(m => m.SetPosition(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void UpdateCurrentState_WhenInvoked_InternalUpdateInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.UpdateCurrentState();

            //Assert
            mockMouse.Verify(m => m.UpdateCurrentState(), Times.Once());
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithLeftButtonDown_OnLeftButtonDownEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);
            var expected = true;
            var actual = false;
            mouse.OnLeftButtonDown += (sender, e) => actual = true;

            //Act
            mouse.UpdateCurrentState();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithLeftButtonDownAndNullEvent_NoEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => mouse.UpdateCurrentState());
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithLeftButtonPressed_OnLeftButtonPressedEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);
            var expected = true;
            var actual = false;
            mouse.OnLeftButtonPressed += (sender, e) => actual = true;

            //Act
            mouse.UpdateCurrentState();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithLeftButtonPressedAndNullEvent_NoEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => mouse.UpdateCurrentState());
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithRightButtonDown_OnRightButtonDownEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.RightButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);
            var expected = true;
            var actual = false;
            mouse.OnRightButtonDown += (sender, e) => actual = true;

            //Act
            mouse.UpdateCurrentState();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithRightButtonDownAndNullEvent_NoEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.RightButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => mouse.UpdateCurrentState());
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithRightButtonPressed_OnRightButtonPressedEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.RightButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);
            var expected = true;
            var actual = false;
            mouse.OnRightButtonPressed += (sender, e) => actual = true;

            //Act
            mouse.UpdateCurrentState();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithRightButtonPressedAndNullEvent_NoEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.RightButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => mouse.UpdateCurrentState());
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithMiddleButtonDown_OnMiddleButtonDownEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.MiddleButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);
            var expected = true;
            var actual = false;
            mouse.OnMiddleButtonDown += (sender, e) => actual = true;

            //Act
            mouse.UpdateCurrentState();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithMiddleButtonDownAndNullEvent_NoEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.MiddleButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => mouse.UpdateCurrentState());
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithMiddleButtonPressed_OnMiddleButtonPressedEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.MiddleButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);
            var expected = true;
            var actual = false;
            mouse.OnMiddleButtonPressed += (sender, e) => actual = true;

            //Act
            mouse.UpdateCurrentState();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateCurrentState_WhenInvokedWithMiddleButtonPressedAndNullEvent_NoEventInvoked()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.MiddleButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => mouse.UpdateCurrentState());
        }


        [Test]
        public void UpdatePreviousState_WhenInvoked_InvokesInternalUpdateMethod()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.MiddleButton)).Returns(true);

            var mouse = new Mouse(mockMouse.Object);

            //Act
            mouse.UpdatePreviousState();

            //Assert
            mockMouse.Verify(m => m.UpdatePreviousState(), Times.Once());
        }
        #endregion

        [TearDown]
        public void TearDown() => Plugins.UnloadPluginFactory();
    }
}

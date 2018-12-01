using NUnit.Framework;
using ScorpionCore;
using ScorpionEngine.Input;


namespace ScorpionEngine.Tests.Input
{
    public class MouseInputStateTests
    {
        [Test]
        public void Position_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var expected = new Vector(11, 22);

            //Act
            state.Position = new Vector(11, 22);
            var actual = state.Position;

            //Arrange
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ScrollWheelValue_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var expected = 1234;

            //Act
            state.ScrollWheelValue = 1234;
            var actual = state.ScrollWheelValue;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void X_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var expected = 1234;

            //Act
            state.X = 1234;
            var actual = state.X;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Y_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var eYpected = 1234;

            //Act
            state.Y = 1234;
            var actual = state.Y;

            //Assert
            Assert.AreEqual(eYpected, actual);
        }


        [Test]
        public void LeftButtonDown_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var expected = true;

            //Act
            state.LeftButtonDown = true;
            var actual = state.LeftButtonDown;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RightButtonDown_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var expected = true;

            //Act
            state.RightButtonDown = true;
            var actual = state.RightButtonDown;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MiddleButtonDown_WhenSettingAndGettingValue_GetsAndSetsCorrectValue()
        {
            //Arrange
            var state = new MouseInputState();
            var expected = true;

            //Act
            state.MiddleButtonDown = true;
            var actual = state.MiddleButtonDown;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

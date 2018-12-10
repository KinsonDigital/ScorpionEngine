using NUnit.Framework;
using KDScorpionCore.Input;
using KDScorpionCore;

namespace KDScorpionCoreTests.Input
{
    public class MouseEventArgsTests
    {
        [Test]
        public void Ctor_WhenInvoking_SetsMouseInputStatePropValue()
        {
            //Arrange
            var mouseEventArgs = new MouseEventArgs(new MouseInputState()
            {
                LeftButtonDown = true,
                RightButtonDown = true,
                MiddleButtonDown = true,
                Position = new Vector(11, 22),
                ScrollWheelValue = 4
            });

            var expected = new MouseInputState()
            {
                LeftButtonDown = true,
                RightButtonDown = true,
                MiddleButtonDown = true,
                Position = new Vector(11, 22),
                ScrollWheelValue = 4
            };

            //Act
            var actual = mouseEventArgs.State;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

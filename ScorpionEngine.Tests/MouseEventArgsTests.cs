﻿using ScorpionEngine.Input;
using ScorpionEngine.Physics;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class MouseEventArgsTests
    {
        [Fact]
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
            Assert.Equal(expected, actual);
        }
    }
}

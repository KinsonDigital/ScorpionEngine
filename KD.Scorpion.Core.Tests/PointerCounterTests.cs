using KDScorpionCore;
using System;
using Xunit;

namespace KDScorpionCoreTests
{
    public class PointerCounterTests
    {
        [Fact]
        public void PackPointerAndUnpackPointer_WhenInvoked_CorrectSetsPointer()
        {
            //Arrange
            var pointerContainer = new PointerContainer();
            var pointer = new IntPtr(1234);

            //Act
            pointerContainer.PackPointer(pointer);

            //Assert
            Assert.Equal(pointer, pointerContainer.UnpackPointer());
        }
    }
}

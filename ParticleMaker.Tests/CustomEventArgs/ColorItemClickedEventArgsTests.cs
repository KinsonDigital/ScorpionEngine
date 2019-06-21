using Xunit;
using System.Windows.Media;

namespace ParticleMaker.CustomEventArgs
{
    public class ColorItemClickedEventArgsTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlySetsProperties()
        {
            //Arrange
            var eventArgs = new ColorItemClickedEventArgs(1234, Color.FromArgb(255, 11, 22, 33));
            var expectedId = 1234;
            var expectedColor = Color.FromArgb(255, 11, 22, 33);

            //Act
            var actualId = eventArgs.Id;
            var actualColor = eventArgs.Color;

            //Assert
            Assert.Equal(expectedId, actualId);
            Assert.Equal(expectedColor, actualColor);
        }
        #endregion
    }
}

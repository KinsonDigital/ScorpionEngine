using NUnit.Framework;
using ParticleMaker.UserControls;
using System.Windows.Media;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class ColorItemClickedEventArgsTests
    {
        #region Constructor Tests
        [Test]
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
            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedColor, actualColor);
        }
        #endregion
    }
}

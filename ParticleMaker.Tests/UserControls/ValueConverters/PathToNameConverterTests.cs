using NUnit.Framework;
using ParticleMaker.UserControls.ValueConverters;
using System.Globalization;

namespace ParticleMaker.Tests.UserControls.ValueConverters
{
    [TestFixture]
    public class PathToNameConverterTests
    {
        #region Method Tests
        [Test]
        public void Convert_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var converter = new PathToNameConverter();
            var expected = "test-file";

            //Act
            var actual = converter.Convert(@"C:\temp\test-file.dat", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Convert_WhenInvokedWithNoExtension_ReturnsEmpty()
        {
            //Arrange
            var converter = new PathToNameConverter();

            //Act
            var actual = converter.Convert(@"C:\temp\test-file", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.AreEqual(string.Empty, actual);
        }


        [Test]
        public void Convert_WhenInvokedWithNullData_ReturnsEmpty()
        {
            //Arrange
            var converter = new PathToNameConverter();

            //Act
            var actual = converter.Convert(null, typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.AreEqual(string.Empty, actual);
        }


        [Test]
        public void ConvertBack_WhenInvoked_ReturnSameData()
        {
            //Arrange
            var converter = new PathToNameConverter();
            var expected = @"C:\temp\test-file.dat";

            //Act
            var actual = converter.ConvertBack(@"C:\temp\test-file.dat", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

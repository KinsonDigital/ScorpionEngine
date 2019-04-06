using NUnit.Framework;
using ParticleMaker.ValueConverters;
using System.Globalization;

namespace ParticleMaker.Tests.UserControls.ValueConverters
{
    [TestFixture]
    public class StringToBoolConverterTests
    {
        #region Method Tests
        [Test]
        public void Convert_WhenInvokedWithNonEmptyData_ReturnsTrue()
        {
            //Arrange
            var converter = new StringToBoolConverter();

            //Act
            var actual = (bool)converter.Convert("sample-string", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.IsTrue(actual);
        }


        [Test]
        public void Convert_WhenInvokedWithNullData_ReturnsFalse()
        {
            //Arrange
            var converter = new StringToBoolConverter();

            //Act
            var actual = (bool)converter.Convert(null, typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.IsFalse(actual);
        }


        [Test]
        public void ConvertBack_WhenInvokedWithData_ReturnsFalse()
        {
            //Arrange
            var converter = new StringToBoolConverter();

            //Act
            var actual = (bool)converter.ConvertBack("sample-data", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.IsFalse(actual);
        }
        #endregion
    }
}

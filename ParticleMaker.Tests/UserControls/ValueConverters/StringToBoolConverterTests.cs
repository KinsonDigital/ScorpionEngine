using Xunit;
using ParticleMaker.ValueConverters;
using System.Globalization;

namespace ParticleMaker.Tests.UserControls.ValueConverters
{
    /// <summary>
    /// Unit tests to test the <see cref="StringToBoolConverter"/> class.
    /// </summary>
    public class StringToBoolConverterTests
    {
        #region Method Tests
        [Fact]
        public void Convert_WhenInvokedWithNonEmptyData_ReturnsTrue()
        {
            //Arrange
            var converter = new StringToBoolConverter();

            //Act
            var actual = (bool)converter.Convert("sample-string", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.True(actual);
        }


        [Fact]
        public void Convert_WhenInvokedWithNullData_ReturnsFalse()
        {
            //Arrange
            var converter = new StringToBoolConverter();

            //Act
            var actual = (bool)converter.Convert(null, typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.False(actual);
        }


        [Fact]
        public void ConvertBack_WhenInvokedWithData_ReturnsFalse()
        {
            //Arrange
            var converter = new StringToBoolConverter();

            //Act
            var actual = (bool)converter.ConvertBack("sample-data", typeof(string), null, CultureInfo.GetCultures(CultureTypes.AllCultures)[0]);

            //Assert
            Assert.False(actual);
        }
        #endregion
    }
}

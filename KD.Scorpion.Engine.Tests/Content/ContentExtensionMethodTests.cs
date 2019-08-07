using Xunit;
using KDScorpionEngine.Content;

namespace KDScorpionEngineTests.Content
{
    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.Content.ExtensionMethods"/> class.
    /// </summary>
    public class ContentExtensionMethodTests
    {
        #region Method Tests
        [Fact]
        public void IsLetter_WhenInvokedWithUpperCaseLetter_ReturnsTrue()
        {
            //Arrange
            var letter = 'Z';
            var expected = true;

            //Act
            var actual = letter.IsLetter();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsLetter_WhenInvokedWithLowerCaseLetter_ReturnsTrue()
        {
            //Arrange
            var letter = 'z';
            var expected = true;

            //Act
            var actual = letter.IsLetter();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsLetter_WhenInvokedWithNumber_ReturnsFalse()
        {
            //Arrange
            var digit = '3';
            var expected = false;

            //Act
            var actual = digit.IsLetter();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsNumber_WhenInvokedWithNumber_ReturnsTrue()
        {
            //Arrange
            var digit = '4';
            var expected = true;

            //Act
            var actual = digit.IsNumber();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsNumber_WhenInvokedWithLetter_ReturnsFalse()
        {
            //Arrange
            var letter = 'T';
            var expected = false;

            //Act
            var actual = letter.IsNumber();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetFirstOccurentOfNumber_WhenInvokedWithNumberInString_ReturnsCorrectIndex()
        {
            //Arrange
            var data = "This number 1234 is my favorite number!";
            var expected = 1234;

            //Act
            var actual = data.GetFirstOccurentOfNumber();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetFirstOccurentOfNumber_WhenInvokedWithNoNumbersInString_ReturnsCorrectValue()
        {
            //Arrange
            var data = "No number is my favorite number!";
            var expected = -1;

            //Act
            var actual = data.GetFirstOccurentOfNumber();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void HasNumbers_WhenInvokedWithNumbers_ReturnsTrue()
        {
            //Arrange
            var data = "The number 1234 is my worst number!!";
            var expected = true;

            //Act
            var actual = data.HasNumbers();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ContainsOnlyLettersAndNumbers_WhenInvokedWithNoSymbols_ReturnsTrue()
        {
            //Arrange
            var data = "TheNumber445IsANumber";
            var expected = true;

            //Act
            var actual = data.ContainsOnlyLettersAndNumbers();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ContainsOnlyLettersAndNumbers_WhenInvokedWithSymbols_ReturnsTrue()
        {
            //Arrange
            var data = "TheNumber445IsANumber!";
            var expected = false;

            //Act
            var actual = data.ContainsOnlyLettersAndNumbers();

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

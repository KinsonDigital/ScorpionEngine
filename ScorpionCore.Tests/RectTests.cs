using NUnit.Framework;

namespace ScorpionCore.Tests
{
    [TestFixture]
    public class RectTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithXAndYAndWidthAndHeight_SetsXAndYProps()
        {
            //Arrange
            var expectedWidth = 11.22f;
            var expectedHeight = 33.44f;

            //Act
            var rect = new Rect(0, 0, 11.22f, 33.44f);
            var actualWidth = rect.Width;
            var actualHeight = rect.Height;

            //Assert
            Assert.AreEqual(expectedWidth, actualWidth);
            Assert.AreEqual(expectedHeight, actualHeight);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void X_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect();
            var expected = 11.22f;

            //Act
            rect.X = 11.22f;
            var actual = rect.X;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Y_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect();
            var expected = 11.22f;

            //Act
            rect.Y = 11.22f;
            var actual = rect.Y;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Position_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect();
            var expected = new Vector(11.22f, 33.44f);

            //Act
            rect.Position = new Vector(11.22f, 33.44f);
            var actual = rect.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Width_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect();
            var expected = 11.22f;

            //Act
            rect.Width = 11.22f;
            var actual = rect.Width;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Height_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect();
            var expected = 11.22f;

            //Act
            rect.Height = 11.22f;
            var actual = rect.Height;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Left_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect()
            {
                X = 11.22f
            };
            var expected = 11.22f;

            //Act
            var actual = rect.Left;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Right_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect()
            {
                X = 11.22f,
                Width = 100f
            };
            var expected = 111.22f;

            //Act
            var actual = rect.Right;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Top_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect()
            {
                Y = 11.22f
            };
            var expected = 11.22f;

            //Act
            var actual = rect.Top;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Bottom_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var rect = new Rect()
            {
                Y = 100f,
                Height = 50f
            };
            var expected = 150f;

            //Act
            var actual = rect.Bottom;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Contains_WhenContainingXAndY_ReturnsTrue()
        {
            //Arrange
            var rect = new Rect()
            {
                Width = 10f,
                Height = 10f
            };
            var expected = true;

            //Act
            var actual = rect.Contains(5.5f, 5.5f);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Contains_WhenNotContainingXAndY_ReturnsFalse()
        {
            //Arrange
            var rect = new Rect()
            {
                Width = 10f,
                Height = 10f
            };
            var expected = false;

            //Act
            var actual = rect.Contains(50.5f, 50.50f);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Contains_WhenContainingVector_ReturnsTrue()
        {
            //Arrange
            var rect = new Rect()
            {
                Width = 10f,
                Height = 10f
            };
            var expected = true;
            var location = new Vector(5.5f, 6.0f);

            //Act
            var actual = rect.Contains(location);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Contains_WhenNotContainingVector_ReturnsFalse()
        {
            //Arrange
            var rect = new Rect()
            {
                Width = 10f,
                Height = 10f
            };
            var expected = false;
            var location = new Vector(50.5f, 60.0f);

            //Act
            var actual = rect.Contains(location);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

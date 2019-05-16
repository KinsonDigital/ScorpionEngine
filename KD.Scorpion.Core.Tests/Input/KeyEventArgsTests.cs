using NUnit.Framework;
using KDScorpionCore.Input;

namespace KDScorpionCoreTests.Input
{
    [TestFixture]
    public class KeyEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsKeysProp()
        {
            //Arrange
            var expected = new KeyCodes[]
            {
                KeyCodes.Left,
                KeyCodes.Right
            };

            //Act
            var eventArgs = new KeyEventArgs(new KeyCodes[] { KeyCodes.Left, KeyCodes.Right });
            var actual = eventArgs.Keys;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Keys_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new KeyCodes[]
            {
                KeyCodes.Up,
                KeyCodes.Down
            };

            //Act
            var eventArgs = new KeyEventArgs(new KeyCodes[] { KeyCodes.Left, KeyCodes.Right })
            {
                Keys = new KeyCodes[] { KeyCodes.Up, KeyCodes.Down }
            };
            var actual = eventArgs.Keys;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

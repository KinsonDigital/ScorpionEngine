using NUnit.Framework;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.CustomEventArgs
{
    [TestFixture]
    public class ValueChangedEventArgsTests
    {
        #region Prop Tests
        [Test]
        public void OldValue_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new ValueChangedEventArgs
            {
                OldValue = 12.34f
            };

            var expected = 12.34f;

            //Act
            var actual = eventArgs.OldValue;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NewValue_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var eventArgs = new ValueChangedEventArgs
            {
                NewValue = 12.34f
            };

            var expected = 12.34f;

            //Act
            var actual = eventArgs.NewValue;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

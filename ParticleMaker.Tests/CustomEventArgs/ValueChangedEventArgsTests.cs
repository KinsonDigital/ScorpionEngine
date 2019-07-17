using Xunit;

namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Unit tests to test the <see cref="ValueChangedEventArgs"/> class.
    /// </summary>
    public class ValueChangedEventArgsTests
    {
        #region Prop Tests
        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

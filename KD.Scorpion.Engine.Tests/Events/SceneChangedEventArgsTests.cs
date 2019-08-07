using Xunit;
using KDScorpionEngine.Events;

namespace KDScorpionEngineTests.Events
{
    /// <summary>
    /// Unit tests to test the <see cref="SceneChangedEventArgs"/> class.
    /// </summary>
    public class SceneChangedEventArgsTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetsPreviousSceneProp()
        {
            //Arrange
            var eventArgs = new SceneChangedEventArgs("PreviousScene", "");
            var expected = "PreviousScene";

            //Act
            var actual = eventArgs.PreviousScene;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_SetsCurrentSceneProp()
        {
            //Arrange
            var eventArgs = new SceneChangedEventArgs("", "CurrentScene");
            var expected = "CurrentScene";

            //Act
            var actual = eventArgs.CurrentScene;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

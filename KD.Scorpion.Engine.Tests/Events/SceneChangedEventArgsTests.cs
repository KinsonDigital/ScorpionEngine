using Xunit;
using KDScorpionEngine.Events;

namespace KDScorpionEngineTests.Events
{
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

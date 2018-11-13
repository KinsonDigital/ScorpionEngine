using NUnit.Framework;
using ScorpionEngine.Events;

namespace ScorpionEngine.Tests.Events
{
    [TestFixture]
    public class SceneChangedEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsPreviousSceneProp()
        {
            //Arrange
            var eventArgs = new SceneChangedEventArgs("PreviousScene", "");
            var expected = "PreviousScene";

            //Act
            var actual = eventArgs.PreviousScene;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_SetsCurrentSceneProp()
        {
            //Arrange
            var eventArgs = new SceneChangedEventArgs("", "CurrentScene");
            var expected = "CurrentScene";

            //Act
            var actual = eventArgs.CurrentScene;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

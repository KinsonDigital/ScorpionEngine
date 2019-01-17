using NUnit.Framework;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class GraphicsEngineFactoryTests
    {
        #region Prop Tests
        [Test]
        public void CoreEngine_WhenGettingValue_ValueIsNotNull()
        {
            //Arrange
            var factory = new GraphicsEngineFactory();
            var expected = true;

            //Act
            var actual = factory.CoreEngine != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

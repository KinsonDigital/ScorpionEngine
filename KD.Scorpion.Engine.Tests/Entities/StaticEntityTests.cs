using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;

namespace KDScorpionEngineTests.Entities
{
    [TestFixture]
    public class StaticEntityTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_PropertlyConstructsObject()
        {
            //Arrange
            var mockPhysicPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicPluginLibrary.Object);

            var mockTexture = new Mock<ITexture>();
            var texture = new Texture() { InternalTexture = mockTexture.Object };
            var entity = new StaticEntity(texture, new Vector(123, 456));
            entity.Initialize();
            var expected = new Vector(123, 456);

            //Act
            var actual = entity.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Update_WhenInvoking_UpdatesBehavior()
        {
            //Arrange
            var mockPhysicPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicPluginLibrary.Object);
            var mockTexture = new Mock<ITexture>();
            var mockBehavior = new Mock<IBehavior>();
            var texture = new Texture() { InternalTexture = mockTexture.Object };
            var entity = new StaticEntity(texture, new Vector(123, 456));
            entity.Behaviors.Add(mockBehavior.Object);

            //Act
            entity.Update(new EngineTime());

            //Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }
        #endregion
    }
}

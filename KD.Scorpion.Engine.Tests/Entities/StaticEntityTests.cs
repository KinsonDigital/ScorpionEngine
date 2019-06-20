using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;
using PluginSystem;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Physics;

namespace KDScorpionEngineTests.Entities
{
    public class StaticEntityTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlyConstructsObject()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.SetupProperty(p => p.X);
            mockPhysicsBody.SetupProperty(p => p.Y);

            var mockTexture = new Mock<ITexture>();
            var texture = new Texture(mockTexture.Object);
            var entity = new StaticEntity(texture, new Vector(123, 456))
            {
                Body = new PhysicsBody(mockPhysicsBody.Object)
            };
            entity.Initialize();

            var expected = new Vector(123, 456);

            //Act
            var actual = entity.Position;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvoking_UpdatesBehavior()
        {
            //Arrange
            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            Plugins.PhysicsPlugins = mockPhysicsPluginLibrary.Object;

            var mockTexture = new Mock<ITexture>();
            var mockBehavior = new Mock<IBehavior>();
            var texture = new Texture(mockTexture.Object);
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

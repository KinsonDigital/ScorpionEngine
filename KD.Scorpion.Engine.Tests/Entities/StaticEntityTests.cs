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
using System;
using KDScorpionEngine;

namespace KDScorpionEngineTests.Entities
{
    public class StaticEntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> _mockPhysicsBody;
        private Mock<IPluginLibrary> _mockPhysicsPluginLib;
        private Plugins _plugins;
        #endregion


        #region Constructors
        public StaticEntityTests()
        {
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(p => p.X);
            _mockPhysicsBody.SetupProperty(p => p.Y);

            _mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            _mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>()).Returns(_mockPhysicsBody.Object);

            _plugins = new Plugins()
            {
                PhysicsPlugins = _mockPhysicsPluginLib.Object
            };

            EnginePluginSystem.SetPlugins(_plugins);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlyConstructsObject()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            var entity = new StaticEntity(new Texture(mockTexture.Object), new Vector(123, 456))
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
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


        #region Public Methods
        public void Dispose()
        {
            _mockPhysicsBody = null;
            _mockPhysicsPluginLib = null;
            _plugins = null;
            EnginePluginSystem.ClearPlugins();
        }
        #endregion
    }
}

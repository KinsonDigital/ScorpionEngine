using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Exceptions;
using KDScorpionEngine.Physics;
using PluginSystem;
using KDScorpionEngine;
using KDScorpionCore.Physics;

namespace KDScorpionCoreTests.Physics
{
    public class PhysicsWorldTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsWorld> _mockPhysicsWorld;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        private Mock<IPluginLibrary> _mockPhysicsPluginLib;
        private Plugins _plugins;
        #endregion


        #region Constructors
        public PhysicsWorldTests()
        {
            _mockPhysicsWorld = new Mock<IPhysicsWorld>();
            _mockPhysicsWorld.SetupGet(m => m.GravityX).Returns(2);
            _mockPhysicsWorld.SetupGet(m => m.GravityY).Returns(4);

            _mockPhysicsBody = new Mock<IPhysicsBody>();

            _mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            _mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>()).Returns(_mockPhysicsBody.Object);

            _plugins = new Plugins()
            {
                PhysicsPlugins = _mockPhysicsPluginLib.Object
            };

            EnginePluginSystem.SetPlugins(_plugins);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Ctro_WhenInvoking_ReturnsGravity()
        {
            //Arrange
            var gravity = new Vector(2, 4);
            var world = new PhysicsWorld(gravity, _mockPhysicsWorld.Object);
            var expected = new Vector(2, 4);

            //Act
            var actual = world.Gravity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AddEntity_WhenInvoking_DoesNotThrowNullRefException()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();

            var texture = new Texture(mockTexture.Object);
            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>());
            var entity = new FakeEntity(texture: texture, position: Vector.Zero)
            {
                Body = body
            };
            entity.Initialize();
            var world = new PhysicsWorld(Vector.Zero, _mockPhysicsWorld.Object);

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                world.AddEntity(entity);
            });
        }


        [Fact]
        public void AddEntity_WhenInvokingWhileNotInitialized_ThrowException()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();

            var texture = new Texture(mockTexture.Object);
            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>());
            var entity = new FakeEntity(texture: texture, position: Vector.Zero)
            {
                Body = body
            };
            var world = new PhysicsWorld(Vector.Zero, _mockPhysicsWorld.Object);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() =>
            {
                world.AddEntity(entity);
            });
        }


        [Fact]
        public void Update_WhenInvoking_DoesNotThrowNullRefException()
        {
            //Arrange
            var world = new PhysicsWorld(Vector.Zero, _mockPhysicsWorld.Object);

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                world.Update(0f);
            });
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _mockPhysicsBody = null;
            _mockPhysicsWorld = null;
            _mockPhysicsPluginLib = null;
            _plugins = null;
            EnginePluginSystem.ClearPlugins();
        }
        #endregion
    }
}

using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Physics;
using ScorpionEngine.Tests.Fakes;

namespace ScorpionEngine.Tests.Physics
{
    public class PhysicsWorldTests
    {
        #region Method Tests
        [Test]
        public void Ctro_WhenInvoking_ReturnsGravity()
        {
            //Arrange
            var mockInternalPhysicsWorld = new Mock<IPhysicsWorld>();
            mockInternalPhysicsWorld.SetupGet(m => m.GravityX).Returns(2);
            mockInternalPhysicsWorld.SetupGet(m => m.GravityY).Returns(4);

            Helpers.SetupPluginLib<IPhysicsWorld, float, float>(mockInternalPhysicsWorld, PluginLibType.Physics);

            var gravity = new Vector(2, 4);
            var world = new PhysicsWorld(gravity);
            var expected = new Vector(2, 4);

            //Act
            var actual = world.Gravity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AddEntity_WhenInvoking_DoesNotThrowNullRefException()
        {
            //Arrange
            var mockPhysicsWorld = new Mock<IPhysicsWorld>();
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPluginLib = new Mock<IPluginLibrary>();

            //Mock method for creating a physics body
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns(() =>
            {
                return mockPhysicsBody.Object;
            });

            //Mock method for loading a physics world
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsWorld>(It.IsAny<float>(), It.IsAny<float>())).Returns(() =>
            {
                return mockPhysicsWorld.Object;
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);
            var mockTexture = new Mock<ITexture>();

            var texture = new Texture() { InternalTexture = mockTexture.Object };
            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero);
            var entity = new FakeEntity(texture: texture, position: Vector.Zero)
            {
                Body = body
            };
            entity.Initialize();
            var world = new PhysicsWorld(Vector.Zero);

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                world.AddEntity(entity);
            });
        }


        [Test]
        public void AddEntity_WhenInvokingWhileNotInitialized_ThrowException()
        {
            //Arrange
            var mockPhysicsWorld = new Mock<IPhysicsWorld>();
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPluginLib = new Mock<IPluginLibrary>();

            //Mock method for creating a physics body
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns(() =>
            {
                return mockPhysicsBody.Object;
            });

            //Mock method for loading a physics world
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsWorld>(It.IsAny<float>(), It.IsAny<float>())).Returns(() =>
            {
                return mockPhysicsWorld.Object;
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);
            var mockTexture = new Mock<ITexture>();

            var texture = new Texture() { InternalTexture = mockTexture.Object };
            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero);
            var entity = new FakeEntity(texture: texture, position: Vector.Zero)
            {
                Body = body
            };
            var world = new PhysicsWorld(Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() =>
            {
                world.AddEntity(entity);
            });
        }


        [Test]
        public void Update_WhenInvoking_DoesNotThrowNullRefException()
        {
            //Arrange
            SetupPluginSystem();
            var world = new PhysicsWorld(Vector.Zero);

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                world.Update(0f);
            });
        }
        #endregion


        #region Private Methods
        private void SetupPluginSystem()
        {
            var mockPhysicsWorld = new Mock<IPhysicsWorld>();
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPluginLib = new Mock<IPluginLibrary>();

            //Mock method for creating a physics body
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns(() =>
            {
                return mockPhysicsBody.Object;
            });

            //Mock method for loading a physics world
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsWorld>(It.IsAny<float>(), It.IsAny<float>())).Returns(() =>
            {
                return mockPhysicsWorld.Object;
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);
        }
        #endregion


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
    }
}

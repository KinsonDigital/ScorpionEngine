using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Exceptions;
using KDScorpionEngine.Physics;
using KDScorpionEngineTests.Fakes;
using PluginSystem;

namespace KDScorpionEngineTests.Physics
{
    public class PhysicsWorldTests
    {
        #region Method Tests
        [Test]
        public void Ctro_WhenInvoking_ReturnsGravity()
        {
            //Arrange
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
            var mockTexture = new Mock<ITexture>();

            var texture = new Texture(mockTexture.Object);
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
            var mockTexture = new Mock<ITexture>();

            var texture = new Texture(mockTexture.Object);
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
            var world = new PhysicsWorld(Vector.Zero);

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                world.Update(0f);
            });
        }
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
            var mockPhysicsWorld = new Mock<IPhysicsWorld>();
            mockPhysicsWorld.SetupGet(m => m.GravityX).Returns(2);
            mockPhysicsWorld.SetupGet(m => m.GravityY).Returns(4);

            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPluginFactory = new Mock<IPluginFactory>();

            //Mock method for creating a physics body
            mockPluginFactory.Setup(m => m.CreatePhysicsBody(It.IsAny<object[]>())).Returns(() => mockPhysicsBody.Object);

            //Mock method for loading a physics world
            mockPluginFactory.Setup(m => m.CreatePhysicsWorld(It.IsAny<float>(), It.IsAny<float>())).Returns(() => mockPhysicsWorld.Object);

            Plugins.LoadPluginFactory(mockPluginFactory.Object);
        }
        #endregion


        [TearDown]
        public void TearDown() => Plugins.UnloadPluginFactory();
    }
}

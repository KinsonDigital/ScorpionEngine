using System;
using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Input;
using ScorpionEngine.Objects;
using ScorpionEngine.Physics;
using ScorpionEngine.Tests.Fakes;
using Xunit;

namespace ScorpionEngine.Tests.Physics
{
    public class PhysicsWorldTests
    {
        private Mock<IPhysicsBody> _mockPhysicsBody;


        #region Method Tests
        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AddEntity_WhenInvoking_DoesNotThrowNullRefException()
        {
            //Arrange
            SetupPluginSystem();

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = _mockPhysicsBody.Object
            };
            var entity = new FakeEntity(Vector.Zero)
            {
                Body = body
            };
            var world = new PhysicsWorld(Vector.Zero);

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                world.AddEntity(entity);
            });
        }


        [Fact]
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
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockPluginLib = new Mock<IPluginLibrary>();

            //Mock method for creating a physics body
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns(() =>
            {
                return _mockPhysicsBody.Object;
            });

            //Mock method for loading a physics world
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsWorld>(It.IsAny<float>(), It.IsAny<float>())).Returns(() =>
            {
                return mockPhysicsWorld.Object;
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);
        }
        #endregion
    }
}

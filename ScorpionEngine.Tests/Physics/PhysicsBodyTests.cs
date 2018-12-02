using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Physics;
using ScorpionEngine.Tests.Fakes;

namespace ScorpionEngine.Tests.Physics
{
    public class PhysicsBodyTests
    {
        #region Prop Tests
        [Test]
        public void Vertices_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctrParams) =>
            {
                return new FakePhysicsBody((float[])ctrParams[0], (float[])ctrParams[1]);
            });
            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);

            var expectedVertices = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66),
            };

            //Act
            var body = new PhysicsBody(expectedVertices, Vector.Zero);
            var actualVertices = body.Vertices;

            //Assert
            Assert.NotNull(actualVertices);
            Assert.AreEqual(expectedVertices, actualVertices);
        }


        [Test]
        public void X_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.X);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 1234.4321f;

            //Act
            body.X = 1234.4321f;
            var actual = body.X;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Y_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.Y);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 456.654f;

            //Act
            body.Y = 456.654f;
            var actual = body.Y;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Angle_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.Angle);

            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<float>())).Returns(() =>
            {
                return mockInternalPhysicsBody.Object;
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPluginLib.Object);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 90.12f;

            //Act
            body.Angle = 90.12f;
            var actual = body.Angle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Density_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.Density);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 14.7f;

            //Act
            body.Density = 14.7f;
            var actual = body.Density;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Friction_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.Friction);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 43.73f;

            //Act
            body.Friction = 43.73f;
            var actual = body.Friction;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Restitution_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.Restitution);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 8.4f;

            //Act
            body.Restitution = 8.4f;
            var actual = body.Restitution;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LinearDeceleration_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.LinearDeceleration);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 3.22f;

            //Act
            body.LinearDeceleration = 3.22f;
            var actual = body.LinearDeceleration;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularDeceleration_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.AngularDeceleration);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 43.73f;

            //Act
            body.AngularDeceleration = 43.73f;
            var actual = body.AngularDeceleration;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LinearVelocity_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.LinearVelocityX);
            mockInternalPhysicsBody.SetupProperty(m => m.LinearVelocityY);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = new Vector(123.321f, 789.987f);

            //Act
            body.LinearVelocity = new Vector(123.321f, 789.987f);
            var actual = body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocity_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.AngularVelocity);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

            var vertices = new Vector[] { Vector.Zero, Vector.Zero };
            var body = new PhysicsBody(vertices, Vector.Zero)
            {
                InternalPhysicsBody = mockInternalPhysicsBody.Object
            };
            var expected = 1973.3791f;

            //Act
            body.AngularVelocity = 1973.3791f;
            var actual = body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
        #endregion
    }
}

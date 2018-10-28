﻿using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Physics;
using ScorpionEngine.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests.Physics
{
    public class PhysicsBodyTests
    {
        #region Prop Tests
        [Fact]
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
            Assert.Equal(expectedVertices, actualVertices);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Angle_WhenGettingAndSettingValue_GetsCorrectValue()
        {
            //Arrange
            var mockInternalPhysicsBody = new Mock<IPhysicsBody>();
            mockInternalPhysicsBody.SetupProperty(m => m.Angle);

            Helpers.SetupPluginLib<IPhysicsBody, float, float>(mockInternalPhysicsBody, PluginLibType.Physics);

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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }



        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

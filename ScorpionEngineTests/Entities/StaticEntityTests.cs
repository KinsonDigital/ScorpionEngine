﻿using Moq;
using Xunit;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;
using System;
using Raptor;
using Raptor.Graphics;
using Raptor.Plugins;

namespace KDScorpionEngineTests.Entities
{
    /// <summary>
    /// Unit tests to test the <see cref="StaticEntity"/> class.
    /// </summary>
    public class StaticEntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public StaticEntityTests()
        {
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(p => p.X);
            _mockPhysicsBody.SetupProperty(p => p.Y);
            _mockPhysicsBody.SetupProperty(p => p.XVertices);
            _mockPhysicsBody.SetupProperty(p => p.YVertices);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlyConstructsObject()
        {
            //Arrange
            var entity = new StaticEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            //Assert
            Assert.NotNull(entity.Body);
            Assert.Equal(Vector.Zero, entity.Position);
            Assert.False(entity.IsStatic);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvoking_UpdatesBehavior()
        {
            //Arrange
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
        public void Dispose() => _mockPhysicsBody = null;
        #endregion
    }
}
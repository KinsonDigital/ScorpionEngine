// <copyright file="StaticEntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor;
    using Raptor.Graphics;
    using Raptor.Plugins;
    using Xunit;

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
        // [Fact]
        public void Ctor_WhenInvoking_ProperlyConstructsObject()
        {
            // Arrange
            var entity = new StaticEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            // Assert
            Assert.NotNull(entity.Body);
            Assert.Equal(Vector2.Zero, entity.Position);
            Assert.False(entity.IsStatic);
        }
        #endregion

        #region Method Tests
        // [Fact]
        public void Update_WhenInvoking_UpdatesBehavior()
        {
            // Arrange
            var mockTexture = new Mock<ITexture>();
            var mockBehavior = new Mock<IBehavior>();
            var texture = new Texture(mockTexture.Object);
            var entity = new StaticEntity(texture, new Vector2(123, 456));
            entity.Behaviors.Add(mockBehavior.Object);

            // Act
            entity.Update(new EngineTime());

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }
        #endregion

        #region Public Methods
        public void Dispose() => _mockPhysicsBody = null;
        #endregion
    }
}

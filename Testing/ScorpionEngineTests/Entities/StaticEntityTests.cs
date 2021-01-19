// <copyright file="StaticEntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using Moq;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="StaticEntity"/> class.
    /// </summary>
    public class StaticEntityTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlyConstructsObject()
        {
            // Arrange
            var texture = new Texture("test-texture", Array.Empty<byte>(), 10, 20);
            var entity = new StaticEntity(texture, new Vector2(123, 456));

            // Assert
            Assert.Equal(Vector2.Zero, entity.Position);
            Assert.False(entity.IsStatic);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvoking_UpdatesBehavior()
        {
            // Arrange
            var mockTexture = new Mock<ITexture>();
            var mockBehavior = new Mock<IBehavior>();
            var texture = new Texture("test-texture", Array.Empty<byte>(), 10, 20);
            var entity = new StaticEntity(texture, new Vector2(123, 456));
            entity.Behaviors.Add(mockBehavior.Object);

            // Act
            entity.Update(new GameTime());

            // Assert
            mockBehavior.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Once());
        }
        #endregion
    }
}

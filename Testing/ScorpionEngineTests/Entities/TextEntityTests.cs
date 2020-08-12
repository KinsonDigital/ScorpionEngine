// <copyright file="TextEntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using KDScorpionEngine.Entities;
    using Moq;
    using Raptor.Graphics;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="TextEntity"/> class.
    /// </summary>
    public class TextEntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> mockPhysicsBody;
        #endregion

        #region Constructors
        public TextEntityTests()
        {
            this.mockPhysicsBody = new Mock<IPhysicsBody>();
            this.mockPhysicsBody.SetupProperty(p => p.X);
            this.mockPhysicsBody.SetupProperty(p => p.Y);
        }
        #endregion

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsUpEntity()
        {
            // Arrange
            var entity = new TextEntity(this.mockPhysicsBody.Object);

            // Assert
            Assert.Equal(string.Empty, entity.Text);
            Assert.Equal(new GameColor(255, 0, 0, 0), entity.ForeColor);
            Assert.Equal(new GameColor(0, 0, 0, 0), entity.BackColor);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void Text_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new TextEntity(this.mockPhysicsBody.Object);
            var expected = "hello world";

            // Act
            entity.Text = "hello world";
            var actual = entity.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForeColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new TextEntity(this.mockPhysicsBody.Object);
            var expected = new GameColor(255, 11, 22, 33);

            // Act
            entity.ForeColor = new GameColor(255, 11, 22, 33);
            var actual = entity.ForeColor;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BackColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new TextEntity(this.mockPhysicsBody.Object);
            var expected = new GameColor(255, 11, 22, 33);

            // Act
            entity.BackColor = new GameColor(255, 11, 22, 33);
            var actual = entity.BackColor;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        public void Dispose()
        {
            this.mockPhysicsBody = null;
            GC.SuppressFinalize(this);
        }
    }
}

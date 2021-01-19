// <copyright file="TextEntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System.Drawing;
    using System.Numerics;
    using KDScorpionEngine.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="TextEntity"/> class.
    /// </summary>
    public class TextEntityTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsUpEntity()
        {
            // Arrange
            var entity = CreateTextEntity();

            // Assert
            Assert.Equal(string.Empty, entity.Text);
            Assert.Equal(Color.FromArgb(255, 0, 0, 0), entity.ForeColor);
            Assert.Equal(Color.FromArgb(0, 0, 0, 0), entity.BackColor);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void Text_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateTextEntity();
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
            var entity = CreateTextEntity();
            var expected = Color.FromArgb(255, 11, 22, 33);

            // Act
            entity.ForeColor = Color.FromArgb(255, 11, 22, 33);
            var actual = entity.ForeColor;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BackColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = CreateTextEntity();
            var expected = Color.FromArgb(255, 11, 22, 33);

            // Act
            entity.BackColor = Color.FromArgb(255, 11, 22, 33);
            var actual = entity.BackColor;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        /// <summary>
        /// Creats a new instance of <see cref="TextEntity"/> for the purpose of testing.
        /// </summary>
        /// <returns>An instance for testing.</returns>
        private TextEntity CreateTextEntity() => new TextEntity("test-entity", Color.Black, Color.White, Vector2.Zero);
    }
}

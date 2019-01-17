﻿using NUnit.Framework;
using System.Windows.Media;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class ColorItemTests
    {
        #region Prop Tests
        [Test]
        public void Id_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var colorItem = new ColorItem();
            var expected = 1234;

            //Act
            colorItem.Id = 1234;
            var actual = colorItem.Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ColorBrush_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var colorItem = new ColorItem();
            var expected = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33));

            //Act
            colorItem.ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33));
            var actual = colorItem.ColorBrush;

            //Assert
            Assert.AreEqual(expected.Color, actual.Color);
        }
        #endregion


        #region Public Methods
        [Test]
        public void ToString_WhenInvoking_ReturnsCorrectStringValue()
        {
            //Arrange
            var colorItem = new ColorItem();
            colorItem.Id = 1234;
            colorItem.ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33));
            var expected = "R:11, G:22, B:33, A:255";

            //Act
            var actual = colorItem.ToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
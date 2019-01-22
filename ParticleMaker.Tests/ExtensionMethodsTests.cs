﻿using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using ParticleMaker.Services;
using System;
using System.Windows.Media;
using MediaColor = System.Windows.Media.Color;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Test]
        public void ToRadians_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var angle = 45.45f;
            var expected = 0.79325217f;

            //Act
            var actual = angle.ToRadians();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToColorItem_WhenInvoked_ReturnsCorrectColorItem()
        {
            //Arrange
            var gameColor = new GameColor(11, 22, 33, 44);
            var expected = new ColorItem()
            {
                Id = 0,
                ColorBrush = new SolidColorBrush(MediaColor.FromArgb(44, 11, 22, 33))
            };

            //Act
            var actual = gameColor.ToColorItem();

            //Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ColorBrush.Color, actual.ColorBrush.Color);
        }


        [Test]
        public void ToEngineTime_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var gameTime = new GameTime()
            {
                ElapsedGameTime = new TimeSpan(11, 22, 33, 44, 55)
            };
            var expected = new TimeSpan(11, 22, 33, 44, 55);

            //Act
            var actual = gameTime.ToEngineTime();

            //Assert
            Assert.AreEqual(expected, actual.ElapsedEngineTime);
        }


        [Test]
        public void Join_WhenInvokingWithItemExclude_ReturnsCorrectValue()
        {
            //Arrange
            var items = new[]
            {
                "C:",
                "parent-folder",
                "child-folder",
                "file-a.txt"
            };
            var expected = @"C:\parent-folder\child-folder";

            //Act
            var actual = items.Join("file-a.txt");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Join_WhenInvokingWithNoExclude_ReturnsCorrectValue()
        {
            //Arrange
            var items = new[]
            {
                "C:",
                "parent-folder",
                "child-folder",
                "file-a.txt"
            };
            var expected = @"C:\parent-folder\child-folder\file-a.txt";

            //Act
            var actual = items.Join();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

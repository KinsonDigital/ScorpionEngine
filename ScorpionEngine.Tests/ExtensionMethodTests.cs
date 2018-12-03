﻿using NUnit.Framework;
using ScorpionCore;
using System;

namespace ScorpionEngine.Tests
{
    [TestFixture]
    public class ExtensionMethodTests
    {
        #region Method Tests
        [Test]
        public void ToDegress_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 45.0f;

            //Act
            var actual = 0.785398185f.ToDegrees();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToRadians_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 0.785398185f;

            //Act
            var actual = 45f.ToRadians();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ForcePositive_WhenNegative_ReturnsPositiveResult()
        {
            //Arrange
            var expected = 10f;

            //Act
            var actual = (-10f).ForcePositive();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ForcePositive_WhenPositive_ReturnsPositiveResult()
        {
            //Arrange
            var expected = 10f;

            //Act
            var actual = (10f).ForcePositive();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ForceNegative_WhenNegative_ReturnsNegativeResult()
        {
            //Arrange
            var expected = -10f;

            //Act
            var actual = (-10f).ForceNegative();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ForceNegative_WhenPositive_ReturnsNegativeResult()
        {
            //Arrange
            var expected = -10f;

            //Act
            var actual = (10f).ForceNegative();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateAround_WhenRotatingClockWise_ReturnsCorrectResult()
        {
            //Arrange
            var vector = new Vector(0, 0);

            var expected = new Vector(6.62790775f, -1.88112736f);

            //Act
            var actual = vector.RotateAround(new Vector(5, 5), 45f, true);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateAround_WhenRotatingCounterClockWise_ReturnsCorrectResult()
        {
            //Arrange
            var vector = new Vector(0, 0);

            var expected = new Vector(-1.88112736f, 6.62790775f);

            //Act
            var actual = vector.RotateAround(new Vector(5, 5), 45f, false);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Next_WhenInvoking_ReturnsValidValueWithinRange()
        {
            //Arrange
            var random = new Random();
            var expected = true;

            //Act
            var actual = true;

            for (int i = 0; i < 1000; i++)
            {
                var randomResult = random.Next(1, 10);

                if (randomResult < 1 && randomResult > 10)
                    actual = false;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

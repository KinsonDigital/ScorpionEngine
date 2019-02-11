using KDParticleEngine;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace KD.Particle.Engine.Tests
{
    [TestFixture]
    public class ParticleSetupTests
    {
        #region Prop Tests
        [Test]
        public void RedMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.RedMin = 123;
            var actual = setup.RedMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RedMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.RedMax = 123;
            var actual = setup.RedMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GreenMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.GreenMin = 123;
            var actual = setup.GreenMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GreenMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.GreenMax = 123;
            var actual = setup.GreenMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BlueMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.BlueMin = 123;
            var actual = setup.BlueMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BlueMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.BlueMax = 123;
            var actual = setup.BlueMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SizeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.SizeMin = 1234.4321f;
            var actual = setup.SizeMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SizeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.SizeMax = 1234.4321f;
            var actual = setup.SizeMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngleMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngleMin = 1234.4321f;
            var actual = setup.AngleMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngleMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngleMax = 1234.4321f;
            var actual = setup.AngleMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocityMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngularVelocityMin = 1234.4321f;
            var actual = setup.AngularVelocityMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocityMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngularVelocityMax = 1234.4321f;
            var actual = setup.AngularVelocityMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityXMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityXMin = 1234.4321f;
            var actual = setup.VelocityXMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityXMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityXMax = 1234.4321f;
            var actual = setup.VelocityXMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityYMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityYMin = 1234.4321f;
            var actual = setup.VelocityYMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityYMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityYMax = 1234.4321f;
            var actual = setup.VelocityYMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LifetimeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.LifetimeMin = 1234;
            var actual = setup.LifetimeMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LifetimeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.LifetimeMax = 1234;
            var actual = setup.LifetimeMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpawnRateMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.SpawnRateMin = 1234;
            var actual = setup.SpawnRateMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpawnRateMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.SpawnRateMax = 1234;
            var actual = setup.SpawnRateMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UseColorsFromList_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = true;

            //Act
            setup.UseColorsFromList = true;
            var actual = setup.UseColorsFromList;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Colors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = new ParticleColor[]
            {
                new ParticleColor() { Alpha = 44, Red = 11, Green = 22, Blue = 33 },
                new ParticleColor() { Alpha = 88, Red = 55, Green = 66, Blue = 77 }
            };

            //Act
            setup.Colors = new ParticleColor[]
            {
                new ParticleColor() { Alpha = 44, Red = 11, Green = 22, Blue = 33 },
                new ParticleColor() { Alpha = 88, Red = 55, Green = 66, Blue = 77 }
            };
            var actual = setup.Colors;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

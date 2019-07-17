using KDParticleEngine;
using Xunit;
using System.Drawing;

namespace KD.Particle.Engine.Tests
{
    /// <summary>
    /// Unit tests to test the <see cref="ParticleSetup"/> class.
    /// </summary>
    public class ParticleSetupTests
    {
        #region Prop Tests
        [Fact]
        public void RedMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.RedMin = 123;
            var actual = setup.RedMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RedMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.RedMax = 123;
            var actual = setup.RedMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GreenMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.GreenMin = 123;
            var actual = setup.GreenMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GreenMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.GreenMax = 123;
            var actual = setup.GreenMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BlueMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.BlueMin = 123;
            var actual = setup.BlueMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BlueMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            byte expected = 123;

            //Act
            setup.BlueMax = 123;
            var actual = setup.BlueMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SizeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.SizeMin = 1234.4321f;
            var actual = setup.SizeMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SizeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.SizeMax = 1234.4321f;
            var actual = setup.SizeMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngleMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngleMin = 1234.4321f;
            var actual = setup.AngleMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngleMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngleMax = 1234.4321f;
            var actual = setup.AngleMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngularVelocityMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngularVelocityMin = 1234.4321f;
            var actual = setup.AngularVelocityMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngularVelocityMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.AngularVelocityMax = 1234.4321f;
            var actual = setup.AngularVelocityMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityXMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityXMin = 1234.4321f;
            var actual = setup.VelocityXMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityXMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityXMax = 1234.4321f;
            var actual = setup.VelocityXMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityYMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityYMin = 1234.4321f;
            var actual = setup.VelocityYMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityYMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234.4321f;

            //Act
            setup.VelocityYMax = 1234.4321f;
            var actual = setup.VelocityYMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LifetimeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.LifeTimeMin = 1234;
            var actual = setup.LifeTimeMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LifetimeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.LifeTimeMax = 1234;
            var actual = setup.LifeTimeMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SpawnRateMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.SpawnRateMin = 1234;
            var actual = setup.SpawnRateMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SpawnRateMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = 1234;

            //Act
            setup.SpawnRateMax = 1234;
            var actual = setup.SpawnRateMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UseColorsFromList_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = true;

            //Act
            setup.UseColorsFromList = true;
            var actual = setup.UseColorsFromList;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Colors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setup = new ParticleSetup();
            var expected = new Color[]
            {
                Color.FromArgb(44, 11, 22, 33),
                Color.FromArgb(88, 55, 66, 77)
            };

            //Act
            setup.Colors = new Color[]
            {
                Color.FromArgb(44, 11, 22, 33),
                Color.FromArgb(88, 55, 66, 77)
            };
            var actual = setup.Colors;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

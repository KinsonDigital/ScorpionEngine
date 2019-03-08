using NUnit.Framework;
using ParticleMaker.Management;

namespace ParticleMaker.Tests.Management
{
    [TestFixture]
    public class DeploymentSettingTests
    {
        #region Prop Tests
        [Test]
        public void SetupName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting();
            var expected = "test-project";

            //Act
            setting.SetupName = "test-project";
            var actual = setting.SetupName;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DeployPath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting();
            var expected = @"C:\deploy-path";

            //Act
            setting.DeployPath = @"C:\deploy-path";
            var actual = setting.DeployPath;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Equals_WhenInvokingWithIdenticalObjects_ReturnsTrue()
        {
            //Arrange
            var settingA = new DeploymentSetting()
            {
                SetupName = "test-setup",
                DeployPath = @"C:\deploy-location"
            };
            
            var settingB = new DeploymentSetting()
            {
                SetupName = "test-setup",
                DeployPath = @"C:\deploy-location"
            };
            var expected = true;

            //Act
            var actual = settingA.Equals(settingB);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Equals_WhenInvokingWithNoIdenticalObjects_ReturnsFalse()
        {
            //Arrange
            var settingA = new DeploymentSetting()
            {
                SetupName = "setupA",
                DeployPath = @"C:\deploy-location"
            };

            var settingB = new DeploymentSetting()
            {
                SetupName = "setupB",
                DeployPath = @"C:\deploy-location"
            };
            var expected = false;

            //Act
            var actual = settingA.Equals(settingB);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetHashCode_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting()
            {
                SetupName = "test-setup",
                DeployPath = @"C:\deploy-location"
            };

            var expected = 1528961070;

            //Act
            var actual = setting.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

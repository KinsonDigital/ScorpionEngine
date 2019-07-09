using Xunit;
using ParticleMaker.Management;

namespace ParticleMaker.Tests.Management
{
    public class DeploymentSettingTests
    {
        #region Prop Tests
        [Fact]
        public void SetupName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting();
            var expected = "test-project";

            //Act
            setting.SetupName = "test-project";
            var actual = setting.SetupName;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void DeployPath_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting();
            var expected = @"C:\deploy-path";

            //Act
            setting.DeployPath = @"C:\deploy-path";
            var actual = setting.DeployPath;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetHashCode_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting()
            {
                SetupName = "test-setup",
                DeployPath = @"C:\deploy-location"
            };

            //Act
            var actual = setting.GetHashCode();

            //Assert
            Assert.NotEqual(0, actual);
        }
        #endregion
    }
}

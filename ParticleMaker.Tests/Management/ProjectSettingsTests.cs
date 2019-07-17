using Xunit;
using ParticleMaker.Management;

namespace ParticleMaker.Tests.Management
{
    /// <summary>
    /// Unit tests to test the <see cref="ProjectSettings"/> class.
    /// </summary>
    public class ProjectSettingsTests
    {
        #region Prop Tests
        [Fact]
        public void SetupDeploySettings_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var setting = new DeploymentSetting()
            {
                SetupName = "test-setup",
                DeployPath = @"C:\deploy-location"
            };
            var projSettings = new ProjectSettings()
            {
                SetupDeploySettings = new[]
                {
                    setting
                }
            };
            var expected = new []
            {
                new DeploymentSetting()
                {
                    SetupName = "test-setup",
                    DeployPath = @"C:\deploy-location"
                }
            };

            //Act
            var actual = projSettings.SetupDeploySettings;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ProjectName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var settings = new ProjectSettings();
            var expected = "test-project";

            //Act
            settings.ProjectName = "test-project";
            var actual = settings.ProjectName;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

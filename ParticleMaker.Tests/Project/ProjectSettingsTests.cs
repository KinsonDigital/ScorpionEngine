using NUnit.Framework;
using ParticleMaker.Project;

namespace ParticleMaker.Tests.Project
{
    [TestFixture]
    public class ProjectSettingsTests
    {
        #region Prop Tests
        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ProjectName_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var settings = new ProjectSettings();
            var expected = "test-project";

            //Act
            settings.ProjectName = "test-project";
            var actual = settings.ProjectName;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

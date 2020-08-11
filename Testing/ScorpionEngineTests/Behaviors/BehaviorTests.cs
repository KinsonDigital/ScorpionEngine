namespace KDScorpionEngineTests.Behaviors
{
    using KDScorpionEngineTests.Fakes;
    using Raptor;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.Behaviors.Behavior"/> class.
    /// </summary>
    public class BehaviorTests
    {
        #region Prop Tests
        [Fact]
        public void Enabled_WhenSettingToFalse_ReturnsFalse()
        {
            // Arrange
            var behavior = new FakeBehavior(setupAction: false);
            var expected = false;

            // Act
            behavior.Enabled = false;
            var actual = behavior.Enabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var behavior = new FakeBehavior(setupAction: false);
            var expected = "John Doe";

            // Act
            behavior.Name = "John Doe";
            var actual = behavior.Name;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokedWhileDisabled_DoesNotInvokeBehaviorAction()
        {
            // Arrange
            var behavior = new FakeBehavior(setupAction: true);
            var expected = false;

            // Act
            behavior.Enabled = false;
            behavior.Update(new EngineTime());
            var actual = behavior.UpdateActionInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokedWithNoSetupAction_DoesNotInvokeAction()
        {
            // Arrange
            var behavior = new FakeBehavior(setupAction: false);
            var expected = false;

            // Act
            behavior.Update(new EngineTime());
            var actual = behavior.UpdateActionInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokedWithSetupActionAndEnabled_InvokesAction()
        {
            // Arrange
            var behavior = new FakeBehavior(setupAction: true);
            var expected = true;

            // Act
            behavior.Update(new EngineTime());
            var actual = behavior.UpdateActionInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

using NUnit.Framework;
using KDScorpionCore;
using ScorpionEngine.Tests.Fakes;

namespace ScorpionEngine.Tests.Behaviors
{
    [TestFixture]
    public class BehaviorTests
    {
        #region Prop Tests
        [Test]
        public void Enabled_WhenSettingToFalse_ReturnsFalse()
        {
            //Arrange
            var behavior = new FakeBehavior(setupAction: false);
            var expected = false;

            //Act
            behavior.Enabled = false;
            var actual = behavior.Enabled;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var behavior = new FakeBehavior(setupAction: false);
            var expected = "John Doe";

            //Act
            behavior.Name = "John Doe";
            var actual = behavior.Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Update_WhenInvokedWhileDisabled_DoesNotInvokeBehaviorAction()
        {
            //Arrange
            var behavior = new FakeBehavior(setupAction: true);
            var expected = false;

            //Act
            behavior.Enabled = false;
            behavior.Update(new EngineTime());
            var actual = behavior.UpdateActionInvoked;
            
            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokedWithNoSetupAction_DoesNotInvokeAction()
        {
            //Arrange
            var behavior = new FakeBehavior(setupAction: false);
            var expected = false;

            //Act
            behavior.Update(new EngineTime());
            var actual = behavior.UpdateActionInvoked;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokedWithSetupActionAndEnabled_InvokesAction()
        {
            //Arrange
            var behavior = new FakeBehavior(setupAction: true);
            var expected = true;

            //Act
            behavior.Update(new EngineTime());
            var actual = behavior.UpdateActionInvoked;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using Xunit;

namespace ParticleMaker.Tests.ViewModels
{
    public class ViewModelTests
    {
        #region Method Tests
        [Fact]
        public void NotifyPropChange_WhenInvoked_InvokesPropertyChangedEvent()
        {
            //Arrange
            var viewModel = new ViewModelFake()
            {
                TestPropA = 1234
            };

            //Act
            var actual = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                actual = true;
            };
            viewModel.TestPropA = 4321;

            //Assert
            Assert.True(actual);
        }


        [Fact]
        public void NotifyAllPropChanges_WhenInvoked_InvokesPropertyChangedEvent()
        {
            //Arrange
            var viewModel = new ViewModelFake()
            {
                TestPropA = 1234,
                TestPropB = 5678
            };

            //Act
            var invokeCount = 0;
            viewModel.PropertyChanged += (sender, e) =>
            {
                invokeCount += 1;
            };

            viewModel.NotifyAllPropChanges(new[] { "TestPropA", "TestPropB" });

            var actual = invokeCount == 2;

            //Assert
            Assert.True(actual);
        }
        #endregion
    }
}

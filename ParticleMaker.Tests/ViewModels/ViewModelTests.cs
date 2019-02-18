﻿using NUnit.Framework;

namespace ParticleMaker.Tests.ViewModels
{
    [TestFixture]
    public class ViewModelTests
    {
        #region Method Tests
        [Test]
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
            Assert.IsTrue(actual);
        }


        [Test]
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
            Assert.IsTrue(actual);
        }
        #endregion
    }
}
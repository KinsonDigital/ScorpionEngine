﻿using NUnit.Framework;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    [TestFixture]
    public class AddSetupClickedEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_ProperlySetsPropertyValue()
        {
            //Arrange
            var expected = "test-item";

            //Act
            var eventArgs = new AddItemClickedEventArgs("test-item");
            var actual = eventArgs.ItemName;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

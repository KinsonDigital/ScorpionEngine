using NUnit.Framework;
using ParticleMaker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class ContentDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingNoParamOverload_SetsCorrectExceptionMsg()
        {
            //Arrange
            var expected = "The content item does not exist in the root content directory.";

            //Act
            var exception = new ContentDoesNotExistException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithParamOverload_SetsCorrectExceptionMsg()
        {
            //Arrange
            var contentItemName = "CONTENT-ITEM";
            var expected = $"The content item '{contentItemName}' does not exist in the root content directory.";

            //Act
            var exception = new ContentDoesNotExistException(contentItemName);
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using NUnit.Framework;
using ParticleMaker.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker.Tests.Dialogs
{
    [TestFixture]
    public class ProjectItemTests
    {
        #region Prop Tests
        [Test]
        public void Name_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var item = new ProjectItem();
            var expected = "test-name";

            //Act
            item.Name = "test-name";
            var actual = item.Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Exists_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var item = new ProjectItem();
            var expected = true;

            //Act
            item.Exists = true;
            var actual = item.Exists;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using NUnit.Framework;
using ParticleMaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class KeyboardServiceTests
    {
        [Test]
        public void IsNumberKey_WhenInvokedWithNumberKey_ReturnsTrue()
        {
            //Arrange
            var key = Key.D3;

            //Act
            //var actual = KeyboardService.IsNumberKey();
        }
    }
}

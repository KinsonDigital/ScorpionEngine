using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class CoreEngineTests
    {
        #region Prop Tests
        [Test]
        public void RenderSurfaceHandle_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new CoreEngine();
            var expected = new IntPtr(1234);

            //Act
            engine.RenderSurfaceHandle = new IntPtr(1234);
            var actual = engine.RenderSurfaceHandle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void WindowPosition_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var engine = new CoreEngine();
            var expected = new Point(11, 22);

            //Act
            engine.WindowPosition = new Point(11, 22);
            var actual = engine.WindowPosition;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

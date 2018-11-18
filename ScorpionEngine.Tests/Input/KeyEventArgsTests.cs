using NUnit.Framework;
using ScorpionEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Tests.Input
{
    [TestFixture]
    public class KeyEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsKeysProp()
        {
            //Arrange
            var expected = new InputKeys[]
            {
                InputKeys.Left,
                InputKeys.Right
            };

            //Act
            var eventArgs = new KeyEventArgs(new InputKeys[] { InputKeys.Left, InputKeys.Right });
            var actual = eventArgs.Keys;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Keys_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new InputKeys[]
            {
                InputKeys.Up,
                InputKeys.Down
            };

            //Act
            var eventArgs = new KeyEventArgs(new InputKeys[] { InputKeys.Left, InputKeys.Right })
            {
                Keys = new InputKeys[] { InputKeys.Up, InputKeys.Down }
            };
            var actual = eventArgs.Keys;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

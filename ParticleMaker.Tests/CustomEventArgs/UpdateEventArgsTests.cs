using Microsoft.Xna.Framework;
using NUnit.Framework;
using ParticleMaker.CustomEventArgs;
using System;

namespace ParticleMaker.CustomEventArgs
{
    [TestFixture]
    public class UpdateEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_ProperlySetsGameTimeProp()
        {
            //Arrange
            var gameTime = new GameTime() { ElapsedGameTime = new TimeSpan(11, 22, 33, 44, 55) };
            var eventArgs = new UpdateEventArgs(gameTime);
            var expected = new TimeSpan(11, 22, 33, 44, 55);

            //Act
            var actual = eventArgs.GameTime.ElapsedGameTime;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using NUnit.Framework;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class RelayCommandTests
    {
        #region Method Tests
        [Test]
        public void CanExecute_WhenInvoking_ExecutesCanAction()
        {
            //Arrange
            var expected = true;
            var command = new RelayCommand(() => { }, () => true);

            //Act
            var actual = command.CanExecute(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Execute_WhenInvoking_ActionIsInvoked()
        {
            //Arrange
            var expected = true;
            var actual = false;
            var command = new RelayCommand(() => { actual = true; }, () => true);

            //Act
            command.Execute(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

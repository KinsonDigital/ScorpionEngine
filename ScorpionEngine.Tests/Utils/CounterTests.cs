using NUnit.Framework;
using ScorpionEngine.Utils;
using System;


namespace ScorpionEngine.Tests.Utils
{
    public class CounterTests
    {
        #region Method Tests
        [Test]
        public void Count_WhenInvoked_IncrementsValue()
        {
            //Arrange
            var counter = new Counter(0, 4, 1);
            var expected = 1;

            //Act
            counter.Count();
            var actual = counter.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenMaxReached_InvokesMaxReachedEvent()
        {
            //Arrange
            var counter = new Counter(0, 2, 1);
            var expected = true;
            var actual = false;
            counter.MaxReachedWhenIncrementing += (obj, e) =>
            {
                actual = true;
            };


            //Act
            counter.Count();
            counter.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenResetTypeSetToAutoAndMaxReached_InvokeReset()
        {
            //Arrange
            var expected = 0;
            var counter = new Counter(0, 2, 1);

            //Act
            counter.Count();
            counter.Count();
            counter.Count();
            var actual = counter.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenResetTypeSetToManualAndMaxReached_DoNotInvokeReset()
        {
            //Arrange
            var expected = 3;
            var counter = new Counter(0, 2, 1)
            {
                ResetMode = ResetType.Manual
            };

            //Act
            counter.Count();
            counter.Count();
            counter.Count();
            var actual = counter.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenDecrementingAndGreaterThanMin_DoNotInvokedMinReachedEvent()
        {
            //Arrange
            var expectedValue = 1;
            var expectedMinReached = false;
            var actualMinReached = false;
            var counter = new Counter(0, 2, 1, 2)
            {
                CountDirection = CountType.Decrement
            };
            counter.MinReachedWhenDecrementing += (sender, e) => actualMinReached = true;

            //Act
            counter.Count();
            var actualValue = counter.Value;

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
            Assert.AreEqual(expectedMinReached, actualMinReached);
        }


        [Test]
        public void Count_WhenInvoked_DecrementsValue()
        {
            //Arrange
            var counter = new Counter(1, 4, 1)
            {
                CountDirection = CountType.Decrement,
                ResetMode = ResetType.Manual
            };
            var expected = -1;

            //Act
            counter.Count();
            var actual = counter.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenMaxReached_InvokesMinReachedEvent()
        {
            //Arrange
            var counter = new Counter(1, 2, 1)
            {
                CountDirection = CountType.Decrement,
                ResetMode = ResetType.Manual
            };
            var expected = true;
            var actual = false;
            counter.MinReachedWhenDecrementing += (obj, e) =>
            {
                actual = true;
            };

            //Act
            counter.Count();
            counter.Count();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenResetTypeSetToAutoAndMinReached_InvokeReset()
        {
            //Arrange
            var expected = 2;
            var counter = new Counter(0, 2, 1)
            {
                CountDirection = CountType.Decrement
            };

            //Act
            counter.Count();
            var actual = counter.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenSettingInvalidCountType_ThrowsException()
        {
            //Arrange
            var counter = new Counter(0, 4, 1)
            {
                CountDirection = (CountType)44
            };

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Count() );
            Assert.AreEqual($"The {nameof(counter.CountDirection)} is set to an invalid enum value.", actual.Message);
        }


        [Test]
        public void Reset_WhenSettingInvalidCountType_ThrowsException()
        {
            //Arrange
            var counter = new Counter(0, 4, 1)
            {
                CountDirection = (CountType)44
            };

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Reset());
            Assert.AreEqual($"The {nameof(counter.CountDirection)} is set to an invalid enum value.", actual.Message);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Min_WhenSettingMinMoreThanMax_ThrowsException()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Min = 5);
            Assert.AreEqual($"The min value of 5 cannot be greater than max value of 4.", actual.Message);
        }


        [Test]
        public void Min_WhenSettingMinLessThanMax_ProperlySetsValue()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);
            var expected = 3;

            //Act
            counter.Min = 3;
            var actual = counter.Min;

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Max_WhenSettingMaxLessThanMin_ThrowsException()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Max = 0);
            Assert.AreEqual($"The max value of 0 cannot be less than min value of 1.", actual.Message);
        }


        [Test]
        public void Max_WhenSettingMaxMoreThanMin_ProperlySetsValue()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);
            var expected = 3;

            //Act
            counter.Max = 3;
            var actual = counter.Max;

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

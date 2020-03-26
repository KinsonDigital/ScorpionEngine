using Xunit;
using KDScorpionEngine;
using KDScorpionEngine.Utils;
using System;


namespace KDScorpionEngineTests.Utils
{
    /// <summary>
    /// Unit tests to test the <see cref="Counter"/> class.
    /// </summary>
    public class CounterTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctr_WhenInvokingWithMinLessThanMax_ThrowsException()
        {
            //Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new Counter(10, 1, 1);
            });
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Count_WhenInvoked_IncrementsValue()
        {
            //Arrange
            var counter = new Counter(0, 4, 1);
            var expected = 1;

            //Act
            counter.Count();
            var actual = counter.Value;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedMinReached, actualMinReached);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Count_WhenSettingInvalidCountType_ThrowsException()
        {
            //Arrange
            var counter = new Counter(0, 4, 1)
            {
                CountDirection = (CountType)44
            };

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Count() );
            Assert.Equal($"The {nameof(counter.CountDirection)} is set to an invalid enum value.", actual.Message);
        }


        [Fact]
        public void Reset_WhenSettingInvalidCountType_ThrowsException()
        {
            //Arrange
            var counter = new Counter(0, 4, 1)
            {
                CountDirection = (CountType)44
            };

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Reset());
            Assert.Equal($"The {nameof(counter.CountDirection)} is set to an invalid enum value.", actual.Message);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Min_WhenSettingMinMoreThanMax_ThrowsException()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Min = 5);
            Assert.Equal($"The min value of 5 cannot be greater than max value of 4.", actual.Message);
        }


        [Fact]
        public void Min_WhenSettingMinLessThanMax_ProperlySetsValue()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);
            var expected = 3;

            //Act
            counter.Min = 3;
            var actual = counter.Min;

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Max_WhenSettingMaxLessThanMin_ThrowsException()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);

            //Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Max = 0);
            Assert.Equal($"The max value of 0 cannot be less than min value of 1.", actual.Message);
        }


        [Fact]
        public void Max_WhenSettingMaxMoreThanMin_ProperlySetsValue()
        {
            //Arrange
            var counter = new Counter(1, 4, 1);
            var expected = 3;

            //Act
            counter.Max = 3;
            var actual = counter.Max;

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

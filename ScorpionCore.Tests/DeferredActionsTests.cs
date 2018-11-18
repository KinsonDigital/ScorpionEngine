using NUnit.Framework;
using System;

namespace ScorpionCore.Tests
{
    [TestFixture]
    public class DeferredActionsTests
    {
        #region Method Tests
        [Test]
        public void Add_WhenInvoking_AddsItem()
        {
            //Arrange
            var actions = new DeferredActions();
            void testAction() { }
            actions.Add(testAction);
            var expected = 1;

            //Act
            var actual = actions.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Clear_WhenInvoking_RemovesAllItems()
        {
            //Arrange
            var actions = new DeferredActions();
            void testAction() { }
            actions.Add(testAction);
            var expected = 0;

            //Act
            actions.Clear();
            var actual = actions.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Contains_WhenInvoking_ReturnsTrue()
        {
            //Arrange
            var actions = new DeferredActions();
            void testAction() { }
            actions.Add(testAction);
            var expected = true;

            //Act
            var actual = actions.Contains(testAction);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CopyTo_WhenInvoking_CorrectlyCopysToArray()
        {
            //Arrange
            var deferredActions = new DeferredActions();
            void testActionA() { }
            void testActionB() { }
            deferredActions.Add(testActionA);
            deferredActions.Add(testActionB);

            var destinationArray = new Action[2];
            var expected = new Action[]
            {
                testActionA,
                testActionB,
            };

            //Act
            deferredActions.CopyTo(destinationArray, 0);
            var actual = deferredActions;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IndexOf_WhenInvoking_ReturnsCorrectIndex()
        {
            //Arrange
            var actions = new DeferredActions();
            void testActionA() { }
            void testActionB() { }
            actions.Add(testActionA);
            actions.Add(testActionB);
            var expected = 1;

            //Act
            var actual = actions.IndexOf(testActionB);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Insert_WhenInvoking_CorrectlyInsertsItem()
        {
            //Arrange
            var actions = new DeferredActions();
            void testActionA() { }
            void testActionB() { }
            void testActionC() { }
            actions.Add(testActionA);
            actions.Add(testActionC);

            var expected = (Action)testActionB;

            //Act
            actions.Insert(1, testActionB);
            var actual = actions[1];

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Remove_WhenInvoking_CorrectlyRemovesItem()
        {
            //Arrange
            var actions = new DeferredActions();
            void testActionA() { }
            void testActionB() { }
            void testActionC() { }
            actions.Add(testActionA);
            actions.Add(testActionB);
            actions.Add(testActionC);

            var expected = 2;

            //Act
            actions.Remove(testActionB);
            var actual = actions.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RemoveAt_WhenInvoking_CorrectlyRemovesItem()
        {
            //Arrange
            var actions = new DeferredActions();
            void testActionA() { }
            void testActionB() { }
            void testActionC() { }
            actions.Add(testActionA);
            actions.Add(testActionB);
            actions.Add(testActionC);

            var expected = 2;

            //Act
            actions.RemoveAt(1);
            var actual = actions.Count;

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual((Action)testActionA, actions[0]);
            Assert.AreEqual((Action)testActionC, actions[1]);
        }


        [Test]
        public void ExecuteAll_WhenInvoking_ExecutesAllActions()
        {
            //Arrange
            var actions = new DeferredActions();
            var expectedCount = 0;
            var expectedActionAExecuted = true;
            var expectedActionBExecuted = true;
            var actualActionAExecuted = false;
            var actualActionBExecuted = false;
            actions.Add(testActionA);
            actions.Add(testActionB);
            void testActionA() { actualActionAExecuted = true; }
            void testActionB() { actualActionBExecuted = true; }


            //Act
            actions.ExecuteAll();
            var actualCount = actions.Count;
            
            //Assert
            Assert.AreEqual(expectedActionAExecuted, actualActionAExecuted);
            Assert.AreEqual(expectedActionBExecuted, actualActionBExecuted);
            Assert.AreEqual(expectedCount, actualCount);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Count_WhenGettingValue_ReturnsCorrectResult()
        {
            //Arrange
            var actions = new DeferredActions();
            void testAction() { }
            var expected = 1;

            //Act
            actions.Add(testAction);
            var actual = actions.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsReadOnly_WhenGettingValue_ReturnsCorrectResult()
        {
            //Arrange
            var actions = new DeferredActions();
            var expected = false;

            //Act
            var actual = actions.IsReadOnly;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IndexProp_WhenGettingAndSettingValue_ReturnsCorrectAction()
        {
            //Arrange
            var actions = new DeferredActions();
            void actionA() { }
            void actionB() { }
            void actionC() { }
            actions.Add(actionA);
            actions.Add(actionB);

            var expected = (Action)actionC;

            //Act
            actions[0] = actionC;
            var actual = actions[0];

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

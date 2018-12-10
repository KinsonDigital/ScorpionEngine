using NUnit.Framework;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Tests.Fakes;

namespace KDScorpionEngine.Tests.Behaviors
{
    [TestFixture]
    public class EntityBehaviorTests
    {
        #region Prop Tests
        [Test]
        public void Count_WhenGettingValueWithItems_ReturnsCorrectCount()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var expected = 4;

            //Act
            for (int i = 0; i < 4; i++)
            {
                behaviors.Add(new FakeBehavior(setupAction: false));
            }
            var actual = behaviors.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsReadOnly_WhenGettingValue_ReturnsFalse()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var expected = false;

            //Act
            var actual = behaviors.IsReadOnly;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetIndexItem_WhenGettingValue_ReturnsFalse()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);
            var expectedCount = 1;
            var expectedContains = true;

            //Act
            behaviors.Add(new FakeBehavior(setupAction: true));
            behaviors[0] = behavior;
            var actualCount = behaviors.Count;
            var actualContains = behaviors.Contains(behavior);

            //Assert
            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedContains, actualContains);
        }


        [Test]
        public void GetItemByIndex_WhenGettingValue_ReturnsCorrectItem()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);

            //Act
            behaviors.Add(new FakeBehavior(setupAction: true));
            behaviors.Add(behavior);
            var actual = behaviors[1];

            //Assert
            Assert.AreEqual(behavior, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Clear_WhenInvokingWithItems_RemovesAllItems()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var expected = 0;

            //Act
            for (int i = 0; i < 4; i++)
            {
                behaviors.Add(new FakeBehavior(setupAction: false));
            }
            behaviors.Clear();
            var actual = behaviors.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Contains_WhenInvokingWithContainedItem_ReturnsTrue()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);
            var expected = true;

            //Act
            behaviors.Add(behavior);
            var actual = behaviors.Contains(behavior);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IndexOf_WhenInvoking_ReturnsCorrectIndex()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);
            var expected = 2;

            //Act
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(behavior);
            behaviors.Add(new FakeBehavior(setupAction: false));

            var actual = behaviors.IndexOf(behavior);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Insert_WhenInvoking_ProperlyInsertsItem()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);
            var expected = 1;

            //Act
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Insert(1, behavior);

            var actual = behaviors.IndexOf(behavior);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Remove_WhenInvoking_RemovesCorrectItem()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);
            var expected = false;

            //Act
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(behavior);
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Remove(behavior);

            var actual = behaviors.Contains(behavior);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RemoveAt_WhenInvoking_RemovesCorrectItem()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);
            var expected = false;

            //Act
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(behavior);
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.Add(new FakeBehavior(setupAction: false));
            behaviors.RemoveAt(1);

            var actual = behaviors.Contains(behavior);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CopyTo_WhenInvoking_CorrectlyCopiesItem()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior1 = new FakeBehavior(setupAction: false);
            var behavior2 = new FakeBehavior(setupAction: false);
            var expected = new FakeBehavior[]
            {
                behavior1,
                behavior2
            };

            //Act
            behaviors.Add(behavior1);
            behaviors.Add(behavior2);

            var actual = new FakeBehavior[2];
            behaviors.CopyTo(actual, 0);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetEnumerator_WhenInvoking_DoesNotReturnNull()
        {
            //Arrange
            var behaviors = new EntityBehaviors();
            var behavior = new FakeBehavior(setupAction: false);

            //Act
            behaviors.Add(behavior);

            var actual = behaviors.GetEnumerator();

            //Assert
            Assert.NotNull(actual);
        }
        #endregion
    }
}

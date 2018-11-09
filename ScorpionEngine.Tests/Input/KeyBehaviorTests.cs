using Moq;
using NUnit.Framework;
using ScorpionCore.Plugins;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Input;


namespace ScorpionEngine.Tests.Input
{
    public class KeyBehaviorTests
    {
        [Test]
        public void Ctor_SinglePropValue_SetsPropsCorrectly()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var expected = true;
            var keyboard = new Keyboard(mockCoreKeyboard.Object);

            //Act
            var behavior = new KeyBehavior(true, keyboard);

            Assert.AreEqual(expected, behavior.Enabled);
        }


        [Test]
        public void Ctor_TwoPropValues_SetsPropsCorrectly()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var expectedEnabled = true;
            var expectedKey = InputKeys.Right;
            var keyboard = new Keyboard(mockCoreKeyboard.Object);

            //Act
            var behavior = new KeyBehavior(InputKeys.Right, true, keyboard);

            Assert.AreEqual(expectedEnabled, behavior.Enabled);
            Assert.AreEqual(expectedKey, behavior.Key);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsKeyDownContinous_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            var expected = InputKeys.Space;
            InputKeys actual = InputKeys.Right;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actual = e.Keys[0];
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsOnceOnPress_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyPressed((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnPress
            };

            var expected = InputKeys.Space;
            InputKeys actual = InputKeys.Right;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actual = e.Keys[0];
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

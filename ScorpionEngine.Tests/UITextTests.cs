using Moq;
using ScorpionCore;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;
using ScorpionEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class UITextTests
    {
        #region Prop Tests
        [Fact]
        public void LabelText_WhenGettingTextValue_ValueIsCorrect()
        {
            //Arrange
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Text).Returns("Original Label Text");

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var uiText = new UIText()
            {
                LabelText = labelText
            };
            var expected = "Original Label Text";

            //Act
            var actual = uiText.LabelText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ValueText_WhenGettingTextValue_ValueIsCorrect()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            mockValueText.SetupGet(m => m.Text).Returns("Original Value Text");

            var valueText = new GameText() { InternalText = mockValueText.Object };
            var uiText = new UIText()
            {
                ValueText = valueText
            };
            var expected = "Original Value Text";

            //Act
            var actual = uiText.ValueText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Width_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Width).Returns(10);
            mockValueText.SetupGet(m => m.Width).Returns(20);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText()
            {
                LabelText = labelText,
                ValueText = valueText,
                SectionSpacing = 5
            };

            var expected = 35;

            //Act
            var actual = uiText.Width;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Height_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Height).Returns(10);
            mockValueText.SetupGet(m => m.Height).Returns(20);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText()
            {
                LabelText = labelText,
                ValueText = valueText
            };

            var expected = 20;

            //Act
            var actual = uiText.Height;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Right_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Width).Returns(10);
            mockValueText.SetupGet(m => m.Width).Returns(20);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText(30, 45)
            {
                LabelText = labelText,
                ValueText = valueText,
                SectionSpacing = 5
            };

            var expected = 65;

            //Act
            var actual = uiText.Right;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Bottom_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Height).Returns(10);
            mockValueText.SetupGet(m => m.Height).Returns(20);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText(30, 10)
            {
                LabelText = labelText,
                ValueText = valueText
            };

            var expected = 30;

            //Act
            var actual = uiText.Bottom;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Ctor_WhenUsingPositionParam_SetsPositionProp()
        {
            //Arrange
            var uiText = new UIText(new Vector(11, 22));
            var expected = new Vector(11, 22);

            //Act
            var actual = uiText.Position;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenUsingXAndYParams_SetsPositionProp()
        {
            //Arrange
            var uiText = new UIText(11, 22);
            var expectedX = 11;
            var expectedY = 22;

            //Act
            var actualX = uiText.Position.X;
            var actualY = uiText.Position.Y;

            //Assert
            Assert.Equal(expectedX, actualX);
            Assert.Equal(expectedY, actualY);
        }


        [Fact]
        public void SetLabelText_WhenSettingValue_UpdateTextIfTimeElapsed()
        {
            //Arrange
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupProperty(m => m.Text);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var uiText = new UIText()
            {
                //Do not ignore the update frequency or the text will be updated
                //even if the elapsed time has not been elapsed.
                IgnoreUpdateFrequency = false,
                UpdateFrequency = 0
            };
            uiText.LabelText = labelText;
            var expected = "Change Attempted";

            //Act
            uiText.SetLabelText("Change Attempted");
            var actual = uiText.LabelText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SetValueText_WhenSettingValue_UpdateTextIfTimeElapsed()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            mockValueText.SetupProperty(m => m.Text);

            var valueText = new GameText() { InternalText = mockValueText.Object };
            var uiText = new UIText()
            {
                //Do not ignore the update frequency or the text will be updated
                //even if the elapsed time has not been elapsed.
                IgnoreUpdateFrequency = false,
                UpdateFrequency = 0
            };
            uiText.ValueText = valueText;
            var expected = "Change Attempted";

            //Act
            uiText.SetValueText("Change Attempted");
            var actual = uiText.ValueText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvoking_UpdatesValueText()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            mockValueText.SetupProperty(m => m.Text);

            var mockEngineTime = new Mock<IEngineTiming>();
            mockEngineTime.SetupGet(m => m.ElapsedEngineTime).Returns(new TimeSpan(0, 0, 0, 0, 100));

            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText()
            {
                ValueText = valueText
            };

            var expected = "Text Updated";

            //Act
            uiText.Update(mockEngineTime.Object);
            uiText.SetValueText("Text Updated");
            var actual = uiText.ValueText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

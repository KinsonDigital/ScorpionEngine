using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;
using ScorpionEngine.UI;
using System;


namespace ScorpionEngine.Tests.UI
{
    public class UITextTests
    {
        #region Prop Tests
        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Height_WhenGettingValueWhileValueTextIsTallest_ValueIsCorrect()
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Height_WhenGettingValueWhileLabelTextIsTallest_ValueIsCorrect()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Height).Returns(30);
            mockValueText.SetupGet(m => m.Height).Returns(15);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText()
            {
                LabelText = labelText,
                ValueText = valueText
            };

            var expected = 30;

            //Act
            var actual = uiText.Height;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TextItemSize_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Width).Returns(7);
            mockLabelText.SetupGet(m => m.Height).Returns(44);

            var mockValueText = new Mock<IText>();
            mockValueText.SetupGet(m => m.Width).Returns(9);
            mockValueText.SetupGet(m => m.Height).Returns(23);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText()
            {
                LabelText = labelText,
                ValueText = valueText,
                SectionSpacing = 5
            };

            var expected = new Vector(21, 44);

            //Act
            var actual = uiText.TextItemSize;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DisabledForecolor_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = new GameColor(11, 22, 33, 44);

            //Act
            uiText.DisabledForecolor = new GameColor(11, 22, 33, 44);
            var actual = uiText.DisabledForecolor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Enabled_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = false;

            //Act
            uiText.Enabled = false;
            var actual = uiText.Enabled;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SelectedColor_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = new GameColor(255, 255, 0, 255);

            //Act
            uiText.SelectedColor = new GameColor(255, 255, 0, 255);
            var actual = uiText.SelectedColor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LabelColor_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = new GameColor(0, 0, 0, 255);

            //Act
            uiText.LabelColor = new GameColor(0, 0, 0, 255);
            var actual = uiText.LabelColor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ValueColor_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = new GameColor(0, 0, 0, 255);

            //Act
            uiText.ValueColor = new GameColor(0, 0, 0, 255);
            var actual = uiText.ValueColor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Name_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = "Kinson";

            //Act
            uiText.Name = "Kinson";
            var actual = uiText.Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Selected_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var uiText = new UIText();
            var expected = true;

            //Act
            uiText.Selected = true;
            var actual = uiText.Selected;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Ctor_WhenUsingPositionParam_SetsPositionProp()
        {
            //Arrange
            var uiText = new UIText(new Vector(11, 22));
            var expected = new Vector(11, 22);

            //Act
            var actual = uiText.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expectedX, actualX);
            Assert.AreEqual(expectedY, actualY);
        }


        [Test]
        public void SetLabelText_WhenSettingValueWhileNotIgnoringUpdateFreq_UpdateTextIfUpdateFreqPassed()
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetLabelText_WhenSettingValueWhileIgnoringUpdateFreq_UpdateTextIfTimeElapsed()
        {
            //Arrange
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupProperty(m => m.Text);

            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var uiText = new UIText()
            {
                //Do not ignore the update frequency or the text will be updated
                //even if the elapsed time has not been elapsed.
                IgnoreUpdateFrequency = true,
                UpdateFrequency = 100
            };
            uiText.LabelText = labelText;
            var expected = "Change Attempted";

            //Act
            uiText.SetLabelText("Change Attempted");
            var actual = uiText.LabelText.Text;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetValueText_WhenSettingValueWhileNotIgnoringUpdateFreq_UpdateTextIfUpdateFreqPassed()
        {
            //Arrange
            var mockValueText = new Mock<IText>();
            mockValueText.SetupProperty(m => m.Text);

            var valueText = new GameText() { InternalText = mockValueText.Object };
            var uiText = new UIText()
            {
                //Do not ignore the update frequency or the text will be updated
                //even if the elapsed time has not been elapsed.
                IgnoreUpdateFrequency = true,
                UpdateFrequency = 100
            };
            uiText.ValueText = valueText;
            var expected = "Change Attempted";

            //Act
            uiText.SetValueText("Change Attempted");
            var actual = uiText.ValueText.Text;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Render_WhenInvoking_InternalRenderGetsProperParamValues()
        {
            //Arrange
            var mockInternalRenderer = new Mock<IRenderer>();
            var mockLabelText = new Mock<IText>();
            mockLabelText.SetupGet(m => m.Width).Returns(3);

            var mockValueText = new Mock<IText>();
            mockValueText.SetupGet(m => m.Width).Returns(3);

            var renderer = new Renderer(mockInternalRenderer.Object);
            var labelText = new GameText() { InternalText = mockLabelText.Object };
            var valueText = new GameText() { InternalText = mockValueText.Object };

            var uiText = new UIText()
            {
                Position = new Vector(3, 7),
                SectionSpacing = 4,
                VerticalValueOffset = 13,
                VerticalLabelOffset = 9,
                LabelText = labelText,
                ValueText = valueText,
            };

            //Act
            uiText.Render(renderer);

            //Assert
            //Verify that each method is being executed as well as the data being entered into them
            mockInternalRenderer.Verify(m => m.Render(uiText.LabelText.InternalText, 3, 16), Times.Once());
            mockInternalRenderer.Verify(m => m.Render(uiText.ValueText.InternalText, 10, 20), Times.Once());
        }
        #endregion
    }
}

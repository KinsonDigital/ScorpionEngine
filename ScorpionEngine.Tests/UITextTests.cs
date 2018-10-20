using Moq;
using ScorpionCore;
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
        #region Props
        [Fact]
        public void LabelText_WhenSettingValue_UpdateTextIfTimeElapsed()
        {
            //Arrange
            var mockLabelFont = new Mock<IText>();
            var mockValueFont = new Mock<IText>();
            var mockEngineTime = new Mock<IEngineTiming>();
            mockEngineTime.Setup(m => m.ElapsedEngineTime).Returns(new TimeSpan(0, 0, 0, 0, 70));

            var uiText = new UIText(mockLabelFont.Object, mockValueFont.Object, "Original Text")
            {
                //Do not ignore the update frequency or the text will be updated
                //even if the elapsed time has not been elapsed.
                IgnoreUpdateFrequency = false
            };
            var expected = "Change Attempted";

            //Act
            uiText.Update(mockEngineTime.Object);
            uiText.LabelText = "Change Attempted";
            var actual = uiText.LabelText;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ValueText_WhenSettingValue_UpdateTextIfTimeElapsed()
        {
            //Arrange
            var mockLabelFont = new Mock<IText>();
            var mockValueFont = new Mock<IText>();
            var mockEngineTime = new Mock<IEngineTiming>();
            mockEngineTime.Setup(m => m.ElapsedEngineTime).Returns(new TimeSpan(0, 0, 0, 0, 70));

            var uiText = new UIText(mockLabelFont.Object, mockValueFont.Object, "", "Original Text")
            {
                //Do not ignore the update frequency or the text will be updated
                //even if the elapsed time has not been elapsed.
                IgnoreUpdateFrequency = false
            };
            var expected = "Change Attempted";

            //Act
            uiText.Update(mockEngineTime.Object);
            uiText.ValueText = "Change Attempted";
            var actual = uiText.ValueText;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

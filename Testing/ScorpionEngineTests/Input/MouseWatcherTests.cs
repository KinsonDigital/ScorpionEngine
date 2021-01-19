// <copyright file="MouseWatcherTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Input
{
    using KDScorpionEngine.Input;

    //TODO: DO NOT DELETE THIS!! This will be reworked once the new keyboard implementation for the Raptor keyboard are finished
    // Refer to cards below for more info
    // 1. https://dev.azure.com/KinsonDigital/GameDevTools/_workitems/edit/2424
    // 2. https://dev.azure.com/KinsonDigital/GameDevTools/_workitems/edit/2425


    /// <summary>
    /// Unit tests to test the <see cref="MouseWatcher"/> class.
    /// </summary>
    public class MouseWatcherTests //: IDisposable
    {
        //private Mock<IMouse> mockMouse;

        //public MouseWatcherTests()
        //{
        //    this.mockMouse = new Mock<IMouse>();
        //    this.mockMouse.Setup(m => m.IsButtonPressed(InputButton.LeftButton)).Returns(true);
        //}

        //#region Constructor Tests
        //[Fact]
        //public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object);

        //    var expectedComboButtons = new List<InputButton>();
        //    var expectedEnabled = true;

        //    // Act
        //    var actualEnabled = mouseWatcher.Enabled;
        //    var actualComboButtons = mouseWatcher.ComboButtons;

        //    // Assert
        //    Assert.Equal(expectedEnabled, actualEnabled);
        //    Assert.Equal(expectedComboButtons, actualComboButtons);
        //}
        //#endregion

        //#region Prop Tests
        //[Fact]
        //public void Button_WhenGettingAndSettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //    };
        //    var expected = InputButton.LeftButton;

        //    // Act
        //    var actual = mouseWatcher.Button;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //        HitCountMax = 10,
        //    };

        //    // This value is meaningless and is only for satisfy the update param
        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 0) };

        //    var expected = 60;

        //    // Act
        //    for (var i = 0; i < 6; i++)
        //    {
        //        mouseWatcher.Update(gameTime);
        //    }

        //    var actual = mouseWatcher.CurrentHitCountPercentage;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void DownElapsedResetMode_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        DownElapsedResetMode = ResetType.Manual,
        //    };

        //    var expected = ResetType.Manual;

        //    // Act
        //    var actual = mouseWatcher.DownElapsedResetMode;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void HitCountResetMode_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        HitCountResetMode = ResetType.Manual,
        //    };

        //    var expected = ResetType.Manual;

        //    // Act
        //    var actual = mouseWatcher.HitCountResetMode;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void ComboButtons_WhenSettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        ComboButtons = new List<InputButton>()
        //        {
        //            InputButton.LeftButton,
        //            InputButton.RightButton,
        //        },
        //    };

        //    var expected = new List<InputButton>()
        //    {
        //        InputButton.LeftButton,
        //        InputButton.RightButton,
        //    };

        //    // Act
        //    var actual = mouseWatcher.ComboButtons;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object);

        //    var expected = 16;

        //    // Act
        //    mouseWatcher.Update(new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 16) });
        //    var actual = mouseWatcher.InputDownElapsedMS;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        InputReleasedTimeout = 2000,
        //    };

        //    var expected = 1.234f;

        //    // Act
        //    mouseWatcher.Update(new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 1234) });
        //    var actual = mouseWatcher.InputDownElapsedSeconds;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object);

        //    var expected = 31;

        //    // Act
        //    mouseWatcher.Update(new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 31) });
        //    var actual = mouseWatcher.InputReleasedElapsedMS;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.MiddleButton)).Returns(false);

        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.MiddleButton,
        //        HitCountMax = 0,
        //        InputReleasedTimeout = 5000,
        //    };
        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 4321) };
        //    var expected = 4.321;

        //    // Act/Assert
        //    mouseWatcher.Update(gameTime); // Run once to get the internal states.
        //    var actual = Math.Round(mouseWatcher.InputReleasedElapsedSeconds, 3);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        InputDownTimeOut = 123,
        //    };

        //    var expected = 123;

        //    // Act
        //    var actual = mouseWatcher.InputDownTimeOut;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        InputReleasedTimeout = 456,
        //    };

        //    var expected = 456;

        //    // Act
        //    var actual = mouseWatcher.InputReleasedTimeout;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void ReleasedElapsedResetMode_WhenSettingAndGettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        ReleasedElapsedResetMode = ResetType.Manual,
        //    };

        //    var expected = ResetType.Manual;

        //    // Act
        //    var actual = mouseWatcher.ReleasedElapsedResetMode;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}
        //#endregion

        //#region Method Tests
        //[Fact]
        //public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //        HitCountMax = 0,
        //    };

        //    // Act/Assert
        //    AssertExt.DoesNotThrowNullReference(() =>
        //    {
        //        mouseWatcher.Update(new GameTime()());
        //    });
        //}

        //[Fact]
        //public void Update_WhenInvokingWhileCountingWithNullCounter_RunsWithNoNullReferenceExceptions()
        //{
        //    // Arrange
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //        HitCountMax = 0,
        //    };

        //    mouseWatcher.SetFieldNull("counter");

        //    // Act/Assert
        //    AssertExt.DoesNotThrowNullReference(() =>
        //    {
        //        mouseWatcher.Update(new GameTime()());
        //    });
        //}

        //[Fact]
        //public void Update_WhenInvokingWithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);

        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //        HitCountMax = 0,
        //    };
        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 10) };
        //    var expectedDownTimerElapsed = 0;
        //    var expectedUpTimerElapsed = 10;

        //    // Act/Assert
        //    mouseWatcher.Update(gameTime); // Run once to get the internal states.
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(false); // Return false to simulate that the button is not down
        //    mouseWatcher.Update(gameTime); // Run update again to set the current and previous states different
        //    mouseWatcher.Update(gameTime);
        //    var actualDownTimerElapsed = mouseWatcher.InputDownElapsedMS;
        //    var actualUpTimerElapsed = mouseWatcher.InputReleasedElapsedMS;

        //    // Assert
        //    Assert.Equal(expectedDownTimerElapsed, actualDownTimerElapsed);
        //    Assert.Equal(expectedUpTimerElapsed, actualUpTimerElapsed);
        //}

        //[Fact]
        //public void Update_WhenDisabled_DoNotUpdateAnything()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);
        //    this.mockMouse.Setup(m => m.IsButtonUp(InputButton.LeftButton)).Returns(true);
        //    this.mockMouse.Setup(m => m.IsButtonPressed(InputButton.LeftButton)).Returns(true);

        //    var expectedCurrentHitCount = 0;
        //    var expectedInputHitCountReachedEventInvoked = false;
        //    var expectedInputDownTimeOutEventInvoked = false;
        //    var expectedInputReleasedTimeOutEventInvoked = false;
        //    var expectedInputComboPressedEventInvoked = false;

        //    var actualInputHitCountReachedEventInvoked = false;
        //    var actualInputDownTimeOutEventInvoked = false;
        //    var actualInputReleasedTimeOutEventInvoked = false;
        //    var actualInputComboPressedEventInvoked = false;

        //    var mouseWatcher = new MouseWatcher(false, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //        HitCountMax = 1,
        //    };

        //    // These events should never be fired.
        //    mouseWatcher.OnInputHitCountReached += (sender, e) => actualInputHitCountReachedEventInvoked = true;
        //    mouseWatcher.OnInputDownTimeOut += (sender, e) => actualInputDownTimeOutEventInvoked = true;
        //    mouseWatcher.OnInputReleasedTimeOut += (sender, e) => actualInputReleasedTimeOutEventInvoked = true;
        //    mouseWatcher.OnInputComboPressed += (sender, e) => actualInputComboPressedEventInvoked = true;

        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 2000) };

        //    // Act
        //    mouseWatcher.Update(gameTime);
        //    mouseWatcher.Update(gameTime);
        //    var actualCurrentHitCount = mouseWatcher.CurrentHitCount;

        //    // Assert
        //    Assert.Equal(expectedCurrentHitCount, actualCurrentHitCount);
        //    Assert.Equal(expectedInputHitCountReachedEventInvoked, actualInputHitCountReachedEventInvoked);
        //    Assert.Equal(expectedInputDownTimeOutEventInvoked, actualInputDownTimeOutEventInvoked);
        //    Assert.Equal(expectedInputReleasedTimeOutEventInvoked, actualInputReleasedTimeOutEventInvoked);
        //    Assert.Equal(expectedInputComboPressedEventInvoked, actualInputComboPressedEventInvoked);
        //}

        //[Fact]
        //public void Update_WhenUpdating_InvokesHitCountReachedEvent()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonPressed(InputButton.LeftButton)).Returns(true);

        //    var expected = true;
        //    var actual = false;
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //        HitCountMax = 2,
        //    };
        //    mouseWatcher.OnInputHitCountReached += (sender, e) => actual = true;

        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 501) };

        //    // Act
        //    mouseWatcher.Update(gameTime);
        //    mouseWatcher.Update(gameTime);
        //    mouseWatcher.Update(gameTime);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void Update_WhenUpdating_InvokesInputDownTimeoutEvent()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);

        //    var expected = true;
        //    var actual = false;
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //    };
        //    mouseWatcher.OnInputDownTimeOut += (sender, e) => actual = true;

        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 501) };

        //    // Act
        //    mouseWatcher.Update(gameTime);
        //    mouseWatcher.Update(gameTime);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void Update_WhenUpdatingWithNullInputDownTimeoutEvent_ShouldNotThrowNullRefException()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);

        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.LeftButton,
        //    };

        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 501) };

        //    // Act/Assert
        //    AssertExt.DoesNotThrow<NullReferenceException>(() =>
        //    {
        //        mouseWatcher.Update(gameTime);
        //        mouseWatcher.Update(gameTime);
        //    });
        //}

        //[Fact]
        //public void Update_WhenUpdating_InvokesInputReleaseTimeoutEvent()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonUp(InputButton.RightButton)).Returns(true);

        //    var expected = true;
        //    var actual = false;
        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.RightButton,
        //    };
        //    mouseWatcher.OnInputReleasedTimeOut += (sender, e) => actual = true;

        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 501) };

        //    // Act
        //    mouseWatcher.Update(gameTime);
        //    mouseWatcher.Update(gameTime);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void Update_WhenUpdatingWithNullInputReleasedTimeoutEvent_ShouldNotThrowNullRefException()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonUp(InputButton.RightButton)).Returns(true);

        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        Button = InputButton.RightButton,
        //    };

        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 501) };

        //    // Act/Assert
        //    AssertExt.DoesNotThrow<NullReferenceException>(() =>
        //    {
        //        mouseWatcher.Update(gameTime);
        //        mouseWatcher.Update(gameTime);
        //    });
        //}

        //[Fact]
        //public void Update_WhenInvokingWithSetComboButtons_InvokesOnInputComboPressedEvent()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.RightButton)).Returns(true);

        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        ComboButtons = new List<InputButton>()
        //        {
        //            InputButton.LeftButton,
        //            InputButton.RightButton,
        //        },
        //    };
        //    var expected = true;
        //    var actual = false;

        //    mouseWatcher.OnInputComboPressed += new EventHandler((sender, e) => actual = true);

        //    // Act
        //    mouseWatcher.Update(new GameTime()());

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void Update_WhenInvokingWithNullOnInputComboPressedEvent_DoesNotThrowNullException()
        //{
        //    // Arrange
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);
        //    this.mockMouse.Setup(m => m.IsButtonDown(InputButton.RightButton)).Returns(true);

        //    var mouseWatcher = new MouseWatcher(true, this.mockMouse.Object)
        //    {
        //        ComboButtons = new List<InputButton>()
        //        {
        //            InputButton.LeftButton,
        //            InputButton.RightButton,
        //        },
        //    };
        //    var gameTime = new GameTime()() { ElapsedGameTime() = new TimeSpan(0, 0, 0, 0, 16) };

        //    // Act & Assert
        //    AssertExt.DoesNotThrowNullReference(() => mouseWatcher.Update(gameTime));
        //}
        //#endregion

        ///// <inheritdoc/>
        //public void Dispose() => this.mockMouse = null;
    }
}

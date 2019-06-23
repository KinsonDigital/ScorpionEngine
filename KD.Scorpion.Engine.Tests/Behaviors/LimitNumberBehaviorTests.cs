namespace KDScorpionEngineTests.Behaviors
{
    public class LimitNumberBehaviorTests
    {
        //#region Contructor Tests
        //[Fact]
        //public void Ctor_WhenInvoking_CorrectlySetsLimitProp()
        //{
        //    //Arrange
        //    var behavior = new LimitNumberBehavior(() => 123, (value) => { }, 112233f);
        //    var expected = 112233f;

        //    //Act
        //    var actual = behavior.LimitValue;

        //    //Assert
        //    Assert.Equal(expected, actual);
        //}


        //[Fact]
        //public void Ctor_WhenInvoking_CorrectlySetsNameProp()
        //{
        //    //Arrange
        //    var behavior = new LimitNumberBehavior(() => 123, (value) => { }, 112233f);
        //    var expected = "LimitNumberBehavior";

        //    //Act
        //    var actual = behavior.Name;

        //    //Assert
        //    Assert.Equal(expected, actual);
        //}
        //#endregion


        //#region Method Tests
        //[Fact]
        //public void UpdateAction_WhenInvokingWithCurrentValueGreatorThanLimit_InvokesSetLimitAction()
        //{
        //    //Arrange
        //    var actual = 0f;
        //    void setLimit(float limitValue) { actual = limitValue; }
        //    float getValue() => 2;
        //    var behavior = new LimitNumberBehavior(getValue, setLimit, 1f);
        //    var expected = 1;

        //    //Act
        //    behavior.UpdateAction(new EngineTime());

        //    //Assert
        //    Assert.Equal(expected, actual);
        //}


        //[Fact]
        //public void UpdateAction_WhenInvokingWithCurrentValueLessThanLimit_InvokesSetLimitAction()
        //{
        //    //Arrange
        //    var actual = 0f;
        //    void setLimit(float limitValue) { actual = limitValue; }
        //    float getValue() => -2;
        //    var behavior = new LimitNumberBehavior(getValue, setLimit, -1f);
        //    var expected = -1;

        //    //Act
        //    behavior.UpdateAction(new EngineTime());

        //    //Assert
        //    Assert.Equal(expected, actual);
        //}


        //[Fact]
        //public void UpdateAction_WhenInvokingWithCurrentValueLessThanLimit_DoesNotInvokesSetLimitAction()
        //{
        //    //Arrange
        //    var actual = 0f;
        //    void setLimit(float limitValue) { actual = limitValue; }
        //    float getValue() => 0;
        //    var behavior = new LimitNumberBehavior(getValue, setLimit, 0f);
        //    var expected = 0;

        //    //Act
        //    behavior.UpdateAction(new EngineTime());

        //    //Assert
        //    Assert.Equal(expected, actual);
        //}
        //#endregion
    }
}

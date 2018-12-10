﻿using Moq;
using NUnit.Framework;
using KDScorpionCore.Plugins;

namespace KDScorpionCore.Tests
{
    [TestFixture]
    public class OnRenderEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_SetsRendererProp()
        {
            //Arrange
            var mockRenderer = new Mock<IRenderer>();
            var expected = mockRenderer.Object;
            var eventArgs = new OnRenderEventArgs(mockRenderer.Object);

            //Act
            var actual = eventArgs.Renderer;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Renderer_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockRendererForCtor = new Mock<IRenderer>();
            var eventArgs = new OnRenderEventArgs(mockRendererForCtor.Object);
            var mockRendererForProp = new Mock<IRenderer>();
            var expected = mockRendererForProp.Object;

            //Act
            eventArgs.Renderer = mockRendererForProp.Object;
            var actual = eventArgs.Renderer;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

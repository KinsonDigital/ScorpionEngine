// <copyright file="GameSceneTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Scene
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor.Content;
    using Raptor.Graphics;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="GameScene"/> class.
    /// </summary>
    public class GameSceneTests
    {
        private readonly Mock<ITexture> mockTexture;

        public GameSceneTests()
        {
            this.mockTexture = new Mock<ITexture>();
        }

        #region Prop Tests
        [Fact]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var scene = new FakeGameScene();
            var expected = "John Doe";

            // Act
            scene.Name = "John Doe";
            var actual = scene.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ContentLoaded_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var scene = new FakeGameScene();
            var expected = true;

            // Act
            scene.ContentLoaded = true;
            var actual = scene.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeManager_WhenGettingValue_NotNull()
        {
            // Arrange
            var scene = new FakeGameScene();

            // Act
            var actual = scene.TimeManager;

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void Initialized_WhenGettingValueAfterInitialized_ReturnsTrue()
        {
            // Arrange
            var scene = new FakeGameScene();
            var expected = true;

            // Act
            scene.Initialize();
            var actual = scene.Initialized;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Active_WhenGettingAndSettingValue_ReturnsTrue()
        {
            // Arrange
            var scene = new FakeGameScene();
            var expected = true;

            // Act
            scene.Active = true;
            var actual = scene.Active;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsRenderingScene_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var scene = new FakeGameScene();
            var expected = true;

            // Act
            scene.IsRenderingScene = true;
            var actual = scene.IsRenderingScene;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Id_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var scene = new FakeGameScene();
            var expected = 10;

            // Act
            scene.Id = 10;
            var actual = scene.Id;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedToTrue()
        {
            // Arrange
            var mockContentLoader = new Mock<IContentLoader>();
            var scene = new FakeGameScene();
            var expected = true;

            // Act
            scene.LoadContent(mockContentLoader.Object);
            var actual = scene.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UnloadContent_WhenInvoked_SetsContentLoadedToFalse()
        {
            // Arrange
            var mockContentLoader = new Mock<IContentLoader>();
            var scene = new FakeGameScene()
            {
                ContentLoaded = true,
            };
            var expected = false;

            // Act
            scene.UnloadContent(mockContentLoader.Object);
            var actual = scene.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvoking_InvokesTimeManagerUpdate()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene()
            {
                TimeManager = mockTimeManager.Object,
            };

            // Act
            scene.Update(new GameTime());

            // Assert
            mockTimeManager.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Once());
        }

        [Fact]
        public void Update_WhenInvoking_InvokesEntityUpdate()
        {
            // Arrange
            var entity = new FakeEntity(this.mockTexture.Object);

            var scene = new FakeGameScene();
            scene.AddEntity(entity);
            var expected = true;

            // Act
            scene.Update(new GameTime());
            var actual = entity.UpdateInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithNullTimeManager_DoesNotThrowException()
        {
            // Arrange
            var scene = new FakeGameScene()
            {
                TimeManager = null,
            };

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                scene.Update(new GameTime());
            });
        }

        [Fact]
        public void Render_WhenInvoking_InvokesAllEntityRenderMethods()
        {
            // Arrange
            var mockTexture = new Mock<ITexture>();

            var entityA = new FakeEntity(this.mockTexture.Object)
            {
            };

            var entityB = new FakeEntity(this.mockTexture.Object)
            {
            };

            var scene = new FakeGameScene();
            scene.AddEntity(entityA, false);
            scene.AddEntity(entityB, false);

            var renderer = new Renderer(10, 20);

            var expected = true;

            // Act
            scene.Render(renderer);
            var actual = entityA.RenderInvoked && entityB.RenderInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

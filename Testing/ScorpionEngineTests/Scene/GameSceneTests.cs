// <copyright file="GameSceneTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Scene
{
    using System;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Physics;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="GameScene"/> class.
    /// </summary>
    public class GameSceneTests : IDisposable
    {
        private readonly Mock<IPhysicsBody> mockPhysicsBody;
        private Mock<IPhysicsWorld> mockPhysicsWorld;

        public GameSceneTests()
        {
            this.mockPhysicsWorld = new Mock<IPhysicsWorld>();
            this.mockPhysicsBody = new Mock<IPhysicsBody>();
        }

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatePhysicsWorld()
        {
            // Arrange
            _ = new FakeGameScene(this.mockPhysicsWorld.Object);

            // Act
            var actual = GameScene.PhysicsWorld;

            // Assert
            Assert.NotNull(actual);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
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
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
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
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);

            // Act
            var actual = scene.TimeManager;

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void Initialized_WhenGettingValueAfterInitialized_ReturnsTrue()
        {
            // Arrange
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
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
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
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
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
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
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
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
            var mockCoreLoader = new Mock<IContentLoader>();
            var loader = new ContentLoader(mockCoreLoader.Object);
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
            var expected = true;

            // Act
            scene.LoadContent(loader);
            var actual = scene.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UnloadContent_WhenInvoked_SetsContentLoadedToFalse()
        {
            // Arrange
            var mockCoreLoader = new Mock<IContentLoader>();
            var loader = new ContentLoader(mockCoreLoader.Object);
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object)
            {
                ContentLoaded = true,
            };
            var expected = false;

            // Act
            scene.UnloadContent(loader);
            var actual = scene.ContentLoaded;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvoking_InvokesTimeManagerUpdate()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene(this.mockPhysicsWorld.Object)
            {
                TimeManager = mockTimeManager.Object,
            };

            // Act
            scene.Update(new EngineTime());

            // Assert
            mockTimeManager.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }

        [Fact]
        public void Update_WhenInvoking_InvokesPhyiscsWorldUpdate()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);

            // Act
            scene.Update(new EngineTime());

            // Assert
            this.mockPhysicsWorld.Verify(m => m.Update(It.IsAny<float>()), Times.Once());
        }

        [Fact]
        public void Update_WhenInvoking_InvokesEntityUpdate()
        {
            // Arrange
            var entity = new FakeEntity(Array.Empty<Vector2>(), Vector2.Zero)
            {
                Body = new PhysicsBody(this.mockPhysicsBody.Object),
            };

            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
            entity.Initialize();
            scene.AddEntity(entity);
            var expected = true;

            // Act
            scene.Update(new EngineTime());
            var actual = entity.UpdateInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithNullTimeManager_DoesNotThrowException()
        {
            // Arrange
            var scene = new FakeGameScene(this.mockPhysicsWorld.Object)
            {
                TimeManager = null,
            };

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                scene.Update(new EngineTime());
            });
        }

        [Fact]
        public void Render_WhenInvoking_InvokesAllEntityRenderMethods()
        {
            // Arrange
            var mockTexture = new Mock<ITexture>();

            var entityA = new FakeEntity(false)
            {
                Body = new PhysicsBody(this.mockPhysicsBody.Object),
                Texture = new Texture(mockTexture.Object),
            };

            var entityB = new FakeEntity(false)
            {
                Body = new PhysicsBody(this.mockPhysicsBody.Object),
                Texture = new Texture(mockTexture.Object),
            };

            var scene = new FakeGameScene(this.mockPhysicsWorld.Object);
            scene.AddEntity(entityA, false);
            scene.AddEntity(entityB, false);

            var renderer = new GameRenderer(new Mock<IRenderer>().Object, new Mock<IDebugDraw>().Object);

            var expected = true;

            // Act
            scene.Render(renderer);
            var actual = entityA.RenderInvoked && entityB.RenderInvoked;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        /// <inheritdoc/>
        public void Dispose() => this.mockPhysicsWorld = null;
    }
}

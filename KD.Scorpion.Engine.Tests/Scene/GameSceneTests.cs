﻿using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Scene;
using KDScorpionEngineTests.Fakes;
using KDScorpionEngine;
using KDScorpionEngine.Graphics;
using KDScorpionCore.Graphics;
using KDScorpionCore.Physics;

namespace KDScorpionEngineTests.Scene
{
    public class GameSceneTests : IDisposable
    {
        #region Fields
        private Mock<IPhysicsWorld> _mockPhysicsWorld;
        #endregion


        #region Constructors
        public GameSceneTests() => _mockPhysicsWorld = new Mock<IPhysicsWorld>();
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatePhysicsWorld()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);

            //Act
            var actual = GameScene.PhysicsWorld;

            //Assert
            Assert.NotNull(actual);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = "John Doe";

            //Act
            scene.Name = "John Doe";
            var actual = scene.Name;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ContentLoaded_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = true;

            //Act
            scene.ContentLoaded = true;
            var actual = scene.ContentLoaded;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void TimeManager_WhenGettingValue_NotNull()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);

            //Act
            var actual = scene.TimeManager;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void Initialized_WhenGettingValueAfterInitialized_ReturnsTrue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = true;

            //Act
            scene.Initialize();
            var actual = scene.Initialized;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Active_WhenGettingAndSettingValue_ReturnsTrue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = true;

            //Act
            scene.Active = true;
            var actual = scene.Active;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsRenderingScene_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = true;

            //Act
            scene.IsRenderingScene = true;
            var actual = scene.IsRenderingScene;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Id_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = 10;

            //Act
            scene.Id = 10;
            var actual = scene.Id;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void LoadContent_WhenInvoked_SetsContentLoadedToTrue()
        {
            //Arrange
            var mockCoreLoader = new Mock<IContentLoader>();
            var loader = new ContentLoader(mockCoreLoader.Object);
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            var expected = true;

            //Act
            scene.LoadContent(loader);
            var actual = scene.ContentLoaded;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UnloadContent_WhenInvoked_SetsContentLoadedToFalse()
        {
            //Arrange
            var mockCoreLoader = new Mock<IContentLoader>();
            var loader = new ContentLoader(mockCoreLoader.Object);
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object)
            {
                ContentLoaded = true
            };
            var expected = false;

            //Act
            scene.UnloadContent(loader);
            var actual = scene.ContentLoaded;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvoking_InvokesTimeManagerUpdate()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object)
            {
                TimeManager = mockTimeManager.Object
            };
            
            //Act
            scene.Update(new EngineTime());

            //Assert
            mockTimeManager.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }


        [Fact]
        public void Update_WhenInvoking_InvokesPhyiscsWorldUpdate()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();

            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);

            //Act
            scene.Update(new EngineTime());

            //Assert
            _mockPhysicsWorld.Verify(m => m.Update(It.IsAny<float>()), Times.Once());
        }


        [Fact]
        public void Update_WhenInvoking_InvokesEntityUpdate()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();

            var entity = new FakeEntity(new Vector[0], Vector.Zero)
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };

            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            entity.Initialize();
            scene.AddEntity(entity);
            var expected = true;

            //Act
            scene.Update(new EngineTime());
            var actual = entity.UpdateInvoked;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvokingWithNullTimeManager_DoesNotThrowException()
        {
            //Arrange
            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object)
            {
                TimeManager = null
            };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                scene.Update(new EngineTime());
            });
        }


        [Fact]
        public void Render_WhenInvoking_InvokesAllEntityRenderMethods()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            var mockTexture = new Mock<ITexture>();
            
            var entityA = new FakeEntity(false)
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>()),
                Texture = new Texture(mockTexture.Object)
            };

            var entityB = new FakeEntity(false)
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>()),
                Texture = new Texture(mockTexture.Object)
            };

            var scene = new FakeGameScene(Vector.Zero, _mockPhysicsWorld.Object);
            scene.AddEntity(entityA, false);
            scene.AddEntity(entityB, false);

            var renderer = new GameRenderer();
            
            var expected = true;

            //Act
            scene.Render(renderer);
            var actual = entityA.RenderInvoked && entityB.RenderInvoked;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose() => _mockPhysicsWorld = null;
        #endregion
    }
}

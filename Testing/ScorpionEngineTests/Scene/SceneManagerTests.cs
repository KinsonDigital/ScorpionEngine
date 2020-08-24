// <copyright file="SceneManagerTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Scene
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Exceptions;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Moq;
    using Raptor;
    using Raptor.Content;
    using Raptor.Input;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="SceneManager"/> class.
    /// </summary>
    public class SceneManagerTests : IDisposable
    {
        private readonly Mock<IContentLoader> contentLoader;
        private Mock<IKeyboard> mockKeyboard;

        public SceneManagerTests()
        {
            this.contentLoader = new Mock<IContentLoader>();

            this.mockKeyboard = new Mock<IKeyboard>();
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Space)).Returns(true);
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Right)).Returns(true);
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Left)).Returns(true);
        }

        #region Prop Tests
        [Fact]
        public void NextFrameStackKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = KeyCode.Left;

            // Act
            manager.NextFrameStackKey = KeyCode.Left;
            var actual = manager.NextFrameStackKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentScene_WhenGettingValue_ReturnsCorrectScene()
        {
            // Arrang
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            var expected = true;

            // Act
            var actual = scene == manager.CurrentScene;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentSceneId_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;
            scene.Id = 100;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            var expected = 100;

            // Act
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PlayCurrentSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = KeyCode.Left;

            // Act
            manager.PlayCurrentSceneKey = KeyCode.Left;
            var actual = manager.PlayCurrentSceneKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PauseCurrentSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = KeyCode.Left;

            // Act
            manager.PlayCurrentSceneKey = KeyCode.Left;
            var actual = manager.PlayCurrentSceneKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NextSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = KeyCode.Left;

            // Act
            manager.NextSceneKey = KeyCode.Left;
            var actual = manager.NextSceneKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PreviousSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = KeyCode.Left;

            // Act
            manager.PreviousSceneKey = KeyCode.Left;
            var actual = manager.PreviousSceneKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UnloadContentOnSceneChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = false;

            // Act
            manager.UnloadContentOnSceneChange = false;
            var actual = manager.UnloadContentOnSceneChange;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InitializeScenesOnAdd_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.InitializeScenesOnAdd = true;
            var actual = manager.InitializeScenesOnAdd;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InitializeScenesOnChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.InitializeScenesOnChange = true;
            var actual = manager.InitializeScenesOnChange;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ActivateSceneOnAdd_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.ActivateSceneOnAdd = true;
            var actual = manager.ActivateSceneOnAdd;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateInactiveScenes_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.UpdateInactiveScenes = true;
            var actual = manager.UpdateInactiveScenes;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeactivateOnSceneChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.DeactivateOnSceneChange = true;
            var actual = manager.DeactivateOnSceneChange;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetSceneAsRenderableOnAdd_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.SetSceneAsRenderableOnAdd = true;
            var actual = manager.SetSceneAsRenderableOnAdd;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UnloadPreviousSceneContent_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.UnloadPreviousSceneContent = true;
            var actual = manager.UnloadPreviousSceneContent;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LoadContentOnSceneChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.LoadContentOnSceneChange = true;
            var actual = manager.LoadContentOnSceneChange;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                new Mock<IScene>().Object,
            };
            var expected = 1;

            // Act
            var actual = manager.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsReadOnly_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrang
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = false;

            // Act
            var actual = manager.IsReadOnly;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SceneManagerClass_WhenGettingAndSettingItemIndex_ReturnsCorrectItem()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);
            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupGet(m => m.Id).Returns(30);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneB.Object,
            };

            manager[1] = mockSceneC.Object;
            var expected = mockSceneC.Object;

            // Act
            var actual = manager[1];

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Add_WhenInvoking_AddsNewScene()
        {
            // Arrange
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = 1;

            // Act
            manager.Add(new Mock<IScene>().Object);
            var actual = manager.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_WhenInvokingWithInitializeScenesOnAddPropSetToTrue_InvokesSceneInitialize()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                InitializeScenesOnAdd = true,
            };

            // Act
            manager.Add(mockScene.Object);

            // Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }

        [Fact]
        public void Add_WhenInvokingWithActivateSceneOnAddPropSetToTrue_SetsSceneToActive()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Active);

            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = true;

            // Act
            manager.Add(scene);
            var actual = scene.Active;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_WhenInvoking_GeneratesNewSceneId()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;
            scene.Id = -1;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expected = 0;

            // Act
            manager.Add(scene);
            var actual = scene.Id;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_WhenInvokingWithAlreadyExistingSceneId_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(10);

            var sceneA = mockSceneA.Object;
            var sceneB = mockSceneB.Object;

            // Act
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
            };

            // Assert
            Assert.Throws<IdAlreadyExistsException>(() =>
            {
                manager.Add(sceneB);
            });
        }

        [Fact]
        public void Add_WhenAddingOnlyOneItem_ReturnsCorrectSceneId()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expectedId = 0;

            // Act
            manager.Add(scene);
            var actualNotNull = manager.CurrentScene != null;
            var actualId = scene.Id;

            // Assert
            Assert.True(actualNotNull);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void Add_WhenAddingOnlyTwoItems_ReturnsCorrectSceneId()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = -1;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = -1;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expectedId = 1;

            // Act
            manager.Add(sceneA);
            manager.Add(sceneB);
            var actualNotNull = manager.CurrentScene != null;
            var actualId = sceneB.Id;

            // Assert
            Assert.True(actualNotNull);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void Add_WhenAddingOnlyThreeItems_ReturnsCorrectSceneId()
        {
            // Arrange
            // First item.  ID = 0
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 0;

            // Second item. ID Should be set to 1
            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;

            // Let the system choose the ID.  It should choose the lowest possible ID
            // out of all the id's.  The system should choose 1 as the ID.  This
            // would be the lowest possible ID between sceneA and sceneB's ID.
            sceneB.Id = -1;

            // Third item. ID = 2
            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupProperty(m => m.Id);
            var sceneC = mockSceneC.Object;
            sceneC.Id = 2;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expectedId = 1;

            // Act
            manager.Add(sceneA);
            manager.Add(sceneC);
            manager.Add(sceneB);
            var actualNotNull = manager.CurrentScene != null;
            var actualId = sceneB.Id;

            // Assert
            Assert.True(actualNotNull);
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void Clear_WhenInvoking_RemovesAllScenes()
        {
            // Arrange
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var mockScene = new Mock<IScene>();
            manager.Add(mockScene.Object);
            var expected = 0;

            // Act
            manager.Clear();
            var actual = manager.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Contains_WhenInvoking_ReturnsTrue()
        {
            // Arrange
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;
            manager.Add(scene);
            var expected = true;

            // Act
            var actual = manager.Contains(scene);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CopyTo_WhenInvoking_SuccessfullyCopiesScenesToArray()
        {
            // Arrange
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;
            manager.Add(scene);
            var expected = new IScene[1] { scene };

            // Act
            var actual = new IScene[1];
            manager.CopyTo(actual, 0);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Remove_WhenInvoking_SuccessfullyRemovesScene()
        {
            // Arrange
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;
            manager.Add(scene);
            var expected = 0;

            // Act
            manager.Remove(scene);
            var actual = manager.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetCurrentSceneID_WhenInvoking_SuccessfullyRemovesScene()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 100;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 200;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expectedId = 100;

            // Act
            manager.Add(sceneA);
            manager.Add(sceneB);
            manager.SetCurrentSceneID(100);
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expectedId, actual);
        }

        [Fact]
        public void SetCurrentSceneID_WhenInvokingWithAlreadyExistingId_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 100;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 200;

            // Act
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };

            // Assert
            Assert.Throws<IdNotFoundException>(() => manager.SetCurrentSceneID(1000));
        }

        [Fact]
        public void LoadAllSceneContent_WhenInvoking_InvokesSceneLoadContentMethod()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            mockSceneA.SetupGet(m => m.ContentLoaded).Returns(true);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 100;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 200;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };

            sceneA.LoadContent(new ContentLoader(this.contentLoader.Object));

            // Act
            manager.LoadAllSceneContent();

            // Assert
            mockSceneB.Verify(m => m.LoadContent(It.IsAny<ContentLoader>()), Times.Once());
        }

        [Fact]
        public void LoadCurrentSceneContent_WhenInvoking_InvokesSceneLoadContentMethod()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            mockSceneA.SetupProperty(m => m.ContentLoaded);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 10;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            mockSceneB.SetupProperty(m => m.ContentLoaded);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 20;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object);
            var expectedSceneAContentLoaded = false;
            var expectedSceneBContentLoaded = true;
            manager.Add(sceneA);
            manager.Add(sceneB);

            var contentLoader = new ContentLoader(this.contentLoader.Object);

            // Act
            manager.LoadCurrentSceneContent();
            var actualSceneAContentLoaded = sceneA.ContentLoaded;
            var actualSceneBContentLoaded = sceneB.ContentLoaded;

            // Assert
            Assert.Equal(expectedSceneAContentLoaded, actualSceneAContentLoaded);
            Assert.Equal(expectedSceneBContentLoaded, actualSceneBContentLoaded);

            mockSceneA.Verify(m => m.LoadContent(It.IsAny<ContentLoader>()), Times.Never());
            mockSceneB.Verify(m => m.LoadContent(It.IsAny<ContentLoader>()), Times.Once());
        }

        [Fact]
        public void LoadCurrentSceneContent_WhenInvokingWithNonExistingId_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            mockSceneA.Object.Id = -1;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            mockSceneB.Object.Id = -1;

            // Act
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneB.Object,
            };
            mockSceneB.Object.Id = 22;

            // Assert
            Assert.Throws<IdNotFoundException>(() => manager.LoadCurrentSceneContent());
        }

        [Fact]
        public void UnloadAllContent_WhenInvoking_InvokesSceneUnloadContent()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };

            // Act
            manager.UnloadAllContent();

            // Assert
            mockScene.Verify(m => m.UnloadContent(It.IsAny<ContentLoader>()), Times.Once());
        }

        [Fact]
        public void RemoveScene_WhenInvoking_SuccessfullyRemovesScene()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(100);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            var expected = 0;

            // Act
            manager.RemoveScene(100);
            var actual = manager.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(100);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };

            // Act
            var actual = manager.Count;

            // Assert
            Assert.Throws<IdNotFoundException>(() => manager.RemoveScene(200));
        }

        [Fact]
        public void NextScene_WhenNotOnLastScene_CorrectlySetsCurrentSceneId()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = 1;
            manager.SetCurrentSceneID(0);

            // Act
            manager.NextScene();
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NextScene_WhenOnLastScene_CorrectlySetsCurrentSceneId()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = 0;

            // Act
            manager.NextScene();
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NextScene_WhenInvoking_InvokesSceneChangedEvent()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            // Act
            manager.NextScene();

            // Assert
            Assert.Equal(expected, actualEventInvoked);
        }

        [Fact]
        public void PreviousScene_WhenNotOnFirstScene_CorrectlySetsCurrentSceneId()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = 0;
            manager.SetCurrentSceneID(1);

            // Act
            manager.PreviousScene();
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PreviousScene_WhenOnFirstScene_CorrectlySetsCurrentSceneId()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = 1;
            manager.SetCurrentSceneID(0);

            // Act
            manager.PreviousScene();
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PreviousScene_WhenInvoking_InvokesSceneChangedEvent()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            // Act
            manager.PreviousScene();

            // Assert
            Assert.Equal(expected, actualEventInvoked);
        }

        [Fact]
        public void SetCurrentScene_WhenInvokingWithId_CurrentlySetsCurrentSceneIdPropValue()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = 0;

            // Act
            manager.SetCurrentScene(0);
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetCurrentScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };

            // Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.SetCurrentScene(100));
        }

        [Fact]
        public void SetCurrentScene_WhenInvokingWithId_InvokesSceneChangedEvent()
        {
            // Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            // Act
            manager.SetCurrentScene(0);

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }

        [Fact]
        public void SetCurrentScene_WhenInvokingWithName_CurrentlySetsCurrentSceneIdPropValue()
        {
            // Arrange
            var sceneA = CreateScene("sceneA");
            var sceneB = CreateScene("sceneB");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expected = 0;

            // Act
            manager.SetCurrentScene("sceneA");
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetCurrentScene_WhenInvokingWithInvalidName_ThrowsException()
        {
            // Arrange
            var sceneA = CreateScene("sceneA");
            var sceneB = CreateScene("sceneB");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };

            // Act/Assert
            Assert.Throws<NameNotFoundException>(() => manager.SetCurrentScene("InvalidName"));
        }

        [Fact]
        public void SetCurrentScene_WhenInvokingWithName_InvokesSceneChangedEvent()
        {
            // Arrange
            var sceneA = CreateScene("sceneA");
            var sceneB = CreateScene("sceneB");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            // Act
            manager.SetCurrentScene("sceneA");

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }

        [Fact]
        public void InitializeCurrentScene_WhenInvoking_InvokesSceneInitializeMethod()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act
            manager.InitializeCurrentScene();

            // Assert
            mockScene.Verify(m => m.Initialize());
        }

        [Fact]
        public void InitializeCurrentScene_WhenInvokingWithInvalidSceneId_ThrowException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };

            scene.Id = 100;

            // Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.InitializeCurrentScene());
        }

        [Fact]
        public void InitializeScene_WhenInvokingWithId_InvokesSceneInitializeMethod()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act
            manager.InitializeScene(10);

            // Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }

        [Fact]
        public void InitializeScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.InitializeScene(20));
        }

        [Fact]
        public void InitializeScene_WhenInvokingWithName_InvokesSceneInitializeMethod()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Name).Returns("SceneA");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act
            manager.InitializeScene("SceneA");

            // Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }

        [Fact]
        public void InitializeScene_WhenInvokingWithInvalidName_ThrowsException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Name).Returns("SceneA");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act/Assert
            Assert.Throws<NameNotFoundException>(() => manager.InitializeScene("SceneB"));
        }

        [Fact]
        public void InitializeAllScenes_WhenInvokingWithInvalidName_InvokesInitializeMethodForAllScenes()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneB.Object,
            };

            // Act
            manager.InitializeAllScenes();

            // Assert
            mockSceneA.Verify(m => m.Initialize());
            mockSceneB.Verify(m => m.Initialize());
        }

        [Fact]
        public void GetScene_WhenInvokingWithId_ReturnsCorrectScene()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                mockSceneB.Object,
            };
            var expected = 10;

            // Act
            var actual = manager.GetScene<IScene>(10).Id;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                mockSceneB.Object,
            };

            // Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.GetScene<IScene>(100));
        }

        [Fact]
        public void GetScene_WhenInvokingWithName_ReturnsCorrectScene()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            mockSceneA.SetupGet(m => m.Name).Returns("SceneA");
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(20);
            mockSceneB.SetupGet(m => m.Name).Returns("SceneB");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                mockSceneB.Object,
            };
            var expected = "SceneA";

            // Act
            var actual = manager.GetScene<IScene>("SceneA").Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetScene_WhenInvokingWithInvalidName_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            mockSceneA.SetupGet(m => m.Name).Returns("SceneA");
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(20);
            mockSceneB.SetupGet(m => m.Name).Returns("SceneB");

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                mockSceneB.Object,
            };

            // Act/Assert
            Assert.Throws<NameNotFoundException>(() => manager.GetScene<IScene>("InvalidSceneName"));
        }

        [Fact]
        public void PlayCurrentScene_WhenInvoking_UnpausesScene()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = true;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            var expected = false;

            // Act
            manager.PlayCurrentScene();
            var actual = scene.TimeManager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PauseCurrentScene_WhenInvoking_PausesScene()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = false;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            var expected = true;

            // Act
            manager.PauseCurrentScene();
            var actual = scene.TimeManager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RunFrameStack_WhenInvoking_InvokesRunFrameStackMethod()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act
            manager.RunFrameStack();

            // Assert
            mockTimeManager.Verify(m => m.RunFrameStack(), Times.Once());
        }

        [Fact]
        public void RunFrames_WhenInvoking_InvokesRunFramesMethod()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act
            manager.RunFrames(It.IsAny<uint>());

            // Assert
            mockTimeManager.Verify(m => m.RunFrames(It.IsAny<uint>()), Times.Once());
        }

        [Fact]
        public void GetEnumerator_WhenInvoking_ReturnsEnumeratorObject()
        {
            // Arrange
            var mockScene = new Mock<IScene>();

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act
            var actual = manager.GetEnumerator();

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void IndexOf_WhenInvoking_ReturnsCorrectIndexValue()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneB.Object,
            };
            var expected = 0;

            // Act
            var actual = manager.IndexOf(mockSceneA.Object);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Insert_WhenInvoking_InsertsIntoCorrectSpot()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupGet(m => m.Id).Returns(30);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneC.Object,
            };
            var expected = mockSceneB.Object;

            // Act
            manager.Insert(1, mockSceneB.Object);
            var actual = manager.GetScene<IScene>(20);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Insert_WhenInvokingWithInvalidIndex_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupGet(m => m.Id).Returns(30);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneC.Object,
            };

            // Act/Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => manager.Insert(100, mockSceneB.Object));
        }

        [Fact]
        public void RemoveAt_WhenInvoking_ThrowsException()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockSceneA.Object,
                mockSceneB.Object,
            };
            var expected = 1;

            // Act
            manager.RemoveAt(0);
            var actual = manager.Count;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveAt_WhenInvokingWithInvalidIndex_ThrowsException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                mockScene.Object,
            };

            // Act/Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => manager.RemoveAt(100));
        }

        [Fact]
        public void Render_WhenInvokingWithInvalidSceneId_ThrowsException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);

            var scene = mockScene.Object;
            scene.Id = 10;

            // Act
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };

            // Assert
            scene.Id = 20;

            Assert.Throws<SceneNotFoundException>(() =>
            {
                manager.Render(It.IsAny<GameRenderer>());
            });
        }

        [Fact]
        public void Render_WhenInvoking_InvokesSceneRenderMethod()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);
            mockScene.SetupProperty(m => m.IsRenderingScene);

            var scene = mockScene.Object;
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };

            // Act
            manager.Render(It.IsAny<GameRenderer>());

            // Assert
            mockScene.Verify(m => m.Render(It.IsAny<GameRenderer>()), Times.Once());
        }

        [Fact]
        public void Render_WhenInvokingWithNoScenes_DoesNotThrowException()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);
            mockScene.SetupProperty(m => m.IsRenderingScene);

            var scene = mockScene.Object;
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
            };

            // Act/Assert
            AssertExt.DoesNotThrow<SceneNotFoundException>(() =>
            {
                manager.Render(It.IsAny<GameRenderer>());
            });
        }

        [Fact]
        public void Update_WhenInvokingWithPlayCurrentSceneKeyPressed_UnpausesCurrentScene()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = true;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(timeManager);

            var scene = mockScene.Object;
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            manager.PlayCurrentSceneKey = KeyCode.Space;
            var expected = false; // Unpaused

            // Act
            manager.Update(new EngineTime());
            var actual = scene.TimeManager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithPauseCurrentSceneKeyPressed_PausesCurrentScene()
        {
            // Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = false;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(timeManager);

            var scene = mockScene.Object;
            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            manager.PauseCurrentSceneKey = KeyCode.Space;
            var expected = true; // Paused

            // Act
            manager.Update(new EngineTime());
            var actual = scene.TimeManager.Paused;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithNextSceneKeyPressed_MovesToNextScene()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);
            var sceneB = mockSceneB.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            manager.NextSceneKey = KeyCode.Right;
            manager.SetCurrentSceneID(10);
            var expected = 20;

            // Act
            manager.Update(new EngineTime());
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithPreviousSceneKeyPressed_MovesToPreviousScene()
        {
            // Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);
            var sceneB = mockSceneB.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                sceneA,
                sceneB,
            };
            manager.PreviousSceneKey = KeyCode.Left;
            var expected = 10;

            // Act
            manager.Update(new EngineTime());
            var actual = manager.CurrentSceneId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithActiveScene_InvokesSceneUpdate()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Active).Returns(true);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };

            // Act
            manager.Update(new EngineTime());

            // Assert
            mockScene.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }

        [Fact]
        public void Update_WhenInvokingWithInActiveSceneAndUpdateInactiveScenesTrue_InvokesSceneUpdate()
        {
            // Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Active).Returns(false);
            var scene = mockScene.Object;

            var manager = new SceneManager(this.contentLoader.Object, this.mockKeyboard.Object)
            {
                scene,
            };
            manager.UpdateInactiveScenes = true;

            // Act
            manager.Update(new EngineTime());

            // Assert
            mockScene.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }
        #endregion

        /// <inheritdoc/>
        public void Dispose() => this.mockKeyboard = null;

        private static IScene CreateScene(string name = "", int sceneId = -1)
        {
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            mockScene.SetupProperty(m => m.Name);

            var result = mockScene.Object;

            result.Id = sceneId;
            result.Name = name;

            return result;
        }
    }
}

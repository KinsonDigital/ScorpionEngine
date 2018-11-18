using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Content;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Scene;
using System;

namespace ScorpionEngine.Tests.Scene
{
    [TestFixture]
    public class SceneManagerTests
    {
        #region Fields
        private Mock<IPluginLibrary> _mockEnginePluginLib;
        private IContentLoader _mockCoreLoader;
        private ContentLoader _contentLoader;
        #endregion


        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_LoadsKeyboardPlugin()
        {
            //Act/Arrange
            var manager = new SceneManager(_contentLoader);

            //Assert
            _mockEnginePluginLib.Verify(m => m.LoadPlugin<IKeyboard>(), Times.Once());
        }
        #endregion


        #region Prop Tests
        [Test]
        public void NextFrameStackKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = InputKeys.Left;

            //Act
            manager.NextFrameStackKey = InputKeys.Left;
            var actual = manager.NextFrameStackKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CurrentScene_WhenGettingValue_ReturnsCorrectScene()
        {
            //Arrang
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            var expected = true;

            //Act
            var actual = scene == manager.CurrentScene;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CurrentSceneId_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;
            scene.Id = 100;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            var expected = 100;

            //Act
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PlayCurrentSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = InputKeys.Left;

            //Act
            manager.PlayCurrentSceneKey = InputKeys.Left;
            var actual = manager.PlayCurrentSceneKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PauseCurrentSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = InputKeys.Left;

            //Act
            manager.PlayCurrentSceneKey = InputKeys.Left;
            var actual = manager.PlayCurrentSceneKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NextSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = InputKeys.Left;

            //Act
            manager.NextSceneKey = InputKeys.Left;
            var actual = manager.NextSceneKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PreviousSceneKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = InputKeys.Left;

            //Act
            manager.PreviousSceneKey = InputKeys.Left;
            var actual = manager.PreviousSceneKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UnloadContentOnSceneChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = false;

            //Act
            manager.UnloadContentOnSceneChange = false;
            var actual = manager.UnloadContentOnSceneChange;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        
        [Test]
        public void InitializeScenesOnAdd_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.InitializeScenesOnAdd = true;
            var actual = manager.InitializeScenesOnAdd;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InitializeScenesOnChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.InitializeScenesOnChange = true;
            var actual = manager.InitializeScenesOnChange;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ActivateSceneOnAdd_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.ActivateSceneOnAdd = true;
            var actual = manager.ActivateSceneOnAdd;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateInactiveScenes_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.UpdateInactiveScenes = true;
            var actual = manager.UpdateInactiveScenes;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DeactivateOnSceneChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.DeactivateOnSceneChange = true;
            var actual = manager.DeactivateOnSceneChange;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetSceneAsRenderableOnAdd_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.SetSceneAsRenderableOnAdd = true;
            var actual = manager.SetSceneAsRenderableOnAdd;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UnloadPreviousSceneContent_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.UnloadPreviousSceneContent = true;
            var actual = manager.UnloadPreviousSceneContent;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LoadContentOnSceneChange_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.LoadContentOnSceneChange = true;
            var actual = manager.LoadContentOnSceneChange;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Count_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader)
            {
                new Mock<IScene>().Object
            };
            var expected = 1;

            //Act
            var actual = manager.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsReadOnly_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrang
            var manager = new SceneManager(_contentLoader);
            var expected = false;

            //Act
            var actual = manager.IsReadOnly;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SceneManagerClass_WhenGettingAndSettingItemIndex_ReturnsCorrectItem()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);
            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupGet(m => m.Id).Returns(30);

            var manager = new SceneManager(_contentLoader)
            {
                mockSceneA.Object,
                mockSceneB.Object
            };

            manager[1] = mockSceneC.Object;
            var expected = mockSceneC.Object;

            //Act
            var actual = manager[1];

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Add_WhenInvoking_AddsNewScene()
        {
            //Arrange
            var manager = new SceneManager(_contentLoader);
            var expected = 1;

            //Act
            manager.Add(new Mock<IScene>().Object);
            var actual = manager.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Add_WhenInvokingWithInitializeScenesOnAddPropSetToTrue_InvokesSceneInitialize()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            var manager = new SceneManager(_contentLoader)
            {
                InitializeScenesOnAdd = true
            };

            //Act
            manager.Add(mockScene.Object);

            //Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }


        [Test]
        public void Add_WhenInvokingWithActivateSceneOnAddPropSetToTrue_SetsSceneToActive()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Active);

            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.Add(scene);
            var actual = scene.Active;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Add_WhenInvokingWithSetSceneAsRenderableOnAddPropSetToTrue_SetsSceneIsRenderingToTrue()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.IsRenderingScene);

            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader);
            var expected = true;

            //Act
            manager.Add(scene);
            var actual = scene.IsRenderingScene;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Add_WhenInvoking_GeneratesNewSceneId()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;
            scene.Id = -1;

            var manager = new SceneManager(_contentLoader);
            var expected = 0;

            //Act
            manager.Add(scene);
            var actual = scene.Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Add_WhenInvokingWithAlreadyExistingSceneId_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(10);

            var sceneA = mockSceneA.Object;
            var sceneB = mockSceneB.Object;

            var manager = new SceneManager(_contentLoader);

            //Act
            manager.Add(sceneA);

            //Assert
            Assert.Throws<IdAlreadyExistsException>(() =>
            {
                manager.Add(sceneB);
            });
        }


        [Test]
        public void Add_WhenAddingOnlyOneItem_ReturnsCorrectSceneId()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader);
            var expectedId = 0;

            //Act
            manager.Add(scene);
            var actualNotNull = manager.CurrentScene != null;
            var actualId = scene.Id;

            //Assert
            Assert.True(actualNotNull);
            Assert.AreEqual(expectedId, actualId);
        }


        [Test]
        public void Add_WhenAddingOnlyTwoItems_ReturnsCorrectSceneId()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = -1;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = -1;

            var manager = new SceneManager(_contentLoader);
            var expectedId = 1;

            //Act
            manager.Add(sceneA);
            manager.Add(sceneB);
            var actualNotNull = manager.CurrentScene != null;
            var actualId = sceneB.Id;

            //Assert
            Assert.True(actualNotNull);
            Assert.AreEqual(expectedId, actualId);
        }


        [Test]
        public void Add_WhenAddingOnlyThreeItems_ReturnsCorrectSceneId()
        {
            //Arrange
            //First item.  ID = 0
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 0;

            //Second item. ID Should be set to 1
            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            //Let the system choose the ID.  It should choose the lowest possible ID
            //out of all the id's.  The system should choose 1 as the ID.  This
            //would be the lowest possible id between sceneA and sceneB's ID.
            sceneB.Id = -1;

            //Third item. ID = 2
            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupProperty(m => m.Id);
            var sceneC = mockSceneC.Object;
            sceneC.Id = 2;

            var manager = new SceneManager(_contentLoader);
            var expectedId = 1;

            //Act
            manager.Add(sceneA);
            manager.Add(sceneC);
            manager.Add(sceneB);
            var actualNotNull = manager.CurrentScene != null;
            var actualId = sceneB.Id;

            //Assert
            Assert.True(actualNotNull);
            Assert.AreEqual(expectedId, actualId);
        }

        
        [Test]
        public void Clear_WhenInvoking_RemovesAllScenes()
        {
            //Arrange
            var manager = new SceneManager(_contentLoader);
            var mockScene = new Mock<IScene>();
            manager.Add(mockScene.Object);
            var expected = 0;

            //Act
            manager.Clear();
            var actual = manager.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Contains_WhenInvoking_ReturnsTrue()
        {
            //Arrange
            var manager = new SceneManager(_contentLoader);
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;
            manager.Add(scene);
            var expected = true;

            //Act
            var actual = manager.Contains(scene);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CopyTo_WhenInvoking_SuccessfullyCopiesScenesToArray()
        {
            //Arrange
            var manager = new SceneManager(_contentLoader);
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;
            manager.Add(scene);
            var expected = new IScene[1] { scene };

            //Act
            var actual = new IScene[1];
            manager.CopyTo(actual, 0);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Remove_WhenInvoking_SuccessfullyRemovesScene()
        {
            //Arrange
            var manager = new SceneManager(_contentLoader);
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;
            manager.Add(scene);
            var expected = 0;

            //Act
            manager.Remove(scene);
            var actual = manager.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetCurrentSceneID_WhenInvoking_SuccessfullyRemovesScene()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 100;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 200;

            var manager = new SceneManager(_contentLoader);
            var expectedId = 100;

            //Act
            manager.Add(sceneA);
            manager.Add(sceneB);
            manager.SetCurrentSceneID(100);
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expectedId, actual);
        }


        [Test]
        public void SetCurrentSceneID_WhenInvokingWithAlreadyExistingId_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 100;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 200;

            var manager = new SceneManager(_contentLoader);

            //Act
            manager.Add(sceneA);
            manager.Add(sceneB);

            //Assert
            Assert.Throws<IdNotFoundException>(() => manager.SetCurrentSceneID(1000));
        }


        [Test]
        public void LoadAllSceneContent_WhenInvoking_InvokesSceneLoadContentMethod()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            mockSceneA.SetupGet(m => m.ContentLoaded).Returns(true);
            var sceneA = mockSceneA.Object;
            sceneA.Id = 100;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = 200;

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };

            sceneA.LoadContent(_contentLoader);

            //Act
            manager.LoadAllSceneContent();

            //Assert
            mockSceneB.Verify(m => m.LoadContent(_contentLoader), Times.Once());
        }


        [Test]
        public void LoadCurrentSceneContent_WhenInvoking_InvokesSceneLoadContentMethod()
        {
            //Arrange
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

            var manager = new SceneManager(_contentLoader);
            var expectedSceneAContentLoaded = false;
            var expectedSceneBContentLoaded = true;
            manager.Add(sceneA);
            manager.Add(sceneB);

            //Act
            manager.LoadCurrentSceneContent();
            var actualSceneAContentLoaded = sceneA.ContentLoaded;
            var actualSceneBContentLoaded = sceneB.ContentLoaded;

            //Assert
            Assert.AreEqual(expectedSceneAContentLoaded, actualSceneAContentLoaded);
            Assert.AreEqual(expectedSceneBContentLoaded, actualSceneBContentLoaded);

            mockSceneA.Verify(m => m.LoadContent(_contentLoader), Times.Never());
            mockSceneB.Verify(m => m.LoadContent(_contentLoader), Times.Once());
        }


        [Test]
        public void LoadCurrentSceneContent_WhenInvokingWithNonExistingId_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupProperty(m => m.Id);
            var sceneA = mockSceneA.Object;
            sceneA.Id = -1;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupProperty(m => m.Id);
            var sceneB = mockSceneB.Object;
            sceneB.Id = -1;

            var manager = new SceneManager(_contentLoader);

            //Act
            manager.Add(sceneA);
            manager.Add(sceneB);
            sceneB.Id = 22;

            //Assert
            Assert.Throws<IdNotFoundException>(() => manager.LoadCurrentSceneContent());
        }


        [Test]
        public void UnloadAllContent_WhenInvoking_InvokesSceneUnloadContent()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };

            //Act
            manager.UnloadAllContent();

            //Assert
            mockScene.Verify(m => m.UnloadContent(_contentLoader), Times.Once());
        }


        [Test]
        public void RemoveScene_WhenInvoking_SuccessfullyRemovesScene()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(100);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            var expected = 0;

            //Act
            manager.RemoveScene(100);
            var actual = manager.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RemoveScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(100);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };

            //Act
            var actual = manager.Count;

            //Assert
            Assert.Throws<IdNotFoundException>(() => manager.RemoveScene(200));
        }


        [Test]
        public void NextScene_WhenNotOnLastScene_CorrectlySetsCurrentSceneId()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = 1;
            manager.SetCurrentSceneID(0);

            //Act
            manager.NextScene();
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NextScene_WhenOnLastScene_CorrectlySetsCurrentSceneId()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = 0;

            //Act
            manager.NextScene();
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NextScene_WhenInvoking_InvokesSceneChangedEvent()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            //Act
            manager.NextScene();

            //Assert
            Assert.AreEqual(expected, actualEventInvoked);
        }


        [Test]
        public void PreviousScene_WhenNotOnFirstScene_CorrectlySetsCurrentSceneId()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = 0;
            manager.SetCurrentSceneID(1);

            //Act
            manager.PreviousScene();
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PreviousScene_WhenOnFirstScene_CorrectlySetsCurrentSceneId()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = 1;
            manager.SetCurrentSceneID(0);

            //Act
            manager.PreviousScene();
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PreviousScene_WhenInvoking_InvokesSceneChangedEvent()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            //Act
            manager.PreviousScene();

            //Assert
            Assert.AreEqual(expected, actualEventInvoked);
        }


        [Test]
        public void SetCurrentScene_WhenInvokingWithId_CurrentlySetsCurrentSceneIdPropValue()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = 0;

            //Act
            manager.SetCurrentScene(0);
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetCurrentScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };

            //Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.SetCurrentScene(100));
        }


        [Test]
        public void SetCurrentScene_WhenInvokingWithId_InvokesSceneChangedEvent()
        {
            //Arrange
            var sceneA = CreateScene();
            var sceneB = CreateScene();

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            //Act
            manager.SetCurrentScene(0);

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void SetCurrentScene_WhenInvokingWithName_CurrentlySetsCurrentSceneIdPropValue()
        {
            //Arrange
            var sceneA = CreateScene("sceneA");
            var sceneB = CreateScene("sceneB");

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expected = 0;

            //Act
            manager.SetCurrentScene("sceneA");
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetCurrentScene_WhenInvokingWithInvalidName_ThrowsException()
        {
            //Arrange
            var sceneA = CreateScene("sceneA");
            var sceneB = CreateScene("sceneB");

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };

            //Act/Assert
            Assert.Throws<NameNotFoundException>(() => manager.SetCurrentScene("InvalidName"));
        }


        [Test]
        public void SetCurrentScene_WhenInvokingWithName_InvokesSceneChangedEvent()
        {
            //Arrange
            var sceneA = CreateScene("sceneA");
            var sceneB = CreateScene("sceneB");

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;
            manager.SceneChanged += (sender, e) => actualEventInvoked = true;

            //Act
            manager.SetCurrentScene("sceneA");

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void InitializeCurrentScene_WhenInvoking_InvokesSceneInitializeMethod()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act
            manager.InitializeCurrentScene();

            //Assert
            mockScene.Verify(m => m.Initialize());
        }


        [Test]
        public void InitializeCurrentScene_WhenInvokingWithInvalidSceneId_ThrowException()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupProperty(m => m.Id);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };

            scene.Id = 100;

            //Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.InitializeCurrentScene());
        }


        [Test]
        public void InitializeScene_WhenInvokingWithId_InvokesSceneInitializeMethod()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act
            manager.InitializeScene(10);

            //Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }


        [Test]
        public void InitializeScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.InitializeScene(20));
        }


        [Test]
        public void InitializeScene_WhenInvokingWithName_InvokesSceneInitializeMethod()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Name).Returns("SceneA");

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act
            manager.InitializeScene("SceneA");

            //Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }


        [Test]
        public void InitializeScene_WhenInvokingWithInvalidName_ThrowsException()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Name).Returns("SceneA");

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act/Assert
            Assert.Throws<NameNotFoundException>(() => manager.InitializeScene("SceneB"));
        }


        [Test]
        public void InitializeAllScenes_WhenInvokingWithInvalidName_InvokesInitializeMethodForAllScenes()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(_contentLoader)
            {
                mockSceneA.Object,
                mockSceneB.Object
            };

            //Act
            manager.InitializeAllScenes();

            //Assert
            mockSceneA.Verify(m => m.Initialize());
            mockSceneB.Verify(m => m.Initialize());
        }


        [Test]
        public void GetScene_WhenInvokingWithId_ReturnsCorrectScene()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                mockSceneB.Object
            };
            var expected = 10;

            //Act
            var actual = manager.GetScene<IScene>(10).Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetScene_WhenInvokingWithInvalidId_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                mockSceneB.Object
            };

            //Act/Assert
            Assert.Throws<IdNotFoundException>(() => manager.GetScene<IScene>(100));
        }


        [Test]
        public void GetScene_WhenInvokingWithName_ReturnsCorrectScene()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            mockSceneA.SetupGet(m => m.Name).Returns("SceneA");
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(20);
            mockSceneB.SetupGet(m => m.Name).Returns("SceneB");

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                mockSceneB.Object
            };
            var expected = "SceneA";

            //Act
            var actual = manager.GetScene<IScene>("SceneA").Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetScene_WhenInvokingWithInvalidName_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            mockSceneA.SetupGet(m => m.Name).Returns("SceneA");
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(20);
            mockSceneB.SetupGet(m => m.Name).Returns("SceneB");

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                mockSceneB.Object
            };

            //Act/Assert
            Assert.Throws<NameNotFoundException>(() => manager.GetScene<IScene>("InvalidSceneName"));
        }


        [Test]
        public void PlayCurrentScene_WhenInvoking_UnpausesScene()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = true;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            var expected = false;

            //Act
            manager.PlayCurrentScene();
            var actual = scene.TimeManager.Paused;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void PauseCurrentScene_WhenInvoking_PausesScene()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = false;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            var expected = true;

            //Act
            manager.PauseCurrentScene();
            var actual = scene.TimeManager.Paused;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RunFrameStack_WhenInvoking_InvokesRunFrameStackMethod()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act
            manager.RunFrameStack();

            //Assert
            mockTimeManager.Verify(m => m.RunFrameStack(), Times.Once());
        }


        [Test]
        public void RunFrames_WhenInvoking_InvokesRunFramesMethod()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(mockTimeManager.Object);

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act
            manager.RunFrames(It.IsAny<int>());

            //Assert
            mockTimeManager.Verify(m => m.RunFrames(It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void GetEnumerator_WhenInvoking_ReturnsEnumeratorObject()
        {
            //Arrange
            var mockScene = new Mock<IScene>();

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act
            var actual = manager.GetEnumerator();

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void IndexOf_WhenInvoking_ReturnsCorrectIndexValue()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(_contentLoader)
            {
                mockSceneA.Object,
                mockSceneB.Object
            };
            var expected = 0;

            //Act
            var actual = manager.IndexOf(mockSceneA.Object);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Insert_WhenInvoking_InsertsIntoCorrectSpot()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupGet(m => m.Id).Returns(30);

            var manager = new SceneManager(_contentLoader)
            {
                mockSceneA.Object,
                mockSceneC.Object
            };
            var expected = mockSceneB.Object;

            //Act
            manager.Insert(1, mockSceneB.Object);
            var actual = manager.GetScene<IScene>(20);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Insert_WhenInvokingWithInvalidIndex_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var mockSceneC = new Mock<IScene>();
            mockSceneC.SetupGet(m => m.Id).Returns(30);

            var manager = new SceneManager(_contentLoader)
            {
                mockSceneA.Object,
                mockSceneC.Object
            };

            //Act/Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => manager.Insert(100, mockSceneB.Object));
        }


        [Test]
        public void RemoveAt_WhenInvoking_ThrowsException()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);

            var manager = new SceneManager(_contentLoader)
            {
                mockSceneA.Object,
                mockSceneB.Object
            };
            var expected = 1;

            //Act
            manager.RemoveAt(0);
            var actual = manager.Count;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RemoveAt_WhenInvokingWithInvalidIndex_ThrowsException()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);

            var manager = new SceneManager(_contentLoader)
            {
                mockScene.Object
            };

            //Act/Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => manager.RemoveAt(100));
        }


        [Test]
        public void Render_WhenInvoking_SetsSceneIsRenderingScenePropToFalseAfterManagerRenders()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);
            mockScene.SetupProperty(m => m.IsRenderingScene);

            var scene = mockScene.Object;
            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            var expected = false;

            //Act
            manager.Render(It.IsAny<Renderer>());
            var actual = scene.IsRenderingScene;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Render_WhenInvoking_InvokesSceneRenderMethod()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Id).Returns(10);
            mockScene.SetupProperty(m => m.IsRenderingScene);

            var scene = mockScene.Object;
            var manager = new SceneManager(_contentLoader)
            {
                scene
            };

            //Act
            manager.Render(It.IsAny<Renderer>());

            //Assert
            mockScene.Verify(m => m.Render(It.IsAny<Renderer>()), Times.Once());
        }


        [Test]
        public void Update_WhenInvokingWithPlayCurrentSceneKeyPressed_UnpausesCurrentScene()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = true;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(timeManager);

            var scene = mockScene.Object;
            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            manager.PlayCurrentSceneKey = InputKeys.Space;
            var expected = false;//Unpaused

            //Act
            manager.Update(new EngineTime());
            var actual = scene.TimeManager.Paused;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithPauseCurrentSceneKeyPressed_PausesCurrentScene()
        {
            //Arrange
            var mockTimeManager = new Mock<ITimeManager>();
            mockTimeManager.SetupProperty(m => m.Paused);
            var timeManager = mockTimeManager.Object;
            timeManager.Paused = false;

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.TimeManager).Returns(timeManager);

            var scene = mockScene.Object;
            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            manager.PauseCurrentSceneKey = InputKeys.Space;
            var expected = true;//Paused

            //Act
            manager.Update(new EngineTime());
            var actual = scene.TimeManager.Paused;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithNextSceneKeyPressed_MovesToNextScene()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);
            var sceneB = mockSceneB.Object;

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            manager.NextSceneKey = InputKeys.Right;
            manager.SetCurrentSceneID(10);
            var expected = 20;

            //Act
            manager.Update(new EngineTime());
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithPreviousSceneKeyPressed_MovesToPreviousScene()
        {
            //Arrange
            var mockSceneA = new Mock<IScene>();
            mockSceneA.SetupGet(m => m.Id).Returns(10);
            var sceneA = mockSceneA.Object;

            var mockSceneB = new Mock<IScene>();
            mockSceneB.SetupGet(m => m.Id).Returns(20);
            var sceneB = mockSceneB.Object;

            var manager = new SceneManager(_contentLoader)
            {
                sceneA,
                sceneB
            };
            manager.PreviousSceneKey = InputKeys.Left;
            var expected = 10;

            //Act
            manager.Update(new EngineTime());
            var actual = manager.CurrentSceneId;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithActiveScene_InvokesSceneUpdate()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Active).Returns(true);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene,
            };

            //Act
            manager.Update(new EngineTime());

            //Assert
            mockScene.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }


        [Test]
        public void Update_WhenInvokingWithInActiveSceneAndUpdateInactiveScenesTrue_InvokesSceneUpdate()
        {
            //Arrange
            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(m => m.Active).Returns(false);
            var scene = mockScene.Object;

            var manager = new SceneManager(_contentLoader)
            {
                scene
            };
            manager.UpdateInactiveScenes = true;

            //Act
            manager.Update(new EngineTime());

            //Assert
            mockScene.Verify(m => m.Update(It.IsAny<EngineTime>()), Times.Once());
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            _mockCoreLoader = new Mock<IContentLoader>().Object;
            _contentLoader = new ContentLoader(_mockCoreLoader);
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyPressed((int)InputKeys.Space)).Returns(true);
            mockCoreKeyboard.Setup(m => m.IsKeyPressed((int)InputKeys.Right)).Returns(true);
            mockCoreKeyboard.Setup(m => m.IsKeyPressed((int)InputKeys.Left)).Returns(true);

            _mockEnginePluginLib = new Mock<IPluginLibrary>();
            _mockEnginePluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadEnginePluginLibrary(_mockEnginePluginLib.Object);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
            _mockEnginePluginLib = null;
        }
        #endregion


        #region Private Methods
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
        #endregion
    }
}

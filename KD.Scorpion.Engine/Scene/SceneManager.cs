using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Events;
using KDScorpionEngine.Exceptions;
using KDScorpionEngine.Graphics;
using KDScorpionEngine.Input;
using PluginSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KDScorpionEngine.Scene
{
    //TODO: Convert this class into an IEnumarable, IList type of class.
    /// <summary>
    /// Manages multiple game scenes.
    /// </summary>
    public class SceneManager : IUpdatable, IDrawable, IEnumerable<IScene>, IList<IScene>
    {
        #region Events
        public event EventHandler<SceneChangedEventArgs> SceneChanged;
        #endregion


        #region Private Vars
        private readonly ContentLoader _contentLoader;
        private readonly List<IScene> _scenes = new List<IScene>();//The list of scenes
        private readonly Keyboard _keyboard;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SceneManager"/>.
        /// <paramref name="contentLoader">The content manager user to load and unload content.</paramref>
        /// </summary>
        public SceneManager(ContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
            _keyboard = new Keyboard(EnginePluginSystem.Plugins.EnginePlugins.LoadPlugin<IKeyboard>());
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the key to be pressed to progress to the next frame stack when the <see cref="Mode"/> property is set to <see cref="RunMode.FrameStack"/>.
        /// </summary>
        public KeyCodes NextFrameStackKey { get; set; } = KeyCodes.None;

        /// <summary>
        /// Gets the currently enabled scene.
        /// </summary>
        public IScene CurrentScene => _scenes[CurrentSceneId];

        /// <summary>
        /// Gets the current scene ID.
        /// </summary>
        public int CurrentSceneId { get; private set; } = -1;

        /// <summary>
        /// The keyboard key used to play the current scene.
        /// </summary>
        public KeyCodes PlayCurrentSceneKey { get; set; } = KeyCodes.None;

        /// <summary>
        /// The keyboard key used to pause the current scene.
        /// </summary>
        public KeyCodes PauseCurrentSceneKey { get; set; } = KeyCodes.None;

        /// <summary>
        /// Gets or sets the key to press to move to the next <see cref="IScene"/>.
        /// If set to <see cref="KeyCodes.None"/>, then nothing will happen.
        /// </summary>
        public KeyCodes NextSceneKey { get; set; } = KeyCodes.None;

        /// <summary>
        /// Gets or sets the key to press to move to the previous <see cref="IScene"/>.
        /// If set to <see cref="KeyCodes.None"/>, then nothing will happen.
        /// </summary>
        public KeyCodes PreviousSceneKey { get; set; } = KeyCodes.None;

        /// <summary>
        /// Gets or sets a value indicting if the current scene content will be unloaded when the scene changes.
        /// WARNING: If false, this means that no content will be unloaded and will take up memory even though it
        /// is not being used.  This might be intentional.
        /// </summary>
        public bool UnloadContentOnSceneChange { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if scenes will be initialized when they are added to the manager.
        /// </summary>
        public bool InitializeScenesOnAdd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the scenes will be initialized when changing
        /// scenes.
        /// </summary>
        public bool InitializeScenesOnChange { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if the scene is activated when it is added.  All other scenes
        /// will be deactivated.
        /// </summary>
        public bool ActivateSceneOnAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if in-active scenes will be updated.
        /// </summary>
        public bool UpdateInactiveScenes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if all other scenes will be deactivated
        /// when setting a scene to active.
        /// </summary>
        public bool DeactivateOnSceneChange { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if a scene will be set to render when adding to the <see cref="SceneManager"/>.
        /// </summary>
        public bool SetSceneAsRenderableOnAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if the previous scene will have its content
        /// unloaded when moving to another scene.
        /// </summary>
        public bool UnloadPreviousSceneContent { get; set; } = true;

        /// <summary>
        /// Gets or sets a valud indicating that the scene will have its content loaded when
        /// moving to the scene.
        /// </summary>
        public bool LoadContentOnSceneChange { get; set; } = true;

        /// <summary>
        /// Gets the total number of scenes in the <see cref="SceneManager"/>.
        /// </summary>
        public int Count => _scenes.Count;

        /// <summary>
        /// Gets a value indicating if the list of scenes is readonly.
        /// </summary>
        public bool IsReadOnly => false;
        
        /// <summary>
        /// Gets or sets the <see cref="IScene"/> at the given index.
        /// </summary>
        /// <param name="index">The index/key of the <see cref="IScene"/>.</param>
        /// <returns></returns>
        public IScene this[int index]
        {
            get => _scenes[index];
            set => _scenes[index] = value;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns an enumerator that iterates through the scenes.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IScene> GetEnumerator()
        {
            return _scenes.GetEnumerator();
        }


        /// <summary>
        /// Returns an enumerator that iterates through the scenes.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            var numbers = new List<int>();

            numbers.GetEnumerator();
            return _scenes.GetEnumerator();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(IScene item)
        {
            return _scenes.IndexOf(item);
        }


        /// <summary>
        /// Inserts a new scene at the given index location.  Duplicate scene Id's cannot
        /// be used and will throw an <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        /// <param name="index">The index of where to insert the scene.</param>
        /// <param name="scene">The scene to add.</param>
        /// <exception cref="IdAlreadyExistsException">Thrown if the given <see cref="IScene"/> with the <see cref="IScene.Id"/> already has been added to the <see cref="SceneManager"/>.</exception>
        public void Insert(int index, IScene scene)
        {
            _scenes.Insert(index, scene);
        }


        /// <summary>
        /// Removes the <see cref="IScene"/> that exists at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index location of the <see cref="IScene"/> to remove.</param>
        public void RemoveAt(int index)
        {
            _scenes.RemoveAt(index);
        }


        /// <summary>
        /// Adds the given <paramref name="scene"/> to the <see cref="SceneManager"/>.
        /// </summary>
        /// <param name="scene">The <see cref="IScene"/> to add.</param>
        public void Add(IScene scene)
        {
            if (SceneIdExists(scene.Id))
                throw new IdAlreadyExistsException(scene.Id);

            //Generate a new scene id if the current id is a -1
            scene.Id = scene.Id == -1 ? GetNewId() : scene.Id;

            _scenes.Add(scene);

            //If the manager is set to initalize now
            if (InitializeScenesOnAdd)
                scene.Initialize();

            //If the manager is set to active the scene on add
            if (ActivateSceneOnAdd)
            {
                DeactivateAllScenes();
                scene.Active = true;
            }

            //If there is only one scene in the manager...set that scene to current scene
            CurrentSceneId = scene.Id;
        }


        /// <summary>
        /// Moves all of the <see cref="IScene"/>s from the <see cref="SceneManager"/>.
        /// </summary>
        public void Clear()
        {
            _scenes.Clear();
        }


        /// <summary>
        /// Returns true if the given <paramref name="scene"/> already exists in the <see cref="SceneManager"/>.
        /// </summary>
        /// <param name="scene">The <see cref="IScene"/> to check for.</param>
        /// <returns></returns>
        public bool Contains(IScene scene)
        {
            var contains = _scenes.Contains(scene);

            return _scenes.Contains(scene);
        }


        /// <summary>
        /// Copies the entire list of <see cref="IScene"/>s to a compatible one-dimensional array,
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="scenes">The list of scenes to copy the internal <see cref="IScene"/>s to.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(IScene[] scenes, int arrayIndex)
        {
            _scenes.CopyTo(scenes, arrayIndex);
        }


        /// <summary>
        /// Removes the given <paramref name="scene"/> from the <see cref="SceneManager"/>.
        /// </summary>
        /// <param name="scene">The <see cref="IScene"/> to remove.</param>
        /// <returns></returns>
        public bool Remove(IScene scene)
        {
            return _scenes.Remove(scene);
        }


        /// <summary>
        /// Sets the current <see cref="IScene"/> ID to the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="IScene"/> to set as the default.</param>
        /// <exception cref="IdNotFoundException">Thrown if the given id does not exist.</exception>
        public void SetCurrentSceneID(int id)
        {
            //If the scene id does not exist, throw an exception
            if (!SceneIdExists(id))
                throw new IdNotFoundException(id);

            CurrentSceneId = id;
        }


        /// <summary>
        /// Loads the content for all of the scenes.  If the scene's content has
        /// already been loaded, loading content for that scene will be skipped.
        /// </summary>
        public void LoadAllSceneContent()
        {
            foreach (var scene in _scenes)
            {
                if (scene.ContentLoaded)
                    continue;

                scene.LoadContent(_contentLoader);

                scene.ContentLoaded = true;
            }
        }


        /// <summary>
        /// Loads the current scenes content that matches the given <paramref name="id"/>.
        /// </summary>
        public void LoadCurrentSceneContent()
        {
            var foundScene = (from s in _scenes
                              where s.Id == CurrentSceneId
                              select s).FirstOrDefault();

            if (foundScene is null)
                throw new IdNotFoundException(CurrentSceneId);

            var currentSceneIndex = _scenes.IndexOf(foundScene);

            _scenes[currentSceneIndex].LoadContent(_contentLoader);
            _scenes[currentSceneIndex].ContentLoaded = true;
        }


        /// <summary>
        /// Unloads all of the content for all of the scenes.
        /// </summary>
        public void UnloadAllContent()
        {
            for (int i = 0; i < _scenes.Count; i++)
            {
                _scenes[i].UnloadContent(_contentLoader);
            }
        }


        /// <summary>
        /// Removes an <see cref="IScene"/> from the <see cref="SceneManager"/> that matches the given id.
        /// </summary>
        /// <param name="id">The id of the scene to remove.</param>
        public void RemoveScene(int id)
        {
            //If the scene id does not exist, throw an exception
            if (!SceneIdExists(id))
                throw new IdNotFoundException(id);

            var foundScene = (from s in _scenes where s.Id == id select s).FirstOrDefault();

            _scenes.Remove(foundScene);
        }


        /// <summary>
        /// Moves to the next scene.  If the current scene is the last scene, the next scene
        /// will be the first scene.
        /// </summary>
        public void NextScene()
        {
            //Find the index of the scene with the currently set scene id
            var currentScene = (from s in _scenes
                                where s.Id == CurrentSceneId
                                select s).FirstOrDefault();

            //Make the previous scene index the index of the current scene before
            //we move to the next scene
            var previousSceneIndex = _scenes.IndexOf(currentScene);

            var nextSceneIndex = previousSceneIndex < _scenes.Count - 1 ? previousSceneIndex + 1 : 0;

            //Update the current scene id to the next scene that is being moved to
            CurrentSceneId = _scenes[nextSceneIndex].Id;

            //Get the id of the previous scene
            var previousSceneId = currentScene.Id;

            ProcessSettingsForPreviousScene(_scenes[previousSceneIndex]);

            ProcessSettingsForCurrentScene(_scenes[nextSceneIndex]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[CurrentSceneId].Name));
        }


        /// <summary>
        /// Moves to the previous scene.  If the current scene is the first scene, the next scene
        /// will be the last scene.
        /// </summary>
        public void PreviousScene()
        {
            //Find the index of the scene with the currently set scene id
            var currentScene = (from s in _scenes
                                where s.Id == CurrentSceneId
                                select s).FirstOrDefault();

            //Make the previous scene index the index of the current scene before
            //we move to the next scene
            var previousSceneIndex = _scenes.IndexOf(currentScene);

            var nextSceneIndex = previousSceneIndex >= 1 ? previousSceneIndex - 1 : _scenes.Count - 1;

            //Update the current scene id to the next scene that is being moved to
            CurrentSceneId = _scenes[nextSceneIndex].Id;

            //Get the id of the previous scene
            var previousSceneId = currentScene.Id;

            ProcessSettingsForPreviousScene(_scenes[previousSceneIndex]);

            ProcessSettingsForCurrentScene(_scenes[nextSceneIndex]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[CurrentSceneId].Name));
        }


        /// <summary>
        /// Sets the current scene that matches the given <paramref name="id"/>.  All other scenes
        /// will be deactivated as long as the <see cref="DeactivateOnSceneChange"/> is set to true.
        /// </summary>
        /// <param name="id">The id of the scene to set.</param>
        public void SetCurrentScene(int id)
        {
            //If the scene id does not exist, throw an exception
            if (!SceneIdExists(id))
                throw new IdNotFoundException(id);

            var previousSceneId = CurrentSceneId;

            CurrentSceneId = id;

            ProcessSettingsForPreviousScene(_scenes[previousSceneId]);

            ProcessSettingsForCurrentScene(_scenes[CurrentSceneId]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[CurrentSceneId].Name));
        }


        /// <summary>
        /// Sets the current scene that matches the given <paramref name="name"/>.  All other scenes
        /// will be deactivated as long as the <see cref="DeactivateOnSceneChange"/> is set to true.
        /// </summary>
        /// <param name="name">The name of the scene to set.</param>
        public void SetCurrentScene(string name)
        {
            var foundScene = _scenes.Where(s => s.Name == name).FirstOrDefault();

            //If no scene was found, throw an exception
            if (foundScene == null)
                throw new NameNotFoundException(name);

            var previousSceneId = CurrentSceneId;

            CurrentSceneId = foundScene.Id;

            ProcessSettingsForPreviousScene(foundScene);

            ProcessSettingsForCurrentScene(foundScene);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[CurrentSceneId].Name));
        }


        /// <summary>
        /// Initializes the currently set scene.
        /// </summary>
        public void InitializeCurrentScene()
        {
            if (SceneIdExists(CurrentSceneId))
            {
                _scenes[CurrentSceneId].Initialize();

                return;
            }


            throw new IdNotFoundException(CurrentSceneId);
        }


        /// <summary>
        /// Initialize the <see cref="IScene"/> that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the scene to initialize.</param>
        public void InitializeScene(int id)
        {
            var foundScene = (from s in _scenes
                              where s.Id == id
                              select s).FirstOrDefault();

            if (foundScene is null)
                throw new IdNotFoundException(id);

            foundScene.Initialize();
        }


        /// <summary>
        /// Initialize the <see cref="IScene"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the scene to initialize.</param>
        public void InitializeScene(string name)
        {
            var foundScene = (from s in _scenes
                              where s.Name == name
                              select s).FirstOrDefault();

            if(foundScene is null)
                throw new NameNotFoundException(name);

            foundScene.Initialize();
        }


        /// <summary>
        /// Initializes all scenes.
        /// </summary>
        public void InitializeAllScenes()
        {
            for (int i = 0; i < _scenes.Count; i++)
            {
                _scenes[i].Initialize();
            }
        }


        /// <summary>
        /// Manages the <see cref="RunMode"/> settings and updates the currently enabled <see cref="IScene"/>.
        /// </summary>
        /// <param name="engineTime">The frame time information of the last frame.</param>
        public void Update(EngineTime engineTime)
        {
            ProcessKeys();

            //Update all of the scenes.
            for (int i = 0; i < _scenes.Count; i++)
            {
                //If the scene is active
                if (_scenes[i].Active || UpdateInactiveScenes)
                {
                    _scenes[i].Update(engineTime);
                }
            }
        }


        /// <summary>
        /// Calls the currently enabled <see cref="IScene"/> render method.
        /// </summary>
        /// <param name="renderer">The renderer to use for rendering.</param>
        public void Render(GameRenderer renderer)
        {
            if (_scenes.Count <= 0)
                return;

            var foundScene = (from s in _scenes
                              where s.Id == CurrentSceneId
                              select s).FirstOrDefault();

            if (foundScene == null)
                throw new SceneNotFoundException(CurrentSceneId);

            if (!foundScene.IsRenderingScene)
            {
                foundScene.IsRenderingScene = true;
                foundScene.Render(renderer);
            }
        }


        /// <summary>
        /// Gets the <see cref="IScene"/> as type <typeparamref name="T"/> that matches the given <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type to return the scene as.</typeparam>
        /// <param name="id">The id of the scene to get.</param>
        /// <returns></returns>
        public T GetScene<T>(int id) where T : class, IScene
        {
            var foundScene = (from s in _scenes
                              where s.Id == id
                              select s).FirstOrDefault();

            if(foundScene is null)
                throw new IdNotFoundException(id);


            return foundScene as T;
        }


        /// <summary>
        /// Gets the <see cref="IScene"/> as type <typeparamref name="T"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type to return the scene as.</typeparam>
        /// <param name="name">The name of the scene to get.</param>
        /// <returns></returns>
        public T GetScene<T>(string name) where T : class, IScene
        {
            var foundScene = (from s in _scenes
                              where s.Name == name
                              select s).FirstOrDefault();

            if (foundScene is null)
                throw new NameNotFoundException(name);


            return foundScene as T;
        }


        /// <summary>
        /// Sets all of the scenes to in-active.
        /// </summary>
        public void DeactivateAllScenes()
        {
            foreach (var scene in _scenes)
            {
                scene.Active = false;
            }
        }


        /// <summary>
        /// Plays the current scene.
        /// </summary>
        public void PlayCurrentScene()
        {
            _scenes[CurrentSceneId].TimeManager.Paused = false;
        }


        /// <summary>
        /// Pauses the current scene.
        /// </summary>
        public void PauseCurrentScene()
        {
            _scenes[CurrentSceneId].TimeManager.Paused = true;
        }


        /// <summary>
        /// Runs a complete stack of frames set by the <see cref="ITimeManager"/> for the current <see cref="IScene"/>.
        /// This will only work if the <see cref="Mode"/> is set to <see cref="RunMode.FrameStack"/> for the <see cref="IScene"/>.
        /// </summary>
        public void RunFrameStack()
        {
            _scenes[CurrentSceneId].TimeManager.RunFrameStack();
        }


        /// <summary>
        /// Runs a set amount of frames for the current <see cref="IScene"/>, given by the <paramref name="frames"/> param and pauses after.
        /// This will only work if the <see cref="Mode"/> is set to <see cref="RunMode.FrameStack"/> for the current <see cref="IScene"/>.
        /// </summary>
        /// <param name="frames">The number of frames to run.</param>
        public void RunFrames(int frames)
        {
            _scenes[CurrentSceneId].TimeManager.RunFrames(frames);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Checks if any of the <see cref="SceneManager"/> keys have been pressed and processes them.
        /// </summary>
        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            if (PlayCurrentSceneKey != KeyCodes.None)
            {
                //If the play key has been pressed
                if (_keyboard.IsKeyPressed(PlayCurrentSceneKey))
                    PlayCurrentScene();
            }

            if (PauseCurrentSceneKey != KeyCodes.None)
            {
                //If the pause key has been pressed
                if (_keyboard.IsKeyPressed(PauseCurrentSceneKey))
                    PauseCurrentScene();
            }


            if (NextSceneKey != KeyCodes.None)
            {
                //If the next scene key has been pressed
                if (_keyboard.IsKeyPressed(NextSceneKey))
                    NextScene();
            }

            if (PreviousSceneKey != KeyCodes.None)
            {
                //If the previous scene key has been pressed
                if (_keyboard.IsKeyPressed(PreviousSceneKey))
                    PreviousScene();
            }

            _keyboard.UpdatePreviousState();
        }


        /// <summary>
        /// Returns a value indicating if an <see cref="IScene"/> exists that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="IScene"/> to check for.</param>
        /// <returns></returns>
        private bool SceneIdExists(int id)
        {
            return _scenes.Any(s => s.Id == id);
        }


        /// <summary>
        /// Gets a new id number based on the current scenes id numbers that already exist.
        /// </summary>
        /// <returns>The new id number that is free to use.</returns>
        private int GetNewId()
        {
            var allIdNumbers = (from s in _scenes select s.Id).ToArray();

            //Get the largest id number
            var largestId = allIdNumbers.Length <= 0 ? 0 : allIdNumbers.Max();

            //Check each id number to see if it is sequential, if not, assign the first id number available from smallest to largest.
            for (int i = 1; i < largestId; i++)
            {
                //If the current possible id does not exist, use it. If it exists, move on.
                if (!allIdNumbers.Contains(i))
                    return i;
            }


            //If this point is reached, then all numbers from 1 to the largestId are being used
            //If this is the case, just return the next one after the largest number
            return allIdNumbers.Length > 0 ? largestId + 1 : largestId;
        }


        /// <summary>
        /// Turn off rendering for all scenes.
        /// </summary>
        private void TurnAllSceneRenderingOff()
        {
            foreach (var scene in _scenes)
            {
                scene.IsRenderingScene = false;
            }
        }


        /// <summary>
        /// Processes all of the manager settings for the given <paramref name="scene"/>.
        /// </summary>
        /// <param name="scene">The current scene to apply the manager settings to.</param>
        private void ProcessSettingsForCurrentScene(IScene scene)
        {
            //If the manager is set to set the current scene to active
            if (ActivateSceneOnAdd)
                scene.Active = true;

            //If the manager is set to initialize a scene
            if (InitializeScenesOnChange)
                scene.Initialize();

            //If the manager is set to load the next scenes content
            if (LoadContentOnSceneChange)
            {
                scene.LoadContent(_contentLoader);
                scene.ContentLoaded = true;
            }

            TurnAllSceneRenderingOff();
        }


        //TODO: TEST FOR THIS!!
        /// <summary>
        /// Processess all of the manager settings for the given previous <paramref name="scene"/>.
        /// </summary>
        /// <param name="scene">The previous scene to apply the manager settings to.</param>
        private void ProcessSettingsForPreviousScene(IScene scene)
        {
            //Unload the previous scenes content if the UnloadContentOnSceneChange is set to true
            if (UnloadContentOnSceneChange)
                scene.UnloadContent(_contentLoader);

            //If the manager is set to deactivate all other scenes
            if (DeactivateOnSceneChange)
                scene.Active = false;
        }
        #endregion
    }
}

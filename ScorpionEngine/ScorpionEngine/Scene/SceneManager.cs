using ScorpionEngine.Content;
using ScorpionEngine.Core;
using ScorpionEngine.EventArguments;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Scene
{
    /// <summary>
    /// Manages multiple game scenes.
    /// </summary>
    public class SceneManager : IUpdatable, IDrawable
    {
        #region Events
        public event EventHandler<SceneChangedEventArgs> SceneChanged;
        #endregion


        #region Private Vars
        private IContentLoader _contentLoader;
        private Dictionary<int, IScene> _scenes = new Dictionary<int, IScene>();//The list of scenes
        private IKeyboard _keyboard;
        private int _currentSceneId = -1;//The currently enabled scene ID.  This is the scene that is currently active and rendering/updating
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SceneManager"/>.
        /// <paramref name="contentLoader">The content manager user to load and unload content.</paramref>
        /// </summary>
        public SceneManager(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the key to be pressed to progress to the next frame stack when the <see cref="Mode"/> property is set to <see cref="RunMode.FrameStack"/>.
        /// </summary>
        public InputKeys NextFrameStackKey { get; set; } = InputKeys.None;

        /// <summary>
        /// Gets the currently enabled scene.
        /// </summary>
        public IScene CurrentScene => _scenes[_currentSceneId];

        /// <summary>
        /// The keyboard key used to play the current scene.
        /// </summary>
        public InputKeys PlayCurrentSceneKey { get; set; } = InputKeys.None;

        /// <summary>
        /// The keyboard key used to pause the current scene.
        /// </summary>
        public InputKeys PauseCurrentSceneKey { get; set; } = InputKeys.None;

        /// <summary>
        /// Gets or sets the key to press to move to the next <see cref="IScene"/>.
        /// If set to <see cref="InputKeys.None"/>, then nothing will happen.
        /// </summary>
        public InputKeys NextSceneKey { get; set; } = InputKeys.None;

        /// <summary>
        /// Gets or sets the key to press to move to the previous <see cref="IScene"/>.
        /// If set to <see cref="InputKeys.None"/>, then nothing will happen.
        /// </summary>
        public InputKeys PreviousSceneKey { get; set; } = InputKeys.None;

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
        #endregion


        #region Public Methods
        /// <summary>
        /// Sets the default <see cref="IScene"/> via the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="IScene"/> to set as the default.</param>
        public void SetDefaultSceneID(int id)
        {
            //If the scene id does not exist, throw an exception
            if (!SceneIdExists(id))
                throw new IdNotFoundException(id);

            _currentSceneId = id;
        }


        /// <summary>
        /// Loads the content for all of the scenes.  If the scene's content has
        /// already been loaded, loading content for that scene will be skipped.
        /// </summary>
        public void LoadAllSceneContent()
        {
            foreach (var id in _scenes.Keys)
            {
                if (_scenes[id].ContentLoaded)
                    continue;

                _scenes[id].LoadContent(_contentLoader);

                _scenes[id].ContentLoaded = true;
            }
        }


        /// <summary>
        /// Loads the current scenes content that matches the given <paramref name="id"/>.
        /// </summary>
        public void LoadCurrentSceneContent()
        {
            if (!SceneIdExists(_currentSceneId))
                throw new IdNotFoundException(_currentSceneId);

            _scenes[_currentSceneId].LoadContent(_contentLoader);
            _scenes[_currentSceneId].ContentLoaded = true;
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
        /// Adds the given <paramref name="scene"/> to the <see cref="SceneManager"/> that has the given <paramref name="id"/>.
        /// If no scenes exist, then this scene will be the default and active scene.  The most recent scene added will be
        /// the active scene.
        /// </summary>
        /// <param name="scene">The <see cref="IScene"/> to add.</param>
        /// <param name="id">The id used to enable the scene.  If the id is -1, then an id will be assigned automatically.
        /// Duplicate id numbers are not allowed.
        /// </param>
        /// <returns>The id number assigned to the newly added scene.</returns>
        public int AddScene(IScene scene, int id = -1)
        {
            //If the scene id already exists, throw an exception
            if (SceneIdExists(id))
                throw new IdAlreadyExistsException(id);

            var idToUse = id == -1 ? GetNewId() : id;

            _scenes.Add(idToUse, scene);

            //If the manager is set to initalize now
            if (InitializeScenesOnAdd)
                _scenes[idToUse].Initialize();

            //If the manager is set to active the scene on add
            if (ActivateSceneOnAdd)
            {
                DeactivateAllScenes();
                _scenes[idToUse].Active = true;
            }

            //If the manager is set to set the scene as render on add
            if (SetSceneAsRenderableOnAdd)
            {
                TurnAllSceneRenderingOff();
                _scenes[idToUse].RenderingScene = true;
            }

            //If there is only one scene in the manager...set that scene to current scene
            _currentSceneId = _scenes.Count == 1 ? 0 : idToUse;


            return idToUse;
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

            _scenes.Remove(id);
        }


        /// <summary>
        /// Moves to the next scene.  If the current scene is the last scene, the next scene
        /// will be the first scene.
        /// </summary>
        public void NextScene()
        {
            //Move to the next scene.  If the current scene is the last item, move to first scene
            _currentSceneId = _currentSceneId < _scenes.Count - 1 ? _currentSceneId + 1 : 0;

            //Get the id of the previous scene
            var previousSceneId = _currentSceneId == 0 ? _scenes.Count - 1 : _currentSceneId - 1;

            ProcessSettingsForPreviousScene(_scenes[previousSceneId]);

            ProcessSettingsForCurrentScene(_scenes[_currentSceneId]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[_currentSceneId].Name));
        }


        /// <summary>
        /// Moves to the previous scene.  If the current scene is the first scene, the next scene
        /// will be the last scene.
        /// </summary>
        public void PreviousScene()
        {
            //Move to the previous scene.  If the current scene is the first scene, move the last scene
            _currentSceneId = _currentSceneId == 0 ? _scenes.Count - 1 : _currentSceneId - 1;

            //Get the id of the previous scene
            var previousSceneId = _currentSceneId == _scenes.Count - 1 ? 0 : _currentSceneId + 1;

            ProcessSettingsForPreviousScene(_scenes[previousSceneId]);

            ProcessSettingsForCurrentScene(_scenes[_currentSceneId]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[_currentSceneId].Name));
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

            var previousSceneId = _currentSceneId;

            _currentSceneId = id;

            ProcessSettingsForPreviousScene(_scenes[previousSceneId]);

            ProcessSettingsForCurrentScene(_scenes[_currentSceneId]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[_currentSceneId].Name));
        }


        /// <summary>
        /// Sets the current scene that matches the given <paramref name="name"/>.  All other scenes
        /// will be deactivated as long as the <see cref="DeactivateOnSceneChange"/> is set to true.
        /// </summary>
        /// <param name="name">The name of the scene to set.</param>
        public void SetCurrentScene(string name)
        {
            var foundScene = _scenes.Where(s => s.Value.Name == name).FirstOrDefault();

            //If no scene was found, throw an exception
            if (foundScene.Value == null)
                throw new NameNotFoundException(name);

            var previousSceneId = _currentSceneId;

            _currentSceneId = foundScene.Key;

            ProcessSettingsForPreviousScene(_scenes[previousSceneId]);

            ProcessSettingsForCurrentScene(_scenes[_currentSceneId]);

            //Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(_scenes[previousSceneId].Name, _scenes[_currentSceneId].Name));
        }


        /// <summary>
        /// Initializes the currently set scene.
        /// </summary>
        public void InitializeCurrentScene()
        {
            if (SceneIdExists(_currentSceneId))
            {
                _scenes[_currentSceneId].Initialize();

                return;
            }


            throw new IdNotFoundException(_currentSceneId);
        }


        /// <summary>
        /// Initialize the <see cref="IScene"/> that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the scene to initialize.</param>
        public void InitializeScene(int id)
        {
            if (SceneIdExists(id))
            {
                _scenes[id].Initialize();

                return;
            }


            throw new IdNotFoundException(id);
        }


        /// <summary>
        /// Initialize the <see cref="IScene"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the scene to initialize.</param>
        public void InitializeScene(string name)
        {
            if (SceneNameExists(name))
            {
                GetScene<IScene>(name).Initialize();

                return;
            }


            throw new NameNotFoundException(name);
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
        /// <param name="gameTime">The frame time information of the last frame.</param>
        public void Update(IEngineTiming gameTime)
        {
            ProcessKeys();

            //Update all of the scenes.
            for (int i = 0; i < _scenes.Count; i++)
            {
                //If the scene is active
                if (_scenes[i].Active || UpdateInactiveScenes)
                {
                    _scenes[i].Update(gameTime);
                }
            }
        }


        /// <summary>
        /// Calls the currently enabled <see cref="IScene"/> render method.
        /// </summary>
        /// <param name="renderer">The renderer to use for rendering.</param>
        public void Render(IRenderer renderer)
        {
            if (_currentSceneId != -1 && _scenes[_currentSceneId].RenderingScene)
                _scenes[_currentSceneId].Render(renderer);
        }


        /// <summary>
        /// Gets the <see cref="IScene"/> as type <typeparamref name="T"/> that matches the given <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type to return the scene as.</typeparam>
        /// <param name="id">The id of the scene to get.</param>
        /// <returns></returns>
        public T GetScene<T>(int id) where T : class, IScene
        {
            //If the scene id does not exist, throw an exception
            if (!SceneIdExists(id))
                throw new IdNotFoundException(id);

            return _scenes[_currentSceneId] as T;
        }


        /// <summary>
        /// Gets the <see cref="IScene"/> as type <typeparamref name="T"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type to return the scene as.</typeparam>
        /// <param name="name">The name of the scene to get.</param>
        /// <returns></returns>
        public T GetScene<T>(string name) where T : class, IScene
        {
            var foundScene = _scenes.Where(s => s.Value.Name == name).FirstOrDefault();

            if (foundScene.Value == null)
                throw new NameNotFoundException("name");

            return foundScene.Value as T;
        }


        /// <summary>
        /// Sets all of the scenes to in-active.
        /// </summary>
        public void DeactivateAllScenes()
        {
            foreach (var scene in _scenes)
            {
                scene.Value.Active = false;
            }
        }


        /// <summary>
        /// Plays the current scene.
        /// </summary>
        public void PlayCurrentScene()
        {
            _scenes[_currentSceneId].TimeManager.Paused = false;
        }


        /// <summary>
        /// Pauses the current scene.
        /// </summary>
        public void PauseCurrentScene()
        {
            _scenes[_currentSceneId].TimeManager.Paused = true;
        }


        /// <summary>
        /// Runs a complete stack of frames set by the <see cref="ITimeManager"/> for the current <see cref="IScene"/>.
        /// This will only work if the <see cref="Mode"/> is set to <see cref="RunMode.FrameStack"/> for the <see cref="IScene"/>.
        /// </summary>
        public void RunFrameStack()
        {
            _scenes[_currentSceneId].TimeManager.RunFrameStack();
        }


        /// <summary>
        /// Runs a set amount of frames for the current <see cref="IScene"/>, given by the <paramref name="frames"/> param and pauses after.
        /// This will only work if the <see cref="Mode"/> is set to <see cref="RunMode.FrameStack"/> for the current <see cref="IScene"/>.
        /// </summary>
        /// <param name="frames">The number of frames to run.</param>
        public void RunFrames(int frames)
        {
            _scenes[_currentSceneId].TimeManager.RunFrames(frames);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Checks if any of the <see cref="SceneManager"/> keys have been pressed and processes them.
        /// </summary>
        private void ProcessKeys()
        {
            _keyboard.UpdateCurrentState();

            if (PlayCurrentSceneKey != InputKeys.None)
            {
                //If the play key has been pressed
                if (_keyboard.IsKeyDown(PlayCurrentSceneKey) && _keyboard.IsKeyUp(PlayCurrentSceneKey))
                {
                    PlayCurrentScene();
                }
            }

            if (PauseCurrentSceneKey != InputKeys.None)
            {
                //If the pause key has been pressed
                if (_keyboard.IsKeyDown(PauseCurrentSceneKey) && _keyboard.IsKeyUp(PauseCurrentSceneKey))
                {
                    PauseCurrentScene();
                }
            }


            if (PlayCurrentSceneKey != InputKeys.None)
            {
                //If the next scene key has been pressed
                if (_keyboard.IsKeyDown(NextSceneKey) && _keyboard.IsKeyUp(NextSceneKey))
                {
                    NextScene();
                }
            }

            if (PauseCurrentSceneKey != InputKeys.None)
            {
                //If the previous scene key has been pressed
                if (_keyboard.IsKeyDown(PreviousSceneKey) && _keyboard.IsKeyUp(PreviousSceneKey))
                {
                    PreviousScene();
                }
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
            return _scenes.Keys.Any(key => key == id);
        }


        /// <summary>
        /// Returns a value indicating if an IScene that matches the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name">The name of the <see cref="IScene"/> to check for.</param>
        /// <returns></returns>
        private bool SceneNameExists(string name)
        {
            return _scenes.Any(s => s.Value.Name == name);
        }


        /// <summary>
        /// Gets a new id number based on the current scenes id numbers that already exist.
        /// </summary>
        /// <returns>The new id number that is free to use.</returns>
        private int GetNewId()
        {
            var allIdNumbers = _scenes.Keys.ToList().ToArray();

            //Get the largest id number
            var largestId = allIdNumbers.Length <= 0 ? 0 : allIdNumbers.Max();

            //Check each id number to see if it is sequential, if not, assign the first id number available from smallest to largest.
            for (int i = 1; i < largestId; i++)
            {
                //If the current possible id does not exist, use it. If it exists, move on.
                if (!allIdNumbers.Contains(i))
                {
                    return i;
                }
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
                scene.Value.RenderingScene = false;
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
            scene.RenderingScene = true;
        }


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

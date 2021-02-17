// <copyright file="SceneManager.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Scene
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using KDScorpionEngine.Events;
    using KDScorpionEngine.Exceptions;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using Raptor.Input;

    /// <summary>
    /// Manages multiple game scenes.
    /// </summary>
    public class SceneManager : IUpdatableObject, IDrawableObject, IEnumerable<IScene>, IList<IScene>
    {
        private readonly IContentLoader contentLoader;
        private readonly IKeyboard keyboard;
        private readonly List<IScene> scenes = new List<IScene>(); // The list of scenes
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneManager"/> class.
        /// </summary>
        /// <param name="contentLoader">The content loaded to inject.</param>
        /// <param name="keyboard">Manages keyboard input.</param>
        public SceneManager(IContentLoader contentLoader, IKeyboard keyboard)
        {
            this.contentLoader = contentLoader;
            this.keyboard = keyboard;
        }

        /// <summary>
        /// Occurs when the currently active scene has changed.
        /// </summary>
        public event EventHandler<SceneChangedEventArgs> SceneChanged;

        /// <summary>
        /// Gets or sets the key to be pressed to progress to the next frame stack when the <see cref="Mode"/> property is set to <see cref="RunMode.FrameStack"/>.
        /// </summary>
        public KeyCode NextFrameStackKey { get; set; } = KeyCode.Unknown;

        /// <summary>
        /// Gets the currently enabled scene.
        /// </summary>
        public IScene CurrentScene => this.scenes[CurrentSceneId];

        /// <summary>
        /// Gets the current scene ID.
        /// </summary>
        public int CurrentSceneId { get; private set; } = -1;

        /// <summary>
        /// The keyboard key used to play the current scene.
        /// </summary>
        public KeyCode PlayCurrentSceneKey { get; set; } = KeyCode.Unknown;

        /// <summary>
        /// The keyboard key used to pause the current scene.
        /// </summary>
        public KeyCode PauseCurrentSceneKey { get; set; } = KeyCode.Unknown;

        /// <summary>
        /// Gets or sets the key to press to move to the next <see cref="IScene"/>.
        /// If set to <see cref="KeyCode.None"/>, then nothing will happen.
        /// </summary>
        public KeyCode NextSceneKey { get; set; } = KeyCode.Unknown;

        /// <summary>
        /// Gets or sets the key to press to move to the previous <see cref="IScene"/>.
        /// If set to <see cref="KeyCode.None"/>, then nothing will happen.
        /// </summary>
        public KeyCode PreviousSceneKey { get; set; } = KeyCode.Unknown;

        /// <summary>
        /// Gets or sets a value indicting if the current scene content will be unloaded when the scene changes.
        /// WARNING: If false, this means that no content will not unloaded and will take up memory even though it
        /// is not being used.  Only set to false if this is part of your game design and to prevent loading times
        /// the next time the scene is loaded.
        /// </summary>
        public bool UnloadContentOnSceneChange { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating if scenes will be initialized when they are added to the manager.
        /// WARNING: If set to false, the scene will have to be manually initialized.
        /// </summary>
        public bool InitializeScenesOnAdd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scenes will be initialized when changing scenes.
        /// </summary>
        public bool InitializeScenesOnChange { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the scene is activated
        /// when it is added.
        /// </summary>
        /// <remarks>All other scenes will be deactivated.</remarks>
        public bool ActivateSceneOnAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether in-active scenes will be updated.
        /// </summary>
        public bool UpdateInactiveScenes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets scenes
        /// will be deactivated when setting a scene to active.
        /// </summary>
        public bool DeactivateOnSceneChange { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a scene will be set to render when added to the <see cref="SceneManager"/>.
        /// </summary>
        public bool SetSceneAsRenderableOnAdd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the previous scene
        /// will have its content unloaded when moving to another scene.
        /// </summary>
        public bool UnloadPreviousSceneContent { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the scene
        /// will have its content loaded when moving to the scene.
        /// </summary>
        public bool LoadContentOnSceneChange { get; set; } = true;

        /// <summary>
        /// Gets the total number of scenes in the <see cref="SceneManager"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public int Count => this.scenes.Count;

        /// <summary>
        /// Gets a value indicating whether the list of scenes is read only.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public IScene this[int index]
        {
            get => this.scenes[index];
            set => this.scenes[index] = value;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public IEnumerator<IScene> GetEnumerator() => this.scenes.GetEnumerator();

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            var numbers = new List<int>();

            numbers.GetEnumerator();
            return this.scenes.GetEnumerator();
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public int IndexOf(IScene scene) => this.scenes.IndexOf(scene);

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public void Insert(int index, IScene scene) => this.scenes.Insert(index, scene);

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public void RemoveAt(int index) => this.scenes.RemoveAt(index);

        /// <inheritdoc/>
        public void Add(IScene scene)
        {
            if (SceneIdExists(scene.Id))
            {
                throw new IdAlreadyExistsException(scene.Id);
            }

            // Generate a new scene ID if the current ID is a -1
            scene.Id = scene.Id == -1 ? GetNewId() : scene.Id;

            this.scenes.Add(scene);

            // If the manager is set to initialize now
            if (InitializeScenesOnAdd)
            {
                scene.Initialize();
            }

            // If the manager is set to active the scene on add
            if (ActivateSceneOnAdd)
            {
                DeactivateAllScenes();
                scene.Active = true;
            }

            // If there is only one scene in the manager...set that scene to current scene
            CurrentSceneId = scene.Id;
        }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public void Clear() => this.scenes.Clear();

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public bool Contains(IScene scene) => this.scenes.Contains(scene);

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public void CopyTo(IScene[] scenes, int arrayIndex) => this.scenes.CopyTo(scenes, arrayIndex);

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public bool Remove(IScene scene) => this.scenes.Remove(scene);

        /// <summary>
        /// Sets the current <see cref="IScene"/> ID to the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the <see cref="IScene"/> to set as the default.</param>
        /// <exception cref="IdNotFoundException">Thrown if the given ID does not exist.</exception>
        public void SetCurrentSceneID(int id)
        {
            // If the scene ID does not exist, throw an exception
            if (!SceneIdExists(id))
            {
                throw new IdNotFoundException(id);
            }

            CurrentSceneId = id;
        }

        /// <summary>
        /// Loads the content for all of the scenes.  If the scene's content has
        /// already been loaded, loading content for that scene will be skipped.
        /// </summary>
        public void LoadAllSceneContent()
        {
            foreach (var scene in this.scenes)
            {
                if (scene.ContentLoaded)
                {
                    continue;
                }

                scene.LoadContent(this.contentLoader);

                scene.ContentLoaded = true;
            }
        }

        /// <summary>
        /// Loads the current scenes content that matches the <see cref="CurrentSceneId"/> value.
        /// </summary>
        public void LoadCurrentSceneContent()
        {
            var foundScene = (from s in this.scenes
                              where s.Id == CurrentSceneId
                              select s).FirstOrDefault();

            if (foundScene is null)
            {
                throw new IdNotFoundException(CurrentSceneId);
            }

            var currentSceneIndex = this.scenes.IndexOf(foundScene);

            this.scenes[currentSceneIndex].LoadContent(this.contentLoader);
            this.scenes[currentSceneIndex].ContentLoaded = true;
        }

        /// <summary>
        /// Unloads all of the content for all of the scenes.
        /// </summary>
        public void UnloadAllContent() => this.scenes.ForEach(s => s.UnloadContent(this.contentLoader));

        /// <summary>
        /// Removes a <see cref="IScene"/> from the <see cref="SceneManager"/> that matches the given ID.
        /// </summary>
        /// <param name="id">The ID of the scene to remove.</param>
        public void RemoveScene(int id)
        {
            // If the scene ID does not exist, throw an exception
            if (!SceneIdExists(id))
            {
                throw new IdNotFoundException(id);
            }

            var foundScene = (from s in this.scenes where s.Id == id select s).FirstOrDefault();

            this.scenes.Remove(foundScene);
        }

        /// <summary>
        /// Moves to the next scene.  If the current scene is the last scene, the next scene
        /// will be the first scene.
        /// </summary>
        public void NextScene()
        {
            // Find the index of the scene with the currently set scene ID
            var currentScene = (from s in this.scenes
                                where s.Id == CurrentSceneId
                                select s).FirstOrDefault();

            // Make the previous scene index the index of the current scene before
            // we move to the next scene
            var previousSceneIndex = this.scenes.IndexOf(currentScene);

            var nextSceneIndex = previousSceneIndex < this.scenes.Count - 1 ? previousSceneIndex + 1 : 0;

            // Update the current scene ID to the next scene that is being moved to
            CurrentSceneId = this.scenes[nextSceneIndex].Id;

            // Get the ID of the previous scene
            var previousSceneId = currentScene.Id;

            ProcessSettingsForPreviousScene(this.scenes[previousSceneIndex]);

            ProcessSettingsForNextScene(this.scenes[nextSceneIndex]);

            // Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(this.scenes[previousSceneId].Name, this.scenes[CurrentSceneId].Name));
        }

        /// <summary>
        /// Moves to the previous scene.  If the current scene is the first scene, the next scene
        /// will be the last scene.
        /// </summary>
        public void PreviousScene()
        {
            // Find the index of the scene with the currently set scene ID
            var currentScene = (from s in this.scenes
                                where s.Id == CurrentSceneId
                                select s).FirstOrDefault();

            // Make the previous scene index the index of the current scene before
            // we move to the next scene
            var previousSceneIndex = this.scenes.IndexOf(currentScene);

            var nextSceneIndex = previousSceneIndex >= 1 ? previousSceneIndex - 1 : this.scenes.Count - 1;

            // Update the current scene ID to the next scene that is being moved to
            CurrentSceneId = this.scenes[nextSceneIndex].Id;

            // Get the ID of the previous scene
            var previousSceneId = currentScene.Id;

            ProcessSettingsForPreviousScene(this.scenes[previousSceneIndex]);

            ProcessSettingsForNextScene(this.scenes[nextSceneIndex]);

            // Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(this.scenes[previousSceneId].Name, this.scenes[CurrentSceneId].Name));
        }

        /// <summary>
        /// Sets the current scene that matches the given <paramref name="id"/>.  All other scenes
        /// will be deactivated as long as the <see cref="DeactivateOnSceneChange"/> is set to true.
        /// </summary>
        /// <param name="id">The ID of the scene to set.</param>
        public void SetCurrentScene(int id)
        {
            // If the scene ID does not exist, throw an exception
            if (!SceneIdExists(id))
            {
                throw new IdNotFoundException(id);
            }

            if (CurrentSceneId == id)
            {
                return;
            }

            var previousSceneId = CurrentSceneId;

            CurrentSceneId = id;

            ProcessSettingsForPreviousScene(this.scenes[previousSceneId]);

            ProcessSettingsForNextScene(this.scenes[CurrentSceneId]);

            // Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(this.scenes[previousSceneId].Name, this.scenes[CurrentSceneId].Name));
        }

        /// <summary>
        /// Sets the current scene that matches the given <paramref name="name"/>.  All other scenes
        /// will be deactivated as long as the <see cref="DeactivateOnSceneChange"/> is set to true.
        /// </summary>
        /// <param name="name">The name of the scene to set.</param>
        public void SetCurrentScene(string name)
        {
            var foundScene = this.scenes.Where(s => s.Name == name).FirstOrDefault();

            // If no scene was found, throw an exception
            if (foundScene == null)
            {
                throw new NameNotFoundException(name);
            }

            if (CurrentSceneId == foundScene.Id)
            {
                return;
            }

            var previousSceneId = CurrentSceneId;

            CurrentSceneId = foundScene.Id;

            ProcessSettingsForPreviousScene(this.scenes[previousSceneId]);

            ProcessSettingsForNextScene(foundScene);

            // Invoke the scene changed event
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(this.scenes[previousSceneId].Name, this.scenes[CurrentSceneId].Name));
        }

        /// <summary>
        /// Initializes the currently set scene.
        /// </summary>
        /// <exception cref="IdNotFoundException">Invoked if the current scene to initialize does not exist.</exception>
        public void InitializeCurrentScene()
        {
            if (SceneIdExists(CurrentSceneId))
            {
                this.scenes[CurrentSceneId].Initialize();

                return;
            }

            throw new IdNotFoundException(CurrentSceneId);
        }

        /// <summary>
        /// Initialize the <see cref="IScene"/> that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the scene to initialize.</param>
        /// <exception cref="IdNotFoundException">Invoked if a scene with the given <paramref name="id"/> does not exist.</exception>
        public void InitializeScene(int id)
        {
            var foundScene = (from s in this.scenes
                              where s.Id == id
                              select s).FirstOrDefault();

            if (foundScene is null)
            {
                throw new IdNotFoundException(id);
            }

            foundScene.Initialize();
        }

        /// <summary>
        /// Initialize the <see cref="IScene"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the scene to initialize.</param>
        /// <exception cref="NameNotFoundException">Invoked if a scene with the given <paramref name="name"/> does not exist.</exception>
        public void InitializeScene(string name)
        {
            var foundScene = (from s in this.scenes
                              where s.Name == name
                              select s).FirstOrDefault();

            if (foundScene is null)
            {
                throw new NameNotFoundException(name);
            }

            foundScene.Initialize();
        }

        /// <summary>
        /// Initializes all scenes.
        /// </summary>
        public void InitializeAllScenes() => this.scenes.ForEach(s => s.Initialize());

        /// <summary>
        /// Updates the <see cref="SceneManager"/> and all of its scenes depending on the manager's settings.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public void Update(GameTime gameTime)
        {
            ProcessKeys();

            // Update all of the scenes.
            this.scenes.ForEach(s =>
            {
                if (s.Active || UpdateInactiveScenes)
                {
                    s.Update(gameTime);
                }
            });
        }

        /// <summary>
        /// Calls the currently enabled <see cref="IScene"/> render method.
        /// </summary>
        /// <param name="renderer">The renderer to use for rendering.</param>
        public void Render(Renderer renderer)
        {
            // TODO: Look into why this is not implemented the same way as the Update() method above
            if (this.scenes.Count <= 0)
            {
                return;
            }

            var foundScene = (from s in this.scenes
                              where s.Id == CurrentSceneId
                              select s).FirstOrDefault();

            if (foundScene == null)
            {
                throw new SceneNotFoundException(CurrentSceneId);
            }

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
        /// <param name="id">The ID of the scene to get.</param>
        /// <returns>A scene.</returns>
        public T GetScene<T>(int id)
            where T : class, IScene
        {
            var foundScene = (from s in this.scenes
                              where s.Id == id
                              select s).FirstOrDefault();

            if (foundScene is null)
            {
                throw new IdNotFoundException(id);
            }

            return foundScene as T;
        }

        /// <summary>
        /// Gets the <see cref="IScene"/> as type <typeparamref name="T"/> that matches the given <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="T">The type to return the scene as.</typeparam>
        /// <param name="name">The name of the scene to get.</param>
        /// <returns>A scene.</returns>
        public T GetScene<T>(string name)
            where T : class, IScene
        {
            var foundScene = (from s in this.scenes
                              where s.Name == name
                              select s).FirstOrDefault();

            if (foundScene is null)
            {
                throw new NameNotFoundException(name);
            }

            return foundScene as T;
        }

        /// <summary>
        /// Sets all of the scenes to in-active.
        /// </summary>
        public void DeactivateAllScenes() => this.scenes.ForEach(s => s.Active = false);

        /// <summary>
        /// Plays the current scene.
        /// </summary>
        public void PlayCurrentScene() => this.scenes[CurrentSceneId].TimeManager.Paused = false;

        /// <summary>
        /// Pauses the current scene.
        /// </summary>
        public void PauseCurrentScene() => this.scenes[CurrentSceneId].TimeManager.Paused = true;

        /// <summary>
        /// Runs a complete stack of frames set by the <see cref="ITimeManager"/> for the current <see cref="IScene"/>.
        /// This will only work if the <see cref="Mode"/> is set to the value of <see cref="RunMode.FrameStack"/> for the <see cref="IScene"/>.
        /// </summary>
        public void RunFrameStack() => this.scenes[CurrentSceneId].TimeManager.RunFrameStack();

        /// <summary>
        /// Runs a set amount of frames for the current <see cref="IScene"/>, given by the <paramref name="frames"/> parameter and pauses after.
        /// This will only work if the <see cref="Mode"/> is set to <see cref="RunMode.FrameStack"/> for the current <see cref="IScene"/>.
        /// </summary>
        /// <param name="frames">The number of frames to run.</param>
        public void RunFrames(uint frames) => this.scenes[CurrentSceneId].TimeManager.RunFrames(frames);

        /// <summary>
        /// Checks if any of the <see cref="SceneManager"/> keys have been pressed and processes them.
        /// </summary>
        private void ProcessKeys()
        {
            this.currentKeyboardState = this.keyboard.GetState();

            if (PlayCurrentSceneKey != KeyCode.Unknown)
            {
                // If the play key has been pressed
                if (this.currentKeyboardState.IsKeyUp(PlayCurrentSceneKey) && this.previousKeyboardState.IsKeyDown(PlayCurrentSceneKey))
                {
                    PlayCurrentScene();
                }
            }

            if (PauseCurrentSceneKey != KeyCode.Unknown)
            {
                // If the pause key has been pressed
                if (this.currentKeyboardState.IsKeyUp(PauseCurrentSceneKey) && this.previousKeyboardState.IsKeyDown(PauseCurrentSceneKey))
                {
                    PauseCurrentScene();
                }
            }

            if (NextSceneKey != KeyCode.Unknown)
            {
                // If the next scene key has been pressed
                if (this.currentKeyboardState.IsKeyUp(NextSceneKey) && this.previousKeyboardState.IsKeyDown(NextSceneKey))
                {
                    NextScene();
                }
            }

            if (PreviousSceneKey != KeyCode.Unknown)
            {
                // If the previous scene key has been pressed
                if (this.currentKeyboardState.IsKeyUp(PreviousSceneKey) && this.previousKeyboardState.IsKeyDown(PreviousSceneKey))
                {
                    PreviousScene();
                }
            }

            this.previousKeyboardState = this.currentKeyboardState;
        }

        /// <summary>
        /// Returns a value indicating if a <see cref="IScene"/> exists that matches the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the <see cref="IScene"/> to check for.</param>
        /// <returns>True if the scene exists.</returns>
        private bool SceneIdExists(int id) => this.scenes.Any(s => s.Id == id);

        /// <summary>
        /// Gets a new ID number based on the current scenes ID numbers that already exist.
        /// </summary>
        /// <returns>The new ID number that is free to use.</returns>
        private int GetNewId()
        {
            var allIdNumbers = (from s in this.scenes select s.Id).ToArray();

            // Get the largest ID number
            var largestId = allIdNumbers.Length <= 0 ? 0 : allIdNumbers.Max();

            // Check each ID number to see if it is sequential, if not, assign the first ID number available from smallest to largest.
            for (var i = 1; i < largestId; i++)
            {
                // If the current possible ID does not exist, use it. If it exists, move on.
                if (!allIdNumbers.Contains(i))
                {
                    return i;
                }
            }

            // If this point is reached, then all numbers from 1 to the largestId are being used
            // If this is the case, just return the next one after the largest number
            return allIdNumbers.Length > 0 ? largestId + 1 : largestId;
        }

        /// <summary>
        /// Turn off rendering for all scenes.
        /// </summary>
        private void TurnAllSceneRenderingOff() => this.scenes.ForEach(s => s.IsRenderingScene = false);

        /// <summary>
        /// Processes all of the manager settings for the given <paramref name="scene"/>.
        /// </summary>
        /// <param name="scene">The current scene to apply the manager settings to.</param>
        private void ProcessSettingsForNextScene(IScene scene)
        {
            // If the manager is set to set the current scene to active
            if (ActivateSceneOnAdd)
            {
                scene.Active = true;
            }

            // If the manager is set to initialize a scene
            if (InitializeScenesOnChange)
            {
                scene.Initialize();
            }

            // If the manager is set to load the next scenes content
            if (LoadContentOnSceneChange)
            {
                scene.LoadContent(this.contentLoader);
                scene.ContentLoaded = true;
            }

            // TODO: Look into this.  This is only used here.  Find out what this is for
            // and why we would turn off rrendering for all scenes here.
            TurnAllSceneRenderingOff();
        }

        /// <summary>
        /// Processes all of the manager settings for the given previous <paramref name="scene"/>.
        /// </summary>
        /// <param name="scene">The previous scene to apply the manager settings to.</param>
        private void ProcessSettingsForPreviousScene(IScene scene)
        {
            // Unload the previous scenes content if the UnloadContentOnSceneChange is set to true
            if (UnloadContentOnSceneChange)
            {
                scene.UnloadContent(this.contentLoader);
            }

            // If the manager is set to deactivate all other scenes
            if (DeactivateOnSceneChange)
            {
                scene.Active = false;
            }
        }
    }
}

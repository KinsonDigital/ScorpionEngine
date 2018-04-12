using System;
using ScorpionEngine.Content;
using ScorpionEngine.EventArguments;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.GameSound;
using ScorpionEngine.Objects;
using MonoRect = Microsoft.Xna.Framework.Rectangle;
using MonoColor = Microsoft.Xna.Framework.Color;
using ScorpionEngine.Utils;

namespace ScorpionEngine
{
    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        #region Fields
        private static EngineCore _engineCore;//Drive the main game engine
        private static World _world;//The one and only game world in the engine
        private static GraphicsDeviceManager _graphicsDeviceManager;//The graphics device
        private static int _framesPreSecondToMaintain = 60;//The set elapsed time for the engine to run at.  This is 60fps
        private static float _currentFPS;
        public static int _prevElapsedTime;
        private SpriteBatch _spriteBatch;//Draws all of the content
        private bool _running;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of engine.
        /// </summary>
        protected Engine()
        {
            //Create the engine core and the graphics device for drawing
            _engineCore = new EngineCore();
            _graphicsDeviceManager = new GraphicsDeviceManager(_engineCore);

            //Register the game loop events
            _engineCore.OnInit += _engineCore_OnInit;
            _engineCore.OnLoadContent += _engineCore_OnLoadContent;
            _engineCore.OnUpdate += _engineCore_OnUpdate;
            _engineCore.OnDraw += _engineCore_OnDraw;

            _engineCore.Content.RootDirectory = "Content";

            _engineCore.IsFixedTimeStep = false;

            SetFPS(_framesPreSecondToMaintain);
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the game world for the engine.
        /// </summary>
        public static World GameWorld
        {
            get
            {
                return _world;
            }
            set
            {
                _world = value;
            }
        }

        /// <summary>
        /// Gets a value indicating that the game engine is currently running.
        /// </summary>
        public bool Running
        {
            get
            {
                return _running;
            }
        }


        #region Static Properties
        /// <summary>
        /// Gets or sets a value indicating if the mouse will be visible when over the game window.
        /// </summary>
        public static bool MouseVisible
        {
            get { return _engineCore.IsMouseVisible; }
            set { _engineCore.IsMouseVisible = value; }
        }

        /// <summary>
        /// Gets or sets title of the game window.
        /// </summary>
        public static string WindowTitle
        {
            get
            {
                return _engineCore.Window.Title;
            }
            set { _engineCore.Window.Title = value; }
        }

        /// <summary>
        /// Gets the graphics device of the engine.
        /// </summary>
        internal static GraphicsDevice GrfxDevice
        {
            get
            {
                return _graphicsDeviceManager.GraphicsDevice;
            }
        }

        /// <summary>
        /// Gets the FPS that the engine is currently running at.
        /// </summary>
        public static float CurrentFPS
        {
            get { return _currentFPS; }
        }

        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public static int WindowWidth
        {
            get { return _graphicsDeviceManager.PreferredBackBufferWidth; }
            set { _graphicsDeviceManager.PreferredBackBufferWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public static int WindowHeight
        {
            get { return _graphicsDeviceManager.PreferredBackBufferHeight; }
            set { _graphicsDeviceManager.PreferredBackBufferHeight = value; }
        }

        /// <summary>
        /// Gets or sets the root directory for the game content.
        /// </summary>
        public static string ContentRootDir
        {
            get { return ContentLoader.ContentRootDirectory; }
            set { ContentLoader.ContentRootDirectory = value; }
        }
        #endregion
        #endregion


        #region Game Loop Methods
        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public virtual void OnInit()
        {
            _spriteBatch = new SpriteBatch(_graphicsDeviceManager.GraphicsDevice);

            _graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphicsDeviceManager.ApplyChanges();

            //Call the initialize method for the world
            _world.OnInit();
        }


        /// <summary>
        /// Loads all of the content.
        /// </summary>
        public void OnLoadContent()
        {
            _world.OnLoadContent();
        }


        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="engineTime">The time passed since last frame and game start.</param>
        private void OnUpdate(EngineTime engineTime)
        {
            var currentTime = engineTime.ElapsedEngineTime.Milliseconds;

            if (!_running) return;//If the engine has not been started, exit

            //Call the update method for the currently set game world
            _world?.OnUpdate(engineTime);

            _prevElapsedTime = currentTime;

            _currentFPS = (1000f/_prevElapsedTime);
        }


        /// <summary>
        /// Draws the game world.
        /// </summary>
        private void OnDraw()
        {
            if (!_running) return;//If the engine has not been started, exit

            //If the game world has not been set, exit
            if (_world == null)
            {
                //Draw a message that says the game world is not set
                return;
            }

            //Clear the screen
            _engineCore.GraphicsDevice.Clear(MonoColor.CornflowerBlue);

            _spriteBatch.Begin();

            //If debug draw is enabled, draw the debug data and not the graphics.
            if (World.DebugDrawEnabled)
            {
                World.DrawDebugData(_spriteBatch);
            }
            else
            {
                #region Draw Backgrounds For The World
                #endregion

                #region Draw GameObjects
                for (var i = 0; i < World.GameObjects.Count; i++)
                {
                    //If the texture of the game object is  null, move to the next object
                    if(World.GameObjects[i].Texture == null) continue;
                    
                    Texture2D texture = null;
                    var srcRect = MonoRect.Empty;
                    var destRect = MonoRect.Empty;

                    //If the texture source is its own texture or from a texture texture
                    switch (World.GameObjects[i].GraphicSource)
                    {
                        case GraphicContentSource.Standard:
                            texture = World.GameObjects[i].Texture;

                            //Create a source rectangle that is the same dimensions as the entire texture itself
                            //This will simply use the entire texture.
                            srcRect = new MonoRect(0, 0, World.GameObjects[i].Width, World.GameObjects[i].Height);

                            //TODO: Figure out why the sprite is not drawing on top of the debug draw shape
                            destRect = new MonoRect((int)World.GameObjects[i].Position.X,
                                                    (int)World.GameObjects[i].Position.Y,
                                                    World.GameObjects[i].Width,
                                                    World.GameObjects[i].Height);

                            break;
                        case GraphicContentSource.Atlas:
                            throw new NotImplementedException("NEEDS TESTING");
                            texture = AtlasManager.GetAtlasTexture(World.GameObjects[i].TextureName);

                            //texture = _atlasTextures[World.GameObjects[i].TextureName];
                            //var subRect = _atlasTextures[World.GameObjects[i].TextureName].SubTextureBounds(World.GameObjects[i].SubTextureName);
                            var subRect = World.GameObjects[i].TextureSourceBounds;
                            var newLocation = new Vector(World.GameObjects[i].Origin.X - World.GameObjects[i].Position.X, World.GameObjects[i].Origin.Y - World.GameObjects[i].Position.Y);

                            //rcRect = new MonoRect(subRect.X, subRect.Y, subRect.Width, subRect.Height);
                            //destRect = new MonoRect((int)destLocation.X, (int)destLocation.Y, srcRect.Width, srcRect.Height);

                            srcRect = new MonoRect(World.GameObjects[i].Animation.CurrentFrameBounds.X,
                                World.GameObjects[i].Animation.CurrentFrameBounds.Y,
                                World.GameObjects[i].Animation.CurrentFrameBounds.Width,
                                World.GameObjects[i].Animation.CurrentFrameBounds.Height);

                            destRect = new MonoRect((int)World.GameObjects[i].Position.X, (int)World.GameObjects[i].Position.Y,
                                World.GameObjects[i].Animation.CurrentFrameBounds.Width, World.GameObjects[i].Animation.CurrentFrameBounds.Height);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (texture != null)
                    {
                        var useAngle = false;

                        //If the object is not a standard object, then that means the object can be moved and angle needs to be taken into account
                        useAngle = World.GameObjects[i].GetType() != typeof (GameObject);

                        var origin = new Vector2
                        {
                            X = World.GameObjects[i].HalfWidth,
                            Y = World.GameObjects[i].HalfHeight
                        };

                        //Draw the texture
                        _spriteBatch.Draw(texture,//Texture
                              destRect, //Destination Rect
                              srcRect, //Source Rect
                              MonoColor.White, //Tint Color
                              useAngle ? ((MovableObject)World.GameObjects[i]).Angle : 0, //Angle
                              origin,
                              //ConvertUnits.ToDisplayUnits(new Vector2(World.GameObjects[i].Origin.X, World.GameObjects[i].Origin.Y)), //Origin - NOTE: This will need further research and implementation when having custom origins
                              SpriteEffects.None, //Sprite Effects
                              0);//Layer Depth ####NOTE: This will need further implementation and use####
                    }
                }
                #endregion

                #region Draw World Entity Pool GameObjects
                //Draw all of the entities in each pool
                foreach (var pool in _world.Pools)
                {
                    foreach (MovableObject gameObject in pool)
                    {
                        //Draw each entity that is visible in all of the entity pools
                        if (! gameObject.Visible) continue;

                        var origin = new Vector2(gameObject.Origin.X, gameObject.Origin.Y);

                        //Create the destination rectangle
                        var destRect = new MonoRect((int) gameObject.Position.X, (int) gameObject.Position.Y, gameObject.Width, gameObject.Height);

                        //Create the source rectangle
                        var srcRect = new MonoRect(0, 0, gameObject.Width, gameObject.Height);

                        _spriteBatch.Draw(gameObject.Texture, destRect, srcRect, MonoColor.White, MathHelper.ToRadians(gameObject.Angle), origin, SpriteEffects.None, 0);
                    }
                }
                #endregion

                #region Draw Each GameObjects Individual Object Pool Objects
                //For each game object, check for child pools
                foreach (var gameObj in World.GameObjects)
                {
                    //For each child pool in each game object
                    foreach (var childPool in gameObj.ChildPools)
                    {
                        //Draw each child pool object
                        for (var i = 0; i < childPool.Count; i++)
                        {
                            var origin = new Vector2(childPool[i].Origin.X, childPool[i].Origin.Y);

                            //Create the destination rectangle
                            var destRect = new MonoRect((int) childPool[i].Position.X, (int) childPool[i].Position.Y, childPool[i].Width, childPool[i].Height);

                            //Create the source rectangle
                            var srcRect = new MonoRect(0, 0, childPool[i].Width, childPool[i].Height);

                            //Draw the texture
                            _spriteBatch.Draw(childPool[i].Texture, destRect, srcRect, MonoColor.White, MathHelper.ToRadians(childPool[i].Angle), origin, SpriteEffects.None, 0);
                        }
                    }
                }
                #endregion
            }

            _spriteBatch.End();
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the game engine.
        /// </summary>
        public void Start()
        {
            //Check to see if a world object has been assigned to the engine, if not, throw exception
            _running = true;
            _engineCore.Run(GameRunBehavior.Synchronous);
        }


        /// <summary>
        /// Stops the game engine.
        /// </summary>
        public void Stop()
        {
            _running = false;
        }


        /// <summary>
        /// Sets the world in the engine to the given world.
        /// </summary>
        /// <param name="world">The world to set the engine to.</param>
        public static void SetWorld(World world)
        {
            //Throw an exception if the world is null
            if (world == null) throw new ArgumentNullException(nameof(world), "The world to assign to the engine must not be null.");

            _world = world;
        }


        /// <summary>
        /// Loads a song with the given name.
        /// </summary>
        /// <param name="songName">The name of the song to load.</param>
        /// <returns></returns>
        public static Song LoadSong(string songName)
        {
            return ContentLoader.LoadSong(songName);
        }


        /// <summary>
        /// Loads a sound effect with the given name.
        /// </summary>
        /// <param name="effectName">The name of the sound effect.</param>
        /// <returns></returns>
        public static SoundEffect LoadSoundEffect(string effectName)
        {
            return ContentLoader.LoadSoundEffect(effectName);
        }


        /// <summary>
        /// Disposes of the engine.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }


        /// <summary>
        /// Sets the frames per second that the engine should run at.
        /// </summary>
        /// <param name="value">The frames per second value to use.</param>
        public static void SetFPS(float value)
        {
            _engineCore.TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / value);
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Disposes of the internal engine components.
        /// </summary>
        /// <param name="disposing">True if the internal engine components should be disposed of.</param>
        private static void Dispose(bool disposing)
        {
            if (! disposing) return;

            if (_engineCore != null)
                _engineCore.Dispose();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Calls the OnInit method.
        /// </summary>
        private void _engineCore_OnInit(object sender, EventArgs e)
        {
            OnInit();
        }


        /// <summary>
        /// Calls the OnUpdate method.
        /// </summary>
        private void _engineCore_OnLoadContent(object sender, EventArgs e)
        {
            OnLoadContent();
        }


        /// <summary>
        /// Callse the OnUpdate method.
        /// </summary>
        private void _engineCore_OnUpdate(object sender, OnUpdateDrawEventArgs e)
        {
            OnUpdate(e.EngineTime);
        }


        /// <summary>
        /// Calls the on draw method.
        /// </summary>
        private void _engineCore_OnDraw(object sender, OnUpdateDrawEventArgs e)
        {
            OnDraw();
        }


        /// <summary>
        /// Calculates a new vector that is relative to the given source vector from the given destination vector.
        /// </summary>
        /// <param name="src">The source vector to set the destination vector relative to.</param>
        /// <param name="objToAdjust">The destination vector that will be relative to the source vector.</param>
        /// <returns></returns>
        private static Vector CalcRelativeVector(Vector src, Vector objToAdjust)
        {
            objToAdjust = new Vector(src.X + objToAdjust.X, objToAdjust.Y);
            objToAdjust = new Vector(objToAdjust.X, src.Y + objToAdjust.Y);

            return objToAdjust;
        }


        private void SetupDebugView()
        {
//            if (World.PhysicsWorld == null || _debugView != null) return;
//
//            // create and configure the debug view
//            _debugView = new DebugViewXNA(World.PhysicsWorld);
//            _debugView.DefaultShapeColor = MonoColor.White;
//            _debugView.SleepingShapeColor = MonoColor.LightGray;
//            _debugView.LoadContent(_graphicsDeviceManager.GraphicsDevice, _engineCore.Content);
        }
        #endregion
    }
}
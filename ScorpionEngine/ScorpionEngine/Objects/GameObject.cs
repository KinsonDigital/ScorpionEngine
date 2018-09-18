﻿using System;
using System.Collections.Generic;
using System.Linq;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using ScorpionEngine.Input;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Represents an imovable game object with a texture, and a location.  
    /// Great for static objects that never move such as walls, a tree, etc.
    /// </summary>
    public class GameObject
    {
        #region Events
        /// <summary>
        /// Fired when the game object is going from hidden to shown.
        /// </summary>
        public event EventHandler OnShow;

        /// <summary>
        /// Fired when the game object is shown to hidden.
        /// </summary>
        public event EventHandler OnHide;

        /// <summary>
        /// Occurs when a key has been pressed.
        /// </summary>
        public event EventHandler<KeyEventArgs> OnKeyPressed;

        /// <summary>
        /// Occurs when a key has been released.
        /// </summary>
        public event EventHandler<KeyEventArgs> OnKeyReleased;

        public event EventHandler Update;

        #endregion
        #region Fields
        private bool _visible = true;//True if the entity will be drawn
        protected IEngineTiming _engineTime;
        private Vector _origin = Vector.Zero;
        private readonly Dictionary<string, Vector> _subLocations = new Dictionary<string, Vector>();
        private readonly List<GameObject> _childObjects = new List<GameObject>();
        private readonly List<ObjectPool> _childPools = new List<ObjectPool>();
        protected ITexture _texture;
        private IKeyboard _keyboard;
        private Rect _textureBounds;//The bounds of the entire standard texture or the current sub texture of an atlas
        private string _subTextureName;//??
        private int _atlasSubTextureWidth;//??
        private int _atlasSubTextureHeight;//??
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of an entity.
        /// </summary>
        /// <param name="textureName">The textureName of the entity.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public GameObject(string textureName = "", Vector[] polyVertices = null)
        {
            InitializeObj(Vector.Zero, textureName, polyVertices);
        }


        /// <summary>
        /// Creates a new instance of an entity.
        /// </summary>
        /// <param name="textureName">The textureName of the entity.</param>
        /// <param name="location">Sets the location of the entity in the game world.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public GameObject(Vector location, string textureName = "", Vector[] polyVertices = null)
        {
            InitializeObj(location, textureName, polyVertices);
        }


        /// <summary>
        /// Loads a texture atlas for the game object to use.
        /// </summary>
        /// <param name="textureAtlasName">The name of the texture to load.</param>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <param name="subTextureID">The name of the sub texture in the atlas texture to render.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public GameObject(string textureAtlasName, string atlasDataName, string subTextureID, Vector[] polyVertices = null)
        {
            GraphicSource = GraphicContentSource.Atlas;//Set the graphic source to the atlas

            //Initialize the atlas
            InitializeAtlas(textureAtlasName, atlasDataName, subTextureID);

            //Initialize the rest of the game object
            InitializeObj(Vector.Zero, textureAtlasName, polyVertices);
        }


        /// <summary>
        /// Loads a texture atlas for the game object to use.
        /// </summary>
        /// <param name="textureAtlasName">The name of the texture to load.</param>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <param name="subTextureID">The name of the sub texture in the atlas to render.</param>
        /// <param name="location">The location to place the game object.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public GameObject(Vector location, string textureAtlasName, string atlasDataName, string subTextureID, Vector[] polyVertices = null)
        {
            //Initialize the atlas
            InitializeAtlas(textureAtlasName, atlasDataName, subTextureID);

            //Initialize the rest of the game object
            InitializeObj(location, textureAtlasName, polyVertices);
        }
        #endregion


        #region Props
        /// <summary>
        /// Used for debugging or passing of arbitrary data.
        /// </summary>
        public string TagData { get; set; }

        /// <summary>
        /// The animation of the GameObject.
        /// </summary>
        public ObjectAnimation Animation { get; private set; } = new ObjectAnimation();

        /// <summary>
        /// The name of the texture.
        /// </summary>
        public string TextureName { get; private set; }

        /// <summary>
        /// Gets the child pools of this GameObject.
        /// </summary>
        public IEnumerable<ObjectPool> ChildPools
        {
            get { return _childPools; }
        }

        /// <summary>
        /// Gets the source that the GameObject will use for its graphic content.
        /// </summary>
        public GraphicContentSource GraphicSource { get; private set; } = GraphicContentSource.Standard;

        /// <summary>
        /// Gets or sets a value indicating if the entity is drawn.
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                bool prevValue = _visible;

                _visible = value;

                //If the game object is going from show to hidden, fire the OnHidden event
                if (prevValue && !_visible)
                {
                    if (OnHide != null)
                        OnHide.Invoke(this, new EventArgs());
                }
                else if (!prevValue && _visible)//Going from hidden to shown
                {
                    if (OnShow != null)
                        OnShow.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets the bounds of the game object.
        /// </summary>
        public Rect Bounds
        {
            get { return new Rect((int)Position.X, (int)Position.Y, Width, Height); }    
        }

        /// <summary>
        /// Gets the location of the entity in the game world in pixel units.
        /// </summary>
        //TODO: Get this property/feature working
        public Vector Position { get; private set; }

        /// <summary>
        /// Gets the width of the entity.
        /// </summary>
        public int Width
        {
            get
            {
                switch (GraphicSource)
                {
                    case GraphicContentSource.Standard:
                        return _texture.Width;
                    case GraphicContentSource.Atlas:
                        return Animation.CurrentFrameBounds.Width;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return - 1;
            }
        }

        /// <summary>
        /// Gets the height of the entity.
        /// </summary>
        public int Height
        {
            get
            {
                switch (GraphicSource)
                {
                    case GraphicContentSource.Standard:
                        return _texture.Height;
                    case GraphicContentSource.Atlas:
                        return Animation.CurrentFrameBounds.Height;
                }

                return - 1;
            }
        }

        public int HalfWidth { get; set; }

        public int HalfHeight { get; set; }

        /// <summary>
        /// Returns the origin that the entity rotates around.
        /// </summary>
        public Vector Origin
        {
            get
            {
                //TODO: Set the origin of rotation for the object
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the unique ID that has been assigned to this entity.
        /// </summary>
        public int ID { get; set; } = -1;

        /// <summary>
        /// Gets or sets the ID number of the GameObject that this GameObject's location is relative to.
        /// </summary>
        public int RelativeToID { get; set; } = -1;


        #region Internal Properties
        /// <summary>
        /// Gets the texture of the game object.
        /// </summary>
        internal ITexture Texture
        {
            get
            {
                return _texture;
            }
        }

        /// <summary>
        /// Gets the bounds of where in the standard or atlas texture to draw from.
        /// </summary>
        internal Rect TextureSourceBounds
        {
            get { return Animation.CurrentFrameBounds; }
        }
        #endregion
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds a child object to this instance of GameObject.
        /// </summary>
        /// <param name="gameObj">The child GameObject to add.</param>
        public void AddChild(GameObject gameObj)
        {
            _childObjects.Add(gameObj);

        }


        /// <summary>
        /// Adds a child pool to this instance of GameObj.
        /// </summary>
        /// <param name="pool">The pool to add.</param>
        public void AddChild(ObjectPool pool)
        {
            _childPools.Add(pool);
        }


        /// <summary>
        /// Sets the origin of the GameObject.
        /// </summary>
        /// <param name="x">The x-component of the origin.</param>
        /// <param name="y">Thy y-compoment of the origin.</param>
        public void SetOrigin(int x, int y)
        {
            _origin = new Vector(x, y);
        }


        /// <summary>
        /// Sets the origin of the GameObject.
        /// </summary>
        /// <param name="type">The preset type of origin to set.</param>
        public void SetOrigin(OriginType type)
        {
            if (GraphicSource == GraphicContentSource.Standard)
            {
                switch (type)
                {
                    case OriginType.Center:
                        _origin = new Vector(Width / 2.0f, Height / 2.0f);
                        break;
                    case OriginType.TopLeft:
                        _origin = Vector.Zero;
                        break;
                    case OriginType.TopRight:
                        _origin = new Vector(Width, 0.0f);
                        break;
                    case OriginType.BottomLeft:
                        _origin = new Vector(0.0f, Height);
                        break;
                    case OriginType.BottomRight:
                        _origin = new Vector(Width, Height);
                        break;
                }
            }
            else if (GraphicSource == GraphicContentSource.Atlas)
            {
                switch (type)
                {
                    case OriginType.Center:
                        _origin = new Vector(Animation.CurrentFrameBounds.Width / 2f, Animation.CurrentFrameBounds.Height / 2f);
                        break;
                    case OriginType.TopLeft:
                        _origin = Vector.Zero;
                        break;
                    case OriginType.TopRight:
                        _origin = new Vector(Animation.CurrentFrameBounds.Width, 0.0f);
                        break;
                    case OriginType.BottomLeft:
                        _origin = new Vector(0f, Animation.CurrentFrameBounds.Height);
                        break;
                    case OriginType.BottomRight:
                        _origin = new Vector(Animation.CurrentFrameBounds.Width, Animation.CurrentFrameBounds.Height);
                        break;
                }
            }
        }


        /// <summary>
        /// Adds a sub location relative to the game object's location. No name duplicates aloud.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        public void AddSubLocation(string name, Vector location)
        {
            _subLocations.Add(name, location);
        }


        /// <summary>
        /// Gets a sub location that is relative to the location of the game object.
        /// </summary>
        /// <param name="name">The name of the location to get.</param>
        /// <returns></returns>
        public Vector GetSubLocation(string name)
        {
            Vector foundVector = Vector.Zero;

            _subLocations.TryGetValue(name, out foundVector);

            return foundVector;
        }
        #endregion


        #region Internal Methods
        /// <summary>
        /// Sets the position of the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="position"></param>
        internal void SetPosition(Vector position)
        {
            Position = position;
        }
        #endregion


        #region Game Loop Methods
        /// <summary>
        /// Updates the game object.
        /// </summary>
        public virtual void OnUpdate(IEngineTiming engineTime)
        {
            _engineTime = engineTime;

            //Get newly pressed keys that are not in the previous key list
            var newlyPressedKeys = (from newKey in _keyboard.GetCurrentPressedKeys() where !_keyboard.GetCurrentPressedKeys().Contains(newKey) select newKey).ToList();

            var newlyReleaseKeys = (from prevKey in _keyboard.GetCurrentPressedKeys() where !_keyboard.GetCurrentPressedKeys().Contains(prevKey) select prevKey).ToList();

            //If there are newly pressed keys, invoke the OnKeyPressed event
            if (_keyboard.GetCurrentPressedKeys().Length > _keyboard.GetCurrentPressedKeys().Length && newlyPressedKeys.Count > 0)
            {
                OnKeyPressed?.Invoke(this, new KeyEventArgs(newlyPressedKeys.ConvertAll(ConvertKey).ToArray()));
            }
            else if (_keyboard.GetCurrentPressedKeys().Length < _keyboard.GetPreviousPressedKeys().Length && newlyReleaseKeys.Count > 0) //Look for newly released keys
            {
                OnKeyReleased?.Invoke(this, new KeyEventArgs(newlyReleaseKeys.ConvertAll(ConvertKey).ToArray()));
            }

            Animation.Update(engineTime);

            //Update all of the child objects
            for (var i = 0; i < _childObjects.Count; i++)
            {
                _childObjects[i].OnUpdate(engineTime);    
            }

            //Update all of the entity pools
            for (var i = 0; i < _childPools.Count; i++)
            {
                _childPools[i].OnUpdate(engineTime);
            }

            Update?.Invoke(this, new EventArgs());
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Initializes the GameObject depending on what constructor has been called.
        /// </summary>
        /// <param name="textureName">The textureName of the entity.</param>
        /// <param name="location">The location of the entity in the game world.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        protected virtual void InitializeObj(Vector location, string textureName = "", Vector[] polyVertices = null)
        {

        }


        /// <summary>
        /// Initializes an atlas with the given information.
        /// </summary>
        /// <param name="textureAtlasName">The name of the atlas texture to load.</param>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <param name="subTextureID">The name of the sub texture in the atlas texture that will be rendered for this GameObject.</param>
        private void InitializeAtlas(string textureAtlasName, string atlasDataName, string subTextureID)
        {
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Converts the given button to a InputKey.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns></returns>
        private static InputKeys ConvertKey(InputKeys key)
        {
            return (InputKeys) key;
        }


        /// <summary>
        /// Returns true if the current and previous key states match.
        /// </summary>
        /// <returns></returns>
        private bool CurrentPrevKeysMatch()
        {
            //If the number of current and previous keys are the same, check to make sure that the keys for both are the same or not
            return _keyboard.GetCurrentPressedKeys().Length == _keyboard.GetPreviousPressedKeys().Length && _keyboard.GetCurrentPressedKeys().All(key => _keyboard.GetPreviousPressedKeys().Contains(key));
        }
        #endregion
    }
}
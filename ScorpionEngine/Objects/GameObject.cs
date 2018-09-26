using System;
using System.Collections.Generic;
using System.Linq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Objects
{
    //TODO: Add docs
    /// <summary>
    /// Represents an imovable game object with a texture, and a location.  
    /// Great for static objects that never move such as walls, a tree, etc.
    /// </summary>
    public class GameObject
    {
        private IPhysicsBody _internalBody;


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
        protected Texture _texture;
        private Keyboard _keyboard;
        #endregion


        #region Constructors
        public GameObject()
        {
            CreateBody();
        }


        public GameObject(Texture texture)
        {
            _texture = texture;
            CreateBody();
        }


        /// <summary>
        /// Creates a new instance of an entity.
        /// </summary>
        /// <param name="textureName">The textureName of the entity.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public GameObject(Vector[] polyVertices)
        {
            CreateBody();
        }


        /// <summary>
        /// Creates a new instance of an entity.
        /// </summary>
        /// <param name="textureName">The textureName of the entity.</param>
        /// <param name="location">Sets the location of the entity in the game world.</param>
        /// <param name="polyVertices">Optional parameter: The vertices that make up the shape of the game object for the internal physics engine.  If left null, then a default rectanglular 
        /// polygon will be used for the shape of the object.  The vertices must be in CCW(count clockwise) direction.</param>
        public GameObject(Vector[] polyVertices, Vector location)
        {
            CreateBody();
        }
        #endregion


        #region Props
        public IPhysicsBody PhysicsBody { get; set; }

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

                //If the game object is going from show to hidden, fire the OnHide event
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
        public Vector Position { get; set; }

        /// <summary>
        /// Gets the width of the entity.
        /// </summary>
        public int Width
        {
            get => _texture.Width;
        }

        /// <summary>
        /// Gets the height of the entity.
        /// </summary>
        public int Height
        {
            get => _texture.Height;
        }

        /// <summary>
        /// Returns the half width of the <see cref="GameObject"/>.
        /// </summary>
        public int HalfWidth => Width / 2;

        /// Returns the half height of the <see cref="GameObject"/>.
        /// </summary>
        public int HalfHeight => Height / 2;

        /// <summary>
        /// Gets the unique ID that has been assigned to this entity.
        /// </summary>
        public int ID { get; set; } = -1;

        /// <summary>
        /// Gets the texture of the game object.
        /// </summary>
        public Texture Texture
        {
            get => _texture;
            set => _texture = value;
        }
        #endregion


        #region Public Methods
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

            Update?.Invoke(this, new EventArgs());
        }
        #endregion


        #region Private Methods
        private void CreateBody()
        {
            _internalBody = PluginLoader.GetPluginByType<IPhysicsBody>();
        }


        /// <summary>
        /// Converts the given button to a InputKey.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns></returns>
        private static InputKeys ConvertKey(InputKeys key)
        {
            return key;
        }


        /// <summary>
        /// Renders the game object.
        /// </summary>
        /// <param name="renderer">The render used to render the object texture.</param>
        public void Render(Renderer renderer)
        {
            if(_texture != null && Visible)
                renderer.Render(_texture, Position.X, Position.Y);
        }
        #endregion
    }
}
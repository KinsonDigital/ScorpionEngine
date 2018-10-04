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
    public abstract class Entity
    {
        protected IPhysicsBody _internalBody;


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
        protected bool _usesPhysics = true;
        protected IEngineTiming _engineTime;
        private Vector _origin = Vector.Zero;
        protected Texture _texture;
        #endregion


        #region Constructors
        public Entity(Vector position, bool isStaticBody = false)
        {
        }


        public Entity(Texture texture, Vector position, bool isStaticBody = false)
        {
            _texture = texture;

            var halfWidth = texture.Width / 2;
            var halfHeight = texture.Height / 2;

            var vertices = new Vector[4]
            {
                new Vector(position.X - halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y + halfHeight),
                new Vector(position.X - halfWidth, position.Y + halfHeight),
            };

            CreateBody(vertices, position, isStaticBody);
        }


        public Entity(Vector[] polyVertices, Vector position, bool isStaticBody = false)
        {
        }


        public Entity(Texture texture, Vector[] polyVertices, Vector position, bool isStaticBody = false)
        {
            _texture = texture;
            CreateBody(polyVertices, position, isStaticBody);
        }
        #endregion


        #region Props
        public bool UsesPhysics { get; private set; }

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
            get { return new Rect((int)Position.X, (int)Position.Y, BoundsWidth, BoundsHeight); }    
        }

        /// <summary>
        /// Gets the location of the entity in the game world in pixel units.
        /// </summary>
        public Vector Position
        {
            get => new Vector(_internalBody.X, _internalBody.Y);
            set
            {
                _internalBody.X = value.X;
                _internalBody.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets the width of the entity.
        /// </summary>
        public int BoundsWidth
        {
            //TODO: This needs to be redone to return the difference between the
            //farthest left and farthest right vertices
            get => -1;
        }

        /// <summary>
        /// Gets the height of the entity.
        /// </summary>
        public int BoundsHeight
        {
            //TODO: This needs to be redone to return the difference between the
            //farthest top and farthest bottom vertices
            get => -1;
        }

        /// <summary>
        /// Returns the half width of the <see cref="Entity"/>.
        /// </summary>
        public int BoundsHalfWidth => BoundsWidth / 2;

        /// Returns the half height of the <see cref="Entity"/>.
        /// </summary>
        public int BoundsHalfHeight => BoundsHeight / 2;

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

        public bool DebugDrawEnabled { get; set; } = false;
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the game object.
        /// </summary>
        public virtual void OnUpdate(IEngineTiming engineTime)
        {
            _engineTime = engineTime;

            Update?.Invoke(this, new EventArgs());
        }
        #endregion


        #region Private Methods
        private void CreateBody(Vector[] vertices, Vector position, bool isStatic)
        {
            var body = new PhysicsBody(vertices, position, isStatic: isStatic);

            Engine.PhysicsWorld.AddBody(body);
            _internalBody = body.Body;
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
using System;
using System.Linq;
using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionCore.Input;
using ScorpionCore.Plugins;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Entities
{
    //TODO: Add docs
    /// <summary>
    /// Represents an imovable game object with a texture, and a location.  
    /// Great for static objects that never move such as walls, a tree, etc.
    /// </summary>
    public abstract class Entity
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
        #endregion


        #region Fields
        private bool _visible = true;//True if the entity will be drawn
        protected bool _usesPhysics = true;
        protected EngineTime _engineTime;
        private Vector _origin = Vector.Zero;
        protected Texture _texture;
        private IDebugDraw _debugDraw;
        private Vector _preInitPosition;
        private Vector[] _preInitVertices;
        private float _preInitFriction;
        #endregion


        #region Constructors
        public Entity(float friction = 0.2f, bool isStaticBody = false)
        {
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }


        public Entity(Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitPosition = position;
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }


        public Entity(Texture texture, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _texture = texture;

            var halfWidth = texture.Width / 2;
            var halfHeight = texture.Height / 2;

            _preInitVertices = new Vector[4]
            {
                new Vector(position.X - halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y + halfHeight),
                new Vector(position.X - halfWidth, position.Y + halfHeight),
            };
            _preInitPosition = position;
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }


        public Entity(Vector[] polyVertices, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = polyVertices;
            _preInitPosition = position;
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }


        public Entity(Texture texture, Vector[] polyVertices, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _texture = texture;
            _preInitVertices = polyVertices;
            _preInitPosition = position;
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }
        #endregion


        #region Props
        public bool IsStatic { get; private set; }

        public EntityBehaviors Behaviors { get; set; } = new EntityBehaviors();
        
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

                //If the game object is going from visible to invisible, fire the OnHide event
                if (prevValue && !_visible)
                {
                    if (OnHide != null)
                        OnHide.Invoke(this, new EventArgs());
                }
                else if (!prevValue && _visible)//Going from invisible to visible
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
            get { return new Rect((int)Position.X, (int)Position.Y, (int)BoundsWidth, (int)BoundsHeight); }    
        }

        /// <summary>
        /// Gets the location of the entity in the game world in pixel units.
        /// </summary>
        public Vector Position
        {
            get => IsInitialized ? 
                new Vector(Body.InternalPhysicsBody.X, Body.InternalPhysicsBody.Y) :
                _preInitPosition;
            set
            {
                if (IsInitialized)
                {
                    Body.InternalPhysicsBody.X = value.X;
                    Body.InternalPhysicsBody.Y = value.Y;
                }
                else
                {
                    _preInitPosition = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the vertices of the <see cref="Entity"/>.
        /// Cannot set the vertices if the <see cref="Entity"/> has already been initialized.
        /// </summary>
        /// <exception cref="Exception">Thrown when the vertices are trying to be set when the 
        /// <see cref="Entity"/> has already been initialized.</exception>
        public Vector[] Vertices
        {
            get
            {
                return Body.Vertices;
            }
            set
            {
                if (IsInitialized)
                    throw new Exception();

                _preInitVertices = value;
            }
        }

        /// <summary>
        /// Gets the width of the entity.
        /// </summary>
        public float BoundsWidth
        {
            get
            {
                var largestX = Body.Vertices != null ? Body.Vertices.Max(v => v.X) : 0;
                var smallestX = Body.Vertices != null ? Body.Vertices.Min(v => v.X) : 0;


                return largestX - smallestX;
            }
        }

        /// <summary>
        /// Gets the height of the entity.
        /// </summary>
        public float BoundsHeight
        {
            get
            {
                var largestY = Body.Vertices != null ? Body.Vertices.Max(v => v.Y) : 0;
                var smallestY = Body.Vertices != null ? Body.Vertices.Min(v => v.Y) : 0;


                return largestY - smallestY;
            }
        }

        /// <summary>
        /// Returns the half width of the <see cref="Entity"/>.
        /// </summary>
        public float BoundsHalfWidth => BoundsWidth / 2;

        /// Returns the half height of the <see cref="Entity"/>.
        /// </summary>
        public float BoundsHalfHeight => BoundsHeight / 2;

        /// <summary>
        /// Gets the texture of the game object.
        /// </summary>
        public Texture Texture
        {
            get => _texture;
            set => _texture = value;
        }

        public bool DebugDrawEnabled
        {
            get
            {
                return _debugDraw != null;
            }
            set
            {
                if (value)
                {
                    _debugDraw = PluginSystem.EnginePlugins.LoadPlugin<IDebugDraw>();
                }
                else
                {
                    _debugDraw = null;
                }
            }
        }

        internal PhysicsBody Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Entity"/> has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        public bool ContentLoaded { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Initializes the <see cref="Entity"/>.
        /// </summary>
        public virtual void Initialize()
        {
            if (_preInitVertices == null)
                throw new MissingVerticesException();

            CreateBody(_preInitVertices, _preInitPosition, IsStatic);
            IsInitialized = true;
        }


        public virtual void LoadContent(ContentLoader contentLoader)
        {
            ContentLoaded = true;
        }


        /// <summary>
        /// Updates the game object.
        /// </summary>
        public virtual void Update(EngineTime engineTime)
        {
            _engineTime = engineTime;

            foreach (IBehavior behavior in Behaviors)
            {
                behavior.Update(_engineTime);
            }
        }


        /// <summary>
        /// Renders the game object.
        /// </summary>
        /// <param name="renderer">The render used to render the object texture.</param>
        public virtual void Render(Renderer renderer)
        {
            if (_texture != null && Visible)
                renderer.Render(_texture, Position.X, Position.Y, Body.InternalPhysicsBody.Angle);

            //Render the physics bodies vertices to show its shape for debugging purposes
            if (DebugDrawEnabled)
                _debugDraw.Draw(renderer.InternalRenderer, Body.InternalPhysicsBody);
        }
        #endregion


        #region Private Methods
        private void CreateBody(Vector[] vertices, Vector position, bool isStatic)
        {
            Body = new PhysicsBody(vertices, position, isStatic: isStatic, friction: _preInitFriction);
        }
        #endregion
    }
}
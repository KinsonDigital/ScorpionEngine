using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Input;
using KDScorpionCore.Physics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Exceptions;
using KDScorpionEngine.Graphics;

namespace KDScorpionEngine.Entities
{
    /// <summary>
    /// Represents a base entity that all entities inherit from.
    /// </summary>
    public abstract class Entity : IUpdatable, IInitialize, IContentLoadable
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
        protected Texture _texture;
        private Vector _preInitPosition;
        private Vector[] _preInitVertices;
        private float _preInitFriction;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Entity"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="body">The physics body to inject.</param>
        internal Entity(IPhysicsBody body)
        {
            Body = body == null ? null : new PhysicsBody(body);
            Setup(null, Vector.Zero, 0f, false);
        }


        /// <summary>
        /// Creates a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = new [] { Vector.Zero, Vector.Zero, Vector.Zero };

            Setup(_preInitVertices, Vector.Zero, friction, isStaticBody);
        }


        /// <summary>
        /// Creates a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = new[] { Vector.Zero, Vector.Zero, Vector.Zero };
            Setup(_preInitVertices, position, friction, isStaticBody);
        }


        /// <summary>
        /// Creates a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="texture">The texture of the entity to render.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
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

            Setup(_preInitVertices, position, friction, isStaticBody);
        }


        /// <summary>
        /// Creates a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Vector[] vertices, Vector position, float friction = 0.2f, bool isStaticBody = false) =>
            Setup(vertices, position, friction, isStaticBody);


        /// <summary>
        /// Creates a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="texture">The texture of the entity to render.</param>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Texture texture, Vector[] vertices, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _texture = texture;
            Setup(vertices, position, friction, isStaticBody);
        }
        #endregion


        #region Props
        /// <summary>
        /// The physics body of the entity.
        /// </summary>
        internal PhysicsBody Body { get; set; }

        /// <summary>
        /// Gets a value indicating if the <see cref="Entity"/> is static and cannot moved.
        /// </summary>
        public bool IsStatic { get; private set; }

        /// <summary>
        /// Gets or sets the list of behaviors that the <see cref="Entity"/> will have.
        /// </summary>
        public EntityBehaviors Behaviors { get; set; } = new EntityBehaviors();
        
        /// <summary>
        /// Gets or sets a value indicating if the entity is rendered to the graphics surface.
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
        /// Gets the rectangular bounds of the <see cref="Entity"/>.
        /// </summary>
        public Rect Bounds => new Rect((int)Position.X, (int)Position.Y, (int)BoundsWidth, (int)BoundsHeight);

        /// <summary>
        /// Gets the position of the <see cref="Entity"/> in the game world in pixel units.
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
        /// Gets or sets the vertices that make up the physical shape of the <see cref="Entity"/>.
        /// Cannot change the vertices if the <see cref="Entity"/> has already been initialized.
        /// </summary>
        /// <exception cref="Exception">Thrown when the vertices are trying to be set when the 
        /// <see cref="Entity"/> has already been initialized.</exception>
        public Vector[] Vertices
        {
            get => IsInitialized ? Body.Vertices : _preInitVertices;
            set
            {
                //The vertices of the entity cannot be set after it has been initialized
                if (IsInitialized)
                    throw new EntityAlreadyInitializedException();

                _preInitVertices = value;
            }
        }

        /// <summary>
        /// Gets the width of the entity bounds.
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
        /// Gets the height of the entity bounds.
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
        /// Returns the half width of the <see cref="Entity"/> bounds.
        /// </summary>
        public float BoundsHalfWidth => BoundsWidth / 2;

        /// Returns the half height of the <see cref="Entity"/> bounds.
        /// </summary>
        public float BoundsHalfHeight => BoundsHeight / 2;

        /// <summary>
        /// Gets or sets the texture of the <see cref="Entity"/>.
        /// </summary>
        public Texture Texture
        {
            get => _texture;
            set => _texture = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if the debug draw outlines should be rendered.
        /// Set to true by default for game development purposes.
        /// </summary>
        public bool DebugDrawEnabled { get; set; } = true;

        /// <summary>
        /// Gets a value indicating if the <see cref="Entity"/> has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets a value indicating if the entities content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets the color of the debug draw outlines.
        /// </summary>
        public GameColor DebugDrawColor { get; set; } = new GameColor(255, 255, 255, 255);
        #endregion


        #region Public Methods
        /// <summary>
        /// Initializes the <see cref="Entity"/>.
        /// </summary>
        public virtual void Initialize()
        {
            CreateBody(_preInitVertices, _preInitPosition, IsStatic);
            IsInitialized = true;
        }


        /// <summary>
        /// Loads the entities content.
        /// </summary>
        /// <param name="contentLoader">The content loader that will be loading the content.</param>
        public virtual void LoadContent(ContentLoader contentLoader) => ContentLoaded = true;


        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        public virtual void Update(EngineTime engineTime)
        {
            _engineTime = engineTime;

            foreach (IBehavior behavior in Behaviors)
            {
                behavior.Update(_engineTime);
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the <see cref="Entity"/> using the given parameter.
        /// </summary>
        /// <param name="polyVertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        private void Setup(Vector[] polyVertices, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = polyVertices ?? (new Vector[0]);
            _preInitPosition = position == null ? Vector.Zero : position;
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }


        /// <summary>
        /// Creates the physics body of the <see cref="Entity"/> to be able to simulate physics between
        /// this <see cref="Entity"/> and other entities in the physics world.
        /// </summary>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        [ExcludeFromCodeCoverage]
        private void CreateBody(Vector[] vertices, Vector position, bool isStatic)
        {
            if (Body == null)
            {
                Body = new PhysicsBody(vertices, position, isStatic: isStatic, friction: _preInitFriction);
            }
            else
            {
                Body.Vertices = vertices;
                Body.X = position.X;
                Body.Y = position.Y;
            }
        }


        /// <summary>
        /// Renders the <see cref="Entity"/> to the graphics surface.
        /// </summary>
        /// <param name="renderer">The renderer that renders the <see cref="Entity"/>.</param>
        [ExcludeFromCodeCoverage]
        public virtual void Render(GameRenderer renderer) { }
        #endregion
    }
}
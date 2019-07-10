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
    //TODO: Add docs
    /// <summary>
    /// Represents an imovable game object with a texture, and a location.  
    /// Great for static objects that never move such as walls, a tree, etc.
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
        internal Entity(IPhysicsBody body)
        {
            Body = body == null ? null : new PhysicsBody(body);
            Setup(null, Vector.Zero, 0f, false);
        }


        public Entity(float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = new [] { Vector.Zero, Vector.Zero, Vector.Zero };

            Setup(_preInitVertices, Vector.Zero, friction, isStaticBody);
        }


        public Entity(Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = new[] { Vector.Zero, Vector.Zero, Vector.Zero };
            Setup(_preInitVertices, position, friction, isStaticBody);
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

            Setup(_preInitVertices, position, friction, isStaticBody);
        }


        public Entity(Vector[] polyVertices, Vector position, float friction = 0.2f, bool isStaticBody = false) =>
            Setup(polyVertices, position, friction, isStaticBody);


        public Entity(Texture texture, Vector[] polyVertices, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _texture = texture;
            Setup(polyVertices, position, friction, isStaticBody);
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
                return IsInitialized ? Body.Vertices : _preInitVertices;
            }
            set
            {
                //The vertices of the entity cannot be set after it has been initialized
                if (IsInitialized)
                    throw new EntityAlreadyInitializedException();

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

        /// <summary>
        /// Gets or sets a value indicating if the debug draw outlines should be rendered.
        /// Set to true by default for game development purposes.
        /// </summary>
        public bool DebugDrawEnabled { get; set; } = true;

        internal PhysicsBody Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Entity"/> has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

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


        public virtual void LoadContent(ContentLoader contentLoader) => ContentLoaded = true;


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
        #endregion


        #region Private Methods
        private void Setup(Vector[] polyVertices, Vector position, float friction = 0.2f, bool isStaticBody = false)
        {
            _preInitVertices = polyVertices ?? (new Vector[0]);
            _preInitPosition = position == null ? Vector.Zero : position;
            IsStatic = isStaticBody;
            _preInitFriction = friction;
        }

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


        [ExcludeFromCodeCoverage]
        public virtual void Render(GameRenderer renderer) { }
        #endregion
    }
}
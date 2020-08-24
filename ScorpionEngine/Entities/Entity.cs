// <copyright file="Entity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Exceptions;
    using KDScorpionEngine.Graphics;
    using Raptor;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;
    using Raptor.Physics;
    using Raptor.Plugins;

    /// <summary>
    /// Represents a base entity that all entities inherit from.
    /// </summary>
    public abstract class Entity : IUpdatable, IInitialize, IContentLoadable
    {
        protected bool usesPhysics = true;

        /// <summary>
        /// The engine time of the entity.
        /// </summary>
        protected EngineTime engineTime;

        /// <summary>
        /// The texture of the entity.
        /// </summary>
        protected Texture texture;
        private bool visible = true; // True if the entity will be drawn
        private Vector2 preInitPosition;
        private Vector2[] preInitVertices;
        private float preInitFriction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(float friction = 0.2f, bool isStaticBody = false)
        {
            this.preInitVertices = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero };

            Setup(this.preInitVertices, Vector2.Zero, friction, isStaticBody);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Vector2 position, float friction = 0.2f, bool isStaticBody = false)
        {
            this.preInitVertices = new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero };
            Setup(this.preInitVertices, position, friction, isStaticBody);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="texture">The texture of the entity to render.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Texture texture, Vector2 position, float friction = 0.2f, bool isStaticBody = false)
        {
            this.texture = texture;

            var halfWidth = texture.Width / 2;
            var halfHeight = texture.Height / 2;

            this.preInitVertices = new Vector2[4]
            {
                new Vector2(position.X - halfWidth, position.Y - halfHeight),
                new Vector2(position.X + halfWidth, position.Y - halfHeight),
                new Vector2(position.X + halfWidth, position.Y + halfHeight),
                new Vector2(position.X - halfWidth, position.Y + halfHeight),
            };

            Setup(this.preInitVertices, position, friction, isStaticBody);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Vector2[] vertices, Vector2 position, float friction = 0.2f, bool isStaticBody = false) =>
            Setup(vertices, position, friction, isStaticBody);

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="texture">The texture of the entity to render.</param>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(Texture texture, Vector2[] vertices, Vector2 position, float friction = 0.2f, bool isStaticBody = false)
        {
            this.texture = texture;
            Setup(vertices, position, friction, isStaticBody);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="body">The physics body to inject.</param>
        internal Entity(IPhysicsBody body)
        {
            Body = body == null ? null : new PhysicsBody(body);
            Setup(null, Vector2.Zero, 0f, false);
        }

        /// <summary>
        /// Occurs when the game object is going from hidden to shown.
        /// </summary>
        public event EventHandler OnShow;

        /// <summary>
        /// Occurs when the game object is shown to hidden.
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

        /// <summary>
        /// Gets a value indicating whether the <see cref="Entity"/> is static and cannot moved.
        /// </summary>
        public bool IsStatic { get; private set; }

        /// <summary>
        /// Gets or sets the list of behaviors that the <see cref="Entity"/> will have.
        /// </summary>
        public EntityBehaviors Behaviors { get; set; } = new EntityBehaviors();

        /// <summary>
        /// Gets or sets a value indicating whether the entity is rendered to the graphics surface.
        /// </summary>
        public bool Visible
        {
            get => this.visible;
            set
            {
                var prevValue = this.visible;

                this.visible = value;

                // If the game object is going from visible to invisible, fire the OnHide event
                if (prevValue && !this.visible)
                {
                    if (OnHide != null)
                    {
                        OnHide.Invoke(this, new EventArgs());
                    }
                }
                else if (!prevValue && this.visible)
                {
                    // Going from invisible to visible
                    if (OnShow != null)
                    {
                        OnShow.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Gets the rectangular bounds of the <see cref="Entity"/>.
        /// </summary>
        public Rect Bounds => new Rect((int)Position.X, (int)Position.Y, (int)BoundsWidth, (int)BoundsHeight);

        /// <summary>
        /// Gets or sets the position of the <see cref="Entity"/> in the game world in pixel units.
        /// </summary>
        public Vector2 Position
        {
            get => IsInitialized ?
                new Vector2(Body.X, Body.Y) :
                this.preInitPosition;
            set
            {
                if (IsInitialized)
                {
                    Body.X = value.X;
                    Body.Y = value.Y;
                }
                else
                {
                    this.preInitPosition = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the vertices that make up the physical shape of the <see cref="Entity"/>.
        /// Cannot change the vertices if the <see cref="Entity"/> has already been initialized.
        /// </summary>
        /// <exception cref="Exception">Thrown when the vertices are trying to be set when the
        /// <see cref="Entity"/> has already been initialized.</exception>
        public Vector2[] Vertices
        {
            get => IsInitialized ? Body.Vertices.ToArray() : this.preInitVertices;
            set
            {
                // The vertices of the entity cannot be set after it has been initialized
                if (IsInitialized)
                {
                    throw new EntityAlreadyInitializedException();
                }

                this.preInitVertices = value;
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
        /// Gets the half width of the <see cref="Entity"/> bounds.
        /// </summary>
        public float BoundsHalfWidth => BoundsWidth / 2;

        /// <summary>
        /// Gets the half height of the <see cref="Entity"/> bounds.
        /// </summary>
        public float BoundsHalfHeight => BoundsHeight / 2;

        /// <summary>
        /// Gets or sets the texture of the <see cref="Entity"/>.
        /// </summary>
        public Texture Texture
        {
            get => this.texture;
            set => this.texture = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the debug draw outlines should be rendered.
        /// </summary>
        /// <remarks>Set to true by default for game development purposes.</remarks>
        public bool DebugDrawEnabled { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Entity"/> has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entities content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets the color of the debug draw outlines.
        /// </summary>
        public GameColor DebugDrawColor { get; set; } = new GameColor(255, 255, 255, 255);

        /// <summary>
        /// Gets or sets the physics body of the entity.
        /// </summary>
        internal PhysicsBody Body { get; set; }

        /// <summary>
        /// Initializes the <see cref="Entity"/>.
        /// </summary>
        public virtual void Initialize()
        {
            CreateBody(this.preInitVertices, this.preInitPosition, IsStatic);
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
        /// <param name="engineTime">The engine time since the last frame.</param>
        public virtual void Update(EngineTime engineTime)
        {
            this.engineTime = engineTime;

            Behaviors.ToList().ForEach(b => b.Update(this.engineTime));
        }

        /// <summary>
        /// Renders the <see cref="Entity"/> to the graphics surface.
        /// </summary>
        /// <param name="renderer">The renderer that renders the <see cref="Entity"/>.</param>
        [ExcludeFromCodeCoverage]
        public virtual void Render(GameRenderer renderer)
        {
        }

        /// <summary>
        /// Sets up the <see cref="Entity"/> using the given parameter.
        /// </summary>
        /// <param name="polyVertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        private void Setup(Vector2[] polyVertices, Vector2 position, float friction = 0.2f, bool isStaticBody = false)
        {
            this.preInitVertices = polyVertices ?? Array.Empty<Vector2>();
            this.preInitPosition = position == null ? Vector2.Zero : position;
            IsStatic = isStaticBody;
            this.preInitFriction = friction;
        }

        /// <summary>
        /// Creates the physics body of the <see cref="Entity"/> to be able to simulate physics between
        /// this <see cref="Entity"/> and other entities in the physics world.
        /// </summary>
        /// <param name="vertices">The polygon vertices that make up the shape of the <see cref="Entity"/>.</param>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="isStatic">True if the body is static and cannot be moved by other objects.</param>
        [ExcludeFromCodeCoverage]
        private void CreateBody(Vector2[] vertices, Vector2 position, bool isStatic)
        {
            if (Body == null)
            {
                Body = new PhysicsBody(vertices, position, isStatic: isStatic, friction: this.preInitFriction);
            }
            else
            {
                // TODO: Get this working
                // Body.Vertices = vertices;
                Body.X = position.X;
                Body.Y = position.Y;
            }
        }
    }
}

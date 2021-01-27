// <copyright file="Entity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Content;
    using KDScorpionEngine.Graphics;
    using Raptor;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    /// <summary>
    /// Represents a base entity that all entities inherit from.
    /// </summary>
    public class Entity : IUpdatableObject, IContentLoadable
    {
        private readonly ILoader<AtlasData>? atlasLoader;
        private AtlasRepository atlasRepo = AtlasRepository.Instance;

        /// <summary>
        /// The engine time of the entity.
        /// </summary>
        protected int frameTime;

        /// <summary>
        /// The texture of the entity.
        /// </summary>
        protected ITexture? texture;
        private bool visible = true; // True if the entity will be drawn

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="position">The position of where to render the <see cref="Entity"/>.</param>
        /// <param name="friction">The friction of the body against other entities or surfaces.</param>
        /// <param name="isStaticBody">True if the body is static and cannot be moved by other objects.</param>
        public Entity(ILoader<AtlasData> atlasLoader, string atlasTextureName)
        {
            this.atlasLoader = atlasLoader;
            Name = atlasTextureName;
            TypeOfTexture = TextureType.Atlas;
        }

        public Entity(ILoader<ITexture> textureLoader, string textureName)
        {
            this.texture = textureLoader.Load(textureName);
            Name = textureName;
            TypeOfTexture = TextureType.Single;
        }

        public Entity(ITexture texture, string textureName)
        {
            this.texture = texture;
            Name = textureName;
            TypeOfTexture = TextureType.Single;
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

        public string Name { get; }

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
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)BoundsWidth, (int)BoundsHeight);

        /// <summary>
        /// Gets or sets the position of the <see cref="Entity"/> in the game world in pixel units.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the vertices that make up the physical shape of the <see cref="Entity"/>.
        /// Cannot change the vertices if the <see cref="Entity"/> has already been initialized.
        /// </summary>
        /// <exception cref="Exception">Thrown when the vertices are trying to be set when the
        /// <see cref="Entity"/> has already been initialized.</exception>
        public Vector2[] Vertices { get; set; }

        /// <summary>
        /// Gets or sets the type texture.
        /// </summary>
        public TextureType TypeOfTexture { get; set; } = TextureType.Single;

        /// <summary>
        /// Gets the width of the entity bounds.
        /// </summary>
        public float BoundsWidth => Texture.Width;

        /// <summary>
        /// Gets the height of the entity bounds.
        /// </summary>
        public float BoundsHeight => Texture.Height;

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
        public ITexture Texture
        {
            get
            {
                switch (TypeOfTexture)
                {
                    case TextureType.Single:
                        return this.texture;
                    case TextureType.Atlas:
                        return this.atlasRepo.GetAtlasTexture(Name);
                    default:
                        throw new Exception($"Uknown texture type of '{TypeOfTexture}'.");
                }
            }
        }

        public AtlasRegionRectangle[] AtlasRegions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the debug draw outlines should be rendered.
        /// </summary>
        /// <remarks>Set to true by default for game development purposes.</remarks>
        public bool DebugDrawEnabled { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether the entities content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets the color of the debug draw outlines.
        /// </summary>
        public Color DebugDrawColor { get; set; } = Color.White;

        /// <summary>
        /// Loads the entities content.
        /// </summary>
        /// <param name="contentLoader">The content loader that will be loading the content.</param>
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            atlasLoader.Load(Name);
            ContentLoaded = true;
        }

        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">The engine time since the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            Behaviors.ToList().ForEach(b => b.Update(gameTime));
        }

        /// <summary>
        /// Renders the <see cref="Entity"/> to the graphics surface.
        /// </summary>
        /// <param name="renderer">The renderer that renders the <see cref="Entity"/>.</param>
        [ExcludeFromCodeCoverage]
        public virtual void Render(Renderer renderer)
        {
        }
    }
}

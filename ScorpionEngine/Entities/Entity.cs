// <copyright file="Entity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    // TODO: Move comment code to the IEntity interface
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Exceptions;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    /// <summary>
    /// Represents a base entity that all entities inherit from.
    /// </summary>
    public class Entity : IEntity
    {
        private ITexture? singleTexture;
        private bool visible = true;

        public Entity()
        {
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public Entity(string name)
        {
            ID = Guid.NewGuid();
            RenderSection.TextureName = name;
            RenderSection.TypeOfTexture = TextureType.WholeTexture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <param name="atlasData">The texture atlas data.</param>
        /// <param name="animator">Manages the animation.</param>
        /// <remarks>Creates an animated type entity.</remarks>
        public Entity(string atlasName, string subTextureName, IAtlasData atlasData, IAnimator animator)
        {
            ID = Guid.NewGuid();
            AtlasData = atlasData;
            RenderSection.Animator = animator;
            RenderSection.TextureName = atlasName;
            RenderSection.SubTextureName = subTextureName;
            RenderSection.TypeOfTexture = TextureType.SubTexture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The name of the sub texture contained in the atlas texture.</param>
        /// <param name="atlasData">The texture atlas data.</param>
        /// <remarks>Creates a non-animating <see cref="Entity"/>.</remarks>
        public Entity(string atlasName, string subTextureName, IAtlasData atlasData)
        {
            ID = Guid.NewGuid();
            AtlasData = atlasData;
            RenderSection.TextureName = atlasName;
            RenderSection.SubTextureName = subTextureName;
            RenderSection.TypeOfTexture = TextureType.SubTexture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <param name="texture">The texture to be rendered.</param>
        /// <remarks>Creates a non-animating <see cref="Entity"/>.</remarks>
        public Entity(string textureName, ITexture texture)
        {
            this.singleTexture = texture;

            ID = Guid.NewGuid();
            RenderSection.TextureName = textureName;
            RenderSection.TypeOfTexture = TextureType.WholeTexture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <remarks>
        ///     This will create a non animating entity and is used by the factories.
        /// </remarks>
        public Entity(string atlasName, string subTextureName)
        {
            ID = Guid.NewGuid();

            RenderSection.TextureName = atlasName;
            RenderSection.SubTextureName = subTextureName;
            RenderSection.TypeOfTexture = TextureType.SubTexture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <param name="animator">Manages the animation.</param>
        /// <remarks>
        ///     Creates an animating type entity and is used by the factories.
        /// </remarks>
        public Entity(string atlasName, string subTextureName, IAnimator animator)
        {
            ID = Guid.NewGuid();
            RenderSection.TextureName = atlasName;
            RenderSection.SubTextureName = subTextureName;
            RenderSection.Animator = animator;
            RenderSection.TypeOfTexture = TextureType.SubTexture;
        }

        /// <summary>
        /// Occurs when the game object is going from hidden to shown.
        /// </summary>
        public event EventHandler<EventArgs>? OnShow;

        /// <summary>
        /// Occurs when the game object is shown to hidden.
        /// </summary>
        public event EventHandler<EventArgs>? OnHide;

        /// <summary>
        /// Occurs when a key has been pressed.
        /// </summary>
        public event EventHandler<KeyEventArgs>? OnKeyPressed;

        /// <summary>
        /// Occurs when a key has been released.
        /// </summary>
        public event EventHandler<KeyEventArgs>? OnKeyReleased;

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        public string Name => RenderSection.Animator is null ? RenderSection.TextureName : RenderSection.SubTextureName;

        public Guid ID { get; }

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
                    this.OnHide?.Invoke(this, EventArgs.Empty);
                }
                else if (!prevValue && this.visible)
                {
                    this.OnShow?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="Entity"/> in the game world in pixel units.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the angle of the entity.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the section of the texture to render.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     If the <see cref="RenderSection.TypeOfTexture"/> is set to <see cref="TextureType.WholeTexture"/>,
        ///     then the entire texture will be rendered.
        /// </para>
        ///
        /// <param>
        ///     If the <see cref="RenderSection.TypeOfTexture"/> is set to <see cref="TextureType.SubTexture"/>,
        ///     then only a defined area of the texture will be rendered.  This is common for texture atlases
        ///     and could be a particular section for an animation or just a static section for a noon-animating entity.
        /// </param>
        /// </remarks>
        public RenderSection RenderSection { get; set; } = new RenderSection();

        /// <summary>
        /// Gets or sets the texture of the <see cref="Entity"/>.
        /// </summary>
        public ITexture? Texture
        {
            get
            {
                switch (RenderSection.TypeOfTexture)
                {
                    case TextureType.WholeTexture:
                        return this.singleTexture;
                    case TextureType.SubTexture:
                        return AtlasData?.Texture;
                    default:
                        throw new TextureTypeException($"Unknown '{nameof(TextureType)}' value of '{RenderSection.TypeOfTexture}'.");
                }
            }
            set
            {
                switch (RenderSection.TypeOfTexture)
                {
                    case TextureType.WholeTexture:
                        this.singleTexture = value;
                        break;
                    case TextureType.SubTexture:
                        if (AtlasData is null)
                        {
                            throw new InvalidOperationException("The atlas data must not be null when setting the texture for sub texture type.");
                        }

                        if (value is null)
                        {
                            throw new InvalidOperationException("The texture atlas must not be null.");
                        }

                        AtlasData.Texture = value;
                        break;
                    default:
                        throw new TextureTypeException($"Unknown '{nameof(TextureType)}' value of '{RenderSection.TypeOfTexture}'.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the texture atlas.
        /// </summary>
        public IAtlasData? AtlasData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the debug draw outlines should be rendered.
        /// </summary>
        /// <remarks>Set to true by default for game development purposes.</remarks>
        public bool DebugDrawEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether the entities content has been loaded.
        /// </summary>
        public bool ContentLoaded { get; private set; }

        /// <summary>
        /// Gets or sets the color of the debug draw outlines.
        /// </summary>
        public Color DebugDrawColor { get; set; } = Color.White;

        public bool Enabled { get; set; } = true;

        /// <inheritdoc/>
        public virtual void Init()
        {
            ContentLoaded = false;
        }

        /// <inheritdoc/>
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            if (contentLoader is null)
            {
                throw new ArgumentNullException(nameof(contentLoader), "The parameter must not be null.");
            }

            if (ContentLoaded)
            {
                return;
            }

            switch (RenderSection.TypeOfTexture)
            {
                case TextureType.WholeTexture:
                    if (this.singleTexture is null)
                    {
                        this.singleTexture = contentLoader.Load<ITexture>(RenderSection.TextureName);
                    }

                    RenderSection.RenderBounds = new Rectangle(0, 0, this.singleTexture.Width, this.singleTexture.Height);
                    break;
                case TextureType.SubTexture:
                    if (AtlasData is null)
                    {
                        AtlasData = contentLoader.Load<IAtlasData>(RenderSection.TextureName);
                    }

                    if (RenderSection.Animator is null)
                    {
                        // Single non animated section of the atlas
                        RenderSection.RenderBounds = AtlasData.GetFrames(RenderSection.SubTextureName).Select(f => f.Bounds).SingleOrDefault();
                    }
                    else
                    {
                        RenderSection.Animator.Frames = AtlasData.GetFrames(RenderSection.SubTextureName).Select(f => f.Bounds).ToArray();
                    }

                    break;
                default:
                    break;
            }

            ContentLoaded = true;
        }

        /// <inheritdoc/>
        public virtual void UnloadContent(IContentLoader contentLoader)
        {
            AtlasData = null;
            this.singleTexture = null;
        }

        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">The engine time since the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            Behaviors.ToList().ForEach(b => b.Update(gameTime));
            RenderSection.Animator?.Update(gameTime);

            if (RenderSection.TypeOfTexture == TextureType.SubTexture && !(RenderSection.Animator is null))
            {
                RenderSection.RenderBounds = RenderSection.Animator.CurrentFrameBounds;
            }
        }
    }
}

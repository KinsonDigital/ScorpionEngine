// <copyright file="Entity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Collections.Generic;
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
    /// Represents an entity that can be updated and rendered in the game.
    /// </summary>
    public class Entity : IEntity
    {
        private ITexture? singleTexture;
        private bool visible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity() => ID = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public Entity(string name)
        {
            ID = Guid.NewGuid();
            SectionToRender.TextureName = name;
            SectionToRender.TypeOfTexture = TextureType.WholeTexture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <param name="atlasData">The texture atlas data.</param>
        /// <param name="animator">Manages the animation.</param>
        /// <remarks>Creates an animated type entity.</remarks>
        public Entity(string atlasTextureName, string subTextureName, IAtlasData atlasData, IAnimator animator)
        {
            ID = Guid.NewGuid();
            AtlasData = atlasData;
            SectionToRender = RenderSection.CreateAnimatedSubTexture(atlasTextureName, subTextureName, animator);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The name of the sub texture contained in the atlas texture.</param>
        /// <param name="atlasData">The texture atlas data.</param>
        /// <remarks>Creates a non-animating <see cref="Entity"/>.</remarks>
        public Entity(string atlasTextureName, string subTextureName, IAtlasData atlasData)
        {
            ID = Guid.NewGuid();
            AtlasData = atlasData;
            SectionToRender = RenderSection.CreateNonAnimatedSubTexture(atlasTextureName, subTextureName);
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
            SectionToRender = RenderSection.CreateNonAnimatedWholeTexture(textureName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <remarks>
        ///     This will create a non animating entity and is used by the factories.
        /// </remarks>
        public Entity(string atlasTextureName, string subTextureName)
        {
            ID = Guid.NewGuid();

            SectionToRender = RenderSection.CreateNonAnimatedSubTexture(atlasTextureName, subTextureName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <param name="animator">Manages the animation.</param>
        /// <remarks>
        ///     Creates an animating type entity and is used by the factories.
        /// </remarks>
        public Entity(string atlasTextureName, string subTextureName, IAnimator animator)
        {
            ID = Guid.NewGuid();
            SectionToRender = RenderSection.CreateAnimatedSubTexture(atlasTextureName, subTextureName, animator);
        }

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? OnShow;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? OnHide;

        /// <inheritdoc/>
        public event EventHandler<KeyEventArgs>? OnKeyPressed;

        /// <inheritdoc/>
        public event EventHandler<KeyEventArgs>? OnKeyReleased;

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                if (SectionToRender.Animator is null)
                {
                    return SectionToRender.TextureName;
                }

                return string.IsNullOrEmpty(SectionToRender.SubTextureName) ? string.Empty : SectionToRender.SubTextureName;
            }
        }

        /// <inheritdoc/>
        public Guid ID { get; }

        /// <inheritdoc/>
        public Vector2 Position { get; set; }

        /// <inheritdoc/>
        public float Angle { get; set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; } = true;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool ContentLoaded { get; private set; }

        /// <inheritdoc/>
        public bool FlippedHorizontally { get; set; }

        /// <inheritdoc/>
        public bool FlippedVertically { get; set; }

        /// <inheritdoc/>
        public ITexture? Texture
        {
            get
            {
                switch (SectionToRender.TypeOfTexture)
                {
                    case TextureType.WholeTexture:
                        return this.singleTexture;
                    case TextureType.SubTexture:
                        return AtlasData?.Texture;
                    default:
                        throw new InvalidTextureTypeException($"Unknown '{nameof(TextureType)}' value of '{SectionToRender.TypeOfTexture}'.");
                }
            }
            set
            {
                switch (SectionToRender.TypeOfTexture)
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
                        throw new InvalidTextureTypeException($"Unknown '{nameof(TextureType)}' value of '{SectionToRender.TypeOfTexture}'.");
                }
            }
        }

        /// <inheritdoc/>
        public IAtlasData? AtlasData { get; set; }

        /// <inheritdoc/>
        public RenderSection SectionToRender { get; set; } = new RenderSection();

        /// <inheritdoc/>
        public EntityBehaviors Behaviors { get; set; } = new EntityBehaviors();

        /// <inheritdoc/>
        public bool DebugDrawEnabled { get; set; }

        /// <inheritdoc/>
        public Color DebugDrawColor { get; set; } = Color.White;

        /// <inheritdoc/>
        public List<IEntity> Entities { get; set; } = new List<IEntity>();

        /// <inheritdoc/>
        public virtual void Init() => ContentLoaded = false;

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

            switch (SectionToRender.TypeOfTexture)
            {
                case TextureType.WholeTexture:
                    if (this.singleTexture is null)
                    {
                        this.singleTexture = contentLoader.Load<ITexture>(SectionToRender.TextureName);
                    }

                    SectionToRender.RenderBounds = new Rectangle(0, 0, this.singleTexture.Width, this.singleTexture.Height);
                    break;
                case TextureType.SubTexture:
                    if (AtlasData is null)
                    {
                        AtlasData = contentLoader.Load<IAtlasData>(SectionToRender.TextureName);
                    }

                    var subTextureName = string.IsNullOrEmpty(SectionToRender.SubTextureName)
                        ? string.Empty
                        : SectionToRender.SubTextureName;

                    if (SectionToRender.Animator is null)
                    {
                        // Single non animated section of the atlas
                        SectionToRender.RenderBounds =
                            AtlasData.GetFrames(subTextureName)
                            .Select(f => f.Bounds).SingleOrDefault();
                    }
                    else
                    {
                        SectionToRender.Animator.Frames =
                            AtlasData.GetFrames(subTextureName)
                                .Select(f => f.Bounds).ToArray().ToReadOnlyCollection();
                    }

                    break;
                default:
                    throw new InvalidTextureTypeException();
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
            SectionToRender.Animator?.Update(gameTime);

            if (SectionToRender.TypeOfTexture == TextureType.SubTexture && !(SectionToRender.Animator is null))
            {
                SectionToRender.RenderBounds = SectionToRender.Animator.CurrentFrameBounds;
            }
        }

        /// <inheritdoc/>
        public virtual void Render(IRenderer renderer)
            => Entities.ForEach(e =>
            {
                renderer.Render(e);
            });
    }
}

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
    public class Entity : IUpdatableObject, IContentLoadable, ICanInitialize
    {
        private readonly string? atlasName;
        private ITexture? singleTexture;
        private bool visible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasName">The name of the texture atlas that contains the sub texture.</param>
        /// <param name="subTextureName">The sub texture in a texture atlas.</param>
        /// <remarks>This will create a non animating entity.</remarks>
        internal Entity(string atlasName, string subTextureName)
        {
            this.atlasName = atlasName;
            Name = subTextureName;
            TypeOfTexture = TextureType.Atlas;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasName"></param>
        /// <param name="subTextureName"></param>
        /// <param name="animator"></param>
        /// /// <remarks>Creates an animating type entity.</remarks>
        internal Entity(string atlasName, string subTextureName, IAnimator animator)
        {
            this.atlasName = atlasName;
            Name = subTextureName;
            Animator = animator;
            TypeOfTexture = TextureType.Atlas;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public Entity(string name) => Name = name;

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
            this.atlasName = atlasName;
            Name = subTextureName;
            AtlasData = atlasData;
            Animator = animator;
            TypeOfTexture = TextureType.Atlas;
            ContentLoaded = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="atlasData">The texture atlas data.</param>
        /// <param name="atlasName">The name of the texture atlas that contains the sub texture.</param>
        /// <remarks>Creates a non-animating <see cref="Entity"/>.</remarks>
        public Entity(IAtlasData atlasData, string atlasName)
        {
            AtlasData = atlasData;
            Name = atlasName;
            TypeOfTexture = TextureType.Atlas;
            ContentLoaded = true;
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
            Name = textureName;
            TypeOfTexture = TextureType.Single;
            ContentLoaded = true;
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
        public string Name { get; }

        /// <summary>
        /// Gets or sets the animator.
        /// </summary>
        public IAnimator? Animator { get; set; }

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
        /// Gets the rectangular bounds of the <see cref="Entity"/>.
        /// </summary>
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)TextureBoundsWidth, (int)TextureBoundsHeight);

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
        /// Gets the position of the texture to render.
        /// </summary>
        public Vector2 TexturePosition
        {
            get
            {
                // TODO: This is proof of concept.  This needs to be improved for use and pref
                // It is not ideal to do a LINQ query every single frame for an atlas for non animated texture

                if (IsAnimated())
                {
                    return new Vector2(Animator.CurrentFrameBounds.X, Animator.CurrentFrameBounds.Y);
                }
                else
                {
                    if (NotAnimatedWithAtlasTexture())
                    {
                        if (AtlasData is null)
                        {
                            throw new Exception("The AtlasData cannot be null");
                        }

                        var singleFrame = AtlasData.GetFrames(Name).Select(f => f.Bounds).SingleOrDefault();

                        // TODO: create extension helper method that converts a rectangle to a Vector2
                        return new Vector2(singleFrame.X, singleFrame.Y);
                    }
                    else if (NotAnimatedWithSingleTexture())
                    {
                        return Vector2.Zero;
                    }
                }

                return Vector2.Zero;
            }
        }

        /// <summary>
        /// Gets the width of the entity bounds.
        /// </summary>
        public int TextureBoundsWidth
        {
            get
            {
                if (IsAnimated())
                {
                    return Animator is null ? 0 : Animator.CurrentFrameBounds.Width;
                }
                else if (NotAnimatedWithAtlasTexture())
                {
                    var singleFrame = AtlasData.GetFrames(Name).Select(f => f.Bounds).SingleOrDefault();

                    return singleFrame.Width;
                }
                else
                {
                    return Texture.Width;
                }
            }
        }

        /// <summary>
        /// Gets the height of the entity bounds.
        /// </summary>
        public int TextureBoundsHeight
        {
            get
            {
                if (IsAnimated())
                {
                    return Animator is null ? 0 : Animator.CurrentFrameBounds.Height;
                }
                else if (NotAnimatedWithAtlasTexture())
                {
                    var singleFrame = AtlasData.GetFrames(Name).Select(f => f.Bounds).SingleOrDefault();

                    return singleFrame.Height;
                }
                else
                {
                    return Texture.Height;
                }
            }
        }

        /// <summary>
        /// Gets the half width of the <see cref="Entity"/> bounds.
        /// </summary>
        public int TextureBoundsHalfWidth => TextureBoundsWidth / 2;

        /// <summary>
        /// Gets the half height of the <see cref="Entity"/> bounds.
        /// </summary>
        public int TextureBoundsHalfHeight => TextureBoundsHeight / 2;

        /// <summary>
        /// Gets or sets the texture of the <see cref="Entity"/>.
        /// </summary>
        public ITexture? Texture
        {
            get
            {
                switch (TypeOfTexture)
                {
                    case TextureType.Single:
                        return this.singleTexture;
                    case TextureType.Atlas:
                        return AtlasData?.Texture;
                    default:
                        throw new Exception($"Uknown '{nameof(TextureType)}' value of '{TypeOfTexture}'");
                }
            }
            set
            {
                switch (TypeOfTexture)
                {
                    case TextureType.Single:
                        this.singleTexture = value;
                        break;
                    case TextureType.Atlas:
                        if (AtlasData is null)
                        {
                            throw new Exception("The atlas data must not be null setting the texture for an atlas texture type.");
                        }

                        throw new Exception("Cannot set AtlasData.Texture property until Raptor library is updated. (v0.23.0)");
                        break;
                    default:
                        break;
                }
            }
        }

        public IAtlasData? AtlasData { get; set; }

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

        public virtual void Init()
        {

        }

        /// <summary>
        /// Loads the entities content.
        /// </summary>
        /// <param name="contentLoader">The content loader that will be loading the content.</param>
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            if (ContentLoaded)
            {
                return;
            }

            switch (TypeOfTexture)
            {
                case TextureType.Single:
                    this.singleTexture = contentLoader.Load<ITexture>(Name);
                    break;
                case TextureType.Atlas:
                    AtlasData = contentLoader.Load<IAtlasData>(this.atlasName);

                    if (!(Animator is null) && !(AtlasData is null))
                    {
                        Animator.Frames = AtlasData.GetFrames(Name).Select(f => f.Bounds).ToArray();
                    }

                    break;
                default:
                    break;
            }

            ContentLoaded = true;
        }

        /// <summary>
        /// Updates the <see cref="Entity"/>.
        /// </summary>
        /// <param name="gameTime">The engine time since the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            Behaviors.ToList().ForEach(b => b.Update(gameTime));
            Animator?.Update(gameTime);
        }

        /// <summary>
        /// Renders the <see cref="Entity"/> to the graphics surface.
        /// </summary>
        /// <param name="renderer">The renderer that renders the <see cref="Entity"/>.</param>
        [ExcludeFromCodeCoverage]
        public virtual void Render(Renderer renderer)
        {
        }

        private bool IsAnimated() => !(Animator is null) && TypeOfTexture == TextureType.Atlas;

        private bool NotAnimatedWithSingleTexture()
            => TypeOfTexture == TextureType.Atlas && Animator is null && !(this.singleTexture is null) && AtlasData is null;

        private bool NotAnimatedWithAtlasTexture()
            => TypeOfTexture == TextureType.Atlas && Animator is null && this.singleTexture is null && !(AtlasData is null);
    }
}

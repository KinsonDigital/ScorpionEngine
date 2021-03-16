// <copyright file="IEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    /// <summary>
    /// Represents an entity that can be updated and rendered in the game.
    /// </summary>
    public interface IEntity : IUpdatableObject, IContentLoadable, IContentUnloadable, ICanInitialize
    {
        /// <summary>
        /// Occurs when the game object is going from hidden to visible.
        /// </summary>
        event EventHandler<EventArgs>? OnShow;

        /// <summary>
        /// Occurs when the game object is visible to hidden.
        /// </summary>
        event EventHandler<EventArgs>? OnHide;

        /// <summary>
        /// Occurs when a key has been pressed.
        /// </summary>
        event EventHandler<KeyEventArgs>? OnKeyPressed;

        /// <summary>
        /// Occurs when a key has been released.
        /// </summary>
        event EventHandler<KeyEventArgs>? OnKeyReleased;

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a unique ID of the texture.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Gets or sets the position of the <see cref="Entity"/> in the game world in pixel units.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the angle of the entity.
        /// </summary>
        float Angle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity will be updated.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is rendered to the graphics surface.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets a value indicating whether the entities content has been loaded.
        /// </summary>
        bool ContentLoaded { get; }

        /// <summary>
        /// Gets or sets the texture of the <see cref="Entity"/>.
        /// </summary>
        ITexture? Texture { get; set; }

        /// <summary>
        /// Gets or sets the texture atlas.
        /// </summary>
        IAtlasData? AtlasData { get; set; }

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
        RenderSection SectionToRender { get; set; }

        /// <summary>
        /// Gets or sets the list of behaviors that the <see cref="Entity"/> will have.
        /// </summary>
        EntityBehaviors Behaviors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the debug draw outlines should be rendered.
        /// </summary>
        /// <remarks>Set to true by default for game development purposes.</remarks>
        bool DebugDrawEnabled { get; set; }

        /// <summary>
        /// Gets or sets the color of the debug draw outlines.
        /// </summary>
        Color DebugDrawColor { get; set; }
    }
}

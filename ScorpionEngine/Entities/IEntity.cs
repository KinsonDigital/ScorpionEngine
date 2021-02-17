// <copyright file="Entity.cs" company="KinsonDigital">
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

    // TODO: Get the comment code from the Entity class and put it in here

    public interface IEntity : IUpdatableObject, IContentLoadable, IContentUnloadable, ICanInitialize
    {
        IAnimator? Animator { get; set; }

        IAtlasData? AtlasData { get; set; }

        EntityBehaviors Behaviors { get; set; }

        bool ContentLoaded { get; }

        Color DebugDrawColor { get; set; }

        bool DebugDrawEnabled { get; set; }

        string Name { get; }

        Guid ID { get; }

        Vector2 Position { get; set; }

        float Angle { get; set; }

        RenderSection RenderSection { get; set; }

        ITexture? Texture { get; set; }

        bool Visible { get; set; }

        bool IsRecycled { get; set; }

        event EventHandler<EventArgs>? OnHide;

        event EventHandler<KeyEventArgs>? OnKeyPressed;

        event EventHandler<KeyEventArgs>? OnKeyReleased;

        event EventHandler<EventArgs>? OnShow;

        void Init();

        void LoadContent(IContentLoader contentLoader);

        void Update(GameTime gameTime);

        void Recycle();
    }
}

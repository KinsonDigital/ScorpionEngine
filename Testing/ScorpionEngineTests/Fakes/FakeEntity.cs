﻿// <copyright file="FakeEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS0067 // The event is never used.
namespace KDScorpionEngineTests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngineTests.Entities;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    /// <summary>
    /// Used for testing in the <see cref="EntityPoolTests"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeEntity : IEntity
    {
        public event EventHandler<EventArgs>? OnHide;

        public event EventHandler<KeyEventArgs>? OnKeyPressed;

        public event EventHandler<KeyEventArgs>? OnKeyReleased;

        public event EventHandler<EventArgs>? OnShow;

        public IAnimator Animator { get; set; }

        public IAtlasData AtlasData { get; set; }

        public EntityBehaviors Behaviors { get; set; }

        public bool ContentLoaded { get; private set; }

        public Color DebugDrawColor { get; set; }

        public bool DebugDrawEnabled { get; set; }

        public string Name { get; }

        public Guid ID { get; } = Guid.NewGuid();

        public Vector2 Position { get; set; }

        public float Angle { get; set; }

        public RenderSection SectionToRender { get; set; } = new RenderSection();

        public ITexture Texture { get; set; }

        public bool Visible { get; set; } = true;

        public bool Enabled { get; set; } = true;

        public bool LoadContentInvoked { get; set; }

        public bool UpdateInvoked { get; set; }

        public List<IEntity> Entities { get; }

        public bool FlippedHorizontally { get; set; }

        public bool FlippedVertically { get; set; }

        ReadOnlyCollection<IEntity> IEntity.Entities { get; }

        public bool IsInitialized { get; private set; }

        public void Init() => IsInitialized = true;

        public void LoadContent(IContentLoader contentLoader)
        {
            ContentLoaded = true;
            Texture = contentLoader.Load<ITexture>(SectionToRender.TextureName);
        }

        public void UnloadContent(IContentLoader contentLoader)
        {
        }

        public void Update(GameTime gameTime)
        {
            UpdateInvoked = true;
        }

        public void Render(IRenderer renderer) => throw new NotImplementedException();

        public void AddChildEntity(IEntity childEntity) => throw new NotImplementedException();
    }
}

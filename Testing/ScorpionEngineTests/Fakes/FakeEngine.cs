// <copyright file="FakeEngine.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable CA1062 // Validate arguments of public methods
namespace KDScorpionEngineTests.Fakes
{
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;

    /// <summary>
    /// Provides a fake implementation of the <see cref="Engine"/> class.
    /// </summary>
    public class FakeEngine : Engine
    {
        public FakeEngine(IContentLoader contentLoader)
            : base(10, 20)
        {
        }

        public bool InitInvoked { get; set; }

        public bool LoadContentInvoked { get; set; }

        public bool UpdateInvoked { get; set; }

        public bool RenderInvoked { get; set; }

        public new void Start()
        {
        }

        public override void Init()
        {
            InitInvoked = true;
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            LoadContentInvoked = true;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateInvoked = true;
        }

        public override void Render(Renderer renderer)
        {
            RenderInvoked = true;
        }
    }
}

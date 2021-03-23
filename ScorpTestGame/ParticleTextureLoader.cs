// <copyright file="ParticleTextureLoader.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame
{
    using KDParticleEngine;
    using Raptor.Content;
    using Raptor.Graphics;

    public class ParticleTextureLoader : ITextureLoader<ITexture>
    {
        private readonly IContentLoader contentLoader;

        public ParticleTextureLoader(IContentLoader contentLoader)
        {
            this.contentLoader = contentLoader;
        }

        public ITexture LoadTexture(string textureName) => this.contentLoader.Load<ITexture>(textureName);
    }
}

// <copyright file="ParticleTextureLoader.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using KDParticleEngine;
    using Raptor.Content;
    using Raptor.Graphics;

    public class ParticleTextureLoader : ITextureLoader<Texture>
    {
        private readonly ContentLoader contentLoader;

        public ParticleTextureLoader(ContentLoader contentLoader)
        {
            this.contentLoader = contentLoader;
        }

        public Texture LoadTexture(string textureName)
        {
            return this.contentLoader.LoadTexture(textureName);
        }
    }
}

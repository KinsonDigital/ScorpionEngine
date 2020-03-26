using KDParticleEngine;
using Raptor.Content;
using Raptor.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScorpTestGame
{
    public class ParticleTextureLoader : ITextureLoader<Texture>
    {
        private readonly ContentLoader _contentLoader;

        public ParticleTextureLoader(ContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }


        public Texture LoadTexture(string textureName)
        {
            return _contentLoader.LoadTexture(textureName);
        }
    }
}

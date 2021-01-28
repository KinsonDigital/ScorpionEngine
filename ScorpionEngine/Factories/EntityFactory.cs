using System;
using System.Collections.Generic;
using System.Text;
using KDScorpionEngine.Content;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;
using Raptor.Content;
using Raptor.Factories;
using Raptor.Graphics;

namespace KDScorpionEngine.Factories
{
    public static class EntityFactory
    {
        private static IContentLoader contentLoader;

        static EntityFactory()
        {
            contentLoader = ContentLoaderFactory.CreateContentLoader();
        }

        public static Entity CreateNonAnimatedFromTextureAtlas(string atlasName, string subTextureName)
        {
            return new Entity(atlasName, subTextureName);
        }

        public static Entity CreateNonAnimatedFromTexture(string name)
        {
            return new Entity(name, contentLoader.Load<ITexture>(name));
        }

        public static Entity CreateAnimated(string atlasName, string subTextureName)
            => new Entity(atlasName, subTextureName, new Animator());
    }
}

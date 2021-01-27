using System;
using System.Collections.Generic;
using System.Text;
using KDScorpionEngine.Content;
using KDScorpionEngine.Entities;
using Raptor.Content;
using Raptor.Graphics;

namespace KDScorpionEngine.Factories
{
    public class EntityFactory
    {
        private static EntityFactory Instance = new EntityFactory();
        private readonly ILoader<AtlasData> atlasDataLoader;
        private readonly ILoader<ITexture> textureLoader;

        static EntityFactory()
        {
            // DO NOT DELETE THIS CONSTRUCTOR!!
            // This explicit static constructor tells the c# compiler not to mark type as 'beforefieldinit'

            // This singleton pattern came from : https://csharpindepth.com/articles/singleton
        }

        private EntityFactory()
        {
            this.atlasDataLoader = IoC.Container.GetInstance<ILoader<AtlasData>>();
            this.textureLoader = IoC.Container.GetInstance<ILoader<ITexture>>();
        }

        public static EntityFactory CreateFactory() => Instance;

        public Entity CreateNonAnimated(string textureName)
        {
            return new Entity(this.textureLoader, textureName);
        }

        public Entity CreateAnimated(string textureName)
        {
            return new Entity(this.atlasDataLoader, textureName);
        }
    }
}

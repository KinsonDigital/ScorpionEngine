using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using Newtonsoft.Json;
using Raptor.Content;
using Raptor.Graphics;

namespace KDScorpionEngine.Content
{
    internal class AtlasDataLoader : ILoader<AtlasData>
    {
        private readonly AtlasRepository atlasRepo;
        private readonly IContentSource contentSource;
        private readonly ILoader<ITexture> textureLoader;
        private readonly IFile file;

        public AtlasDataLoader(IContentSource contentSource, ILoader<ITexture> textureLoader, IFile file)
        {
            //TODO: here is where you would use the new ContentSource factory from Raptor to create the content source
            //Then you can remove the 'contentSource' param
            this.atlasRepo = AtlasRepository.Instance;
            this.contentSource = contentSource;
            this.textureLoader = textureLoader;
            this.file = file;
        }

        public AtlasData Load(string contentNameOrPath)
        {
            if (this.atlasRepo.AtlasLoaded(contentNameOrPath))
            {
                return this.atlasRepo.GetAtlasData(contentNameOrPath);
            }

            var contentPath = this.contentSource.GetContentPath(contentNameOrPath);

            var rawData = this.file.ReadAllText(contentPath);
            var atlasSpriteData = JsonConvert.DeserializeObject<AtlasSpriteData[]>(rawData);

            var atlasTexture = this.textureLoader.Load(contentNameOrPath);

            var atlasData = new AtlasData(atlasSpriteData, atlasTexture, contentNameOrPath);

            this.atlasRepo.AddAtlasData(contentNameOrPath, atlasData);

            return atlasData;
        }
    }
}

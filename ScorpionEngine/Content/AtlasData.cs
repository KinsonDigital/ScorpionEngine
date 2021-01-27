// <copyright file="AtlasData.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Content
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using Raptor.Graphics;

    /// <summary>
    /// Holds texture atlas data.
    /// </summary>
    // TODO: Look into using this later during the building of a test game.
    [ExcludeFromCodeCoverage]
    public class AtlasData
    {
        private readonly AtlasRepository atlasRepo;
        private readonly AtlasSpriteData[] atlasSprites;


        // TODO: check to see if this copy by reference is needed

        /// <summary>
        /// Initializes a new instance of the <see cref="AtlasData"/> class.
        /// </summary>
        /// <param name="atlasSubTexutureData">The sub texture data of all of the sub textures in the atlas.</param>
        public AtlasData(AtlasSpriteData[] atlasSubTexutureData, ITexture texture, string atlasName)
        {
            this.atlasRepo = AtlasRepository.Instance;

            this.atlasSprites = atlasSubTexutureData;
            Texture = texture;
            AtlasName = atlasName;
        }

        /// <summary>
        /// Gets a list of all of the sub texture ID's.
        /// </summary>
        public List<string> FrameNameList => this.atlasSprites.Select(item => item.Name).ToList();

        public string AtlasName { get; }

        public ITexture Texture { get; }

        /// <summary>
        /// Gets or sets the width of the atlas.
        /// </summary>
        public int Width => Texture.Width;

        /// <summary>
        /// Gets or sets the height of the atlas.
        /// </summary>
        public int Height => Texture.Height;

        /// <summary>
        /// Returns the value at the specified key.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns>The atlas sprite data.</returns>
        public AtlasSpriteData this[int index] => this.atlasSprites[index];

        /// <summary>
        /// Gets the all of the frames that have the given sub texture id.
        /// </summary>
        /// <param name="subTextureID">The sub texture ID of the frames to return.</param>
        /// <returns>The list of frame rectangles.</returns>
        internal Rectangle[] GetFrames(string subTextureID)
        {
            var foundSpriteData = GetAtlasSpriteData(subTextureID);

            if (!AtlasRepository.IsAnimatingFrame(foundSpriteData.Name))
            {
                return this.atlasSprites.Where(item => item.Name == subTextureID).ToList().ConvertAll(item => item.Bounds).ToArray();
            }

            var returnItems = new List<Rectangle>();

            // Animating Frame Sorting
            var unsortedItems = this.atlasSprites.Where(item => AtlasRepository.IsAnimatingFrame(item.Name)).ToList();
            var currentIndexNum = 0;

            // Sort the animating frame names in ascending order using there index number
            while (true)
            {
                // Get the item of current index
                var foundItem = unsortedItems.Find(item => AtlasRepository.ExtractFrameNumber(item.Name) == currentIndexNum);

                if (foundItem != null)
                {
                    returnItems.Add(foundItem.Bounds);
                    currentIndexNum += 1;
                }
                else
                {
                    break;
                }
            }

            return returnItems.ToArray();
        }

        /// <summary>
        /// Returns true if a sub texture with the given name exists in the atlas.
        /// </summary>
        /// <param name="name">The name of the sub texture to check for.</param>
        /// <returns>True if the sub texture exists.</returns>
        internal bool SubTextureExists(string name) => this.atlasSprites.Any(item => item.Name == name);

        /// <summary>
        /// Returns the bounds of the sub texture with the given name.
        /// </summary>
        /// <param name="name">The name of the sub texture.</param>
        /// <returns>The sub texture bounds.</returns>
        internal Rectangle SubTextureBounds(string name) => GetAtlasSpriteData(name).Bounds;

        private AtlasSpriteData GetAtlasSpriteData(string subTextureID)
        {
            var foundSpriteData = (from s in this.atlasSprites
                                   where s.Name.Contains(subTextureID, StringComparison.Ordinal)
                                   select s).FirstOrDefault();

            if (foundSpriteData is null)
            {
                throw new Exception($"Sprite data with the name '{subTextureID}' could not be found for atlas '{AtlasName}'.");
            }

            return foundSpriteData;
        }
    }
}

// <copyright file="AtlasData.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Content
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Raptor;

    /// <summary>
    /// Holds texture atlas data.
    /// </summary>
    //TODO: Look into using this later during the building of a test game.
    [ExcludeFromCodeCoverage]
    internal class AtlasData
    {
        #region Private Fields
        private readonly List<AtlasSpriteData> _atlasSprites;
        #endregion


        #region Constructors
        /// <summary>
        /// Loads a texture atlas with the given texture name and atlas data name.
        /// </summary>
        /// <param name="atlasSubTexutureData">The sub texture data of all of the sub textures in the atlas.</param>
        public AtlasData(List<AtlasSpriteData> atlasSubTexutureData)
        {
            //TODO: check to see if this copy by reference is needed
            _atlasSprites = atlasSubTexutureData;
        }
        #endregion


        #region Props
        /// <summary>
        /// Returns the value at the specified key.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns></returns>
        public AtlasSpriteData this[int index]
        {
            get { return _atlasSprites[index]; }
        }

        /// <summary>
        /// Returns at list of all of the sub texure ID's.
        /// </summary>
        public List<string> FrameNameList
        {
            get
            {
                return _atlasSprites.Select(item => item.Name).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the width of the atlas.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the atlas.
        /// </summary>
        public int Height { get; set; }
        #endregion


        #region Internal Methods
        /// <summary>
        /// Gets the all of the frames that have the given sub texture id.
        /// </summary>
        /// <param name="subTextureID">The sub texture ID of the frames to return.</param>
        /// <returns></returns>
        internal List<Rect> GetFrames(string subTextureID)
        {
            //If the frame is a non animating frame, just return the single frame
            if (!AtlasManager.IsAnimatingFrame(_atlasSprites.Find(item => item.Name.Contains(subTextureID)).Name))
            {
                return _atlasSprites.Where(item => item.Name == subTextureID).ToList().ConvertAll(item => item.Bounds);
            }

            var returnItems = new List<Rect>();

            #region Animating Frame Sorting
            var unsortedItems = _atlasSprites.Where(item => AtlasManager.IsAnimatingFrame(item.Name)).ToList();
            var currentIndexNum = 0;

            //Sort the animating frame names in ascending order using there index number
            while (true)
            {
                //Get the item of current index
                var foundItem = unsortedItems.Find(item => AtlasManager.ExtractFrameNumber(item.Name) == currentIndexNum);

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
            #endregion

            return returnItems;
        }


        /// <summary>
        /// Returns true if a sub texture with the given name exists in the atlas.
        /// </summary>
        /// <param name="name">The name of the sub texture to check for.</param>
        /// <returns></returns>
        internal bool SubTextureExists(string name)
        {
            return _atlasSprites.Find(item => item.Name == name) != null;
        }


        /// <summary>
        /// Returns the bounds of the sub texture with the given name.
        /// </summary>
        /// <param name="name">The name of the sub texture.</param>
        /// <returns></returns>
        internal Rect SubTextureBounds(string name)
        {
            return _atlasSprites.Find(item => item.Name == name).Bounds;
        }
        #endregion
    }
}
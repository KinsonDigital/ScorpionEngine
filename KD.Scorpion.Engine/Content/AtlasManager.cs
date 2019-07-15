using KDScorpionCore.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace KDScorpionEngine.Content
{
    /// <summary>
    /// Manages all of the atlases loaded into the game.
    /// </summary>
    //TODO: Look into using this later during the building of a test game.
    [ExcludeFromCodeCoverage]
    internal static class AtlasManager
    {
        #region Private Fields
        private static readonly Dictionary<string, AtlasData> _allAtlasData = new Dictionary<string, AtlasData>();//The atlas data
        private static readonly Dictionary<string, ITexture> _allAtlasTextures = new Dictionary<string, ITexture>();//The atlas textures
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given atlas data to the atlas manager and assigns it the given unique textureAtlasID.
        /// </summary>
        /// <param name="textureAtlasID">The unique atlas ID to assign to the given texture.</param>
        /// <param name="atlasDataID">The unique atlas data ID to assign to the given texture.</param>
        /// <param name="texture">The texture to add.</param>
        /// <param name="data">The atlas data to add.</param>
        public static void AddAtlasData(string textureAtlasID, string atlasDataID,  ITexture texture, AtlasData data)
        {
            //todo: possibly do not need this method, id is checked below
            CheckID(textureAtlasID);//See if the atlas has already been added
            
            //Check the atlas data to see if it has a valid format and structure
            CheckData(data);

            //As long as the atlas data has not already been added
            if(!_allAtlasData.ContainsKey(atlasDataID))
                _allAtlasData.Add(atlasDataID, data);//Add the atlas data

            if(!_allAtlasTextures.ContainsKey(textureAtlasID))
                _allAtlasTextures.Add(textureAtlasID, texture);//Add the atlas texture
        }


        /// <summary>
        /// Removes the atlas data and texture that matches the given ID.
        /// </summary>
        /// <param name="id">The textureAtlasID of the atlas data and texture to remove.</param>
        public static void RemoveAtlasData(string id)
        {
            CheckID(id);//See if the atlas has already been added

            _allAtlasData.Remove(id);
            _allAtlasTextures.Remove(id);
        }


        /// <summary>
        /// Gets the atlas data that matches the given ID.
        /// </summary>
        /// <param name="id">The textureAtlasID of the atlas data to get.</param>
        /// <returns></returns>
        public static AtlasData GetAtlasData(string id)
        {
            CheckID(id);//See if the atlas has already been added

            return _allAtlasData[id];
        }


        /// <summary>
        /// Gets the atlas texture that matches the given ID.
        /// </summary>
        /// <param name="id">The textureAtlasID of the atlas texture to get.</param>
        /// <returns></returns>
        public static ITexture GetAtlasTexture(string id)
        {
            CheckID(id);//See if the atlas has already been added

            return _allAtlasTextures[id];
        }
        #endregion


        #region Internal Methods
        /// <summary>
        /// Returns the reason of validity of the given sub texture id.
        /// </summary>
        /// <returns></returns>
        /// <param name="subTextureID">The name of the sub texture string to check.</param>
        internal static InValidReason GetValidationReason(string subTextureID)
        {
            var leftBracketIndex = subTextureID.IndexOf('[');//Index of the left bracket
            var rightBracketIndex = subTextureID.IndexOf(']');//Index of the right bracket
            var dashIndex = subTextureID.IndexOf('-');//Index of the dash
            var leftBracketCount = subTextureID.Count(item => item == '[');//Total number of left brackets
            var rightBracketCount = subTextureID.Count(item => item == ']');//Total number of right brackets
            var dashCount = subTextureID.Count(item => item == '-');//Total number of dashes

            //If the index of the left bracket is not 0
            if (leftBracketIndex != 0)
                return InValidReason.LeftBracketNotFirstChar;

            //If the index of the right bracket is not the last chart
            if (rightBracketIndex != subTextureID.Length - 1)
                return InValidReason.RightBracketNotLastChar;

            //If the order of brackets is not correct
            if (leftBracketIndex > rightBracketIndex)
                return InValidReason.BracketOrderIncorrect;

            //If the dash index is not in between the left bracket and right bracket
            if (dashIndex < leftBracketIndex || dashIndex > rightBracketIndex)
                return InValidReason.DashLocationIncorrect;

            //If the left bracket count is over 1
            if (leftBracketCount > 1)
                return InValidReason.TooManyLeftBrackets;

            //If the right bracket count is over 1
            if (rightBracketCount > 1)
                return InValidReason.TooManyRightBrackets;

            //If the number of dashes is over 1
            return dashCount > 1 ? InValidReason.TooManyDashes : InValidReason.ValidFormat;
        }


        /// <summary>
        /// Returns true if the given sub texture name contains any of the animating structure symbols.
        /// </summary>
        /// <param name="frameName">The name of the entire frame.</param>
        /// <returns></returns>
        internal static bool IsAnimatingFrame(string frameName)
        {
            return (frameName.Contains('[') && frameName.Contains('-') && frameName.Contains(']'));
        }


        /// <summary>
        /// Extracts the sub texture name from a formatted animating sub texture frame name.
        /// </summary>
        /// <returns></returns>
        internal static string ExtractSubTextureID(string frameName)
        {
            var returnValue = new StringBuilder();

            if (!IsAnimatingFrame(frameName)) return frameName;

            var startFound = false;

            //Go through each character
            foreach (var c in frameName)
            {
                //If the start was found
                if (startFound)
                {
                    //If the character is a right bracket
                    if (c == '-')
                    {
                        break;
                    }
                    else
                    {
                        returnValue.Append(c);
                    }
                }
                else
                {
                    //If the character is a left bracket
                    if (c != '[') continue;

                    startFound = true;
                }
            }

            return returnValue.ToString();
        }


        /// <summary>
        /// Extracts the sub texture name from a formatted animating sub texture frame name.
        /// </summary>
        /// <param name="frameName"></param>
        /// <returns></returns>
        internal static int ExtractFrameNumber(string frameName)
        {
            var foundCharacters = new StringBuilder();
            var dashFound = false;

            foreach (var character in frameName)
            {
                //If the character is a dash
                if (character == '-')
                {
                    dashFound = true;
                }

                //If the dash has been found, add the characters to the found characters
                if (dashFound)
                {
                    foundCharacters.Append(character);
                }
            }

            foundCharacters.Replace("-", "");//Remove any dashes
            foundCharacters.Replace("]", "");//Remove any right square brackets

            return Convert.ToInt32(foundCharacters.ToString());
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// If the given ID does not exist, throw an exception.
        /// </summary>
        /// <param name="id">The ID to check for.</param>
        /// <returns></returns>
        private static void CheckID(string id)
        {
            //First check to see if the id is in the atlas data or atlas texture lists
            if (_allAtlasData.Count <= 0 || _allAtlasTextures.Count <= 0) return;

            //todo: look into uncommenting this code
            //If there are items in the lists, check for item duplication
//            if (_allAtlasData.key.Count(dataKey => dataKey == id) > 1 || _allAtlasTextures.key.Count(key => key == id) > 1)
//                throw new Exception("The atlas data with the textureAtlasID \"" + id + "\" has already been added to the atlas manager.");
        }


        /// <summary>
        /// Checks all of the atlas data to see if it is valid.  If not then exceptions will be thrown.
        /// </summary>
        /// <param name="data">The data to check.</param>
        private static void CheckData(AtlasData data)
        {
            #region Description
            /* DESCRIPTION
             * This method will check the individual sub texture names to see if they are valid.  Some rules must be followed for proper animation and sub texture bounds
             * references to work correctly.
             * 
             * ATLAS RULES
             * 1. Each individual sub texture name may or may not have meta data structure symbols embeded into the name itself
             * 2. All meta data must be of a particular format and at the very end of the name
             * 
             * META DATA STRUCTURE
             * 
             * STRUCTURE - The meta data describes a group of sub textures for animation. It consists of 3 structure symbols [, -, and ]. It consists of 2 chunks of data that
             * describe the sub texture ID of the entire animation group, and the frame index of the animation.
             * 
             *                                              [   enemy   -   0   ]
             *                                              ^     ^     ^   ^   ^
             *                                              |     |     |   |   |
             *                                              |     |     |   |   |
             *              Left Structure Bracket-----------     |     |   |   ----------Right Structure Bracket
             *                  Animation ID-----------------------     |   --------------Frame Index   
             *                                                          |
             *                                                          |
             *                                                   Dash Structure
             * 
             * 
             * 
             * 
             * META DATA RULES
             * 1. Meta data must contain these symbols.  ===> [-]
             * 2. The left bracket must be left of the right bracket and the first character of the entire name
             * 3. The right bracket must be to the right of the left bracker and the last character of the entire name
             * 3. The dash must be between the 2 brackets
             * 4. The meta data starts at the left bracket and ends at the right bracket
             * 5. There must be data inbetween the left bracket and the dash and the right bracket and the dash
             * 6. The data to the left of the dash is aloud to be any character length and of any combination of numbers and letters
             * 7. The data to the right of the dash must only be whole numbers with digits that are 0-9
             * 8. The data to the right of the dash must not have any duplicate numbers that reside in the same group of animiation sub textures with the same sub texture ID.
             */
            #endregion

            var frameNameList = data.FrameNameList;
            var items = new List<string>();

            #region Check For Single Name Structure
            //Loop through each sub texture name to check if its valid
            foreach (var id in frameNameList)
            {
                //If the frame is an animating frame
                if (IsAnimatingFrame(id))
                {
                    var reason = GetValidationReason(id);

                    //If the format is valid
                    if (reason == InValidReason.ValidFormat)
                    {
                        //Check if the sub texture ID is valid
                        if (!ExtractSubTextureID(id).ContainsOnlyLettersAndNumbers())
                            throw new Exception("Animation Sub Texture Name Invalid");
                    }
                    else
                    {
                        throw new Exception("Atlas Sub Texture Name Format Invalid ==> " + reason);
                    }
                }
                else//Not an animating frame
                {
                    //If the non animating frame is NOT valid
                    if (!id.ContainsOnlyLettersAndNumbers())
                    {
                        throw new Exception("Animation Sub Texture Name Invalid");
                    }
                }
            }
            #endregion

            #region Check For Name Duplication
            var nonAnimatedFrames = data.FrameNameList.Where(item => !IsAnimatingFrame(item)).ToArray();

            //First check each non animating sub texture frame to see if its duplicated
            if (nonAnimatedFrames.Any(name => nonAnimatedFrames.Count(item => item == name) > 1))
            {
                throw new Exception("Duplicate names for non animating sub texture frames not aloud.");
            }
            #endregion

            #region Animating Sub Texture Frame Index Sequence Checking
            var animatingFrameNames = new List<string>();

            //Check each animating frame name
            for (var i = 0; i < animatingFrameNames.Count; i++)
            {
                //If the name has a number in it
                if (!animatingFrameNames[i].HasNumbers())
                    throw new Exception("Animating frame sub texture name has no frame index number");

                //If the numbers in the current name is in any of the animating frame names, throw an exception
                if (animatingFrameNames.Count(item => item.GetFirstOccurentOfNumber() == animatingFrameNames[i].GetFirstOccurentOfNumber()) > 1)
                    throw new Exception("Duplicate frame index number within the animating frame group " + animatingFrameNames[i]);

                //If this point is reached, then no errors were found, remove the item from the list to remove duplicate checks
                animatingFrameNames.RemoveAt(i);

                i--;
            }
            #endregion
        }
        #endregion
    }
}
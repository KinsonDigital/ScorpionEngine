using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.GameSound;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using SysFont = System.Drawing.Font;
using SysImage = System.Drawing.Image;
using Color = System.Drawing.Color;

namespace ScorpionEngine.Content
{
    /// <summary>
    /// Manages the loading of content like graphics, sounds, and other custom data specific to games, and brings them into the engine.
    /// </summary>
    internal static class ContentLoader
    {
        #region Fields
        private static readonly string _gamePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);//The path to the game
        private static string _contentRootDir = _gamePath + "\\" + "Content";
        #endregion

        #region Properties
        /// <summary>
        /// Gets the path to the executable game.
        /// </summary>
        public static string GamePath
        {
            get { return _gamePath; }
        }

        /// <summary>
        /// Gets or sets the root directory for the game's content.
        /// </summary>
        public static string ContentRootDirectory
        {
            get { return _contentRootDir; }
            set { _contentRootDir = value; }
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Loads a texture with the given name.
        /// </summary>
        /// <param name="gd">The graphics device that will be drawing the texture.</param>
        /// <param name="textureName">The name of the texture to load.</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string textureName)
        {
            var fullPath = _contentRootDir + "\\" + Path.GetDirectoryName(textureName) + "Graphics\\" + Path.GetFileName(textureName) + ".png";

            var dir = Path.GetDirectoryName(textureName);

            Texture2D newTexture;

            using (var fileStream = File.OpenRead(fullPath)) 
            {
                newTexture = Texture2D.FromStream(Engine.GrfxDevice, fileStream);
            }

            return newTexture;
        }

        /// <summary>
        /// Loads a font to be drawing with the given text, font settings, forecolor, and backcolor.
        /// </summary>
        /// <param name="gd">The graphics device that will be drawing the texture.</param>
        /// <param name="text">The text of the font texture.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        /// <returns></returns>
        public static Texture2D LoadFontTexture(string text, SysFont font, Color foreColor, Color backColor)
        {
            Texture2D newFontTexture;

            //Create an image with the given text rendered to it
            var textureImage = CreateTextImage(text, font, foreColor, backColor);

            //Create a memory stream to save the bitmap data to
            using (var memStream = new MemoryStream())
            {
                textureImage.Save(memStream, ImageFormat.Png);

                //Create the texture2d from the memory stream
                newFontTexture = Texture2D.FromStream(Engine.GrfxDevice, memStream);
            }

            return newFontTexture;
        }

        /// <summary>
        /// Loads a texture atlas with the given name and the atlas data with the given name.
        /// </summary>
        /// <param name="atlasDataName">The name of the atlas data to load.</param>
        /// <returns></returns>
        public static AtlasData LoadAtlasData(string atlasDataName)
        {
            var atlasSpriteData = new List<AtlasSpriteData>();
            var xmlReader = XmlReader.Create(_contentRootDir + "\\" + atlasDataName + ".xml");

            //Keep processing the xml data until it is all read
            while (xmlReader.Read())
            {
                xmlReader.MoveToContent();

                //If the node type is an element and has a name of TextureAtlas
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "TextureAtlas")
                {
//                    //Move to the height attribute
//                    success = xmlReader.MoveToAttribute("height");
//
//                    //If the node is an element and the name is height
//                    if (xmlReader.NodeType == XmlNodeType.Attribute && success)
//                    {
//                        textureHeight = (int)xmlReader.ReadContentAsDouble();
//                    }
//
//                    //Move to the width attribute
//                    success = xmlReader.MoveToAttribute("width");
//
//                    //If the node is an element and the name is height
//                    if (xmlReader.NodeType == XmlNodeType.Attribute && success)
//                    {
//                        textureWidth = (int)xmlReader.ReadContentAsDouble();
//                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "sprite")
                {
                    var newRect = new Rect();

                    //Move to the name of the sprite to get the name as a key
                    xmlReader.MoveToAttribute("n");

                    var spriteName = Path.GetFileNameWithoutExtension(xmlReader.Value);

                    //Move to the x attribute to get the x value
                    xmlReader.MoveToAttribute("x");
                    newRect.X = (int)xmlReader.ReadContentAsDouble();

                    //Move to the y attribute to get the y value
                    xmlReader.MoveToAttribute("y");
                    newRect.Y = (int)xmlReader.ReadContentAsDouble();

                    //Move to the w attribute to get the width attribute
                    xmlReader.MoveToAttribute("w");
                    newRect.Width = (int)xmlReader.ReadContentAsDouble();

                    //Move to the h attribute to get the height attribute
                    xmlReader.MoveToAttribute("h");
                    newRect.Height = (int)xmlReader.ReadContentAsDouble();

                    //Add the new item to the list of rectangles
                    if (spriteName != null) atlasSpriteData.Add(new AtlasSpriteData { Bounds = newRect, Name = spriteName});
                }
            }

            return new AtlasData(atlasSpriteData);
        }

        /// <summary>
        /// Creates an image with the given text rendered to to the surface with the given font and color settings.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        /// <returns></returns>
        private static SysImage CreateTextImage(String text, SysFont font, Color textColor, Color backColor)
        {
            //Measure the text so the bitmap can be created to the right size
            var textSize = Graphics.FromImage(new Bitmap(1, 1)).MeasureString(text, font);

            //create a new image of the right size
            var img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            //Set the drawing to the surface of the image
            var drawing = Graphics.FromImage(img);

            //Set the settings to high quality
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBicubic;
            drawing.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Paint the background to the given color
            drawing.Clear(backColor);

            //Draw the text to the bitmap surface
            drawing.DrawString(text, font, new SolidBrush(textColor), 0, 0);

            drawing.Dispose();

            return img;
        }

        /// <summary>
        /// Loads a song with the given name.
        /// </summary>
        /// <param name="songName">The name of the song to load.</param>
        /// <returns></returns>
        public static Song LoadSong(string songName)
        {
            var fullPath = _contentRootDir + "\\" + Path.GetDirectoryName(songName) + "\\" + Path.GetFileName(songName) + ".ogg";

            return new Song(fullPath);
        }

        /// <summary>
        /// Loads a sound effect with the given name.
        /// </summary>
        /// <param name="effectName">The name of the sound effect to load.</param>
        /// <returns></returns>
        public static SoundEffect LoadSoundEffect(string effectName)
        {
            var fullPath = _contentRootDir + "\\" + Path.GetDirectoryName(effectName) + "\\" + Path.GetFileName(effectName) + ".wav";

            return new SoundEffect(fullPath);
        }
        #endregion
    }
}
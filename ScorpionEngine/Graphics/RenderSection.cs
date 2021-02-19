// <copyright file="RenderSection.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System.Drawing;
    using System.Numerics;
    using Raptor.Content;

    public class RenderSection
    {
        public string TextureName { get; set; } = string.Empty;

        public string? SubTextureName { get; set; } = string.Empty;

        // TODO: Continually update this in Entity
        public Rectangle RenderBounds { get; set; }

        public int HalfWidth => RenderBounds.Width / 2;

        public int HalfHeight => RenderBounds.Height / 2;

        public TextureType TypeOfTexture { get; set; }

        public bool IsAnimated { get; set; }

        public void Reset()
        {
            TextureName = string.Empty;
            SubTextureName = string.Empty;
            RenderBounds = Rectangle.Empty;
            TypeOfTexture = TextureType.WholeTexture;
            IsAnimated = false;
        }

        public Vector2 GetTexturePosition()
        {
            if (IsAnimated)
            {
                return new Vector2(RenderBounds.X, RenderBounds.Y);
            }
            else
            {
                switch (TypeOfTexture)
                {
                    case TextureType.WholeTexture:
                        return Vector2.Zero;
                    case TextureType.SubTexture:
                        return new Vector2(RenderBounds.X, RenderBounds.Y);
                    default:
                        break;
                }
            }

            return Vector2.Zero;
        }

        public static RenderSection CreateNonAnimatedWholeTexture(string wholeTextureName)
            => new RenderSection()
            {
                TextureName = wholeTextureName,
                SubTextureName = string.Empty,
                IsAnimated = false,
                TypeOfTexture = TextureType.WholeTexture,
            };

        public static RenderSection CreateNonAnimatedSubTexture(string atlasTextureName, string subTextureName)
            => new RenderSection()
            {
                TextureName = atlasTextureName,
                SubTextureName = subTextureName,
                IsAnimated = false,
                TypeOfTexture = TextureType.SubTexture,
            };

        public static RenderSection CreateAnimatedSubTexture(string atlasTextureName, string subTextureName)
            => new RenderSection()
            {
                TextureName = atlasTextureName,
                SubTextureName = subTextureName,
                IsAnimated = true,
                TypeOfTexture = TextureType.SubTexture
            };
    }
}

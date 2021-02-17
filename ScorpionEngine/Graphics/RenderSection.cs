using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using Raptor.Content;
using Raptor.Graphics;

namespace KDScorpionEngine.Graphics
{
    public class RenderSection
    {
        public string TextureName { get; set; } = string.Empty;

        public string? SubTextureName { get; set; } = string.Empty;

        // TODO: Continually update this in Entity
        public Rectangle RenderBounds { get; set; }

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

        public int GetTextureWidth()
        {
            return RenderBounds.Width;
        }

        public int GetTextureHeight()
        {
            return RenderBounds.Height;
        }

        public int GetTextureHalfWidth()
        {
            return GetTextureWidth() / 2;
        }

        public int GetTextureHalfHeight()
        {
            return GetTextureHeight() / 2;
        }
    }
}

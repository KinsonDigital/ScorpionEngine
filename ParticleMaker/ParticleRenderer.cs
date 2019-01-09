using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleMaker
{
    public class ParticleRenderer : IRenderer
    {
        private SpriteBatch _spriteBatch;


        //TODO: Move the injection of this sprite batch to the InjectData method.
        public ParticleRenderer(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }


        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            throw new NotImplementedException();
        }


        public void End()
        {
            throw new NotImplementedException();
        }


        public void FillCircle(float x, float y, float radius, byte[] color)
        {
            throw new NotImplementedException();
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }

        //TODO: Use this to inject the spritbatch into class.
        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public void Line(float startX, float startY, float endX, float endY, byte[] color)
        {
            throw new NotImplementedException();
        }


        public void Render(ITexture texture, float x, float y)
        {
            throw new NotImplementedException();
        }


        public void Render(ITexture texture, float x, float y, float angle)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> using the given parameters.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate on the screen to render the texture.</param>
        /// <param name="y">The X coordinate on the screen to render the texture.</param>
        /// <param name="angle">The angle to set the texture at.</param>
        /// <param name="size">The size of the texture.</param>
        /// <param name="red">The red component of the color to apply to the texture.</param>
        /// <param name="green">The green component of the color to apply to the texture.</param>
        /// <param name="blue">The blue component of the color to apply to the texture.</param>
        /// <param name="alpha">The alpha component of the color to apply to the texture.</param>
        public void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, null, new Color(red, green, blue, alpha), angle.ToRadians(), textureOrigin, size, SpriteEffects.None, 0f);
        }


        public void Render(IText text, float x, float y)
        {
            throw new NotImplementedException();
        }


        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY)
        {
            throw new NotImplementedException();
        }


        public void RenderTextureArea(ITexture texture, Rect area, float x, float y)
        {
            throw new NotImplementedException();
        }


        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}

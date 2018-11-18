using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionCore;
using ScorpionCore.Plugins;
using System;

namespace MonoScorpPlugin
{
    //TODO: Add docs to this file
    public class MonoRenderer : IRenderer
    {
        private static SpriteBatch _spriteBatch;
        //This will treated as a MonoGame graphics device.
        private static dynamic _graphicsDevice;


        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            _graphicsDevice.Clear(new Color(red, green, blue, alpha));
        }


        public void Start()
        {
            _spriteBatch.Begin();
        }


        public void End()
        {
            _spriteBatch.End();
        }


        public void Render(ITexture texture, float x, float y)
        {
            var srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            //TODO: The Begin() and End() should be called after ALL sprites have been rendered.
            //Need to move the usaged of the Begin() and End() to the scene and items will
            //not be drawn unless they are part of the scene.  Upon adding an Entity to the
            //scene is when they will be added to the PhysicsWorld side of things.  An array
            //of entities need to be added to the scene to store ALL of the entitites in that scene
            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, srcRect, Color.White, 0, textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        //Angle is in degrees
        public void Render(ITexture texture, float x, float y, float angle)
        {
            var srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, srcRect, Color.White, angle.ToRadians(), textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        public void Render(IText text, float x, float y)
        {
            var color = new Color(text.Color[0], text.Color[1], text.Color[2]);

            _spriteBatch.DrawString(text.GetText<SpriteFont>(), text.Text, new Vector2(x, y), color);
        }


        public void InjectData<T>(T data) where T : class
        {
            _graphicsDevice = data;

            _spriteBatch = new SpriteBatch(_graphicsDevice as GraphicsDevice);
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY)
        {
            //TODO: The Begin() and End() should be called after ALL sprites have been rendered.
            //Need to move the usaged of the Begin() and End() to the scene and items will
            //not be drawn unless they are part of the scene.  Upon adding an Entity to the
            //scene is when they will be added to the PhysicsWorld side of things.  An array
            //of entities need to be added to the scene to store ALL of the entitites in that scene
            _spriteBatch.DrawLine(lineStartX, lineStartY, lineStopX, lineStopY, Color.White);
        }
    }
}

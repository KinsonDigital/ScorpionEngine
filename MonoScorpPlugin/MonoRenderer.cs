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


        public void Render(ITexture texture, float x, float y)
        {
            //TODO: Need to use the code below to use proper overload of spritebatch that will
            //allow the proper placement of the texture when it is rendered.  This placement should
            //align the texture to match the physical outline dictated by the vertices.
            //Render the border of the triangle physics shape.  Need to get the texture first
            //to be able to create the srcRect and textureOrigin values.

            //EXAMPLE CODE
            //var worldVertices = Util.ToVector2(poly.Vertices);
            //var srcRect = new XNARectangle(0, 0, texture.Width, texture.Height);
            //var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            //spriteBatch.Draw(texture, Util.ToVector2(poly.Position), srcRect, color, poly.Angle, textureOrigin, poly.Scale, SpriteEffects.None, 1f);

            //TODO: The Begin() and End() should be called after ALL sprites have been rendered.
            //Need to move the usaged of the Begin() and End() to the scene and items will
            //not be drawn unless they are part of the scene.  Upon adding an Entity to the
            //scene is when they will be added to the PhysicsWorld side of things.  An array
            //of entities need to be added to the scene to store ALL of the entitites in that scene
            _spriteBatch.Begin();

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), new Vector2(x, y), Color.White);

            _spriteBatch.End();
        }


        public void Render(IText text, float x, float y)
        {
            //TODO: The Begin() and End() should be called after ALL sprites have been rendered.
            //Need to move the usaged of the Begin() and End() to the scene and items will
            //not be drawn unless they are part of the scene.  Upon adding an Entity to the
            //scene is when they will be added to the PhysicsWorld side of things.  An array
            //of entities need to be added to the scene to store ALL of the entitites in that scene
            _spriteBatch.Begin();

            var color = new Color(text.Color[0], text.Color[1], text.Color[2]);

            _spriteBatch.DrawString(text.GetText<SpriteFont>(), text.Text, new Vector2(x, y), color);

            _spriteBatch.End();
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
            _spriteBatch.Begin();

            _spriteBatch.DrawLine(lineStartX, lineStartY, lineStopX, lineStopY, Color.White);

            _spriteBatch.End();
        }
    }
}

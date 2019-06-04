using Microsoft.Xna.Framework;
using KDScorpionCore;
using KDScorpionCore.Plugins;
using System;

namespace MonoScorpPlugin
{
    //TODO: Need to get this imported into the GameEngine using DI
    public class MonoDebugDraw : IDebugDraw
    {
        public void Draw(IRenderer renderer, IPhysicsBody body)
        {
            int max = body.XVertices.Length;

            var origin = new Vector2(body.X, body.Y);

            for (int i = 0; i < max; i++)
            {
                var start = new Vector2(body.XVertices[i], body.YVertices[i]).ToVector().RotateAround(origin.ToVector(), body.Angle);
                var stop = new Vector2(body.XVertices[i < max - 1 ? i + 1 : 0], body.YVertices[i < max - 1 ? i + 1 : 0]).ToVector().RotateAround(origin.ToVector(), body.Angle);

                //TODO: Try to add color as a parameter to this Render() method call
                renderer.RenderLine(start.X, start.Y, stop.X, stop.Y);
            }
        }


        public T GetData<T>(int option) where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
    }
}

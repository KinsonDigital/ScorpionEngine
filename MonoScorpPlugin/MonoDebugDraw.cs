using Microsoft.Xna.Framework;
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
                var start = RotateAround(new Vector2(body.XVertices[i], body.YVertices[i]), origin, body.Angle.ToRadians());
                var stop = RotateAround(new Vector2(body.XVertices[i < max - 1 ? i + 1 : 0], body.YVertices[i < max - 1 ? i + 1 : 0]), origin, body.Angle.ToRadians());

                //TODO: Try to add color as a parameter to this
                renderer.RenderLine(start.X, start.Y, stop.X, stop.Y);
            }
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="rotateOrigin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The vector to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in radians to rotate <paramref name="vector"/>.  Value must be positive.</param>
        /// <returns></returns>
        private static Vector2 RotateAround(Vector2 vector, Vector2 origin, float angle, bool clockWise = true)
        {
            angle = clockWise ? angle : angle * -1;

            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);

            var dx = vector.X - origin.X;//The delta x
            var dy = vector.Y - origin.Y;//The delta y

            var tempX = dx * cos - dy * sin;
            var tempY = dx * sin + dy * cos;

            var x = tempX + origin.X;
            var y = tempY + origin.Y;


            return new Vector2(x, y);
        }
    }
}

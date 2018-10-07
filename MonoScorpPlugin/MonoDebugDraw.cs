using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoScorpPlugin
{
    //TODO: Need to get this imported into the GameEngine using DI
    public class MonoDebugDraw : IDebugDraw
    {
        public void Draw(IRenderer renderer, IPhysicsBody body)
        {
            int max = body.XVertices.Length;

            for (int i = 0; i < max; i++)
            {
                float startX = body.XVertices[i];
                float startY = body.YVertices[i];
                float stopX = body.XVertices[i < max - 1 ? i + 1 : 0];
                float stopY = body.YVertices[i < max - 1 ? i + 1 : 0];

                //TODO: Try to add color as a parameter to this
                renderer.RenderLine(startX, startY, stopX, stopY);
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
    }
}

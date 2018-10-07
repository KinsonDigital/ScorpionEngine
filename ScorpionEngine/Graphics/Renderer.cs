using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Graphics
{
    public class Renderer
    {
        internal Renderer(IRenderer renderer)
        {
            InternalRenderer = renderer;
        }


        #region Props
        internal IRenderer InternalRenderer { get; set; }
        #endregion


        #region Public Methods
        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            InternalRenderer.Clear(red, green, blue, alpha);
        }


        public T GetData<T>(string dataType) where T : class
        {
            //TODO: If rendering stops working, the change to the GetData
            //interface might be the cause.  Casting the object to type T
            //might be the issue
            return InternalRenderer.GetData(dataType) as T;
        }


        public void InjectData<T>(T data) where T : class
        {
            InternalRenderer.InjectData<T>(data);
        }


        public void Render(Texture texture, float x, float y)
        {
            InternalRenderer.Render(texture, x, y);
        }


        public void Render(TextItem text, float x, float y)
        {
            InternalRenderer.Render(text, x, y);
        }
        #endregion
    }
}

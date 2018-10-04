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
        private IRenderer _internalRenderer;


        internal Renderer(IRenderer renderer)
        {
            _internalRenderer = renderer;
        }


        public Renderer()
        {

        }


        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            _internalRenderer.Clear(red, green, blue, alpha);
        }


        public T GetData<T>(string dataType) where T : class
        {
            //TODO: If rendering stops working, the change to the GetData
            //interface might be the cause.  Casting the object to type T
            //might be the issue
            return _internalRenderer.GetData(dataType) as T;
        }


        public void InjectData<T>(T data) where T : class
        {
            _internalRenderer.InjectData<T>(data);
        }


        public void Render(Texture texture, float x, float y)
        {
            _internalRenderer.Render(texture, x, y);
        }


        public void Render(TextItem text, float x, float y)
        {
            _internalRenderer.Render(text, x, y);
        }
    }
}

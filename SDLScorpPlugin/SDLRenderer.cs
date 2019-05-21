using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDLScorpPlugin
{
    public class SDLRenderer : IRenderer
    {
        private bool _beginInvoked = false;


        public void Start()
        {
            _beginInvoked = true;
        }


        public void End()
        {
            if (!_beginInvoked)
                throw new Exception($"The '{nameof(Start)}' method must be invoked first before the '{nameof(End)}' method.");

        }


        public void FillCircle(float x, float y, float radius, byte[] color)
        {
            throw new NotImplementedException();
        }


        public void Clear(byte red, byte green, byte blue, byte alpha)
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


        public void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha)
        {
            throw new NotImplementedException();
        }


        public void Render(IText text, float x, float y)
        {
            throw new NotImplementedException();
        }


        public void RenderLine(float startX, float startY, float endX, float endY, byte[] color)
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

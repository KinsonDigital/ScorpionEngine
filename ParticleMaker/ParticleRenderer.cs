using SDL2;
using System;
using System.Drawing;

namespace ParticleMaker
{
    public class ParticleRenderer
    {
        private IntPtr _rendererPtr;
        private bool _beginInvoked;


        public ParticleRenderer(IntPtr rendererPtr) => _rendererPtr = rendererPtr;


        public void Begin() => _beginInvoked = true;

        public void End()
        {
            if (!_beginInvoked)
                throw new Exception($"The method {nameof(Begin)} must be invoked first before invoking method {nameof(End)}.");

            SDL.SDL_RenderPresent(_rendererPtr);
            _beginInvoked = false;
        }


        public void Clear(Color color)
        {
            if (!_beginInvoked)
                throw new Exception($"The method {nameof(Begin)} must be invoked first before invoking method {nameof(Clear)}.");

            SDL.SDL_SetRenderDrawColor(_rendererPtr, color.R, color.G, color.B, color.A);
            SDL.SDL_RenderClear(_rendererPtr);
        }


        public void Render(ParticleTexture texture)
        {
            SDL.SDL_SetTextureColorMod(texture.TexturePointer, texture.Color.R, texture.Color.G, texture.Color.B);
            SDL.SDL_SetTextureAlphaMod(texture.TexturePointer, texture.Color.A);
            SDL.SDL_SetTextureBlendMode(texture.TexturePointer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            var srcRect = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = texture.Width,
                h = texture.Height
            };

            var destRect = new SDL.SDL_Rect()
            {
                x = texture.X,
                y = texture.Y,
                w = texture.Width,
                h = texture.Height
            };

            //Render texture to screen
            SDL.SDL_RenderCopy(_rendererPtr, texture.TexturePointer, ref srcRect, ref destRect);
        }


        public void Render(ParticleTexture texture, int x, int y)
        {
            SDL.SDL_SetTextureColorMod(texture.TexturePointer, texture.Color.R, texture.Color.G, texture.Color.B);
            SDL.SDL_SetTextureAlphaMod(texture.TexturePointer, texture.Color.A);
            SDL.SDL_SetTextureBlendMode(texture.TexturePointer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            var srcRect = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = texture.Width,
                h = texture.Height
            };

            var destRect = new SDL.SDL_Rect()
            {
                x = x,
                y = y,
                w = texture.Width,
                h = texture.Height
            };

            //Render texture to screen
            SDL.SDL_RenderCopy(_rendererPtr, texture.TexturePointer, ref srcRect, ref destRect);
        }


        public void RenderLine(int startX, int startY, int endX, int endY, Color color)
        {
            SDL.SDL_SetRenderDrawColor(_rendererPtr, color.R, color.G, color.B, color.A);
            SDL.SDL_RenderDrawLine(_rendererPtr, startX, startY, endX, endY);
        }
    }
}

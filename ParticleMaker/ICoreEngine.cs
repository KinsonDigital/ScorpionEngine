using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker
{
    public interface ICoreEngine
    {
        event EventHandler<DrawEventArgs> OnDraw;
        event EventHandler<EventArgs> OnInitialize;
        event EventHandler<EventArgs> OnLoadContent;
        event EventHandler<EventArgs> OnUnLoadContent;
        event EventHandler<UpdateEventArgs> OnUpdate;


        GameWindow Window { get; }
        GraphicsDevice GraphicsDevice { get; }

        IntPtr RenderSurfaceHandle { get; set; }

        void Run();

        void Exit();
    }
}
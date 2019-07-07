using KDScorpionCore;
using KDScorpionCore.Plugins;
using System;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeEngineCore : IEngineCore
    {
        #region Events
        public event EventHandler<OnUpdateEventArgs> OnUpdate;
        public event EventHandler<OnRenderEventArgs> OnRender;
        public event EventHandler OnInitialize;
        public event EventHandler OnLoadContent;
        #endregion


        #region Props
        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public IRenderer Renderer { get; set; }

        public bool DisposeInvoked { get; private set; }
        #endregion


        #region Unit Testing Methods
        /// <summary>
        /// Used in unit testing to invoke the <see cref="EngineCore"/> events.
        /// </summary>
        public void InvokeAllEvents()
        {
            OnInitialize?.Invoke(null, null);
            OnLoadContent?.Invoke(null, null);
            OnUpdate?.Invoke(null, new OnUpdateEventArgs(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) }));
            OnRender?.Invoke(null, new OnRenderEventArgs(Renderer));
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            DisposeInvoked = true;
        }


        public bool IsRunning()
        {
            return true;
        }


        public void SetFPS(float value)
        {
            
        }


        public void Start()
        {
            OnInitialize.Invoke(null, null);
        }


        public void Stop()
        {
            throw new NotImplementedException();
        }


        public T GetData<T>(int option) where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

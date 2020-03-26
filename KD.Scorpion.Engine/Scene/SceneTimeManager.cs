using KDScorpionEngine.Events;
using Raptor;
using System;

namespace KDScorpionEngine.Scene
{
    /// <summary>
    /// Manages how a scene is updated and give the ability for the scene to be paused.
    /// </summary>
    public class SceneTimeManager : ITimeManager
    {
        #region Events
        /// <summary>
        /// Occurs when the stack of set frames has finished processing.
        /// </summary>
        public event EventHandler<FrameStackFinishedEventArgs> FrameStackFinished;
        #endregion


        #region Private Fields
        private Action _frameStackCallback;//Invoked after a frame stack has been run
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the amount of elapsed time in milliseconds that the current frame has ran.
        /// </summary>
        public int ElapsedFrameTime { get; set; }

        /// <summary>
        /// Gets or sets the total frames that have elapsed for the current frame stack.
        /// </summary>
        public uint ElapsedFramesForStack { get; set; } = 1;

        /// <summary>
        /// Gets or sets the amount of frames to run per frame stack.
        /// </summary>
        public uint FramesPerStack { get; set; } = 50;

        /// <summary> 
        /// Gets or sets the time in milliseconds that each frame should take. 
        /// NOTE: This is restricted to the incoming game engine frame time. If this time is less then the  
        /// engine updating this manager, then this will now work.
        /// </summary>
        public int FrameTime { get; set; } = 16;

        /// <summary>
        /// Gets or sets a value indicating if the system is paused.
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Gets or sets the total number of frames ran.
        /// </summary>
        public int TotalFramesRan { get; set; }

        /// <summary>
        /// Gets or sets the mode that the manager runs in.
        /// </summary>
        public RunMode Mode { get; set; } = RunMode.Continuous;
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the <see cref="SceneTimeManager"/>.
        /// </summary>
        /// <param name="gameTime">The engine time.</param>
        public void Update(EngineTime gameTime)
        {
            //Continous mode
            if (Mode == RunMode.Continuous)
            {
                TotalFramesRan += 1;
            }
            else if (Mode == RunMode.FrameStack)//Step mode
            {
                //If the current stack of frames are being ran
                if (!Paused)
                {
                    ElapsedFrameTime += gameTime.ElapsedEngineTime.Milliseconds;

                    //If the elapsed time frame has passed
                    if (ElapsedFrameTime >= FrameTime)
                    {
                        ElapsedFrameTime = 0;//Reset the elapsed time back to 0
                        ElapsedFramesForStack += 1;//Update the total number of frames that has elapsed for this frame stack
                        TotalFramesRan += 1;//Update the total number of frames that have passed

                        //If the required number of frames for this frame stack have elapsed
                        if (ElapsedFramesForStack > FramesPerStack)
                        {
                            ElapsedFramesForStack = 0;
                            Paused = true;

                            //Invoke the frame stack callback
                            _frameStackCallback?.Invoke();

                            //Invoke the frame stack finished event
                            FrameStackFinished?.Invoke(this, new FrameStackFinishedEventArgs((int)FramesPerStack));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Plays the scene.
        /// </summary>
        public void Play() => Paused = false;


        /// <summary>
        /// Pauses the scene.
        /// </summary>
        public void Pause() => Paused = true;


        /// <summary>
        /// Runs a complete stack of frames set by the <see cref="SceneTimeManager"/>.
        /// This will only work if the <see cref="Mode"/> is set to the value of <see cref="RunMode.FrameStack"/>.
        /// </summary>
        public void RunFrameStack() => Paused = Mode == RunMode.FrameStack ? false : Paused;


        /// <summary>
        /// Runs a set amount of frames given by the <paramref name="frames"/> param and pauses after.
        /// This will only work if the <see cref="Mode"/> is set to <see cref="RunMode.FrameStack"/>.
        /// </summary>
        /// <param name="frames">The number of frames to run.</param>
        public void RunFrames(uint frames)
        {
            //If the mode is not in frame stack mode or if the callback is not null.
            //If the callback is not null, that means a previous call is still running.
            if (Mode != RunMode.FrameStack || _frameStackCallback != null)
                return;

            Paused = false;

            //Save the currently set frames ran per stack
            var oldFramesPerStack = FramesPerStack;

            //Temporarily set the frames per stack to the requested frames
            FramesPerStack = frames;

            //Set the frame stack callback
            //This will be invoked once the frames are finished running
            _frameStackCallback = () =>
            {
                //Set the frames per stack back to what it was before the RunFrames() method was called
                FramesPerStack = oldFramesPerStack;

                //Destroy the callback so it is not called anymore.
                _frameStackCallback = null;
            };
        }
        #endregion
    }
}

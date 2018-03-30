using System.Collections.Generic;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Control animation at a particular frames per second.
    /// </summary>
    public class ObjectAnimation
    {
        #region Fields
        private int _fps = 10;//The frames per second that the animation will run at
        private int _elapsedTime;//The amount of time elapsed since the last animation frame was changed
        private AnimationDirection _direction = AnimationDirection.Forward;//The direction that the animation is running
        private AnimationState _state = AnimationState.Stopped;//The state of the animation
        private int _currentFrame;//The current frame of the animation
        private List<Rect> _frames = new List<Rect>();//The bounds of all the frames of the animation
        private bool _looping = true;//True if the animation loops 
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of Animation.
        /// </summary>
        public ObjectAnimation()
        {
            
        }

        /// <summary>
        /// Creates a new instance of Animation.
        /// </summary>
        /// <param name="frameBounds">The bounds data for the animation.</param>
        public ObjectAnimation(List<Rect> frameBounds)
        {
            _frames = frameBounds;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the frames per second of the animation.
        /// </summary>
        public int FPS
        {
            get { return _fps; }
            set
            {
                //Make sure that the incoming value stays at a minimum of 1
                value = value <= 0 ? 1 : value;

                _fps = value;
            }
        }

        /// <summary>
        /// Gets or sets the direction of the animation.
        /// </summary>
        public AnimationDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Gets the state of the animation.
        /// </summary>
        public AnimationState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Gets the frame bounds of the current frame.
        /// </summary>
        public Rect CurrentFrameBounds
        {
            get { return _frames[_currentFrame]; }
        }

        /// <summary>
        /// Gets or sets a value indicating if the animation loops.
        /// </summary>
        public bool Looping
        {
            get { return _looping; }
            set { _looping = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Plays the animation.
        /// </summary>
        public void Play()
        {
            _state = AnimationState.Running;
        }

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Pause()
        {
            _state = AnimationState.Paused;
        }

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void Stop()
        {
            _state = AnimationState.Stopped;
            _currentFrame = 0;//Set the current frame back to the first frame
        }

        /// <summary>
        /// Updates the animation.
        /// </summary>
        /// <param name="engineTime">The engine time.</param>
        public void Update(EngineTime engineTime)
        {
            switch (_state)
            {
                case AnimationState.Running:
                    //Update the elapsed time since the last time the engine loop was called
                    _elapsedTime += engineTime.ElapsedEngineTime.Milliseconds;

                    //If the amount of time has passed for the next frame of the animation to be shown
                    if (_elapsedTime >= 1000 / _fps)
                    {
                        _elapsedTime = 0;

                        //If the animation is running foward or backward
                        switch (_direction)
                        {
                            case AnimationDirection.Forward:
                                //If the current frame is NOT the last frame
                                if (_currentFrame < _frames.Count - 1)
                                {
                                    _currentFrame += 1;
                                }
                                else if(_currentFrame >= _frames.Count - 1 && _looping)//At the last frame, move back to the first frame
                                {
                                    _currentFrame = 0;
                                }
                                break;
                            case AnimationDirection.Backward:
                                //If the current frame is NOT the last frame
                                if (_currentFrame > 0)
                                {
                                    _currentFrame -= 1;
                                }
                                else if (_currentFrame <= 0 && _looping)//At the last frame, move back to the first frame
                                {
                                    _currentFrame = _frames.Count - 1;
                                }
                                break;
                        }
                    }
                    break;
                case AnimationState.Stopped:
                    break;
            }
        }
        #endregion
    }
}

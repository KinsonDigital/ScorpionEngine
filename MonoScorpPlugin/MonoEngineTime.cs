﻿using KDScorpionCore;
using Microsoft.Xna.Framework;
using System;

namespace MonoScorpPlugin
{
    /// <summary>
    /// The timing of the game engine in values that can be used by variable step(real time) or fixed step(game time).
    /// </summary>
    public class MonoEngineTime : IEngineTiming
    {
        #region Private Fields
        private readonly GameTime _gameTime;//The game time
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of EngineTime.
        /// </summary>
        /// <param name="totalEngineTime">The amount of engine time since the game started.</param>
        /// <param name="elapsedEngineTime">The amount of engine time since the last update.</param>
        public MonoEngineTime(TimeSpan totalEngineTime, TimeSpan elapsedEngineTime) => _gameTime = new GameTime(totalEngineTime, elapsedEngineTime);
        #endregion


        #region Props
        /// <summary>
        /// The amount of engine time since the game started.
        /// </summary>
        public TimeSpan TotalEngineTime
        {
            get => _gameTime.TotalGameTime;
            set => _gameTime.TotalGameTime = value;
        }

        /// <summary>
        /// The amount of engine time since the last update.
        /// </summary>
        public TimeSpan ElapsedEngineTime
        {
            get => _gameTime.ElapsedGameTime;
            set => _gameTime.ElapsedGameTime = value;
        }
        #endregion
    }
}
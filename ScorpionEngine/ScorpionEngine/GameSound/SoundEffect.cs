﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace ScorpionEngine.GameSound
{
    /// <summary>
    /// A short sound effect that can be played for simple sounds such as shooting weapons or footsetps.
    /// </summary>
    public class SoundEffect
    {
        #region Fields

        private Sound _sound;
        #endregion

        #region Constructors

        public SoundEffect(string soundEffectName)
        {
            SoundBuffer buffer = new SoundBuffer(soundEffectName);

            _sound = new Sound(buffer);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating if the sound should loop back to the start and play again after the sound has ended.
        /// </summary>
        public bool Looping
        {
            get { return _sound.Loop; } set { _sound.Loop = value; }
        }


        /// <summary>
        /// Gets or sets the volume of the sound effect.
        /// </summary>
        public float Volume
        {
            get { return _sound.Volume; } set { _sound.Volume = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Plays the sound.
        /// </summary>
        public void Play()
        {
            _sound.Play();
        }

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        public void Pause()
        {
            _sound.Pause();
        }

        /// <summary>
        /// Stops the sound.
        /// </summary>
        public void Stop()
        {
            _sound.Stop();
        }
        #endregion
    }
}

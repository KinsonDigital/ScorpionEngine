using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleMaker
{
    public class ParticleTexture : ITexture
    {
        #region Fields
        private readonly Texture2D _texture;
        #endregion


        #region Constructors
        public ParticleTexture(Texture2D texture)
        {
            _texture = texture;
        }
        #endregion


        #region Props
        public int Width => _texture.Width;

        public int Height => _texture.Height;
        #endregion


        #region Public Methods
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public T GetTexture<T>() where T : class
        {
            var result = _texture as T;

            if (result == null)
                throw new Exception($"Generic param T must be of type {nameof(Texture2D)}");


            return result;
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

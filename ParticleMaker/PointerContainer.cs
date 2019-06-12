using System;

namespace ParticleMaker
{
    /// <summary>
    /// Wraps a pointer for the purpose of passing it around.
    /// </summary>
    public class PointerContainer
    {
        #region Private Fields
        private IntPtr _pointer;
        #endregion


        #region Public Methods
        /// <summary>
        /// Packs the pointer inside the container for storage to pass around.
        /// </summary>
        /// <param name="pointer"></param>
        public void PackPointer(IntPtr pointer) => _pointer = pointer;


        /// <summary>
        /// Unpacks the pointer from the container.
        /// </summary>
        /// <returns></returns>
        public IntPtr UnpackPointer() => _pointer;
        #endregion
    }
}

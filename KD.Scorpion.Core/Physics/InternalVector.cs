namespace KDScorpionCore.Physics
{
    internal struct InternalVector
    {
        #region Constructors
        public InternalVector(float x, float y)
        {
            X = x;
            Y = y;
        }
        #endregion


        #region Props
        public float X { get; set; }

        public float Y { get; set; }
        #endregion
    }
}

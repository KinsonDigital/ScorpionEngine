using KDScorpionCore;

namespace KDScorpionEngine.Physics
{
    internal struct InternalVector
    {
        public InternalVector(float x, float y)
        {
            X = x;
            Y = y;
        }


        public float X { get; set; }
        public float Y { get; set; }
    }
}

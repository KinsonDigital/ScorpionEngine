using KDScorpionCore;

namespace KDScorpionEngine
{
    /// <summary>
    /// Makes an object an updatable for the game engine.
    /// </summary>
    public interface IUpdatable
    {
        void Update(EngineTime engineTime);
    }
}

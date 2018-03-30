namespace ScorpionEngine
{
    /// <summary>
    /// Makes an object and updatable update for the game engine.
    /// </summary>
    interface IUpdatable
    {
        void Update(EngineTime engineTime);
    }
}

namespace KDScorpionEngine.Behaviors
{
    public interface IBehavior : IUpdatable
    {
        bool Enabled { get; set; }

        string Name { get; set; }
    }
}
namespace VelcroPhysicsPlugin
{
    public struct PhysicsBodySettings
    {
        public float[] XVertices { get; set; }

        public float[] YVertices { get; set; }

        public float XPosition { get; set; }

        public float YPosition { get; set; }

        public float Angle { get; set; }

        public float Density { get; set; }

        public float Friction { get; set; }

        public float Restitution { get; set; }

        public bool IsStatic { get; set; }
    }
}

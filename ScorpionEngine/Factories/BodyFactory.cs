using ScorpionEngine.Physics;

namespace ScorpionEngine.Factories
{
    public static class BodyFactory
    {
        public static T Create<T>(Vector[] vertices, Vector position, float angle = 0, float density = 1, float friction = 0.2f, float restitution = 0, bool isStatic = false) where T : PhysicsBody
        {
            //object[] ctorParams = new object[9];

            ////Setup the vertices
            //var verticesParam = new List<InternalVector>();
            //foreach (var vector in vertices)
            //    verticesParam.Add(new InternalVector(vector.X, vector.Y));

            //ctorParams[0] = (from v in verticesParam.ToArray() select v.X).ToArray();
            //ctorParams[1] = (from v in verticesParam.ToArray() select v.Y).ToArray();
            //ctorParams[2] = position.X;
            //ctorParams[3] = position.Y;
            //ctorParams[4] = angle;
            //ctorParams[5] = density;
            //ctorParams[6] = friction;
            //ctorParams[7] = restitution;
            //ctorParams[8] = isStatic;

            //return PluginSystem.PhysicsPlugins.LoadPlugin<IPhysicsBody>(ctorParams);
            return null;
        }
    }
}

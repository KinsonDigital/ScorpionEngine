using ScorpionEngine.Entities;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Factories
{
    public static class EntityFactory
    {
        public static T Create<T>(Texture texture, Vector position, bool isStaticBody = false) where T : Entity, new()
        {
            var result = new T();

            var halfWidth = texture.Width / 2;
            var halfHeight = texture.Height / 2;

            var vertices = new Vector[4]
            {
                new Vector(position.X - halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y - halfHeight),
                new Vector(position.X + halfWidth, position.Y + halfHeight),
                new Vector(position.X - halfWidth, position.Y + halfHeight),
            };

            result.Body = new PhysicsBody(vertices, position, isStatic: isStaticBody);


            return result;
        }
    }
}

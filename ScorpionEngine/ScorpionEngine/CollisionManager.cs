using System.Collections.Generic;
using ScorpionEngine.Objects;

namespace ScorpionEngine
{
    /// <summary>
    /// Manages collision between objects.
    /// </summary>
    internal static class CollisionManager
    {
        #region Fields
        private static readonly List<string> _groups = new List<string>();
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds a group type for collision detection with another group type.
        /// </summary>
        /// <param name="type">The group type to add.</param>
        public static void AddType(string type)
        {
            _groups.Add(type);
        }


        /// <summary>
        /// Checks if the two given bounds are colliding.
        /// </summary>
        /// <returns></returns>
        /// <param name="a">The object that triggers the collision.</param>
        /// <param name="b">The object that is being collided into.</param>
        public static bool IsColliding(Rect a, Rect b)
        {
            return a.Contains(b);
        }


        /// <summary>
        /// Checks if the two given objects are colliding.
        /// </summary>
        /// <param name="objA">The object that triggers the collision.</param>
        /// <param name="objB">The object that is being collided into.</param>
        /// <returns></returns>
        public static bool IsColliding(GameObject objA, GameObject objB)
        {
            return objA.Bounds.Contains(objB.Bounds);
        }
        #endregion
    }
}
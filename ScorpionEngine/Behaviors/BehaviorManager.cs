using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace KDScorpionEngine.Behaviors
{
    internal static class BehaviorManager
    {
        private static Dictionary<Guid, IBehavior> behaviors = new Dictionary<Guid, IBehavior>();

        public static void Add(Guid entityId, IBehavior behavior)
        {
            behaviors.Add(entityId, behavior);
        }

        public static void Remove(Guid entityid, Guid behaviorId)
        {
            var entityBehaviors = (from b in behaviors
                                   where b.Key == entityid
                                   select b).ToArray();

            for (var i = 0; i < entityBehaviors.Length; i++)
            {
                if (entityBehaviors[i].Value.ID == behaviorId)
                {
                    behaviors.Remove(entityBehaviors[i].Key);
                    break;
                }
            }
        }
    }
}

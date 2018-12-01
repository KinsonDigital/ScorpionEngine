using ScorpionCore;
using System;

namespace ScorpionEngine.Behaviors
{
    public abstract class Behavior : IBehavior
    {
        private Action<EngineTime> _behaviorAction;


        #region Props
        public bool Enabled { get; set; } = true;

        public string Name { get; set; }
        #endregion


        #region Public Methods
        public void Update(EngineTime engineTime)
        {
            if (_behaviorAction == null || !Enabled)
                return;

            _behaviorAction(engineTime);
        }


        protected void SetUpdateAction(Action<EngineTime> action)
        {
            _behaviorAction = action;
        }
        #endregion
    }
}

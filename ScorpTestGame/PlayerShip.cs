using ScorpionEngine;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Content;
using ScorpionEngine.Entities;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Physics;

namespace ScorpTestGame
{
    /// <summary>
    /// The player ship in the game.
    /// </summary>
    public class PlayerShip : DynamicEntity
    {
        private Keyboard _keyboard;


        /// <summary>
        /// Creates a new instance of <see cref="PlayerShip"/>.
        /// </summary>
        public PlayerShip()
        {
            DebugDrawEnabled = true;
            MaxLinearSpeed = 0.5f;
            MaxRotationSpeed = 0.5f;
            AngularDeceleration = 0.25f;
            Position = new Vector(330, 200);

            Vertices = new Vector[3]
            {
                new Vector(-0, -21),
                new Vector(21, 21),
                new Vector(-21, 21)
            };

            var keyMovementBehavior = new MovementByKeyboardBehavior<PlayerShip>(this, 1f)
            {
                MoveUpKey = InputKeys.W,
                MoveDownKey = InputKeys.S,
                MoveLeftKey = InputKeys.A,
                MoveRightKey = InputKeys.D
            };

            Behaviors.Add(keyMovementBehavior);
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(EngineTime engineTime)
        {
            base.Update(engineTime);
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            Texture = contentLoader.LoadTexture("Ship");

            base.LoadContent(contentLoader);
        }
    }
}

using ScorpionEngine;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Content;
using ScorpionEngine.Entities;
using ScorpionEngine.Input;
using ScorpionEngine.Physics;

namespace ScorpTestGame
{
    /// <summary>
    /// The player ship in the game.
    /// </summary>
    public class PlayerShip : DynamicEntity
    {
        private Keyboard _keyboard = new Keyboard();


        /// <summary>
        /// Creates a new instance of <see cref="PlayerShip"/>.
        /// </summary>
        public PlayerShip() : base(friction: 0f)
        {
            DebugDrawEnabled = true;
            AngularDeceleration = 100f;
            Position = new Vector(500, 300);

            Vertices = new Vector[3]
            {
                new Vector(0, -21),
                new Vector(21, 21),
                new Vector(-21, 21)
            };

            var movementBehavior = new MoveFowardKeyboardBehavior<PlayerShip>(this, 1f, 0.25f)
            {
                MoveFowardKey = InputKeys.Up,
                RotateCW = InputKeys.Right,
                RotateCCW = InputKeys.Left
            };

            Behaviors.Add(movementBehavior);
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(EngineTime engineTime)
        {
            //_keyboard.UpdateCurrentState();
            //_keyboard.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            Texture = contentLoader.LoadTexture("Ship");

            base.LoadContent(contentLoader);
        }
    }
}

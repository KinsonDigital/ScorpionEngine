using ScorpionEngine;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Content;
using ScorpionEngine.Entities;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Particles;
using ScorpionEngine.Physics;
using ScorpionEngine.Utils;

namespace ScorpTestGame
{
    /// <summary>
    /// The player ship in the game.
    /// </summary>
    public class PlayerShip : DynamicEntity
    {
        private Keyboard _keyboard = new Keyboard();
        private Vector _thrusterPosition;
        private ParticleEngine _particleEngine;


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

            _thrusterPosition = new Vector(Position.X, Position.Y + 22.5f);
            _thrusterPosition = Tools.RotateAround(_thrusterPosition, Position, Angle);

            var colors = new GameColor[]
            {
                new GameColor(255, 216, 0, 255),
                new GameColor(255, 0, 0, 255),
                new GameColor(255, 106, 0, 255)
            };

            _particleEngine = new ParticleEngine(_thrusterPosition)
            { 
                UseRandomVelocity = true,
                TotalParticlesAliveAtOnce = 60,
                UseTintColorList = false,
                TintColors = colors,
                RedMin = 255,
                RedMax = 255,
                GreenMin = 132,
                GreenMax = 209,
                BlueMin = 0,
                BlueMax = 0,
                SizeMin = 0.05f,
                SizeMax = 0.20f,
                LifeTimeMax = 700,
                VelocityXMin = -0.25f,
                VelocityXMax = 0.25f,
                VelocityYMin = 0,
                VelocityYMax = 1f
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


        public override void LoadContent(ContentLoader contentLoader)
        {
            Texture = contentLoader.LoadTexture("Ship");

            _particleEngine.AddTexture(contentLoader.LoadTexture(@"Particles\ShipThruster"));

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            //_keyboard.UpdateCurrentState();

            _thrusterPosition = new Vector(Position.X, Position.Y + 22.5f);
            _thrusterPosition = Tools.RotateAround(_thrusterPosition, Position, Angle);

            _particleEngine.SpawnLocation = _thrusterPosition;
            _particleEngine.Update(engineTime);

            //_keyboard.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _particleEngine.Render(renderer);

            base.Render(renderer);
        }
    }
}

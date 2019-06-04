using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Input;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Input;
using KDScorpionEngine.Utils;

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
        private MoveFowardKeyboardBehavior<PlayerShip> _movementBehavior;


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
            _thrusterPosition = _thrusterPosition.RotateAround(Position, Angle);

            var colors = new GameColor[]
            {
                new GameColor(255, 255, 216, 0),
                new GameColor(255, 255, 0, 0),
                new GameColor(255, 255, 106, 0)
            };

            _particleEngine = new ParticleEngine(new RandomizerService())
            { 
                SpawnLocation = _thrusterPosition,
                UseRandomVelocity = true,
                TotalParticlesAliveAtOnce = 40,
                UseColorsFromList = false,
                TintColors = colors,
                RedMin = 255,
                RedMax = 255,
                GreenMin = 132,
                GreenMax = 209,
                BlueMin = 0,
                BlueMax = 0,
                SizeMin = 0.2f,
                SizeMax = 0.3f,
                LifeTimeMax = 250,
                VelocityXMin = -1.5f,
                VelocityXMax = 1.5f,
                VelocityYMin = -1.5f,
                VelocityYMax = 1.5f,
                AngularVelocityMin = 0,
                AngularVelocityMax = 2
            };

            _movementBehavior = new MoveFowardKeyboardBehavior<PlayerShip>(this, 2f, 0.25f)
            {
                MoveFowardKey = KeyCodes.Up,
                RotateCWKey = KeyCodes.Right,
                RotateCCWKey = KeyCodes.Left,
                Enabled = true
            };
            
            Behaviors.Add(_movementBehavior);
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            Texture = contentLoader.LoadTexture(@"Ship");

            _particleEngine.AddTexture(contentLoader.LoadTexture(@"Particles\Triangle"));

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            _keyboard.UpdateCurrentState();

            //Update the spawn position of the thruster particels
            _thrusterPosition = new Vector(Position.X, Position.Y + 22.5f);
            _thrusterPosition = _thrusterPosition.RotateAround(Position, Angle);

            _particleEngine.SpawnLocation = _thrusterPosition;

            _particleEngine.Enabled = _movementBehavior.IsMovingForward;

            _movementBehavior.Update(engineTime);
            _particleEngine.Update(engineTime);

            base.Update(engineTime);

            _keyboard.UpdatePreviousState();
        }


        public override void Render(Renderer renderer)
        {
            _particleEngine.Render(renderer);

            base.Render(renderer);
        }
    }
}

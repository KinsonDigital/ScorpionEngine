using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Input;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Input;

namespace ScorpTestGame
{
    /// <summary>
    /// The player ship in the game.
    /// </summary>
    public class PlayerShip : DynamicEntity
    {
        #region Private Fields
        private readonly Keyboard _keyboard = new Keyboard();
        private readonly Mouse _mouse = new Mouse();
        private Vector _thrusterPosition;
        private readonly ParticleEngine<Texture> _particleEngine;
        private readonly MoveFowardKeyboardBehavior<PlayerShip> _movementBehavior;
        private const float _particleVelocityMagnitudeMin = 0.25f;
        private const float _particleVelocityMagnitudeMax = 2;
        private readonly GameColor[] _orangeColors = new GameColor[]
        {
            new GameColor(255, 255, 216, 0),
            new GameColor(255, 255, 0, 0),
            new GameColor(255, 255, 106, 0)
        };
        private readonly GameColor[] _blueColors = new GameColor[]
        {
            new GameColor(255, 0, 255, 255),
            new GameColor(255, 0, 38, 255),
            new GameColor(255, 0, 19, 127)
        };
        private readonly GameColor[] _purpleColors = new GameColor[]
        {
            new GameColor(255, 255, 0, 220),
            new GameColor(255, 178, 0, 255),
            new GameColor(255, 87, 0, 127)
        };
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="PlayerShip"/>.
        /// </summary>
        public PlayerShip() : base(friction: 0f)
        {
            DebugDrawEnabled = true;
            AngularDeceleration = 100f;
            Position = new Vector(300, 250);

            //Ship vertices
            Vertices = new Vector[3]
            {
                new Vector(0, -21),
                new Vector(21, 21),
                new Vector(-21, 21)
            };

            _particleEngine = new ParticleEngine<Texture>(new RandomizerService())
            {
                SpawnLocation = _thrusterPosition.ToPointF(),
                UseRandomVelocity = true,
                TotalParticlesAliveAtOnce = 40,
                UseColorsFromList = true,
                TintColors = _orangeColors.ToNETColors(),
                RedMin = 255,
                RedMax = 255,
                GreenMin = 132,
                GreenMax = 209,
                BlueMin = 0,
                BlueMax = 0,
                SizeMin = 0.2f,
                SizeMax = 0.3f,
                LifeTimeMax = 250,
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
        #endregion


        #region Public Methods
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
            _mouse.UpdateCurrentState();

            if (_mouse.IsButtonPressed(InputButton.LeftButton))
            {
                _particleEngine.TintColors = _orangeColors.ToNETColors();
            }

            if (_mouse.IsButtonPressed(InputButton.RightButton))
            {
                _particleEngine.TintColors = _blueColors.ToNETColors();
            }

            if (_mouse.IsButtonPressed(InputButton.MiddleButton))
            {
                _particleEngine.TintColors = _purpleColors.ToNETColors();
            }

            if (_keyboard.IsKeyPressed(KeyCodes.Enter))
            {
                _particleEngine.TintColors = _purpleColors.ToNETColors();
            }

            if (_keyboard.IsKeyPressed(KeyCodes.RightShift))
            {
                _particleEngine.TintColors = _blueColors.ToNETColors();
            }


            //Update the spawn position of the thruster particels
            _thrusterPosition = new Vector(Position.X, Position.Y + 22.5f);
            _thrusterPosition = _thrusterPosition.RotateAround(Position, Angle);

            //Update the X and Y velocity of the particles
            var rotatedParticleMin = new Vector(0, _particleVelocityMagnitudeMin).RotateAround(Vector.Zero, Angle);
            var rotatedParticleMax = new Vector(0, _particleVelocityMagnitudeMax).RotateAround(Vector.Zero, Angle);
            _particleEngine.VelocityXMin = rotatedParticleMin.X;
            _particleEngine.VelocityXMax = rotatedParticleMax.X;
            _particleEngine.VelocityYMin = rotatedParticleMin.Y;
            _particleEngine.VelocityYMax = rotatedParticleMax.Y;

            _particleEngine.SpawnLocation = _thrusterPosition.ToPointF();

            _particleEngine.Enabled = _movementBehavior.IsMovingForward;

            _movementBehavior.Update(engineTime);
            _particleEngine.Update(engineTime.ToTimeSpan());

            _mouse.UpdatePreviousState();
            _keyboard.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            if (_particleEngine.Enabled)
            {
                foreach (var particle in _particleEngine.Particles)
                {
                    renderer.RennderParticle(particle);
                }
            }


            base.Render(renderer);
        }
        #endregion
    }
}

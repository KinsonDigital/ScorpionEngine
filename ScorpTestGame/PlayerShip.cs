using KDParticleEngine;
using KDParticleEngine.Behaviors;
using KDParticleEngine.Services;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;
using Raptor;
using Raptor.Content;
using Raptor.Graphics;
using Raptor.Input;
using System.Drawing;
using System.Linq;
using System.Numerics;

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
        private Vector2 _thrusterPosition;
        private ParticleEngine<Texture> _particleEngine;
        private readonly MoveFowardKeyboardBehavior<PlayerShip> _movementBehavior;
        private ITextureLoader<Texture> _textureLoader;
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
            DebugDrawEnabled = false;
            AngularDeceleration = 100f;
            Position = new Vector2(300, 250);
            Angle = 45;
            

            //Ship vertices
            Vertices = new Vector2[3]
            {
                new Vector2(0, -22),
                new Vector2(23, 22),
                new Vector2(-23, 22)
            };


            _movementBehavior = new MoveFowardKeyboardBehavior<PlayerShip>(this, 1f, 0.12f)
            {
                MoveFowardKey = KeyCode.Up,
                RotateCWKey = KeyCode.Right,
                RotateCCWKey = KeyCode.Left,
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

            _textureLoader = new ParticleTextureLoader(contentLoader);

            _particleEngine = new ParticleEngine<Texture>(_textureLoader, new TrueRandomizerService());

            IBehaviorFactory behaviorFactory = new BehaviorFactory();

            var settings = new BehaviorSetting[]
            {
                new BehaviorSetting()
                {
                    ApplyToAttribute = ParticleAttribute.Y,
                    StartMin = Position.Y,
                    StartMax = Position.Y,
                    ChangeMin = 100,
                    ChangeMax = 100,
                    TotalTimeMin = 2,
                    TotalTimeMax = 2,
                    TypeOfBehavior = BehaviorType.EaseIn
                }
            };
            var effect = new ParticleEffect(@"Particles\Triangle", settings)
            {
                SpawnLocation = Position.ToPointF(),
                SpawnRateMin = 250,
                SpawnRateMax = 250,
                UseColorsFromList = false,
                TotalParticlesAliveAtOnce = 100
            };

            _particleEngine.CreatePool(effect, behaviorFactory);

            _particleEngine.LoadTextures();

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            _keyboard.UpdateCurrentState();
            _mouse.UpdateCurrentState();

            //TODO: Figure out how to accoomplish the code below with new particle engine changes
            //if (_mouse.IsButtonPressed(InputButton.LeftButton))
            //{
            //    _particleEngine.TintColors = _orangeColors.ToNETColors();
            //}

            //if (_mouse.IsButtonPressed(InputButton.RightButton))
            //{
            //    _particleEngine.TintColors = _blueColors.ToNETColors();
            //}

            //if (_mouse.IsButtonPressed(InputButton.MiddleButton))
            //{
            //    _particleEngine.TintColors = _purpleColors.ToNETColors();
            //}

            //if (_keyboard.IsKeyPressed(KeyCode.Enter))
            //{
            //    _particleEngine.TintColors = _purpleColors.ToNETColors();
            //}

            //if (_keyboard.IsKeyPressed(KeyCode.RightShift))
            //{
            //    _particleEngine.TintColors = _blueColors.ToNETColors();
            //}


            //Update the spawn position of the thruster particels
            _thrusterPosition = new Vector2(Position.X, Position.Y + 11);
            _thrusterPosition = _thrusterPosition.RotateAround(Position, Angle);


            //TODO: Need to figure out how to create a follow angle behavior as well as a follow position behavior
            //Update the X and Y velocity of the particles
            //var rotatedParticleMin = new Vector2(0, _particleVelocityMagnitudeMin).RotateAround(Vector2.Zero, Angle);
            //var rotatedParticleMax = new Vector2(0, _particleVelocityMagnitudeMax).RotateAround(Vector2.Zero, Angle);

            //_particleEngine.VelocityXMin = rotatedParticleMin.X;
            //_particleEngine.VelocityXMax = rotatedParticleMax.X;
            //_particleEngine.VelocityYMin = rotatedParticleMin.Y;
            //_particleEngine.VelocityYMax = rotatedParticleMax.Y;

            //Update the pawn location of the particles
            _particleEngine.ParticlePools[0].Effect.SpawnLocation = _thrusterPosition.ToPointF();

            //_particleEngine.Enabled = _movementBehavior.IsMovingForward;

            _movementBehavior.Update(engineTime);

            _particleEngine.Update(engineTime.ToTimeSpan());

            _mouse.UpdatePreviousState();
            _keyboard.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(GameRenderer renderer)
        {
            foreach (var pool in _particleEngine.ParticlePools)
            {
                foreach (var particle in pool.Particles)
                {
                    renderer.Render(pool.PoolTexture, particle.Position.X, particle.Position.Y, particle.Angle, 1f, new GameColor(255, 255, 255, 255));
                }
            }
        }
        #endregion
    }
}

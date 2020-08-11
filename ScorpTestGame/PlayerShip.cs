// <copyright file="PlayerShip.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame
{
    using System.Drawing;
    using System.Linq;
    using System.Numerics;
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

    /// <summary>
    /// The player ship in the game.
    /// </summary>
    public class PlayerShip : DynamicEntity
    {
        private readonly Keyboard keyboard = new Keyboard();
        private readonly Mouse mouse = new Mouse();
        private Vector2 thrusterPosition;
        private ParticleEngine<Texture> particleEngine;
        private readonly MoveFowardKeyboardBehavior<PlayerShip> movementBehavior;
        private ITextureLoader<Texture> textureLoader;
        private const float particleVelocityMagnitudeMin = 0.25f;
        private const float particleVelocityMagnitudeMax = 2;
        private readonly GameColor[] orangeColors = new GameColor[]
        {
            new GameColor(255, 255, 216, 0),
            new GameColor(255, 255, 0, 0),
            new GameColor(255, 255, 106, 0),
        };
        private readonly GameColor[] blueColors = new GameColor[]
        {
            new GameColor(255, 0, 255, 255),
            new GameColor(255, 0, 38, 255),
            new GameColor(255, 0, 19, 127),
        };
        private readonly GameColor[] purpleColors = new GameColor[]
        {
            new GameColor(255, 255, 0, 220),
            new GameColor(255, 178, 0, 255),
            new GameColor(255, 87, 0, 127),
        };

        /// <summary>
        /// Creates a new instance of <see cref="PlayerShip"/>.
        /// </summary>
        public PlayerShip()
            : base(friction: 0f)
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
                new Vector2(-23, 22),
            };

            this.movementBehavior = new MoveFowardKeyboardBehavior<PlayerShip>(this, 1f, 0.12f)
            {
                MoveFowardKey = KeyCode.Up,
                RotateCWKey = KeyCode.Right,
                RotateCCWKey = KeyCode.Left,
                Enabled = true,
            };

            Behaviors.Add(this.movementBehavior);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(ContentLoader contentLoader)
        {
            Texture = contentLoader.LoadTexture(@"Ship");

            this.textureLoader = new ParticleTextureLoader(contentLoader);

            this.particleEngine = new ParticleEngine<Texture>(this.textureLoader, new TrueRandomizerService());

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
                },
            };
            var effect = new ParticleEffect(@"Particles\Triangle", settings)
            {
                SpawnLocation = Position.ToPointF(),
                SpawnRateMin = 250,
                SpawnRateMax = 250,
                UseColorsFromList = false,
                TotalParticlesAliveAtOnce = 100,
            };

            this.particleEngine.CreatePool(effect, behaviorFactory);

            this.particleEngine.LoadTextures();

            base.LoadContent(contentLoader);
        }

        public override void Update(EngineTime engineTime)
        {
            this.keyboard.UpdateCurrentState();
            this.mouse.UpdateCurrentState();

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
            this.thrusterPosition = new Vector2(Position.X, Position.Y + 11);
            this.thrusterPosition = this.thrusterPosition.RotateAround(Position, Angle);

            //TODO: Need to figure out how to create a follow angle behavior as well as a follow position behavior
            //Update the X and Y velocity of the particles
            //var rotatedParticleMin = new Vector2(0, _particleVelocityMagnitudeMin).RotateAround(Vector2.Zero, Angle);
            //var rotatedParticleMax = new Vector2(0, _particleVelocityMagnitudeMax).RotateAround(Vector2.Zero, Angle);

            //_particleEngine.VelocityXMin = rotatedParticleMin.X;
            //_particleEngine.VelocityXMax = rotatedParticleMax.X;
            //_particleEngine.VelocityYMin = rotatedParticleMin.Y;
            //_particleEngine.VelocityYMax = rotatedParticleMax.Y;

            //Update the pawn location of the particles
            this.particleEngine.ParticlePools[0].Effect.SpawnLocation = this.thrusterPosition.ToPointF();

            //_particleEngine.Enabled = _movementBehavior.IsMovingForward;

            this.movementBehavior.Update(engineTime);

            this.particleEngine.Update(engineTime.ToTimeSpan());

            this.mouse.UpdatePreviousState();
            this.keyboard.UpdatePreviousState();

            base.Update(engineTime);
        }

        public override void Render(GameRenderer renderer)
        {
            foreach (var pool in this.particleEngine.ParticlePools)
            {
                foreach (var particle in pool.Particles)
                {
                    renderer.Render(pool.PoolTexture, particle.Position.X, particle.Position.Y, particle.Angle, 1f, new GameColor(255, 255, 255, 255));
                }
            }
        }
    }
}

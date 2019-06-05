using KDParticleEngine.Services;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KDParticleEngine
{
    //TODO: Look into using a better random number generator than the built in .NET framework one

    /// <summary>
    /// Manages multiple <see cref="Particle"/>s with various settings that dictate
    /// how all of the <see cref="Particle"/>s behave and look on the screen.
    /// </summary>
    public class ParticleEngine
    {
        #region Public Events
        public event EventHandler<EventArgs> LivingParticlesCountChanged;
        #endregion


        #region Fields
        private readonly List<Particle> _particles;
        private readonly List<Texture> _textures = new List<Texture>();
        private int _totalParticlesAliveAtOnce = 10;
        private int _spawnRateElapsed = 0;
        private float _angleMin;
        private float _angleMax = 360;
        private bool _enabled = true;
        private int _spawnRate;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleEngine"/>.
        /// </summary>
        public ParticleEngine(IRandomizerService randomizer)
        {
            _particles = new List<Particle>();
            Randomizer = randomizer;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the randomizer used when generating new particles.
        /// </summary>
        public IRandomizerService Randomizer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the engine is enabled or disabled.
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;

                //If the engine is disabled, kill all the particles
                if (!_enabled)
                {
                    for (int i = 0; i < _particles.Count; i++)
                    {
                        _particles[i].IsDead = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the location on the screen of where to spawn the <see cref="Particle"/>s.
        /// </summary>
        public Vector SpawnLocation { get; set; }

        /// <summary>
        /// Gets or sets the total number of <see cref="Particle"/>s that can be alive at once.
        /// </summary>
        public int TotalParticlesAliveAtOnce
        {
            get => _totalParticlesAliveAtOnce;
            set
            {
                _totalParticlesAliveAtOnce = value;
                GenerateAllParticles();
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle in degrees that a <see cref="Particle"/> will be set at when its first created.
        /// Only valid range is 0 to 360.  Any value outside of that range will be set to 0.
        /// </summary>
        public float AngleMin
        {
            get => _angleMin;
            set
            {
                _angleMin = value < 0 ? 360 : value;
                _angleMin = value > 360 ? 0 : value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum angle in degrees that a <see cref="Particle"/> will be set at when its first created.
        /// Only valid range is 0 to 360.  Any value outside of that range will be set to 0.
        /// </summary>
        public float AngleMax
        {
            get => _angleMax;
            set
            {
                _angleMax = value < 0 ? 360 : value;
                _angleMax = value > 360 ? 0 : value;
            }
        }

        /// <summary>
        /// The minimum amount of time in milliseconds that a newly generated <see cref="Particle"/> will last.
        /// </summary>
        public int LifeTimeMin { get; set; } = 125;

        /// <summary>
        /// The maximum amount of time in milliseconds that a newly generated <see cref="Particle"/> will last.
        /// </summary>
        public int LifeTimeMax { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the minimum size that a newly generated <see cref="Particle"/> will be.
        /// The value of 1 will represent 100% or the normal size of the <see cref="Particle"/>
        /// <see cref="Texture"/>s.
        /// </summary>
        public float SizeMin { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the maximum size that a newly generated <see cref="Particle"/> will be.
        /// The value of 1 will represent 100% or the normal size of the <see cref="Particle"/>
        /// <see cref="Texture"/>s.
        /// </summary>
        public float SizeMax { get; set; } = 1.5f;

        /// <summary>
        /// Gets or sets the minimum angular velocity in degrees of a newly generated <see cref="Particle"/>.
        /// </summary>
        public float AngularVelocityMin { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the maximum angular velocity in degrees of a newly generated <see cref="Particle"/>.
        /// </summary>
        public float AngularVelocityMax { get; set; } = 4f;

        /// <summary>
        /// Gets or sets the minimum amount of time for the spawn rate range that a <see cref="Particle"/> will next be spawned.
        /// </summary>
        public int SpawnRateMin { get; set; } = 62;

        /// <summary>
        /// Gets or sets the maximum amount of time for the spawn rate range that a <see cref="Particle"/> will next be spawned.
        /// </summary>
        public int SpawnRateMax { get; set; } = 62;

        /// <summary>
        /// Gets current total number of living <see cref="Particle"/>s.
        /// </summary>
        public int TotalLivingParticles => _particles.Count(p => p.IsAlive);

        /// <summary>
        /// Gets the current total number of dead <see cref="Particle"/>s.
        /// </summary>
        public int TotalDeadParticles => _particles.Count(p => p.IsDead);

        /// <summary>
        /// Gets or sets a value that indicates if the <see cref="ParticleEngine"/> will 
        /// spawn <see cref="Particle"/>s with a random or set velocity.
        /// </summary>
        public bool UseRandomVelocity { get; set; } = true;

        /// <summary>
        /// Gets or sets the velocity of newly spawned <see cref="Particle"/>s. This is only used
        /// when the <see cref="UseRandomVelocity"/> property is set to false.
        /// <seealso cref="UseRandomVelocity"/>
        /// </summary>
        public Vector ParticleVelocity { get; set; } = new Vector(0, 1);

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="ParticleEngine"/> will
        /// spawn <see cref="Particle"/>s with a randomly chosen tint color from the list or
        /// if the <see cref="ParticleEngine"/> will randomize the tint color components themselves
        /// using the various color component property values.
        /// <para/>
        /// See the list of properties below for randomizing color components<para/>
        /// <list type="bullet">
        ///     <item>
        ///         <description>
        ///             <see cref="RedMin"/>,
        ///             <see cref="RedMax"/>,
        ///             <see cref="GreenMin"/>,
        ///             <see cref="GreenMax"/>,
        ///             <para/>
        ///             <see cref="BlueMin"/>,
        ///             <see cref="BlueMax"/>,
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool UseColorsFromList { get; set; }

        /// <summary>
        /// Gets or sets the list of colors that the <see cref="ParticleEngine"/> will
        /// randomly choose from when spawning a new <see cref="Particle"/>.
        /// Only used if the <see cref="UseColorsFromList"/> is set to true.
        /// </summary>
        public GameColor[] TintColors { get; set; } = new GameColor[0];

        /// <summary>
        /// Gets or sets the minimum value for the red component when randomly choosing
        /// the <see cref="Particle.TintColor"/> when spawing a new <see cref="Particle"/>.
        /// </summary>
        public byte RedMin { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value for the red component when randomly choosing
        /// the <see cref="Particle.TintColor"/> when spawing a new <see cref="Particle"/>.
        /// </summary>
        public byte RedMax { get; set; } = 255;

        /// <summary>
        /// Gets or sets the minimum value for the green component when randomly choosing
        /// the <see cref="Particle.TintColor"/> when spawing a new <see cref="Particle"/>.
        /// </summary>
        public byte GreenMin { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value for the green component when randomly choosing
        /// the <see cref="Particle.TintColor"/> when spawing a new <see cref="Particle"/>.
        /// </summary>
        public byte GreenMax { get; set; } = 255;

        /// <summary>
        /// Gets or sets the minimum value for the blue component when randomly choosing
        /// the <see cref="Particle.TintColor"/> when spawing a new <see cref="Particle"/>.
        /// </summary>
        public byte BlueMin { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value for the blue component when randomly choosing
        /// the <see cref="Particle.TintColor"/> when spawing a new <see cref="Particle"/>.
        /// </summary>
        public byte BlueMax { get; set; } = 255;

        /// <summary>
        /// Gets or sets the minimum X value for the <see cref="Particle.Velocity"/> when
        /// spawning new <see cref="Particle"/>.
        /// </summary>
        public float VelocityXMin { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum X value for the <see cref="Particle.Velocity"/> when
        /// spawning new <see cref="Particle"/>.
        /// </summary>
        public float VelocityXMax { get; set; } = 1;

        /// <summary>
        /// Gets or sets the minimum Y value for the <see cref="Particle.Velocity"/> when
        /// spawning new <see cref="Particle"/>.
        /// </summary>
        public float VelocityYMin { get; set; } = -1;

        /// <summary>
        /// Gets or sets the maximum Y value for the <see cref="Particle.Velocity"/> when
        /// spawning new <see cref="Particle"/>.
        /// </summary>
        public float VelocityYMax { get; set; } = 1f;
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given <paramref name="texture"/> to the engine.
        /// </summary>
        /// <param name="texture">The texture to add.</param>
        public void AddTexture(Texture texture)
        {
            _textures.Add(texture);
            GenerateAllParticles();
        }


        /// <summary>
        /// Adds the given <paramref name="textures"/> to the engine.
        /// </summary>
        /// <param name="textures">The list of textures to add.</param>
        public void AddTextures(Texture[] textures)
        {
            _textures.AddRange(textures);
            GenerateAllParticles();
        }


        /// <summary>
        /// Clears all of the textures.
        /// </summary>
        public void ClearTextures()
        {
            _textures.Clear();
        }


        /// <summary>
        /// Updates all of the <see cref="Particle"/>s.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the <see cref="Engine"/> since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
            if (!Enabled)
                return;

            _spawnRateElapsed += engineTime.ElapsedEngineTime.Milliseconds;

            //If the amount of time to spawn a new particle has passed
            if (_spawnRateElapsed >= _spawnRate)
            {
                _spawnRate = GetRandomSpawnRate();

                SpawnNewParticle();

                _spawnRateElapsed = 0;
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsDead)
                    continue;

                //Update the current particle
                _particles[i].Update(engineTime);

                //If the current particle's time to live has expired, kill the particle
                if (_particles[i].LifeTime <= 0)
                {
                    _particles[i].IsAlive = false;

                    //Invoke the engine updated event
                    LivingParticlesCountChanged?.Invoke(this, new EventArgs());
                }
            }
        }


        /// <summary>
        /// Renders all of the <see cref="Particle"/>s.
        /// </summary>
        /// <param name="renderer">Renders the <see cref="Particle"/>s to the screen.</param>
        public void Render(Renderer renderer)
        {
            if (!Enabled)
                return;

            foreach (var particle in _particles)
            {
                particle.Render(renderer);
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Spawns a new <see cref="Particle"/>.  This simple finds the first dead <see cref="Particle"/> and
        /// sets it back to alive and sets all of its parameters to random values.
        /// </summary>
        private void SpawnNewParticle()
        {
            //Find the first dead particle and bring it back to life
            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsDead)
                {
                    _particles[i].Texture = GetRandomTexture();
                    _particles[i].Position = SpawnLocation;
                    _particles[i].Velocity = UseRandomVelocity ? GetRandomVelocity() : ParticleVelocity;
                    _particles[i].Angle = GetRandomAngle();
                    _particles[i].AngularVelocity = GetRandomAngularVelocity();
                    _particles[i].TintColor = GetRandomColor();
                    _particles[i].Size = GetRandomSize();
                    _particles[i].LifeTime = GetRandomLifeTime();
                    _particles[i].IsAlive = Enabled;
                }
            }
        }


        /// <summary>
        /// Returns a random time in milliseconds that the <see cref="Particle"/> will be spawned next.
        /// </summary>
        /// <returns></returns>
        private int GetRandomSpawnRate()
        {
            if (SpawnRateMin <= SpawnRateMax)
                return Randomizer.GetValue(SpawnRateMin, SpawnRateMax);


            return Randomizer.GetValue(SpawnRateMax, SpawnRateMin);
        }


        /// <summary>
        /// Generates all of the particles.
        /// </summary>
        private void GenerateAllParticles()
        {
            if (_textures.Count <= 0)
                return;

            _particles.Clear();

            for (int i = 0; i < TotalParticlesAliveAtOnce; i++)
            {
                _particles.Add(GenerateParticle());
            }
        }


        /// <summary>
        /// Generates a single <see cref="Particle"/> with random settings based on the <see cref="ParticleEngine"/>s
        /// range settings.
        /// </summary>
        /// <returns></returns>
        private Particle GenerateParticle()
        {
            var texture = GetRandomTexture();

            var position = SpawnLocation;

            var velocity = UseRandomVelocity ? GetRandomVelocity() : ParticleVelocity;

            var angle = GetRandomAngle();

            var angularVelocity = GetRandomAngularVelocity();

            var color = GetRandomColor();

            var size = GetRandomSize();

            var lifeTime = GetRandomLifeTime();

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifeTime);
        }


        /// <summary>
        /// Returns a randomly chosen <see cref="Texture"/> out of the total list of textures to use for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private Texture GetRandomTexture()
        {
            var result = Randomizer.GetValue(0, _textures.Count);


            return _textures[result >= 0 && result <= _textures.Count ? result : 0];
        }


        /// <summary>
        /// Returns a random <see cref="Particle.Velocity"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private Vector GetRandomVelocity()
        {
            var velXRandomResult = Randomizer.GetValue(VelocityXMin, VelocityXMax);
            var velYRandomResult = Randomizer.GetValue(VelocityYMin, VelocityYMax);

            return new Vector(velXRandomResult,
                              velYRandomResult);
        }


        /// <summary>
        /// Returns a random <see cref="Particle.Angle"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private float GetRandomAngle()
        {
            var result = Randomizer.GetValue(AngleMin, AngleMax);

            return Randomizer.GetValue(AngleMin, AngleMax);
        }


        /// <summary>
        /// Returns a random <see cref="Particle.AngularVelocity"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private float GetRandomAngularVelocity()
        {
            return Randomizer.GetValue(AngularVelocityMin, AngularVelocityMax) * (Randomizer.FlipCoin() ? 1 : -1);
        }


        /// <summary>
        /// Returns a random <see cref="Particle.TintColor"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private GameColor GetRandomColor()
        {
            if (UseColorsFromList)
            {
                return TintColors == null || TintColors.Length == 0 ? new GameColor(255, 255, 255, 255) : TintColors[Randomizer.GetValue(0, TintColors.Length)];
            }
            else
            {
                var red = RedMin <= RedMax ?
                    (byte)Randomizer.GetValue(RedMin, RedMax) :
                    (byte)Randomizer.GetValue(RedMax, RedMin);
                var green = GreenMin <= GreenMax ? 
                    (byte)Randomizer.GetValue(GreenMin, GreenMax) :
                    (byte)Randomizer.GetValue(GreenMax, GreenMin);
                var blue = BlueMin <= BlueMax ?
                    (byte)Randomizer.GetValue(BlueMin, BlueMax) :
                    (byte)Randomizer.GetValue(BlueMax, BlueMin);

                return new GameColor(255, red, green, blue);
            }
        }


        /// <summary>
        /// Returns a random <see cref="Particle.Size"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private float GetRandomSize()
        {
            return Randomizer.GetValue(SizeMin, SizeMax);
        }


        /// <summary>
        /// Returns a random <see cref="Particle.LifeTime"/> for a spawned <see cref="Particle"/>.
        /// If the max is less than the min, the <see cref="Particle.LifeTime"/> will still be chosen
        /// randomly between the two values.
        /// </summary>
        /// <returns></returns>
        private int GetRandomLifeTime()
        {
            if (LifeTimeMin <= LifeTimeMax)
                return Randomizer.GetValue(LifeTimeMin, LifeTimeMax);

            var myResult = Randomizer.GetValue(LifeTimeMin, LifeTimeMax);

            return Randomizer.GetValue(LifeTimeMax, LifeTimeMin);
        }


        /// <summary>
        /// Applies the given <paramref name="setupData"/> to the engine.
        /// </summary>
        /// <param name="setupData">The setup data to apply.</param>
        public void ApplySetup(ParticleSetup setupData)
        {
            RedMin = setupData.RedMin;
            RedMax = setupData.RedMax;

            GreenMin = setupData.GreenMin;
            GreenMax = setupData.GreenMax;

            BlueMin = setupData.BlueMin;
            BlueMax = setupData.BlueMax;

            SizeMin = setupData.SizeMin;
            SizeMax = setupData.SizeMax;

            AngleMin = setupData.AngleMin;
            AngleMax = setupData.AngleMax;

            AngularVelocityMin = setupData.AngularVelocityMin;
            AngularVelocityMax = setupData.AngularVelocityMax;

            VelocityXMin = setupData.VelocityXMin;
            VelocityXMax = setupData.VelocityXMax;

            VelocityYMin = setupData.VelocityYMin;
            VelocityYMax = setupData.VelocityYMax;

            LifeTimeMin = setupData.LifeTimeMin;
            LifeTimeMax = setupData.LifeTimeMax;

            SpawnRateMin = setupData.SpawnRateMin;
            SpawnRateMax = setupData.SpawnRateMax;

            TotalParticlesAliveAtOnce = setupData.TotalParticlesAliveAtOnce;

            UseColorsFromList = setupData.UseColorsFromList;
            TintColors = setupData.Colors.ToGameColors();
        }


        /// <summary>
        /// Generates a particle setup from the current settings of the <see cref="ParticleEngine"/>.
        /// </summary>
        /// <returns></returns>
        public ParticleSetup GenerateParticleSetup()
        {
            return new ParticleSetup()
            {
                RedMin = RedMin,
                RedMax = RedMax,
                GreenMin = GreenMin,
                GreenMax = GreenMax,
                BlueMin = BlueMin,
                BlueMax = BlueMax,
                TotalParticlesAliveAtOnce = TotalParticlesAliveAtOnce,
                SizeMin = SizeMin,
                SizeMax = SizeMax,
                AngleMin = AngleMin,
                AngleMax = AngleMax,
                AngularVelocityMin = AngularVelocityMin,
                AngularVelocityMax = AngularVelocityMax,
                VelocityXMin = VelocityXMin,
                VelocityXMax = VelocityXMax,
                VelocityYMin = VelocityYMin,
                VelocityYMax = VelocityYMax,
                LifeTimeMin = LifeTimeMin,
                LifeTimeMax = LifeTimeMax,
                SpawnRateMin = SpawnRateMin,
                SpawnRateMax = SpawnRateMax,
                UseColorsFromList = UseColorsFromList,
                Colors = TintColors.ToParticleColors()
            };
        }
        #endregion
    }
}

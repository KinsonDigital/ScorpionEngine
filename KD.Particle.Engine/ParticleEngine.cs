using KDParticleEngine.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;

namespace KDParticleEngine
{
    //TODO: Look into using a better random number generator than the built in .NET framework one

    /// <summary>
    /// Manages multiple <see cref="Particle"/>s with various settings that dictate
    /// how all of the <see cref="Particle"/>s behave and look on the screen.
    /// </summary>
    public class ParticleEngine<Texture> : IList<Texture> where Texture : class
    {
        #region Public Events
        /// <summary>
        /// Occurs every time the total living particles has changed.
        /// </summary>
        public event EventHandler<EventArgs> LivingParticlesCountChanged;
        #endregion


        #region Private Fields
        private readonly List<Particle<Texture>> _particles;
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
            _particles = new List<Particle<Texture>>();
            Randomizer = randomizer;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets and sets a texture by its index.
        /// </summary>
        /// <param name="i">The index value of the item to get or set.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public Texture this[int i]
        {
            get => _textures[i];
            set => _textures[i] = value;
        }

        //TODO: Add code docs
        public Particle<Texture>[] Particles => _particles.ToArray();

        /// <summary>
        /// Gets or sets the randomizer used when generating new particles.
        /// </summary>
        public IRandomizerService Randomizer { get; private set; }

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
                    KillAllParticles();
            }
        }

        /// <summary>
        /// Gets or sets the location on the screen of where to spawn the <see cref="Particle"/>s.
        /// </summary>
        public PointF SpawnLocation { get; set; }

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
                _angleMin = value;
                _angleMin = _angleMin < 0 ? 360 : _angleMin;
                _angleMin = _angleMin > 360 ? 0 : _angleMin;
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
                _angleMax = value;
                _angleMax = _angleMax < 0 ? 360 : _angleMax;
                _angleMax = _angleMax > 360 ? 0 : _angleMax;
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
        /// The value of 1 will represent 100% or the normal size of the <see cref="Particle"/> texture.
        /// </summary>
        public float SizeMin { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets the maximum size that a newly generated <see cref="Particle"/> will be.
        /// The value of 1 will represent 100% or the normal size of the <see cref="Particle"/> texture.
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
        public PointF ParticleVelocity { get; set; } = new PointF(0, 1);

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
        public Color[] TintColors { get; set; } = new Color[0];

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

        /// <summary>
        /// Returns a value indicating if the list of <see cref="ParticleEngine{ITexture}"/>
        /// <see cref="Texture"/>s is readonly.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Returns a value indicating if the list of <see cref="ParticleEngine{ITexture}"/>
        /// <see cref="Texture"/>s has a fixed size.
        /// </summary>
        public bool IsFixedSize => false;

        /// <summary>
        /// Returns the total number of <see cref="ParticleEngine{ITexture}"/> <see cref="Texture"/>s.
        /// </summary>
        public int Count => _textures.Count;

        /// <summary>
        /// Gets or sets the syncronization object for multi-threaded operations.
        /// </summary>
        public object SyncRoot { get; set; }

        /// <summary>
        /// Returns a value indicating if the list of <see cref="Texture"/>s is syncrhonized
        /// for multi-threaded operations.
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        /// Gets or stets the custom process to occurr right before the particles
        /// update when the engine update process is invoked.  Will be invoked as long
        /// as the action is not null.
        /// </summary>
        public Action PreParticleUpdate { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given texture to the <see cref="ParticleEngine{ITexture}"/>.
        /// </summary>
        /// <param name="texture">The texture to add.</param>
        public void Add(Texture texture)
        {
            _textures.Add(texture);
            GenerateAllParticles();
        }


        /// <summary>
        /// Adds the given texture to the <see cref="ParticleEngine{ITexture}"/> as long as the
        /// given <paramref name="predicate"/> returns true.
        /// </summary>
        /// <param name="texture">The texture to add.</param>
        /// <param name="predicate">Returning true will add the texture to the <see cref="ParticleEngine{ITexture}"/>.</param>
        public void Add(Texture texture, Predicate<Texture> predicate)
        {
            if (!predicate(texture))
                return;

            _textures.Add(texture);
            GenerateAllParticles();
        }


        /// <summary>
        /// Adds the given <paramref name="textures"/> to the engine.
        /// </summary>
        /// <param name="textures">The list of textures to add.</param>
        public void Add(Texture[] textures)
        {
            _textures.AddRange(textures);
            GenerateAllParticles();
        }


        /// <summary>
        /// Kills all of the particles.
        /// </summary>
        public void KillAllParticles() => _particles.ForEach(p => p.IsDead = true);


        /// <summary>
        /// Returns a value indicating if the given <paramref name="texture"/> is in the particle engine.
        /// </summary>
        /// <param name="texture">The texture to check for.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public bool Contains(Texture texture) => _textures.Contains(texture);


        /// <summary>
        /// Clears all of the <see cref="Texture"/>s from the <see cref="ParticleEngine{ITexture}"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Clear() => _textures.Clear();


        /// <summary>
        /// Returns the index location of the given <paramref name="texture"/>.
        /// </summary>
        /// <param name="texture">The texture to get the index of.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public int IndexOf(Texture texture) => _textures.IndexOf(texture);


        /// <summary>
        /// Inserts the given <paramref name="texture"/> at the given <paramref name="index"/>
        /// in the list of <see cref="Texture"/>s in the <see cref="ParticleEngine{ITexture}"/>.
        /// </summary>
        /// <param name="index">The index location to insert the <see cref="Texture"/> at.</param>
        /// <param name="texture">The texture to insert.</param>
        [ExcludeFromCodeCoverage]
        public void Insert(int index, Texture texture) => _textures.Insert(index, texture);


        /// <summary>
        /// Removes the given <paramref name="texture"/> from the <see cref="ParticleEngine{ITexture}"/>.
        /// Returns true if the <paramref name="texture"/> was successfully removed.
        /// </summary>
        /// <param name="texture">The texture to remove.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public bool Remove(Texture texture)
        {
            var totalBeforeRemoval = _textures.Count;
            _textures.Remove(texture);


            return _textures.Count < totalBeforeRemoval;
        }


        /// <summary>
        /// Removes a texture located at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the texture to remove.</param>
        [ExcludeFromCodeCoverage]
        public void RemoveAt(int index) => _textures.RemoveAt(index);


        /// <summary>
        /// Copies the all of the <see cref="ParticleEngine{ITexture}"/> <see cref="Texture"/>s
        /// to the given <paramref name="array"/> starting at the index in the given <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The array to copy the <see cref="Texture"/>s to.</param>
        /// <param name="index">The starting index of the target array to start the copy process at.</param>
        [ExcludeFromCodeCoverage]
        public void CopyTo(Texture[] array, int index) => _textures.CopyTo(array, index);


        /// <summary>
        /// Returns the enumerator of the list of <see cref="ParticleEngine{ITexture}"/> textures.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public IEnumerator<Texture> GetEnumerator() => _textures.GetEnumerator();


        /// <summary>
        /// Returns the enumerator of the list of <see cref="ParticleEngine{ITexture}"/> textures.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() => _textures.GetEnumerator();


        /// <summary>
        /// Updates all of the <see cref="Particle"/>s.
        /// </summary>
        /// <param name="timeElapsed">The amount of time that has passed in the <see cref="Engine"/> since the last frame.</param>
        public void Update(TimeSpan timeElapsed)
        {
            if (!Enabled)
                return;

            _spawnRateElapsed += (int)timeElapsed.TotalMilliseconds;

            //If the amount of time to spawn a new particle has passed
            if (_spawnRateElapsed >= _spawnRate)
            {
                _spawnRate = GetRandomSpawnRate();

                SpawnNewParticle();

                _spawnRateElapsed = 0;
            }

            PreParticleUpdate?.Invoke();

            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].IsDead)
                    continue;
                
                //Update the current particle
                _particles[i].Update(timeElapsed);

                //If the current particle's time to live has expired, kill the particle
                if (_particles[i].LifeTime <= 0)
                {
                    _particles[i].IsAlive = false;

                    //Invoke the engine updated event
                    LivingParticlesCountChanged?.Invoke(this, new EventArgs());
                }
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
        private Particle<Texture> GenerateParticle()
        {
            var texture = GetRandomTexture();

            var position = SpawnLocation;

            var velocity = UseRandomVelocity ? GetRandomVelocity() : ParticleVelocity;

            var angle = GetRandomAngle();

            var angularVelocity = GetRandomAngularVelocity();

            var color = GetRandomColor();

            var size = GetRandomSize();

            var lifeTime = GetRandomLifeTime();


            return new Particle<Texture>(texture, position, velocity, angle, angularVelocity, color, size, lifeTime);
        }


        /// <summary>
        /// Returns a randomly chosen <see cref="Texture"/> out of the total list of textures to use for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private Texture GetRandomTexture()
        {
            var result = Randomizer.GetValue(0, _textures.Count - 1);


            return _textures[result >= 0 && result <= _textures.Count - 1 ? result : 0];
        }


        /// <summary>
        /// Returns a random <see cref="Particle.Velocity"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private PointF GetRandomVelocity()
        {
            var velXRandomResult = Randomizer.GetValue(VelocityXMin, VelocityXMax);
            var velYRandomResult = Randomizer.GetValue(VelocityYMin, VelocityYMax);


            return new PointF(velXRandomResult,
                              velYRandomResult);
        }


        /// <summary>
        /// Returns a random <see cref="Particle.Angle"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private float GetRandomAngle() => Randomizer.GetValue(AngleMin, AngleMax);


        /// <summary>
        /// Returns a random <see cref="Particle.AngularVelocity"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private float GetRandomAngularVelocity() => Randomizer.GetValue(AngularVelocityMin, AngularVelocityMax) * (Randomizer.FlipCoin() ? 1 : -1);


        /// <summary>
        /// Returns a random <see cref="Particle.TintColor"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private Color GetRandomColor()
        {
            if (UseColorsFromList)
            {
                return TintColors == null || TintColors.Length == 0 ? Color.FromArgb(255, 255, 255, 255) : TintColors[Randomizer.GetValue(0, TintColors.Length - 1)];
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

                return Color.FromArgb(255, red, green, blue);
            }
        }


        /// <summary>
        /// Returns a random <see cref="Particle.Size"/> for a spawned <see cref="Particle"/>.
        /// </summary>
        /// <returns></returns>
        private float GetRandomSize() => Randomizer.GetValue(SizeMin, SizeMax);


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
            TintColors = setupData.Colors;
        }


        /// <summary>
        /// Generates a particle setup from the current settings of the <see cref="ParticleEngine"/>.
        /// </summary>
        /// <returns></returns>
        public ParticleSetup GenerateParticleSetup() => new ParticleSetup()
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
                Colors = TintColors
            };
        #endregion
    }
}

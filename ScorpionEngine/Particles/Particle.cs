using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Particles
{
    /// <summary>
    /// Represents a single particle with various properties that dictate how the <see cref="Particle"/>
    /// behaves and looks on the screen.
    /// </summary>
    public class Particle
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Particle"/>.
        /// </summary>
        /// <param name="texture">The texture used for rendering the <see cref="Particle"/>.</param>
        /// <param name="position">The position of the particle.</param>
        /// <param name="velocity">The direction and speed at which the particle is moving.</param>
        /// <param name="angle">The angle that the particle starts when it is spawned.</param>
        /// <param name="angularVelocity">The speed at which the particle is rotating.</param>
        /// <param name="color">The color to tint the <see cref="Texture"/>.</param>
        /// <param name="size">The size of the <see cref="Particle"/>.</param>
        /// <param name="timeToLive">The amount of time in milliseconds for the particle to stay alive.</param>
        public Particle(Texture texture, Vector position, Vector velocity, float angle, float angularVelocity, GameColor color, float size, int timeToLive)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            TintColor = color;
            Size = size;
            LifeTime = timeToLive;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the texture of the <see cref="Particle"/>.
        /// </summary>
        public Texture Texture { get; set; }

        /// <summary>
        /// Gets or sets the position of the <see cref="Particle"/>.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the velocity of the <see cref="Particle"/>.
        /// </summary>
        public Vector Velocity { get; set; }

        /// <summary>
        /// Gets or sets the angle that the <see cref="Particle"/> is at when first spawned.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the speed and direction of rotation of the <see cref="Particle"/>.
        /// </summary>
        public float AngularVelocity { get; set; }

        /// <summary>
        /// Gets or sets the color that the <see cref="Texture"/> will be tinted.
        /// </summary>
        public GameColor TintColor { get; set; }

        /// <summary>
        /// Gets or sets the sized of the <see cref="Particle"/>.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the <see cref="Particle"/> will stay alive.
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="Particle"/> is alive or dead.
        /// </summary>
        public bool IsAlive { get; set; } = true;

        /// <summary>
        /// Gets or sets if the <see cref="Particle"/> is dead or alive.
        /// </summary>
        public bool IsDead
        {
            get => !IsAlive;
            set => IsAlive = !value;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the <see cref="Particle"/>.
        /// </summary>
        /// <param name="engineTime">The amount of time that the <see cref="Engine"/> has passed since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
            LifeTime -= engineTime.ElapsedEngineTime.Milliseconds;
            Position += Velocity;
            Angle += AngularVelocity;
        }


        /// <summary>
        /// Renders the particle to the screen.
        /// </summary>
        /// <param name="renderer">Renders the particle.</param>
        public void Render(Renderer renderer)
        {
            TintColor = new GameColor(TintColor.Red, TintColor.Green, TintColor.Blue, 255);
            renderer.Render(Texture, Position.X, Position.Y, Angle, Size, TintColor);
        }
        #endregion
    }
}

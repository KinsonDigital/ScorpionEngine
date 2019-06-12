using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionEngine.Input;
using KDScorpionEngine.Scene;
using System.Drawing;

namespace ScorpTestGame.Scenes
{
    public class ParticleTestingScene : GameScene
    {
        private ParticleEngine<Texture> _particleEngine;
        private Mouse _mouse;


        public ParticleTestingScene() : base(Vector.Zero)
        {

        }


        public override void Initialize()
        {
            _mouse = new Mouse();

            var colors = new GameColor[]
            {
                new GameColor(255, 255, 216, 0),
                new GameColor(255, 255, 0, 0),
                new GameColor(255, 255, 106, 0)
            };

            _particleEngine = new ParticleEngine<Texture>(new RandomizerService())
            {
                SpawnLocation = new PointF(400, 400),
                UseRandomVelocity = true,
                TotalParticlesAliveAtOnce = 60,
                UseColorsFromList = false,
                TintColors = colors.ToNETColors(),
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

            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            var textures = new Texture[]
            {
                contentLoader.LoadTexture(@"Particles\ShipThruster")
            };

            _particleEngine.AddTextures(textures);

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            _mouse.UpdateCurrentState();

            _particleEngine.Update(engineTime.ToTimeSpan());

            _particleEngine.SpawnLocation = new PointF(_mouse.X, _mouse.Y);

            _mouse.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            foreach (var particle in _particleEngine.Particles)
            {
                renderer.Render(particle.Texture, particle.Position.ToVector());
            }

            base.Render(renderer);
        }
    }
}

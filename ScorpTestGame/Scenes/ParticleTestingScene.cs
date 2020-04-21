using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionEngine.Graphics;
using KDScorpionEngine.Scene;
using Raptor;
using Raptor.Content;
using Raptor.Graphics;
using Raptor.Input;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace ScorpTestGame.Scenes
{
    public class ParticleTestingScene : GameScene
    {
        private ParticleEngine<Texture> _particleEngine;
        private Mouse _mouse;


        public ParticleTestingScene() : base(Vector2.Zero)
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

            //TODO: Figure out how to accomplish the code below with new particle engine changes.
            //_particleEngine = new ParticleEngine<Texture>(new RandomizerService())
            //{
            //    SpawnLocation = new PointF(400, 400),
            //    UseRandomVelocity = true,
            //    TotalParticlesAliveAtOnce = 60,
            //    UseColorsFromList = false,
            //    TintColors = colors.ToNETColors(),
            //    RedMin = 255,
            //    RedMax = 255,
            //    GreenMin = 132,
            //    GreenMax = 209,
            //    BlueMin = 0,
            //    BlueMax = 0,
            //    SizeMin = 0.05f,
            //    SizeMax = 0.20f,
            //    LifeTimeMax = 700,
            //    VelocityXMin = -0.25f,
            //    VelocityXMax = 0.25f,
            //    VelocityYMin = 0,
            //    VelocityYMax = 1f
            //};

            base.Initialize();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            var textures = new Texture[]
            {
                contentLoader.LoadTexture(@"Particles\ShipThruster")
            };

            //TODO: Figure out how to accomplish the code below with new particle engine changes.
            //_particleEngine.Add(textures);

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            _mouse.UpdateCurrentState();

            _particleEngine.Update(engineTime.ToTimeSpan());

            //TODO: Figure out how to accomplish the code below with new particle engine changes.
            //_particleEngine.SpawnLocation = new PointF(_mouse.X, _mouse.Y);

            _mouse.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(GameRenderer renderer)
        {
            //TODO: Figure out how to accomplish the code below with new particle engine changes.
            //_particleEngine.Particles.ToList().ForEach(p => renderer.Render(p.Texture, p.Position.ToVector()));

            base.Render(renderer);
        }
    }
}

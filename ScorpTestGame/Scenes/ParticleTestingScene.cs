﻿using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Input;
using KDScorpionEngine.Scene;

namespace ScorpTestGame.Scenes
{
    public class ParticleTestingScene : GameScene
    {
        private ParticleEngine _particleEngine;
        private Mouse _mouse;


        public ParticleTestingScene() : base(Vector.Zero)
        {

        }


        public override void Initialize()
        {
            _mouse = new Mouse();

            var colors = new GameColor[]
            {
                new GameColor(255, 216, 0, 255),
                new GameColor(255, 0, 0, 255),
                new GameColor(255, 106, 0, 255)
            };

            _particleEngine = new ParticleEngine(new RandomizerService())
            {
                SpawnLocation = new Vector(400, 400),
                UseRandomVelocity = true,
                TotalParticlesAliveAtOnce = 60,
                UseColorsFromList = false,
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

            _particleEngine.Update(engineTime);

            _particleEngine.SpawnLocation = new Vector(_mouse.X, _mouse.Y);

            _mouse.UpdatePreviousState();

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _particleEngine.Render(renderer);

            base.Render(renderer);
        }
    }
}

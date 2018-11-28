﻿using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Input;
using ScorpionEngine.Particles;
using ScorpionEngine.Physics;
using ScorpionEngine.Scene;
using System.Collections.Generic;

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

            _particleEngine = new ParticleEngine(new Vector(400, 400))
            {
                UseRandomVelocity = true,
                TotalParticlesAliveAtOnce = 60,
                UseTintColorList = false,
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
            var textures = new List<Texture>()
            {
                contentLoader.LoadTexture(@"Particles\ShipThruster")
            };

            _particleEngine.Textures = textures;

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

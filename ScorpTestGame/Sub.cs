// <copyright file="Sub.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System.Linq;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    public class Sub : Entity
    {
        public IKeyboard keyboard;
        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;

        public Sub()
            : base("sub")
        {
        }

        public override void Init()
        {
            Animator = new Animator();
            Position = new Vector2(400, 400);
            this.keyboard = new Keyboard();
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            AtlasData = contentLoader.Load<IAtlasData>("Main-Atlas");
            Animator.Frames = AtlasData.GetFrames("sub").Select(f => f.Bounds).ToArray();
        }

        public override void Update(GameTime gameTime)
        {
            this.currentKeyState = this.keyboard.GetState();

            if (this.currentKeyState.IsKeyUp(KeyCode.Space) && this.previousKeyState.IsKeyDown(KeyCode.Space))
            {
                switch (Animator.CurrentState)
                {
                    case AnimateState.Playing:
                        Animator.CurrentState = AnimateState.Paused;
                        break;
                    case AnimateState.Paused:
                        Animator.CurrentState = AnimateState.Playing;
                        break;
                }
            }

            this.previousKeyState = this.currentKeyState;
            base.Update(gameTime);
        }
    }
}

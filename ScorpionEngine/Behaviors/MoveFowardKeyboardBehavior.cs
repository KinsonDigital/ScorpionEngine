// <copyright file="MoveFowardKeyboardBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Entities;
    using Raptor.Input;

    /// <summary>
    /// Moves a <see cref="Entity"/> forward in the direction it is facing with added rotation
    /// using the keyboard.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Entity"/> to move.</typeparam>
    public class MoveFowardKeyboardBehavior<T> : Behavior
        where T : Entity
    {
        private KeyBehavior moveFowardKeyBehavior;
        private KeyBehavior rotateCWKeyBehavior;
        private KeyBehavior rotateCCWKeyBehavior;
        private readonly T Entity;
        private KeyCode moveFowardKey = KeyCode.Up;
        private KeyCode rotateCWKey = KeyCode.Right;
        private KeyCode rotateCCWKey = KeyCode.Left;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// </summary>
        /// <param name="keyboard">Manages keyboard input.</param>
        /// <param name="entity">The <see cref="Entity"/> to perform keyboard movement behavior upon.</param>
        /// <param name="linearSpeed">The speed that the <see cref="Entity"/> will move at.</param>
        /// <param name="angularSpeed">The speed that the <see cref="Entity"/> will rotate at.</param>
        [ExcludeFromCodeCoverage]
        public MoveFowardKeyboardBehavior(IGameInput<KeyCode, KeyboardState> keyboard, T entity, float linearSpeed, float angularSpeed)
        {
            this.keyboard = keyboard;
            LinearSpeed = linearSpeed;
            AngularSpeed = angularSpeed;

            CreateBehaviors();
            SetupBehaviors();

            this.Entity = entity;

            SetUpdateAction(UpdateAction);
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="Entity"/> foward in the direction it is facing.
        /// </summary>
        public KeyCode MoveFowardKey
        {
            get => this.moveFowardKey;
            set
            {
                this.moveFowardKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will rotate the <see cref="Entity"/> clockwise.
        /// </summary>
        public KeyCode RotateCWKey
        {
            get => this.rotateCWKey;
            set
            {
                this.rotateCWKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will rotate the <see cref="Entity"/> counter clockwise.
        /// </summary>
        public KeyCode RotateCCWKey
        {
            get => this.rotateCCWKey;
            set
            {
                this.rotateCCWKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets a value indicating if the <see cref="Entity"/> is moving forward in the direction it is facing.
        /// </summary>
        public bool IsMovingForward { get; private set; }

        private readonly IGameInput<KeyCode, KeyboardState> keyboard;

        /// <summary>
        /// Gets or sets the linear speed of the <see cref="Entity"/>.
        /// </summary>
        public float LinearSpeed { get; set; }

        /// <summary>
        /// Gets or sets the angular speed of the <see cref="Entity"/>.
        /// </summary>
        public float AngularSpeed { get; set; }

        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other internal behaviors.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        private void UpdateAction(GameTime gameTime)
        {
            this.currentKeyboardState = this.keyboard.GetState();

            IsMovingForward = this.currentKeyboardState.IsKeyDown(this.moveFowardKey);

            this.moveFowardKeyBehavior.Update(gameTime);
            this.rotateCWKeyBehavior.Update(gameTime);
            this.rotateCCWKeyBehavior.Update(gameTime);

            this.previousKeyboardState = this.currentKeyboardState;
        }

        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="Entity"/> movement.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateBehaviors()
        {
            // Setup the move foward key behavior
            this.moveFowardKeyBehavior = new KeyBehavior(this.moveFowardKey, this.keyboard, true);
            this.moveFowardKeyBehavior.KeyDownEvent += MoveFoward_KeyDown;

            // Setup the rotate clockwise key behavior
            this.rotateCWKeyBehavior = new KeyBehavior(this.rotateCWKey, this.keyboard, true);
            this.rotateCWKeyBehavior.KeyDownEvent += RotateCW_KeyDown;

            // Setup the rotate counter clockwise key behavior
            this.rotateCCWKeyBehavior = new KeyBehavior(this.rotateCCWKey, this.keyboard, true);
            this.rotateCCWKeyBehavior.KeyDownEvent += RotateCCW_KeyDown;
        }

        /// <summary>
        /// Sets up the behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            // Setup the move foward key behavior
            this.moveFowardKeyBehavior.Key = this.moveFowardKey;
            this.moveFowardKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Setup the rotate clockwise key behavior
            this.rotateCWKeyBehavior.Key = this.rotateCWKey;
            this.rotateCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Setup the rotate counter clockwise key behavior
            this.rotateCCWKeyBehavior.Key = this.rotateCCWKey;
            this.rotateCCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }

        /// <summary>
        /// Moves the <see cref="Entity"/> forward in the direction it is facing.
        /// </summary>
        private void MoveFoward_KeyDown(object sender, KeyEventArgs e)
        {
            IsMovingForward = true;
        }

        /// <summary>
        /// Rotates the <see cref="Entity"/> clockwise.
        /// </summary>
        private void RotateCW_KeyDown(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Rotates the <see cref="Entity"/> counter clockwise.
        /// </summary>
        private void RotateCCW_KeyDown(object sender, KeyEventArgs e) { }
    }
}

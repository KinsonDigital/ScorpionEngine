// <copyright file="MovementByKeyboardBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Entities;
    using Raptor.Input;

    /// <summary>
    /// Creates a behavior that controls the left, right, up, and down movement of a
    /// <see cref="Entity"/> using the keyboard.
    /// <typeparamref name="T">The type of <see cref="Entity"/> to apply the movement to.</typeparamref>.
    /// </summary>
    public class MovementByKeyboardBehavior<T> : Behavior
        where T : Entity
    {
        private KeyBehavior moveRightOnKeyDown;
        private KeyBehavior moveLeftOnKeyDown;
        private KeyBehavior moveUpOnKeyDown;
        private KeyBehavior moveDownOnKeyDown;
        private readonly T gameObject;
        private readonly IKeyboard keyboard;
        private KeyCode moveUpKey = KeyCode.Up;
        private KeyCode moveDownKey = KeyCode.Down;
        private KeyCode moveLeftKey = KeyCode.Left;
        private KeyCode moveRightKey = KeyCode.Right;

        // TODO: Find a way to improve this class to allow unit testing without having to have this internal constructor.
        // It is not a good idea to have a constructor for the sole purpose of testing and this points to architecture issues.
        // This is an issue all over the code base with various classes.  The factory pattern might be the best way to deal with
        // this by giving internal access to various factories for producing objects such as entities and behaviors.
        // Another option is to just simply expose and allow the constructor with all the required dependencies exposed for injection
        // so the developer can simply choose to create the manually or with an IoC container.

        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to perform keyboard movement behavior upon.</param>
        /// <param name="movementSpeed">The movement speed that the <see cref="Entity"/> will move at.</param>
        [ExcludeFromCodeCoverage]
        public MovementByKeyboardBehavior(IKeyboard keyboard, T entity, float movementSpeed)
        {
            this.keyboard = keyboard;
            throw new System.Exception("Need to implement the movementSpeed param");

            CreateBehaviors();
            SetupBehaviors();

            this.gameObject = entity;

            SetUpdateAction(UpdateAction);
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="Entity"/> up.
        /// </summary>
        public KeyCode MoveUpKey
        {
            get => this.moveUpKey;
            set
            {
                this.moveUpKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="Entity"/> down.
        /// </summary>
        public KeyCode MoveDownKey
        {
            get => this.moveDownKey;
            set
            {
                this.moveDownKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="Entity"/> left.
        /// </summary>
        public KeyCode MoveLeftKey
        {
            get => this.moveLeftKey;
            set
            {
                this.moveLeftKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="Entity"/> right.
        /// </summary>
        public KeyCode MoveRightKey
        {
            get => this.moveRightKey;
            set
            {
                this.moveRightKey = value;
                SetupBehaviors();
            }
        }

        // TODO: Rename this to something else.  This was name this before due to farseer physics engine before
        // Looking into making a physics engine abstraction that can be used to add physics to stuff
        /// <summary>
        /// Gets or sets the linear speed of the <see cref="Entity"/>.
        /// </summary>
        public float LinearSpeed { get; set; }

        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other behaviors.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        private void UpdateAction(GameTime gameTime)
        {
            this.moveRightOnKeyDown.Update(gameTime);
            this.moveLeftOnKeyDown.Update(gameTime);
            this.moveUpOnKeyDown.Update(gameTime);
            this.moveDownOnKeyDown.Update(gameTime);
        }

        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="Entity"/> movement.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateBehaviors()
        {
            this.moveRightOnKeyDown = new KeyBehavior(this.moveRightKey, this.keyboard, true);
            this.moveLeftOnKeyDown = new KeyBehavior(this.moveLeftKey, this.keyboard, true);
            this.moveUpOnKeyDown = new KeyBehavior(this.moveUpKey, this.keyboard, true);
            this.moveDownOnKeyDown = new KeyBehavior(this.moveDownKey, this.keyboard, true);
        }

        /// <summary>
        /// Sets up the behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            // Setup the move right key behavior
            this.moveRightOnKeyDown.Key = this.moveRightKey;
            this.moveRightOnKeyDown.KeyDownEvent += MoveRight_KeyDown;
            this.moveRightOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Setup the move left key behavior
            this.moveLeftOnKeyDown.Key = this.moveLeftKey;
            this.moveLeftOnKeyDown.KeyDownEvent += MoveLeft_KeyDown;
            this.moveLeftOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Setup the move up key behavior
            this.moveUpOnKeyDown.Key = this.moveUpKey;
            this.moveUpOnKeyDown.KeyDownEvent += MoveUp_KeyDown;
            this.moveUpOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Setup the move down key behavior
            this.moveDownOnKeyDown.Key = this.moveDownKey;
            this.moveDownOnKeyDown.KeyDownEvent += MoveDown_KeyDown;
            this.moveDownOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }

        /// <summary>
        /// Moves the <see cref="Entity"/> to the right.
        /// </summary>
        private void MoveRight_KeyDown(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Moves the <see cref="Entity"/> to the left.
        /// </summary>
        private void MoveLeft_KeyDown(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Moves the <see cref="Entity"/> up.
        /// </summary>
        private void MoveUp_KeyDown(object sender, KeyEventArgs e) { }

        /// <summary>
        /// Moves the <see cref="Entity"/> down.
        /// </summary>
        private void MoveDown_KeyDown(object sender, KeyEventArgs e) { }
    }
}

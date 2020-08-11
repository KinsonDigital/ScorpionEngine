// <copyright file="MovementByKeyboardBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Entities;
    using Raptor;
    using Raptor.Input;
    using Raptor.Plugins;

    /// <summary>
    /// Creates a behavior that controls the left, right, up, and down movement of a
    /// <see cref="DynamicEntity"/> using the keyboard.
    /// <typeparamref name="T">The type of <see cref="DynamicEntity"/> to apply the movement to.</typeparamref>
    /// </summary>
    public class MovementByKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        private KeyBehavior moveRightOnKeyDown;
        private KeyBehavior moveLeftOnKeyDown;
        private KeyBehavior moveUpOnKeyDown;
        private KeyBehavior moveDownOnKeyDown;
        private readonly T gameObject;
        private KeyCode moveUpKey = KeyCode.Up;
        private KeyCode moveDownKey = KeyCode.Down;
        private KeyCode moveLeftKey = KeyCode.Left;
        private KeyCode moveRightKey = KeyCode.Right;

        //TODO: Find a way to improve this class to allow unit testing without having to have this internal constructor.
        //It is not a good idea to have a constructor for the sole purpose of testing and this points to architecture issues.
        //This is an issue all over the code base with various classes.  The factory pattern might be the best way to deal with
        //this by giving internal access to various factories for producing objects such as entities and behaviors.
        //Another option is to just simply expose and allow the constructor with all the required dependencies exposed for injection
        //so the developer can simply choose to create the manually or with an IoC container.

        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject.</param>
        /// <param name="dyanmicEntity">The entity to inject.</param>
        internal MovementByKeyboardBehavior(IKeyboard keyboard, T dyanmicEntity)
        {
            CreateBehaviors(keyboard);
            SetupBehaviors();

            this.gameObject = dyanmicEntity;

            SetUpdateAction(UpdateAction);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// </summary>
        /// <param name="entity">The <see cref="DynamicEntity"/> to perform keyboard movement behavior upon.</param>
        /// <param name="movementSpeed">The movement speed that the <see cref="DynamicEntity"/> will move at.</param>
        [ExcludeFromCodeCoverage]
        public MovementByKeyboardBehavior(T entity, float movementSpeed)
        {
            LinearSpeed = movementSpeed;

            CreateBehaviors();
            SetupBehaviors();

            this.gameObject = entity;

            SetUpdateAction(UpdateAction);
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> up.
        /// </summary>
        public KeyCode MoveUpKey
        {
            get
            {
                return this.moveUpKey;
            }
            set
            {
                this.moveUpKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> down.
        /// </summary>
        public KeyCode MoveDownKey
        {
            get
            {
                return this.moveDownKey;
            }
            set
            {
                this.moveDownKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> left.
        /// </summary>
        public KeyCode MoveLeftKey
        {
            get
            {
                return this.moveLeftKey;
            }
            set
            {
                this.moveLeftKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> right.
        /// </summary>
        public KeyCode MoveRightKey
        {
            get
            {
                return this.moveRightKey;
            }
            set
            {
                this.moveRightKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the linear speed of the <see cref="DynamicEntity"/>.
        /// </summary>
        public float LinearSpeed { get; set; }

        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other behaviors.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        private void UpdateAction(EngineTime engineTime)
        {
            this.moveRightOnKeyDown.Update(engineTime);
            this.moveLeftOnKeyDown.Update(engineTime);
            this.moveUpOnKeyDown.Update(engineTime);
            this.moveDownOnKeyDown.Update(engineTime);
        }

        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/> movement.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateBehaviors()
        {
            this.moveRightOnKeyDown = new KeyBehavior(this.moveRightKey, true);
            this.moveLeftOnKeyDown = new KeyBehavior(this.moveLeftKey, true);
            this.moveUpOnKeyDown = new KeyBehavior(this.moveUpKey, true);
            this.moveDownOnKeyDown = new KeyBehavior(this.moveDownKey, true);
        }

        /// <summary>
        /// Sets up all of the KeyBehaviors using the given <paramref name="keyboard"/>
        /// </summary>
        /// <param name="keyboard">The keyboard to inject into the behaviors for testing.</param>
        private void CreateBehaviors(IKeyboard keyboard)
        {
            this.moveRightOnKeyDown = new KeyBehavior(keyboard);
            this.moveLeftOnKeyDown = new KeyBehavior(keyboard);
            this.moveUpOnKeyDown = new KeyBehavior(keyboard);
            this.moveDownOnKeyDown = new KeyBehavior(keyboard);
        }

        /// <summary>
        /// Sets up the behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            //Setup the move right key behavior
            this.moveRightOnKeyDown.Key = this.moveRightKey;
            this.moveRightOnKeyDown.KeyDownEvent += MoveRight_KeyDown;
            this.moveRightOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move left key behavior
            this.moveLeftOnKeyDown.Key = this.moveLeftKey;
            this.moveLeftOnKeyDown.KeyDownEvent += MoveLeft_KeyDown;
            this.moveLeftOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move up key behavior
            this.moveUpOnKeyDown.Key = this.moveUpKey;
            this.moveUpOnKeyDown.KeyDownEvent += MoveUp_KeyDown;
            this.moveUpOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move down key behavior
            this.moveDownOnKeyDown.Key = this.moveDownKey;
            this.moveDownOnKeyDown.KeyDownEvent += MoveDown_KeyDown;
            this.moveDownOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right.
        /// </summary>
        private void MoveRight_KeyDown(object sender, KeyEventArgs e) => this.gameObject.MoveRight(LinearSpeed);

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left.
        /// </summary>
        private void MoveLeft_KeyDown(object sender, KeyEventArgs e) => this.gameObject.MoveLeft(LinearSpeed);

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up.
        /// </summary>
        private void MoveUp_KeyDown(object sender, KeyEventArgs e) => this.gameObject.MoveUp(LinearSpeed);

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down.
        /// </summary>
        private void MoveDown_KeyDown(object sender, KeyEventArgs e) => this.gameObject.MoveDown(LinearSpeed);
    }
}

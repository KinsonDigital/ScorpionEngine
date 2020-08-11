// <copyright file="MoveFowardKeyboardBehavior.cs" company="KinsonDigital">
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
    /// Moves a <see cref="DynamicEntity"/> forward in the direction it is facing with added rotation
    /// using the keyboard.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="DynamicEntity"/> to move.</typeparam>
    public class MoveFowardKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        private KeyBehavior moveFowardKeyBehavior;
        private KeyBehavior rotateCWKeyBehavior;
        private KeyBehavior rotateCCWKeyBehavior;
        private readonly Keyboard keyboard;
        private readonly T dynamicEntity;
        private KeyCode moveFowardKey = KeyCode.Up;
        private KeyCode rotateCWKey = KeyCode.Right;
        private KeyCode rotateCCWKey = KeyCode.Left;

        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>
        /// and injects the given <paramref name="keyboard"/> and <paramref name="dynamicEntity"/>
        /// for the purpose of unit testing.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject for testing.</param>
        /// <param name="dynamicEntity">The entity to use for testing.</param>
        internal MoveFowardKeyboardBehavior(IKeyboard keyboard, T dynamicEntity)
        {
            this.keyboard = new Keyboard(keyboard);

            CreateBehaviors(keyboard);
            SetupBehaviors();

            this.dynamicEntity = dynamicEntity;

            SetUpdateAction(UpdateAction);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// </summary>
        /// <param name="entity">The <see cref="DynamicEntity"/> to perform keyboard movement behavior upon.</param>
        /// <param name="linearSpeed">The speed that the <see cref="DynamicEntity"/> will move at.</param>
        /// <param name="angularSpeed">The speed that the <see cref="DynamicEntity"/> will rotate at.</param>
        [ExcludeFromCodeCoverage]
        public MoveFowardKeyboardBehavior(T entity, float linearSpeed, float angularSpeed)
        {
            this.keyboard = new Keyboard();
            LinearSpeed = linearSpeed;
            AngularSpeed = angularSpeed;

            CreateBehaviors();
            SetupBehaviors();

            this.dynamicEntity = entity;

            SetUpdateAction(UpdateAction);
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> foward in the direction it is facing.
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
        /// Gets or sets the keyboard key that will rotate the <see cref="DynamicEntity"/> clockwise.
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
        /// Gets or sets the keyboard key that will rotate the <see cref="DynamicEntity"/> counter clockwise.
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
        /// Gets a value indicating if the <see cref="DynamicEntity"/> is moving forward in the direction it is facing.
        /// </summary>
        public bool IsMovingForward { get; private set; }

        /// <summary>
        /// Gets or sets the linear speed of the <see cref="DynamicEntity"/>.
        /// </summary>
        public float LinearSpeed { get; set; }

        /// <summary>
        /// Gets or sets the angular speed of the <see cref="DynamicEntity"/>.
        /// </summary>
        public float AngularSpeed { get; set; }

        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other internal behaviors.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        private void UpdateAction(EngineTime engineTime)
        {
            this.keyboard.UpdateCurrentState();

            IsMovingForward = this.keyboard.IsKeyDown(this.moveFowardKey);

            this.moveFowardKeyBehavior.Update(engineTime);
            this.rotateCWKeyBehavior.Update(engineTime);
            this.rotateCCWKeyBehavior.Update(engineTime);

            this.keyboard.UpdatePreviousState();
        }

        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/> movement.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateBehaviors()
        {
            //Setup the move foward key behavior
            this.moveFowardKeyBehavior = new KeyBehavior(this.moveFowardKey, true);
            this.moveFowardKeyBehavior.KeyDownEvent += MoveFoward_KeyDown;

            //Setup the rotate clockwise key behavior
            this.rotateCWKeyBehavior = new KeyBehavior(this.rotateCWKey, true);
            this.rotateCWKeyBehavior.KeyDownEvent += RotateCW_KeyDown;

            //Setup the rotate counter clockwise key behavior
            this.rotateCCWKeyBehavior = new KeyBehavior(this.rotateCCWKey, true);
            this.rotateCCWKeyBehavior.KeyDownEvent += RotateCCW_KeyDown;
        }

        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject into the behaviors for testing.</param>
        private void CreateBehaviors(IKeyboard keyboard)
        {
            this.moveFowardKeyBehavior = new KeyBehavior(keyboard)
            {
                Key = moveFowardKey
            };
            this.moveFowardKeyBehavior.KeyDownEvent += MoveFoward_KeyDown;

            this.rotateCWKeyBehavior = new KeyBehavior(keyboard)
            {
                Key = rotateCWKey
            };
            this.rotateCWKeyBehavior.KeyDownEvent += RotateCW_KeyDown;

            this.rotateCCWKeyBehavior = new KeyBehavior(keyboard)
            {
                Key = rotateCCWKey
            };
            this.rotateCCWKeyBehavior.KeyDownEvent += RotateCCW_KeyDown;
        }

        /// <summary>
        /// Sets up the behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            //Setup the move foward key behavior
            this.moveFowardKeyBehavior.Key = this.moveFowardKey;
            this.moveFowardKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate clockwise key behavior
            this.rotateCWKeyBehavior.Key = this.rotateCWKey;
            this.rotateCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate counter clockwise key behavior
            this.rotateCCWKeyBehavior.Key = this.rotateCCWKey;
            this.rotateCCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }

        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> forward in the direction it is facing.
        /// </summary>
        private void MoveFoward_KeyDown(object sender, KeyEventArgs e)
        {
            IsMovingForward = true;
            this.dynamicEntity.MoveAtSetAngle(LinearSpeed);
        }

        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise.
        /// </summary>
        private void RotateCW_KeyDown(object sender, KeyEventArgs e) => this.dynamicEntity.RotateCW(AngularSpeed);

        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise.
        /// </summary>
        private void RotateCCW_KeyDown(object sender, KeyEventArgs e) => this.dynamicEntity.RotateCCW(AngularSpeed);
    }
}

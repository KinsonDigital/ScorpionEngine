// <copyright file="MovementByKeyboardBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using KDScorpionEngine.Entities;
using Raptor;
using Raptor.Input;
using Raptor.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// Creates a behavior that controls the left, right, up, and down movement of a
    /// <see cref="DynamicEntity"/> using the keyboard.
    /// <typeparamref name="T">The type of <see cref="DynamicEntity"/> to apply the movement to.</typeparamref>
    /// </summary>
    public class MovementByKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        #region Private Fields
        private KeyBehavior _moveRightOnKeyDown;
        private KeyBehavior _moveLeftOnKeyDown;
        private KeyBehavior _moveUpOnKeyDown;
        private KeyBehavior _moveDownOnKeyDown;
        private readonly T _gameObject;
        private KeyCode _moveUpKey = KeyCode.Up;
        private KeyCode _moveDownKey = KeyCode.Down;
        private KeyCode _moveLeftKey = KeyCode.Left;
        private KeyCode _moveRightKey = KeyCode.Right;
        #endregion


        #region Constructors
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

            _gameObject = dyanmicEntity;

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

            _gameObject = entity;

            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> up.
        /// </summary>
        public KeyCode MoveUpKey
        {
            get
            {
                return _moveUpKey;
            }
            set
            {
                _moveUpKey = value;
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
                return _moveDownKey;
            }
            set
            {
                _moveDownKey = value;
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
                return _moveLeftKey;
            }
            set
            {
                _moveLeftKey = value;
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
                return _moveRightKey;
            }
            set
            {
                _moveRightKey = value;
                SetupBehaviors();
            }
        }


        /// <summary>
        /// Gets or sets the linear speed of the <see cref="DynamicEntity"/>.
        /// </summary>
        public float LinearSpeed { get; set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other behaviors.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        private void UpdateAction(EngineTime engineTime)
        {
            _moveRightOnKeyDown.Update(engineTime);
            _moveLeftOnKeyDown.Update(engineTime);
            _moveUpOnKeyDown.Update(engineTime);
            _moveDownOnKeyDown.Update(engineTime);
        }


        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/> movement.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateBehaviors()
        {
            _moveRightOnKeyDown = new KeyBehavior(_moveRightKey, true);
            _moveLeftOnKeyDown = new KeyBehavior(_moveLeftKey, true);
            _moveUpOnKeyDown = new KeyBehavior(_moveUpKey, true);
            _moveDownOnKeyDown = new KeyBehavior(_moveDownKey, true);
        }


        /// <summary>
        /// Sets up all of the KeyBehaviors using the given <paramref name="keyboard"/>
        /// </summary>
        /// <param name="keyboard">The keyboard to inject into the behaviors for testing.</param>
        private void CreateBehaviors(IKeyboard keyboard)
        {
            _moveRightOnKeyDown = new KeyBehavior(keyboard);
            _moveLeftOnKeyDown = new KeyBehavior(keyboard);
            _moveUpOnKeyDown = new KeyBehavior(keyboard);
            _moveDownOnKeyDown = new KeyBehavior(keyboard);
        }


        /// <summary>
        /// Sets up the behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            //Setup the move right key behavior
            _moveRightOnKeyDown.Key = _moveRightKey;
            _moveRightOnKeyDown.KeyDownEvent += MoveRight_KeyDown;
            _moveRightOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move left key behavior
            _moveLeftOnKeyDown.Key = _moveLeftKey;
            _moveLeftOnKeyDown.KeyDownEvent += MoveLeft_KeyDown;
            _moveLeftOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move up key behavior
            _moveUpOnKeyDown.Key = _moveUpKey;
            _moveUpOnKeyDown.KeyDownEvent += MoveUp_KeyDown;
            _moveUpOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;


            //Setup the move down key behavior
            _moveDownOnKeyDown.Key = _moveDownKey;
            _moveDownOnKeyDown.KeyDownEvent += MoveDown_KeyDown;
            _moveDownOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }


        #region Event Methods
        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right.
        /// </summary>
        private void MoveRight_KeyDown(object sender, KeyEventArgs e) => _gameObject.MoveRight(LinearSpeed);


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left.
        /// </summary>
        private void MoveLeft_KeyDown(object sender, KeyEventArgs e) => _gameObject.MoveLeft(LinearSpeed);


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up.
        /// </summary>
        private void MoveUp_KeyDown(object sender, KeyEventArgs e) => _gameObject.MoveUp(LinearSpeed);


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down.
        /// </summary>
        private void MoveDown_KeyDown(object sender, KeyEventArgs e) => _gameObject.MoveDown(LinearSpeed);
        #endregion
        #endregion
    }
}

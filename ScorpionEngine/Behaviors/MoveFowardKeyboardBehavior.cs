using KDScorpionEngine.Entities;
using Raptor;
using Raptor.Input;
using Raptor.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// Moves a <see cref="DynamicEntity"/> forward in the direction it is facing with added rotation
    /// using the keyboard.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="DynamicEntity"/> to move.</typeparam>
    public class MoveFowardKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        #region Private Fields
        private KeyBehavior _moveFowardKeyBehavior;
        private KeyBehavior _rotateCWKeyBehavior;
        private KeyBehavior _rotateCCWKeyBehavior;
        private readonly Keyboard _keyboard;
        private readonly T _gameObject;
        private KeyCode _moveFowardKey = KeyCode.Up;
        private KeyCode _rotateCWKey = KeyCode.Right;
        private KeyCode _rotateCCWKey = KeyCode.Left;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>
        /// and injects the given <paramref name="mockedKeyboard"/> and <paramref name="mockedEntity"/>
        /// for the purpose of unit testing.
        /// </summary>
        /// <param name="mockedKeyboard">The mocked keyboard to inject for testing.</param>
        /// <param name="mockedEntity">The mocked entity to use for testing.</param>
        internal MoveFowardKeyboardBehavior(IKeyboard mockedKeyboard, T mockedEntity)
        {
            _keyboard = new Keyboard(mockedKeyboard);

            CreateBehaviors(mockedKeyboard);
            SetupBehaviors();

            _gameObject = mockedEntity;

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
            _keyboard = new Keyboard();
            LinearSpeed = linearSpeed;
            AngularSpeed = angularSpeed;

            CreateBehaviors();
            SetupBehaviors();

            _gameObject = entity;

            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> foward in the direction it is facing.
        /// </summary>
        public KeyCode MoveFowardKey
        {
            get => _moveFowardKey;
            set
            {
                _moveFowardKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will rotate the <see cref="DynamicEntity"/> clockwise.
        /// </summary>
        public KeyCode RotateCWKey
        {
            get => _rotateCWKey;
            set
            {
                _rotateCWKey = value;
                SetupBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will rotate the <see cref="DynamicEntity"/> counter clockwise.
        /// </summary>
        public KeyCode RotateCCWKey
        {
            get => _rotateCCWKey;
            set
            {
                _rotateCCWKey = value;
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
        #endregion


        #region Private Methods
        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other internal behaviors.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        private void UpdateAction(EngineTime engineTime)
        {
            _keyboard.UpdateCurrentState();

            IsMovingForward = _keyboard.IsKeyDown(_moveFowardKey);

            _moveFowardKeyBehavior.Update(engineTime);
            _rotateCWKeyBehavior.Update(engineTime);
            _rotateCCWKeyBehavior.Update(engineTime);

            _keyboard.UpdatePreviousState();
        }


        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/> movement.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void CreateBehaviors()
        {
            //Setup the move foward key behavior
            _moveFowardKeyBehavior = new KeyBehavior(_moveFowardKey, true);
            _moveFowardKeyBehavior.KeyDownEvent += MoveFoward_KeyDown;

            //Setup the rotate clockwise key behavior
            _rotateCWKeyBehavior = new KeyBehavior(_rotateCWKey, true);
            _rotateCWKeyBehavior.KeyDownEvent += RotateCW_KeyDown;

            //Setup the rotate counter clockwise key behavior
            _rotateCCWKeyBehavior = new KeyBehavior(_rotateCCWKey, true);
            _rotateCCWKeyBehavior.KeyDownEvent += RotateCCW_KeyDown;
        }


        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject into the behaviors for testing.</param>
        private void CreateBehaviors(IKeyboard keyboard)
        {
            _moveFowardKeyBehavior = new KeyBehavior(keyboard)
            {
                Key = _moveFowardKey
            };
            _moveFowardKeyBehavior.KeyDownEvent += MoveFoward_KeyDown;

            _rotateCWKeyBehavior = new KeyBehavior(keyboard)
            {
                Key = _rotateCWKey
            };
            _rotateCWKeyBehavior.KeyDownEvent += RotateCW_KeyDown;

            _rotateCCWKeyBehavior = new KeyBehavior(keyboard)
            {
                Key = _rotateCCWKey
            };
            _rotateCCWKeyBehavior.KeyDownEvent += RotateCCW_KeyDown;
        }


        /// <summary>
        /// Sets up the behaviors.
        /// </summary>
        private void SetupBehaviors()
        {
            //Setup the move foward key behavior
            _moveFowardKeyBehavior.Key = _moveFowardKey;
            _moveFowardKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate clockwise key behavior
            _rotateCWKeyBehavior.Key = _rotateCWKey;
            _rotateCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate counter clockwise key behavior
            _rotateCCWKeyBehavior.Key = _rotateCCWKey;
            _rotateCCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }


        #region Event Methods
        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> forward in the direction it is facing.
        /// </summary>
        private void MoveFoward_KeyDown(object sender, KeyEventArgs e)
        {
            IsMovingForward = true;
            _gameObject.MoveAtSetAngle(LinearSpeed);
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise.
        /// </summary>
        private void RotateCW_KeyDown(object sender, KeyEventArgs e) => _gameObject.RotateCW(AngularSpeed);


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise.
        /// </summary>
        private void RotateCCW_KeyDown(object sender, KeyEventArgs e) => _gameObject.RotateCCW(AngularSpeed);
        #endregion
        #endregion
    }
}

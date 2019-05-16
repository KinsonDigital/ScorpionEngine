using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionEngine.Entities;

namespace KDScorpionEngine.Behaviors
{
    public class MoveFowardKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        #region Fields
        private KeyBehavior _moveFowardKeyBehavior;
        private KeyBehavior _rotateCWKeyBehavior;
        private KeyBehavior _rotateCCWKeyBehavior;
        private Keyboard _keyboard = new Keyboard();
        private T _gameObject;
        private readonly float _movementSpeed;
        private KeyCodes _moveFowardKey = KeyCodes.Up;
        private KeyCodes _rotateCWKey = KeyCodes.Right;
        private KeyCodes _rotateCCWKey = KeyCodes.Left;
        private float _rotateSpeed;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// </summary>
        /// <param name="entity">The <see cref="DynamicEntity"/> to perform keyboard movement behavior upon.</param>
        /// <param name="movementSpeed">The speed that the <see cref="DynamicEntity"/> will move at.</param>
        /// <param name="rotateSpeed">The speed that the <see cref="DynamicEntity"/> will rotate at.</param>
        public MoveFowardKeyboardBehavior(T entity, float movementSpeed, float rotateSpeed)
        {
            _movementSpeed = movementSpeed;
            _rotateSpeed = rotateSpeed;

            CreateBehaviors();

            _gameObject = entity;

            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> foward in the direction it is facing.
        /// </summary>
        public KeyCodes MoveFowardKey
        {
            get => _moveFowardKey;
            set
            {
                _moveFowardKey = value;
                CreateBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will rotate the <see cref="DynamicEntity"/> clockwise.
        /// </summary>
        public KeyCodes RotateCWKey
        {
            get => _rotateCWKey;
            set
            {
                _rotateCWKey = value;
                CreateBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will rotate the <see cref="DynamicEntity"/> counter clockwise.
        /// </summary>
        public KeyCodes RotateCCWKey
        {
            get => _rotateCCWKey;
            set
            {
                _rotateCCWKey = value;
                CreateBehaviors();
            }
        }

        /// <summary>
        /// Gets a value indicating if the attempt to move foward is true.
        /// </summary>
        public bool IsMovingForward { get; private set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// The action that will be invoked by the behavior.  This will update the other behaviors.
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
        private void CreateBehaviors()
        {
            //Setup the move foward key behavior
            _moveFowardKeyBehavior = new KeyBehavior(_moveFowardKey, true);
            _moveFowardKeyBehavior.KeyDownEvent += MoveFoward_KeyDown;
            _moveFowardKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate clockwise key behavior
            _rotateCWKeyBehavior = new KeyBehavior(_rotateCWKey, true);
            _rotateCWKeyBehavior.KeyDownEvent += RotateCW_KeyDown;
            _rotateCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate counter clockwise key behavior
            _rotateCCWKeyBehavior = new KeyBehavior(_rotateCCWKey, true);
            _rotateCCWKeyBehavior.KeyDownEvent += RotateCCW_KeyDown;
            _rotateCCWKeyBehavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }


        #region Event Methods
        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> forward in the direction it is facing.
        /// </summary>
        private void MoveFoward_KeyDown(object sender, KeyEventArgs e)
        {
            IsMovingForward = true;
            _gameObject.MoveAtSetAngle(_movementSpeed);
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> clockwise.
        /// </summary>
        private void RotateCW_KeyDown(object sender, KeyEventArgs e)
        {
            _gameObject.RotateCW(_rotateSpeed);
        }


        /// <summary>
        /// Rotates the <see cref="DynamicEntity"/> counter clockwise.
        /// </summary>
        private void RotateCCW_KeyDown(object sender, KeyEventArgs e)
        {
            _gameObject.RotateCCW(_rotateSpeed);
        }
        #endregion
        #endregion
    }
}

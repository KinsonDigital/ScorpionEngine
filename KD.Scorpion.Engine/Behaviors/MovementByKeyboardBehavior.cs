using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionEngine.Entities;

namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// Creates a behavior that controls the left, right, up, and down movement of a <see cref="DynamicEntity"/>.
    /// <typeparamref name="T">The type of <see cref="DynamicEntity"/> to apply the movement to.</typeparamref>
    /// </summary>
    public class MovementByKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        #region Fields
        private KeyBehavior _moveRightOnKeyDown;
        private KeyBehavior _moveLeftOnKeyDown;
        private KeyBehavior _moveUpOnKeyDown;
        private KeyBehavior _moveDownOnKeyDown;
        private T _gameObject;
        private readonly float _movementSpeed;
        private InputKeys _moveUpKey = InputKeys.Up;
        private InputKeys _moveDownKey = InputKeys.Down;
        private InputKeys _moveLeftKey = InputKeys.Left;
        private InputKeys _moveRightKey = InputKeys.Right;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MovementByKeyboardBehavior{T}"/>.
        /// </summary>
        /// <param name="entity">The <see cref="DynamicEntity"/> to perform keyboard movement behavior upon.</param>
        /// <param name="movementSpeed">The movement speed that the <see cref="DynamicEntity"/> will move at.</param>
        public MovementByKeyboardBehavior(T entity, float movementSpeed)
        {
            _movementSpeed = movementSpeed;

            CreateBehaviors();

            _gameObject = entity;

            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> up.
        /// </summary>
        public InputKeys MoveUpKey
        {
            get
            {
                return _moveUpKey;
            }
            set
            {
                _moveUpKey = value;
                CreateBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> down.
        /// </summary>
        public InputKeys MoveDownKey
        {
            get
            {
                return _moveDownKey;
            }
            set
            {
                _moveDownKey = value;
                CreateBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> left.
        /// </summary>
        public InputKeys MoveLeftKey
        {
            get
            {
                return _moveLeftKey;
            }
            set
            {
                _moveLeftKey = value;
                CreateBehaviors();
            }
        }

        /// <summary>
        /// Gets or sets the keyboard key that will move the <see cref="DynamicEntity"/> right.
        /// </summary>
        public InputKeys MoveRightKey
        {
            get
            {
                return _moveRightKey;
            }
            set
            {
                _moveRightKey = value;
                CreateBehaviors();
            }
        }
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
        private void CreateBehaviors()
        {
            //Setup the move right key behavior
            _moveRightOnKeyDown = new KeyBehavior(_moveRightKey, true);
            _moveRightOnKeyDown.KeyDownEvent += MoveRight_KeyDown;
            _moveRightOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move left key behavior
            _moveLeftOnKeyDown = new KeyBehavior(_moveLeftKey, true);
            _moveLeftOnKeyDown.KeyDownEvent += MoveLeft_KeyDown;
            _moveLeftOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move up key behavior
            _moveUpOnKeyDown = new KeyBehavior(_moveUpKey, true);
            _moveUpOnKeyDown.KeyDownEvent += MoveUp_KeyDown;
            _moveUpOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move down key behavior
            _moveDownOnKeyDown = new KeyBehavior(_moveDownKey, true);
            _moveDownOnKeyDown.KeyDownEvent += MoveDown_KeyDown;
            _moveDownOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }


        #region Event Methods
        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right.
        /// </summary>
        private void MoveRight_KeyDown(object sender, KeyEventArgs e)
        {
            _gameObject.MoveRight(_movementSpeed);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the left.
        /// </summary>
        private void MoveLeft_KeyDown(object sender, KeyEventArgs e)
        {
            _gameObject.MoveLeft(_movementSpeed);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> up.
        /// </summary>
        private void MoveUp_KeyDown(object sender, KeyEventArgs e)
        {
            _gameObject.MoveUp(_movementSpeed);
        }


        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> down.
        /// </summary>
        private void MoveDown_KeyDown(object sender, KeyEventArgs e)
        {
            _gameObject.MoveDown(_movementSpeed);
        }
        #endregion
        #endregion
    }
}
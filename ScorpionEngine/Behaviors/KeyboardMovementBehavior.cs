using ScorpionEngine.Entities;
using ScorpionEngine.Input;

namespace ScorpionEngine.Behaviors
{
    /// <summary>
    /// Creates a a behavior that controls the left, right, up, and down movement of a movable game object.
    /// </summary>
    public class KeyboardMovementBehavior<T> : Behavior where T : DynamicEntity
    {
        #region Fields
        private KeyBehavior _moveRightOnKeyDown;
        private KeyBehavior _moveLeftOnKeyDown;
        private KeyBehavior _moveUpOnKeyDown;
        private KeyBehavior _moveDownOnKeyDown;
        private T _gameObject;
        private readonly float _movementSpeed;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of MovementBehavior.
        /// </summary>
        /// <param name="obj">The game object to perform keyboard movement behavior on.</param>
        public KeyboardMovementBehavior(T obj, float movementSpeed)
        {
            _movementSpeed = movementSpeed;

            CreateBehaviors();

            _gameObject = obj;

            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Private Methods
        private void UpdateAction(EngineTime engineTime)
        {
            _moveRightOnKeyDown.Update(engineTime);
            _moveLeftOnKeyDown.Update(engineTime);
            _moveUpOnKeyDown.Update(engineTime);
            _moveDownOnKeyDown.Update(engineTime);
        }


        private void CreateBehaviors()
        {
            //Setup the move right key behavior
            _moveRightOnKeyDown = new KeyBehavior(InputKeys.Right, true);
            _moveRightOnKeyDown.KeyDownEvent += MoveRightKeyDownOnPressKeyDownOnPressEvent;
            _moveRightOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move left key behavior
            _moveLeftOnKeyDown = new KeyBehavior(InputKeys.Left, true);
            _moveLeftOnKeyDown.KeyDownEvent += MoveLeftKeyDownOnPressKeyDownEvent;
            _moveLeftOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move up key behavior
            _moveUpOnKeyDown = new KeyBehavior(InputKeys.Up, true);
            _moveUpOnKeyDown.KeyDownEvent += MoveUpKeyDownOnPressKeyDownEvent;
            _moveUpOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move down key behavior
            _moveDownOnKeyDown = new KeyBehavior(InputKeys.Down, true);
            _moveDownOnKeyDown.KeyDownEvent += MoveDownKeyDownOnPressKeyDownEvent;
            _moveDownOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }


        #region Event Methods
        /// <summary>
        /// Moves the game object to the right.
        /// </summary>
        private void MoveRightKeyDownOnPressKeyDownOnPressEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveRight(_movementSpeed);
        }


        /// <summary>
        /// Moves the game object to the left.
        /// </summary>
        private void MoveLeftKeyDownOnPressKeyDownEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveLeft(_movementSpeed);
        }


        /// <summary>
        /// Moves the game object up.
        /// </summary>
        private void MoveUpKeyDownOnPressKeyDownEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveUp(_movementSpeed);
        }


        /// <summary>
        /// Moves the game object down.
        /// </summary>
        private void MoveDownKeyDownOnPressKeyDownEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveDown(_movementSpeed);
        }
        #endregion
        #endregion
    }
}
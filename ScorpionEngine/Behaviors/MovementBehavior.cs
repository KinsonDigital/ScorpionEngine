using ScorpionEngine.Entities;
using ScorpionEngine.Input;
using ScorpionCore;
using ScorpionCore.Plugins;

namespace ScorpionEngine.Behaviors
{
    /// <summary>
    /// Creates a a behavior that controls the left, right, up, and down movement of a movable game object.
    /// </summary>
    public class MovementBehavior : Behavior
    {
        #region Fields
        private KeyBehavior _moveRightKeyOnPress;
        private KeyBehavior _moveLeftKeyOnPress;
        private KeyBehavior _moveUpKeyOnPress;
        private KeyBehavior _moveDownKeyOnPress;
        private DynamicEntity _gameObject;
        private readonly float _movementSpeed;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of MovementBehavior.
        /// </summary>
        /// <param name="obj">The game object to perform the movement behavior on.</param>
        public MovementBehavior(DynamicEntity obj, float movementSpeed)
        {
            _movementSpeed = movementSpeed;

            CreateBehaviors();

            _gameObject = obj;

            //Setup the move right key behavior
            _moveRightKeyOnPress.KeyDownEvent += MoveRightKeyDownOnPressKeyDownOnPressEvent;
            _moveRightKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move left key behavior
            _moveLeftKeyOnPress.KeyDownEvent += MoveLeftKeyDownOnPressKeyDownEvent;
            _moveLeftKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move up key behavior
            _moveUpKeyOnPress.KeyDownEvent += MoveUpKeyDownOnPressKeyDownEvent;
            _moveUpKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the move down key behavior
            _moveDownKeyOnPress.KeyDownEvent += MoveDownKeyDownOnPressKeyDownEvent;
            _moveDownKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            SetUpdateAction(UpdateAction);
        }
        #endregion


        #region Private Methods
        private void UpdateAction(EngineTime engineTime)
        {
            _moveRightKeyOnPress.Update(engineTime);
            _moveLeftKeyOnPress.Update(engineTime);
            _moveUpKeyOnPress.Update(engineTime);
            _moveDownKeyOnPress.Update(engineTime);
        }


        private void CreateBehaviors()
        {
            _moveRightKeyOnPress = new KeyBehavior(InputKeys.Right, true);
            _moveLeftKeyOnPress = new KeyBehavior(InputKeys.Left, true);
            _moveUpKeyOnPress = new KeyBehavior(InputKeys.Up, true);
            _moveDownKeyOnPress = new KeyBehavior(InputKeys.Down, true);
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


        /// <summary>
        /// Stops the movement of the game object.
        /// </summary>
        private void AllKeysDownOnReleaseKeyDownEvent(object sender, KeyEventArgs e)
        {
        }
        #endregion
        #endregion
    }
}
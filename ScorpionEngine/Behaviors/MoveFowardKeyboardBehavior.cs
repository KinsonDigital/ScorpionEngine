using ScorpionEngine.Entities;
using ScorpionEngine.Input;

namespace ScorpionEngine.Behaviors
{
    public class MoveFowardKeyboardBehavior<T> : Behavior where T : DynamicEntity
    {
        #region Fields
        private KeyBehavior _moveFowardOnKeyDown;
        private KeyBehavior _rotateCWOnKeyDown;
        private KeyBehavior _rotateCCWOnKeyDown;
        private T _gameObject;
        private readonly float _movementSpeed;
        private InputKeys _moveFowardKey = InputKeys.Up;
        private InputKeys _rotateCWKey = InputKeys.Right;
        private InputKeys _rotateCCWKey = InputKeys.Left;
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
        public InputKeys MoveFowardKey
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
        public InputKeys RotateCW
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
        public InputKeys RotateCCW
        {
            get => _rotateCCWKey;
            set
            {
                _rotateCCWKey = value;
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
            _moveFowardOnKeyDown.Update(engineTime);
            _rotateCWOnKeyDown.Update(engineTime);
            _rotateCCWOnKeyDown.Update(engineTime);
        }


        /// <summary>
        /// Creates all of the keyboard behaviors that deal with <see cref="DynamicEntity"/> movement.
        /// </summary>
        private void CreateBehaviors()
        {
            //Setup the move foward key behavior
            _moveFowardOnKeyDown = new KeyBehavior(_moveFowardKey, true);
            _moveFowardOnKeyDown.KeyDownEvent += MoveFoward_KeyDown;
            _moveFowardOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate clockwise key behavior
            _rotateCWOnKeyDown = new KeyBehavior(_rotateCWKey, true);
            _rotateCWOnKeyDown.KeyDownEvent += RotateCW_KeyDown;
            _rotateCWOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            //Setup the rotate counter clockwise key behavior
            _rotateCCWOnKeyDown = new KeyBehavior(_rotateCCWKey, true);
            _rotateCCWOnKeyDown.KeyDownEvent += RotateCCW_KeyDown;
            _rotateCCWOnKeyDown.BehaviorType = KeyBehaviorType.KeyDownContinuous;
        }


        #region Event Methods
        /// <summary>
        /// Moves the <see cref="DynamicEntity"/> to the right.
        /// </summary>
        private void MoveFoward_KeyDown(object sender, KeyEventArgs e)
        {
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

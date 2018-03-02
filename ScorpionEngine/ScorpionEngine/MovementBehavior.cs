using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using ScorpionEngine.Objects;
using ScorpionEngine.Input;

namespace ScorpionEngine
{
    /// <summary>
    /// Creates a a behavior that controls the left, right, up, and down movement of a movable game object.
    /// </summary>
    public class MovementBehavior
    {
        #region Fields
        private readonly KeyBehavior _moveRightKeyOnPress = new KeyBehavior(InputKeys.Right, true);
        private readonly KeyBehavior _moveLeftKeyOnPress = new KeyBehavior(InputKeys.Left, true);
        private readonly KeyBehavior _moveUpKeyOnPress = new KeyBehavior(InputKeys.Up, true);
        private readonly KeyBehavior _moveDownKeyOnPress = new KeyBehavior(InputKeys.Down, true);
        private readonly KeyBehavior _moveRightKeyOnRelease = new KeyBehavior(InputKeys.Right, true);
        private readonly KeyBehavior _moveLefttKeyOnRelease = new KeyBehavior(InputKeys.Left, true);
        private readonly KeyBehavior _moveUpKeyOnRelease = new KeyBehavior(InputKeys.Up, true);
        private readonly KeyBehavior _moveDownKeyOnRelease = new KeyBehavior(InputKeys.Down, true);
        private MovableObject _gameObject;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of MovementBehavior.
        /// </summary>
        /// <param name="obj">The game object to perform the movement behavior on.</param>
        public MovementBehavior(MovableObject obj)
        {
            _gameObject = obj;

            //Setup the move right key behavior
            _moveRightKeyOnPress.KeyDownEvent += MoveRightKeyDownOnPressKeyDownOnPressEvent;
            _moveRightKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;
            _moveRightKeyOnRelease.KeyDownEvent += AllKeysDownOnReleaseKeyDownEvent;
            _moveRightKeyOnRelease.BehaviorType = KeyBehaviorType.OnceOnRelease;

            //Setup the move left key behavior
            _moveLeftKeyOnPress.KeyDownEvent += MoveLeftKeyDownOnPressKeyDownEvent;
            _moveLeftKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;
            _moveLefttKeyOnRelease.KeyDownEvent += AllKeysDownOnReleaseKeyDownEvent;
            _moveLefttKeyOnRelease.BehaviorType = KeyBehaviorType.OnceOnRelease;

            //Setup the move up key behavior
            _moveUpKeyOnPress.KeyDownEvent += MoveUpKeyDownOnPressKeyDownEvent;
            _moveUpKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;
            _moveUpKeyOnRelease.KeyDownEvent += AllKeysDownOnReleaseKeyDownEvent;
            _moveUpKeyOnRelease.BehaviorType = KeyBehaviorType.OnceOnRelease;

            //Setup the move down key behavior
            _moveDownKeyOnPress.KeyDownEvent += MoveDownKeyDownOnPressKeyDownEvent;
            _moveDownKeyOnPress.BehaviorType = KeyBehaviorType.KeyDownContinuous;
            _moveDownKeyOnRelease.KeyDownEvent += AllKeysDownOnReleaseKeyDownEvent;
            _moveDownKeyOnRelease.BehaviorType = KeyBehaviorType.OnceOnRelease;
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// Moves the game object to the right.
        /// </summary>
        private void MoveRightKeyDownOnPressKeyDownOnPressEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveRight();
        }

        /// <summary>
        /// Moves the game object to the left.
        /// </summary>
        private void MoveLeftKeyDownOnPressKeyDownEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveLeft();
        }

        /// <summary>
        /// Moves the game object up.
        /// </summary>
        private void MoveUpKeyDownOnPressKeyDownEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveUp();
        }

        /// <summary>
        /// Moves the game object down.
        /// </summary>
        private void MoveDownKeyDownOnPressKeyDownEvent(object sender, KeyEventArgs e)
        {
            _gameObject.MoveDown();
        }

        /// <summary>
        /// Stops the movement of the game object.
        /// </summary>
        private void AllKeysDownOnReleaseKeyDownEvent(object sender, KeyEventArgs e)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the key behaviors.
        /// </summary>
        /// <param name="engineTime">The engine time.</param>
        public void Update(EngineTime engineTime)
        {
            _moveRightKeyOnPress.Update(engineTime);
            _moveLeftKeyOnPress.Update(engineTime);
            _moveUpKeyOnPress.Update(engineTime);
            _moveDownKeyOnPress.Update(engineTime);
            _moveRightKeyOnRelease.Update(engineTime);
            _moveLefttKeyOnRelease.Update(engineTime);
            _moveUpKeyOnRelease.Update(engineTime);
            _moveDownKeyOnRelease.Update(engineTime);
        }
        #endregion
    }
}

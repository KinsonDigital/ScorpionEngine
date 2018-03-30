using System;
using System.Collections;
using System.Collections.Generic;
using ScorpionEngine.EventArguments;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Represents a set amount of entities that can be used over and over.
    /// </summary>
    public abstract class ObjectPool : IEnumerable
    {
        #region Delegates/Events
        /// <summary>
        /// Occurs whenever an entity in the entity pool goes out of bounds.
        /// </summary>
        public event EventHandler<OutOfBoundsTriggerEventsArgs> OnOutOfBounds;
        #endregion


        #region Fields
        private readonly List<MovableObject> _objects = new List<MovableObject>();//The pool of objects to manage.
        private Rect _triggerBounds;//The bounds used to trigger the out of bounds trigger
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the name of the entity pool.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or inserts a entity in the entity pool.
        /// </summary>
        /// <returns></returns>
        public MovableObject this[int index]//Indexer property
        {
            get
            {
                return _objects[index];
            }
            set
            {
                _objects.Insert(index, value);
            }
        }

        /// <summary>
        /// Gets total number of entities in the entity pool.
        /// </summary>
        public int Count
        {
            get
            {
                return _objects.Count;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the out of bounds trigger will be processed for each entity.
        /// </summary>
        public bool OutOfBoundsTriggerEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the entities in the pool will have there OnUpdate method called when visible only.
        /// </summary>
        /// <returns></returns>
        public bool OnlyUpdateWhenVisible { get; set; }

        /// <summary>
        /// Gets or sets the direction that the entities will move when visible.
        /// </summary>
        public Direction DirectionWhenVisible { get; set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new pool of entities.
        /// </summary>
        public ObjectPool()
        {
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given GameObject to the entity pool.
        /// </summary>
        /// <param name="obj">The GameObject to add.</param>
        public void AddGameObject(MovableObject obj)
        {
            _objects.Add(obj);
        }
        #endregion


        #region Game Loop Methods
        /// <summary>
        /// Shows the next available item in the pool.
        /// </summary>
        public void ShowNext()
        {
            //Check for the first entity that is visible
            for (int i = 0; i < _objects.Count; i++)
            {
                if (!_objects[i].Visible)
                {
                    _objects[i].Visible = true;
                    break;
                }
            }
        }


        /// <summary>
        /// Sets bounds for the out of bounds trigger.
        /// </summary>
        /// <param name="bounds">The bounds used to invoke the out of bounds trigger for each entity.</param>
        public void SetOutOfBoundsTrigger(Rect bounds)
        {
            _triggerBounds = bounds;
        }


        /// <summary>
        /// Returns an enumerator that iterates through the entity pool.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _objects.GetEnumerator();
        }


        /// <summary>
        /// Shows all of the entities.
        /// </summary>
        public void ShowAll()
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                _objects[i].Visible = true;
            }
        }


        /// <summary>
        /// Hides all of the entities.
        /// </summary>
        public void HideAll()
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                _objects[i].Visible = false;
            }
        }


        /// <summary>
        /// Applies the given speed to an entity that are of the given description.
        /// </summary>
        /// <param name="thatAre">The description of the entity that the speed is to be applied to.</param>
        /// <param name="speed">The speed to apply.</param>
        public void ApplySpeedTo(EntitiesThatAre thatAre, Vector speed)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Applies the given auto rotate setting to entities that are of the given description.
        /// </summary>
        /// <param name="thatAre">The description of the entity that the speed is to be applied to.</param>
        /// <param name="autoRotate">The auto rotate setting to apply.</param>
        public void ApplyAutoRotateTo(EntitiesThatAre thatAre, bool autoRotate)
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                switch (thatAre)
                {
                    case EntitiesThatAre.Visible:
                        if (_objects[i].Visible)
                            _objects[i].AutoRotate = autoRotate;
                        break;
                    case EntitiesThatAre.Hidden:
                        if (_objects[i].Visible)
                            _objects[i].AutoRotate = autoRotate;
                        break;
                    case EntitiesThatAre.Anything:
                        _objects[i].AutoRotate = autoRotate;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(thatAre), thatAre, null);
                }
            }
        }


        /// <summary>
        /// Applies the given auto rotate direction to entities that are of the given description.
        /// </summary>
        /// <param name="thatAre">The description of the entity that the speed is to be applied to.</param>
        /// <param name="rotateDirection">The direction to rotate.</param>
        public void ApplyAutoRotateDirectionTo(EntitiesThatAre thatAre, RotationDirection rotateDirection)
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                switch (thatAre)
                {
                    case EntitiesThatAre.Visible:
                        if (!_objects[i].Visible)
                            _objects[i].AutoRotateDirection = rotateDirection;
                        break;
                    case EntitiesThatAre.Hidden:
                        if (_objects[i].Visible)
                            _objects[i].AutoRotateDirection = rotateDirection;
                        break;
                    case EntitiesThatAre.Anything:
                        _objects[i].AutoRotateDirection = rotateDirection;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(thatAre), thatAre, null);
                }
            }
        }


        /// <summary>
        /// Sets the given auto move setting to entities that fit the given description.
        /// </summary>
        /// <param name="thatAre">The description of the entities that the setting applies to.</param>
        /// <param name="autoMove">The setting to apply.</param>
        public void ApplyAutoMoveTo(EntitiesThatAre thatAre, bool autoMove)
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                switch (thatAre)
                {
                    case EntitiesThatAre.Visible:
                        if (!_objects[i].Visible)
                            _objects[i].AutoMove = autoMove;
                        break;
                    case EntitiesThatAre.Hidden:
                        if (_objects[i].Visible)
                            _objects[i].AutoMove = autoMove;
                        break;
                    case EntitiesThatAre.Anything:
                        _objects[i].AutoMove = autoMove;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(thatAre), thatAre, null);
                }
            }
        }


        /// <summary>
        /// Sets the given auto move direction to entities that fit the given description.
        /// </summary>
        /// <param name="thatAre">The description of the entities that the setting applies to.</param>
        /// <param name="autoMove">The setting to apply.</param>
        public void ApplyAutoMoveDirectionTo(EntitiesThatAre thatAre, Direction autoDirection)
        {
            for (var i = 0; i < _objects.Count; i++)
            {
                switch (thatAre)
                {
                    case EntitiesThatAre.Visible:
                        if (!_objects[i].Visible)
                            _objects[i].AutoDirection = autoDirection;
                        break;
                    case EntitiesThatAre.Hidden:
                        if (_objects[i].Visible)
                            _objects[i].AutoDirection = autoDirection;
                        break;
                    case EntitiesThatAre.Anything:
                        _objects[i].AutoDirection = autoDirection;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(thatAre), thatAre, null);
                }
            }
        }


        /// <summary>
        /// Sets the given angle speed to entities that fit the given description.
        /// </summary>
        /// <param name="thatAre">The description of the entities that the setting applies to.</param>
        /// <param name="angleSpeed">The setting to apply.</param>
        public void ApplyAngleSpeedTo(EntitiesThatAre thatAre, int angleSpeed)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Virtual Methods
        /// <summary>
        /// Updates the entity pool.
        /// </summary>
        public virtual void OnUpdate(EngineTime engineTime)
        {
            //Update each entity
            for (var i = 0; i < _objects.Count; i++)
            {
                if (OutOfBoundsTriggerEnabled && _objects[i].Visible)
                {
                    //If the position of the current entity is out of bounds,
                    //invoke the out of bounds trigger event
                    if (!_triggerBounds.Contains(_objects[i].Position))
                    {
                        OnOutOfBounds?.Invoke(this, new OutOfBoundsTriggerEventsArgs(_objects[i]));
                    }
                }

                if (OnlyUpdateWhenVisible)
                {
                    //Checks if the current entity is visible, if so then the entity will be moved
                    if (_objects[i].Visible)
                    {
                        _objects[i].OnUpdate(engineTime);
                    }
                }
                else
                {
                    _objects[i].OnUpdate(engineTime);
                }
            }
        }
        #endregion
    }
}
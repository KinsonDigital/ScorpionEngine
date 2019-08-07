using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KDScorpionCore;
using KDScorpionEngine.Events;

namespace KDScorpionEngine.Entities
{
    /// <summary>
    /// Represents a set amount of entities that can be used over and over.
    /// </summary>
    //TODO: Get this working and setup unit tests.  Wait for when you create a game for testing
    //Also take the out of bounds concept and remove it from this class and create a custom
    //behavior called OutOfBoundsBehavior.  This behavior can then be added to this class to pull off this concept.
    [ExcludeFromCodeCoverage]
    public abstract class EntityPool : IEnumerable
    {
        #region Delegates/Events
        /// <summary>
        /// Occurs whenever an entity in the entity pool goes out of bounds.
        /// </summary>
        public event EventHandler<OutOfBoundsTriggerEventsArgs> OnOutOfBounds;
        #endregion


        #region Private Fields
        private readonly List<DynamicEntity> _objects = new List<DynamicEntity>();//The pool of objects to manage.
        private Rect _triggerBounds;//The bounds used to trigger the out of bounds trigger
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="EntityPool"/>.
        /// </summary>
        public EntityPool() { }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the entity pool.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an entity in the entity pool.
        /// </summary>
        /// <returns></returns>
        public DynamicEntity this[int index]
        {
            get => _objects[index];
            set => _objects.Insert(index, value);
        }

        /// <summary>
        /// Gets total number of entities in the entity pool.
        /// </summary>
        public int Count => _objects.Count;

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


        #region Public Methods
        /// <summary>
        /// Adds the given <paramref name="entity"/> to the entity pool.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void AddGameObject(DynamicEntity entity) => _objects.Add(entity);
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
        public void SetOutOfBoundsTrigger(Rect bounds) => _triggerBounds = bounds;


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
        /// Applies the given speed to an entity that match the given <paramref name="thatAre"/> parameter.
        /// </summary>
        /// <param name="thatAre">The state of the entity that the speed will be applied to.</param>
        /// <param name="speed">The speed to apply.</param>
        public void ApplySpeedTo(EntitiesThatAre thatAre, Vector speed) => throw new NotImplementedException();


        /// <summary>
        /// Sets the given angle velocity to entities that fit the given description.
        /// </summary>
        /// <param name="thatAre">The state of the entity that the velocity will be applied to.</param>
        /// <param name="angleVelocity">The setting to apply.</param>
        public void ApplyAngleVelocityTo(EntitiesThatAre thatAre, int angleVelocity) => throw new NotImplementedException();
        #endregion


        #region Virtual Methods
        /// <summary>
        /// Updates the entity pool.
        /// </summary>
        public virtual void Update(IEngineTiming engineTime)
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
                        //_objects[i].OnUpdate(engineTime);
                    }
                }
                else
                {
                    //_objects[i].OnUpdate(engineTime);
                }
            }
        }
        #endregion
    }
}
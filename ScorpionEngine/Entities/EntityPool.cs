// <copyright file="EntityPool.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
    using KDScorpionEngine.Events;
    using Raptor;

    /// <summary>
    /// Represents a set amount of entities that can be used over and over.
    /// </summary>
    // TODO: Get this working and setup unit tests.  Wait for when you create a game for testing
    // Also take the out of bounds concept and remove it from this class and create a custom
    // behavior called OutOfBoundsBehavior.  This behavior can then be added to this class to pull off this concept.
    [ExcludeFromCodeCoverage]
    public abstract class EntityPool : IEnumerable
    {
        private readonly List<DynamicEntity> objects = new List<DynamicEntity>(); // The pool of objects to manage.
        private Rect triggerBounds; // The bounds used to trigger the out of bounds trigger

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityPool"/> class.
        /// </summary>
        public EntityPool()
        {
        }

        /// <summary>
        /// Occurs whenever an entity in the entity pool goes out of bounds.
        /// </summary>
        public event EventHandler<OutOfBoundsTriggerEventsArgs> OnOutOfBounds;

        /// <summary>
        /// Gets or sets the name of the entity pool.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets total number of entities in the entity pool.
        /// </summary>
        public int Count => this.objects.Count;

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

        /// <summary>
        /// Gets or sets an entity in the entity pool.
        /// </summary>
        /// <returns></returns>
        public DynamicEntity this[int index]
        {
            get => this.objects[index];
            set => this.objects.Insert(index, value);
        }

        /// <summary>
        /// Adds the given <paramref name="entity"/> to the entity pool.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void AddGameObject(DynamicEntity entity) => this.objects.Add(entity);

        /// <summary>
        /// Shows the next available item in the pool.
        /// </summary>
        public void ShowNext()
        {
            // Check for the first entity that is visible
            for (var i = 0; i < this.objects.Count; i++)
            {
                if (!this.objects[i].Visible)
                {
                    this.objects[i].Visible = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Sets bounds for the out of bounds trigger.
        /// </summary>
        /// <param name="bounds">The bounds used to invoke the out of bounds trigger for each entity.</param>
        public void SetOutOfBoundsTrigger(Rect bounds) => this.triggerBounds = bounds;

        /// <summary>
        /// Returns an enumerator that iterates through the entity pool.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => this.objects.GetEnumerator();

        /// <summary>
        /// Shows all of the entities.
        /// </summary>
        public void ShowAll()
        {
            for (var i = 0; i < this.objects.Count; i++)
            {
                this.objects[i].Visible = true;
            }
        }

        /// <summary>
        /// Hides all of the entities.
        /// </summary>
        public void HideAll()
        {
            for (var i = 0; i < this.objects.Count; i++)
            {
                this.objects[i].Visible = false;
            }
        }

        /// <summary>
        /// Applies the given speed to an entity that match the given <paramref name="thatAre"/> parameter.
        /// </summary>
        /// <param name="thatAre">The state of the entity that the speed will be applied to.</param>
        /// <param name="speed">The speed to apply.</param>
        public void ApplySpeedTo(EntitiesThatAre thatAre, Vector2 speed) => throw new NotImplementedException();

        /// <summary>
        /// Sets the given angle velocity to entities that fit the given description.
        /// </summary>
        /// <param name="thatAre">The state of the entity that the velocity will be applied to.</param>
        /// <param name="angleVelocity">The setting to apply.</param>
        public void ApplyAngleVelocityTo(EntitiesThatAre thatAre, int angleVelocity) => throw new NotImplementedException();

        /// <summary>
        /// Updates the entity pool.
        /// </summary>
        public virtual void Update(IEngineTiming engineTime)
        {
            // Update each entity
            for (var i = 0; i < this.objects.Count; i++)
            {
                if (OutOfBoundsTriggerEnabled && this.objects[i].Visible)
                {
                    // If the position of the current entity is out of bounds,
                    // invoke the out of bounds trigger event
                    if (!this.triggerBounds.Contains(this.objects[i].Position))
                    {
                        OnOutOfBounds?.Invoke(this, new OutOfBoundsTriggerEventsArgs(this.objects[i]));
                    }
                }

                if (OnlyUpdateWhenVisible)
                {
                    // Checks if the current entity is visible, if so then the entity will be moved
                    if (this.objects[i].Visible)
                    {
                        // _objects[i].OnUpdate(engineTime);
                    }
                }
                else
                {
                    // _objects[i].OnUpdate(engineTime);
                }
            }
        }
    }
}

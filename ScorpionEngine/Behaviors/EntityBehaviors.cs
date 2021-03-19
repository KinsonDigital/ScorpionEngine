// <copyright file="EntityBehaviors.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Entities;

    /// <summary>
    /// Manages a list of <see cref="Entity"/> behaviors.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EntityBehaviors : IEnumerable, IList<IBehavior>
    {
        private readonly List<IEntityBehavior> items = new List<IEntityBehavior>();

        /// <summary>
        /// Gets the number of <see cref="IBehavior"/>s in the collection of <see cref="IBehavior"/>s.
        /// </summary>
        public int Count => this.items.Count;

        /// <summary>
        /// Gets a value indicating whether the collection of <see cref="IBehavior"/>s is readonly.
        /// </summary>
        public bool IsReadOnly { get; } = false;

        /// <summary>
        /// Gets an <see cref="IBehavior"/> at a specified index.
        /// </summary>
        /// <param name="index">The index of of the item to return.</param>
        /// <returns></returns>
        public IEntityBehavior this[int index]
        {
            get => this.items[index];
            set => this.items[index] = value;
        }

        /// <summary>
        /// Adds the given <paramref name="item"/> to the collection of <see cref="IBehavior"/>s.
        /// </summary>
        /// <param name="item">The behavior to add.</param>
        public void Add(IEntityBehavior item) => this.items.Add(item);

        /// <summary>
        /// Removes all of the <see cref="IBehavior"/>s from the collection of <see cref="IBehavior"/>s.
        /// </summary>
        public void Clear() => this.items.Clear();

        /// <summary>
        /// Determines whether the collection of <see cref="IBehavior"/>s contains the the specific
        /// <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The item check for.</param>
        /// <returns></returns>
        public bool Contains(IEntityBehavior item) => this.items.Contains(item);

        /// <summary>
        /// Copies the <see cref="IBehavior"/>s to the given collection of <paramref name="behaviors"/>
        /// starting at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="behaviors">The list of behaviors to copy the items to.</param>
        /// <param name="index">The index of where to start copying the source items.</param>
        public void CopyTo(IEntityBehavior[] behaviors, int index) => this.items.CopyTo(behaviors, index);

        /// <summary>
        /// Returns an enumerator that iterates through the collection of <see cref="IBehavior"/>s.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() => this.items.GetEnumerator();

        /// <summary>
        /// Determines the index of the given <paramref name="item"/> in the collection of <see cref="IBehavior"/>s.
        /// </summary>
        /// <param name="item">The item to get the index of.</param>
        /// <returns></returns>
        public int IndexOf(IBehavior item) => this.items.IndexOf(item);
        public int IndexOf(IEntityBehavior item) => this.items.IndexOf(item);

        /// <summary>
        /// Inserts the given <paramref name="item"/> to the collection of <see cref="IBehavior"/>s at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of where the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, IEntityBehavior item) => this.items.Insert(index, item);

        /// <summary>
        /// Removes the first occurence of the given <paramref name="item"/> from
        /// the collection of <see cref="IBehavior"/>s.
        /// </summary>
        /// <param name="item">The behavior to remove.</param>
        /// <returns></returns>
        public bool Remove(IBehavior item) => this.items.Remove(item);
        public bool Remove(IEntityBehavior item) => this.items.Remove(item);

        /// <summary>
        /// Removes the <see cref="IBehavior"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the behavior to remove.</param>
        public void RemoveAt(int index) => this.items.RemoveAt(index);

        /// <summary>
        /// Returns an enumerator that iterates through the collection of <see cref="IBehavior"/>s.
        /// </summary>
        /// <returns></returns>
        IEnumerator<IEntityBehavior> IEnumerable<IEntityBehavior>.GetEnumerator() => this.items.GetEnumerator();
    }
}

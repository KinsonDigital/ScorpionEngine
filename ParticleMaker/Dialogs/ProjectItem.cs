using System;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Represents a single project item.
    /// </summary>
    public class ProjectItem : IEquatable<ProjectItem>
    {
        #region Props
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the project with the given <see cref="Name"/> exists.
        /// </summary>
        public bool Exists { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Indicates whether the given <paramref name="obj"/> is equal to another object of the same type.
        /// </summary>
        /// <param name="other">A <see cref="ProjectItem"/> object to compare with this object.</param>
        /// <returns>True if the current object is equal to the '<paramref name="other"/>' parameter; otherwise, false.</returns>
        public bool Equals(ProjectItem other) => Name == other.Name && Exists == other.Exists;


        /// <summary>
        /// Returns a hashcode that represents the uniqueness of this <see cref="ProjectItem"/> instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Name.GetHashCode() + Exists.GetHashCode();
        #endregion
    }
}

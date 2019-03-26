using System;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Represents a single project item with an exists indication value.
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
        /// <summary>Indicates whether the given object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">A <see cref="ProjectItem"/> object to compare with this object.</param>
        public bool Equals(ProjectItem other) => Name == other.Name && Exists == other.Exists;
        #endregion
    }
}

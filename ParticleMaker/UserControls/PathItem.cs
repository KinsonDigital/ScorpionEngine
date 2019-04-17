using System.Collections.Generic;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Represents a single file path to an item.
    /// </summary>
    public class PathItem
    {
        #region Props
        /// <summary>
        /// Gets or sets the path to the file.
        /// </summary>
        public string FilePath { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if this current object is equal to the given <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PathItem pathItem))
                return false;


            return FilePath == pathItem.FilePath;
        }


        /// <summary>
        /// Returns the hash code of this object that makes this object unique.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => 1230029444 + EqualityComparer<string>.Default.GetHashCode(FilePath);
        #endregion
    }
}

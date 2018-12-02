using ScorpionCore.Content;

namespace ScorpionCore
{
    /// <summary>
    /// Provides the ability to load content.
    /// </summary>
    public interface IContentLoadable
    {
        /// <summary>
        /// Load the content using the given <paramref name="contentLoader"/>.
        /// </summary>
        /// <param name="contentLoader">Used to load content.</param>
        void LoadContent(ContentLoader contentLoader);
    }
}

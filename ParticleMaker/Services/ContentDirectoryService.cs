using System.IO;
using System.Reflection;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides content directory management and content item checking.
    /// </summary>
    public class ContentDirectoryService : IContentDirectoryService
    {
        #region Fields
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private const string CONTENT_DIR = "Content";
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ContentDirectoryService"/>
        /// </summary>
        public ContentDirectoryService(IDirectoryService directoryService, IFileService fileService)
        {
            _directoryService = directoryService;
            _fileService = fileService;

            ContentRootDirectory = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{CONTENT_DIR}";

            if (!_directoryService.Exists(ContentRootDirectory))
                _directoryService.Create(ContentRootDirectory);
        }
        #endregion


        #region Props
        /// <summary>
        /// The root directory where the texture to load is located.
        /// </summary>
        public string ContentRootDirectory { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns a value indicating if a content item with the given name exists in the <see cref="ContentRootDirectory"/>.
        /// </summary>
        /// <param name="itemName">The name of the content item.</param>
        /// <returns></returns>
        public bool ContentItemExists(string itemName)
        {
            return _fileService.Exists($@"{ContentRootDirectory}\{itemName}.png");
        }
        #endregion
    }
}

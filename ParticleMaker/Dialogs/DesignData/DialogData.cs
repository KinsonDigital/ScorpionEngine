using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker.Dialogs.DesignData
{
    /// <summary>
    /// Sample data for use during design time for the dialogs of the application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DialogData
    {
        /// <summary>
        /// Gets or sets the dialog title data.
        /// </summary>
        public string DialogTitle { get; set; }

        /// <summary>
        /// Gets or sets the message data.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the input value data.
        /// </summary>
        public string InputValue { get; set; }

        /// <summary>
        /// Gets or sets the project names list data.
        /// </summary>
        public ProjectItem[] ProjectNames { get; set; }

        /// <summary>
        /// Gets or sets the selected project.
        /// </summary>
        public string SelectedProject { get; set; } = "test-project";
    }
}

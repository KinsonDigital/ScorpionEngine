using System.Collections.Generic;
using System.Windows;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectListDialog.xaml
    /// </summary>
    public partial class ProjectListDialog : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectListDialog"/>.
        /// </summary>
        public ProjectListDialog()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ProjectPaths"/> property.
        /// </summary>
        public static readonly DependencyProperty ProjectPathsProperty =
            DependencyProperty.Register(nameof(ProjectPaths), typeof(string[]), typeof(ProjectListDialog), new PropertyMetadata(new string[0], ProjectPathsChanged));

        /// <summary>
        /// Registers the <see cref="ProjectNames"/>.
        /// </summary>
        protected static readonly DependencyProperty ProjectNamesProperty =
            DependencyProperty.Register(nameof(ProjectNames), typeof(string[]), typeof(ProjectListDialog), new PropertyMetadata(new string[0]));

        /// <summary>
        /// Registers the <see cref="SelectedProject"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedProjectProperty =
            DependencyProperty.Register(nameof(SelectedProject), typeof(string), typeof(ProjectListDialog), new PropertyMetadata(""));
        #endregion


        /// <summary>
        /// Gets or sets the paths to all of the projects.
        /// </summary>
        public string[] ProjectPaths
        {
            get { return (string[])GetValue(ProjectPathsProperty); }
            set { SetValue(ProjectPathsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the list of project names.
        /// </summary>
        protected string[] ProjectNames
        {
            get { return (string[])GetValue(ProjectNamesProperty); }
            set { SetValue(ProjectNamesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected project.
        /// </summary>
        public string SelectedProject
        {
            get { return (string)GetValue(SelectedProjectProperty); }
            set { SetValue(SelectedProjectProperty, value); }
        }
        #endregion


        #region Private Methods
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// Cancels the dialog window.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedProject = string.Empty;

            DialogResult = false;

            Close();
        }


        /// <summary>
        /// Updates the project names list.
        /// </summary>
        private static void ProjectPathsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = (ProjectListDialog)d;

            if (dialog == null)
                return;

            if (!(e.NewValue is string[] newValue))
                return;

            var names = new List<string>();

            foreach (var item in newValue)
            {
                var sections = item.Split('\\');

                if (sections.Length > 0)
                    names.Add(sections[sections.Length - 1]);
            }

            dialog.ProjectNames = names.ToArray();
        }
        #endregion
        }
}

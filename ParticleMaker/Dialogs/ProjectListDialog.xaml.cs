using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectListDialog.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ProjectListDialog : Window
    {
        #region Fields
        private CancellationTokenSource _tokenSrc;
        private Task _autoRefreshTask;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectListDialog"/>.
        /// </summary>
        public ProjectListDialog(string title = "Project List")
        {
            InitializeComponent();

            Title = title;

            Loaded += ProjectListDialog_Loaded;
            Closing += ProjectListDialog_Closing;
            _tokenSrc = new CancellationTokenSource();

            _autoRefreshTask = new Task(AutoRefreshAction, _tokenSrc.Token);

            Unloaded += ProjectListDialog_Unloaded;

            //Cancel the dialog if the esc key is pressed
            Keyboard.AddKeyUpHandler(this, KeyUpHandler);
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
            DependencyProperty.Register(nameof(ProjectNames), typeof(ProjectItem[]), typeof(ProjectListDialog), new PropertyMetadata(new ProjectItem[0]));

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
        protected ProjectItem[] ProjectNames
        {
            get { return (ProjectItem[])GetValue(ProjectNamesProperty); }
            set { SetValue(ProjectNamesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected project.
        /// </summary>
        public string SelectedProject { get; set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// Starts the auto refresh task.
        /// </summary>
        private void ProjectListDialog_Loaded(object sender, RoutedEventArgs e)
        {
            _autoRefreshTask.Start();
        }


        /// <summary>
        /// Unregisters the <see cref="KeyUpHandler(object, KeyEventArgs)"/> method.
        /// </summary>
        private void ProjectListDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            Keyboard.RemoveKeyUpHandler(this, KeyUpHandler);
        }


        /// <summary>
        /// Cancels the auto refresh task.
        /// </summary>
        private void ProjectListDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _tokenSrc.Cancel();
        }


        /// <summary>
        /// Closes the dialog window and accepts the selected project value.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }


        /// <summary>
        /// Cancels the dialog window and clears the selected project value.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedProject = string.Empty;

            DialogResult = false;

            Close();
        }


        /// <summary>
        /// Updates the selected project name.
        /// </summary>
        private void ProjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems.Count > 0 ? e.AddedItems[0] as ProjectItem : null;

            if (selectedItem == null)
                return;

            SelectedProject = selectedItem.Name;
        }


        /// <summary>
        /// Processes keyboard presses to add behavior to the dialog.
        /// </summary>
        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
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

            dialog.Refresh(newValue);
        }


        /// <summary>
        /// Invoked by the auto refresh task to refresh the UI of the user control
        /// at a specified interval.
        /// </summary>
        private void AutoRefreshAction()
        {
            //Keep refreshing as long as the task has not been cancelled.
            while (!_tokenSrc.IsCancellationRequested)
            {
                _tokenSrc.Token.WaitHandle.WaitOne(3000);

                Dispatcher.Invoke(() =>
                {
                    Refresh(ProjectPaths);
                });
            }
        }


        /// <summary>
        /// Refresh the UI based on the given list of <paramref name="projPaths"/>.
        /// </summary>
        /// <param name="projPaths">The list of project paths.</param>
        private void Refresh(string[] projPaths)
        {
            var paths = new List<ProjectItem>();

            foreach (var path in projPaths)
            {
                var sections = path.Split('\\');

                if (sections.Length > 0)
                {
                    var projName = sections[sections.Length - 1];
                    var pathExists = Directory.Exists(path);

                    paths.Add(new ProjectItem()
                    {
                        Name = projName,
                        Exists = pathExists
                    });
                }
            }

            var selectedIndex = ProjectListBox.SelectedIndex;

            ProjectNames = paths.ToArray();

            ProjectListBox.SelectedIndex = selectedIndex;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for <see cref="ProjectListDialog"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ProjectListDialog : Window
    {
        #region Private Fields
        private readonly CancellationTokenSource _tokenSrc;
        private readonly Task _autoRefreshTask;
        private bool _firstTimeRefreshed = true;
        private int _clickCount = 0;
        private DateTime _firstClickStamp;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectListDialog"/>.
        /// </summary>
        public ProjectListDialog(string title = "Project List")
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += (sender, e) =>
            {
                Keyboard.RemoveKeyUpHandler(this, KeyUpHandler);
                Mouse.RemoveMouseUpHandler(this, MouseUpHandler);
            };

            Title = title;

            Loaded += ProjectListDialog_Loaded;
            Closing += ProjectListDialog_Closing;
            _tokenSrc = new CancellationTokenSource();

            _autoRefreshTask = new Task(AutoRefreshAction, _tokenSrc.Token);

            Unloaded += ProjectListDialog_Unloaded;

            //Cancel the dialog if the esc key is pressed
            Keyboard.AddKeyUpHandler(this, KeyUpHandler);

            //Used to accept and item by double clicking it.
            Mouse.AddMouseUpHandler(this, MouseUpHandler);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the paths to all of the projects.
        /// </summary>
        public string[] ProjectPaths
        {
            get => (string[])GetValue(ProjectPathsProperty);
            set => SetValue(ProjectPathsProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="ProjectPaths"/> property.
        /// </summary>
        public static readonly DependencyProperty ProjectPathsProperty =
            DependencyProperty.Register(nameof(ProjectPaths), typeof(string[]), typeof(ProjectListDialog), new PropertyMetadata(new string[0], ProjectPathsChanged));


        /// <summary>
        /// Gets or sets the list of project names.
        /// </summary>
        protected ProjectItem[] ProjectNames
        {
            get => (ProjectItem[])GetValue(ProjectNamesProperty);
            set => SetValue(ProjectNamesProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="ProjectNames"/> property.
        /// </summary>
        protected static readonly DependencyProperty ProjectNamesProperty =
            DependencyProperty.Register(nameof(ProjectNames), typeof(ProjectItem[]), typeof(ProjectListDialog), new PropertyMetadata(new ProjectItem[0]));


        /// <summary>
        /// Gets or sets the selected project.
        /// </summary>
        public string SelectedProject { get; set; }

        /// <summary>
        /// Registers the <see cref="SelectedProject"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedProjectProperty =
            DependencyProperty.Register(nameof(SelectedProject), typeof(string), typeof(ProjectListDialog), new PropertyMetadata(""));
        #endregion


        #region Private Methods
        /// <summary>
        /// Starts the auto refresh task.
        /// </summary>
        private void ProjectListDialog_Loaded(object sender, RoutedEventArgs e) => _autoRefreshTask.Start();


        /// <summary>
        /// Unregisters the <see cref="KeyUpHandler(object, KeyEventArgs)"/> method.
        /// </summary>
        private void ProjectListDialog_Unloaded(object sender, RoutedEventArgs e) => Keyboard.RemoveKeyUpHandler(this, KeyUpHandler);


        /// <summary>
        /// Cancels the auto refresh task.
        /// </summary>
        private void ProjectListDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e) => _tokenSrc.Cancel();


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
        /// Processes keyboard key releases to add behavior to the dialog.
        /// </summary>
        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
            else if (e.Key == Key.Enter && ProjectListBox.SelectedItem != null)
            {
                DialogResult = true;
                Close();
            }
        }


        /// <summary>
        /// Detects double clicks of an item and if it has been double clicked, 
        /// accepts the item by closing the dialog with a result of true.
        /// </summary>
        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (ProjectListBox.SelectedItem == null)
                return;

            if (_clickCount <= 1 && e.LeftButton == MouseButtonState.Released)
            {
                _clickCount += 1;

                if (_clickCount == 1)
                    _firstClickStamp = DateTime.Now;
            }


            if (_clickCount >= 2)
            {
                var clickTiming = (DateTime.Now - _firstClickStamp).TotalMilliseconds;

                if (clickTiming <= 2000)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    _clickCount = 0;
                }
            }
        }


        /// <summary>
        /// Updates the project names list.
        /// </summary>
        private static void ProjectPathsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = (ProjectListDialog)d;

            if (dialog == null)
                return;
            if (!(e.NewValue is string[]))
                return;

            dialog.Refresh();
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
                    Refresh();
                });
            }
        }


        /// <summary>
        /// Refresh the UI based on the given list of <paramref name="projPaths"/>.
        /// </summary>
        private void Refresh()
        {
            var paths = ToProjectItems(ProjectPaths);

            //Check if any of the path items are not the same as the items in the list.
            //If any items are not the same, something has changed. Update the list.
            if (paths.Length != ProjectNames.Length)
            {
                ProjectNames = paths.ToArray();
            }
            else
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    if (!ProjectNames.Any(item => item.Equals(paths[i])))
                    {
                        ProjectNames = paths.ToArray();
                        break;
                    }
                }
            }

            if (_firstTimeRefreshed)
            {
                if (ProjectListBox.Items.Count > 0)
                    ProjectListBox.SelectedValue = ProjectListBox.Items[0];

                _firstTimeRefreshed = false;

                ProjectListBox.Focus();
            }
            else if(ProjectListBox.SelectedItem ==  null)
            {
                var selectedIndex = ProjectListBox.SelectedIndex;

                ProjectListBox.SelectedIndex = selectedIndex;
            }
        }


        /// <summary>
        /// Converts the given array of <paramref name="projPaths"/> to an array of <see cref="ProjectItem"/>s.
        /// </summary>
        /// <param name="projPaths">This list of items to convert.</param>
        /// <returns></returns>
        private ProjectItem[] ToProjectItems(string[] projPaths)
        {
            var paths = new List<ProjectItem>();

            foreach (var path in projPaths)
            {
                var sections = path.Split('\\');

                if (sections.Length > 0)
                {
                    var projName = sections[^1];
                    var pathExists = Directory.Exists(path);

                    paths.Add(new ProjectItem()
                    {
                        Name = projName,
                        Exists = pathExists
                    });
                }
            }


            return paths.ToArray();
        }
        #endregion
    }
}

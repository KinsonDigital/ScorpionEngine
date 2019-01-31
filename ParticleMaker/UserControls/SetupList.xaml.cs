using ParticleMaker.CustomEventArgs;
using ParticleMaker.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupList.xaml
    /// </summary>
    public partial class SetupList : UserControl, IDisposable
    {
        #region Public Events
        /// <summary>
        /// Invoked when the add setup button has been clicked.
        /// </summary>
        public event EventHandler<EventArgs> AddSetupClicked;

        /// <summary>
        /// Invoked when the refresh button has been clicked.
        /// </summary>
        public event EventHandler<EventArgs> RefreshClicked;

        /// <summary>
        /// Occurs when any item in the list has been renamed.
        /// </summary>
        public event EventHandler<RenameItemEventArgs> ItemRenamed;
        #endregion


        #region Fields
        private char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private Task _refreshTask;
        private CancellationTokenSource _refreshTaskTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupList"/>.
        /// </summary>
        public SetupList()
        {
            InitializeComponent();

            _refreshTaskTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTaskTokenSrc.Token);

            _refreshTask.Start();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="Setups"/> property.
        /// </summary>
        public static readonly DependencyProperty SetupsProperty =
            DependencyProperty.Register(nameof(Setups), typeof(PathItem[]), typeof(SetupList), new PropertyMetadata(new PathItem[0], SetupsChanged));
        #endregion

        /// <summary>
        /// Gets or sets the list of setup paths.
        /// </summary>
        public PathItem[] Setups
        {
            get { return (PathItem[])GetValue(SetupsProperty); }
            set { SetValue(SetupsProperty, value); }
        }

        /// <summary>
        /// Gets the selected setup.
        /// </summary>
        public PathItem SelectedSetup { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Finds the item that matches the given <paramref name="oldPath"/> and replaces it with
        /// the given <paramref name="newPath"/>.
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        public void UpdateItemPath(string oldPath, string newPath)
        {
            var updatedSetupList = (from p in Setups
                                    where p.FilePath != oldPath
                                    select p).ToList();

            updatedSetupList.Add(new PathItem() { FilePath = newPath });

            Setups = updatedSetupList.ToArray();
        }


        /// <summary>
        /// Refreshes the UI.
        /// </summary>
        /// <param name="ctrl">The control to apply the refresh to.</param>
        /// <param name="projPath">The path to the project.</param>
        public void Refresh()
        {
            var particleItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            //Refresh each setup list item
            foreach (var item in particleItems)
            {
                item.Refresh();
            }
        }


        /// <summary>
        /// Dispose of the control.
        /// </summary>
        public void Dispose()
        {
            _refreshTaskTokenSrc.Cancel();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Adds a new setup to the project.
        /// </summary>
        private void AddSetupButton_Click(object sender, EventArgs e)
        {
            AddSetupClicked?.Invoke(this, e);
        }


        /// <summary>
        /// Invokes the <see cref="ItemRenamed"/> event.
        /// </summary>
        private void ListBoxItems_RenameClicked(object sender, RenameItemEventArgs e)
        {
            var illegalNames = (from item in Setups select Path.GetFileNameWithoutExtension(item.FilePath)).ToArray();

            var inputDialog = new InputDialog("Rename particle", $"Rename the particle '{e.OldName}'.", e.OldName, _illegalCharacters, illegalNames);

            inputDialog.ShowDialog();

            if (inputDialog.DialogResult == true)
            {
                e.NewName = inputDialog.InputValue;
                e.NewPath = $@"{Path.GetDirectoryName(e.OldPath)}\{inputDialog.InputValue}{Path.GetExtension(e.OldPath)}";

                ItemRenamed?.Invoke(this, e);
            }
        }


        /// <summary>
        /// Refreshes the list.
        /// </summary>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshClicked?.Invoke(this, new EventArgs());

            Refresh();
        }


        /// <summary>
        /// Refreshes the list when the list of setup items change.
        /// </summary>
        private static void SetupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SetupList)d;

            if (ctrl == null)
                return;

            ctrl.SubscribeEvents();

            ctrl.Refresh();
        }


        /// <summary>
        /// Updates the currently selected item when the internal list box selectd item changes.
        /// </summary>
        private void SetupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SetupListBox.SelectedItem as PathItem;

            if (selectedItem == null)
                return;

            SelectedSetup = selectedItem;
        }


        /// <summary>
        /// Returns true if the given path exists.
        /// </summary>
        /// <param name="path">The path to check for.</param>
        /// <returns></returns>
        private static bool ProjectPathExists(string path)
        {
            return Directory.Exists(path);
        }


        /// <summary>
        /// Invokes the refresh method at a specified interval.
        /// </summary>
        private void RefreshAction()
        {
            while (!_refreshTaskTokenSrc.IsCancellationRequested)
            {
                _refreshTaskTokenSrc.Token.WaitHandle.WaitOne(1000);

                Dispatcher.Invoke(() =>
                {
                    Refresh();
                    SubscribeEvents();
                });
            }
        }


        /// <summary>
        /// Subscribes to all of the events
        /// </summary>
        private void SubscribeEvents()
        {
            var listItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            //Subsribe all of the events
            foreach (var item in listItems)
            {
                if (!item.IsRenameSubscribed)
                {
                    item.SubscribeRenameClicked(ListBoxItems_RenameClicked);
                }
            }
        }


        /// <summary>
        /// Unsubscribes to all of the events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            var listItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            //Unsubsribe all of the events
            foreach (var item in listItems)
            {
            }
        }
        #endregion
    }
}

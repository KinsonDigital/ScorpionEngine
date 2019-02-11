using ParticleMaker.CustomEventArgs;
using ParticleMaker.Dialogs;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupList.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class SetupList : UserControl, IDisposable
    {
        #region Public Events
        /// <summary>
        /// Occurs when any item in the list has been renamed.
        /// </summary>
        public event EventHandler<RenameSetupItemEventArgs> ItemRenamed;

        /// <summary>
        /// Occurs when any item in the list has been deleted.
        /// </summary>
        public event EventHandler<SetupItemEventArgs> ItemDeleted;
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

        /// <summary>
        /// Registers the <see cref="ItemSelectedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register(nameof(ItemSelectedCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="AddItemCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty commandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));
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

        /// <summary>
        /// Gets or sets the command to be executed when a list item has been selected.
        /// </summary>
        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to be executed when the add item button has been clicked.
        /// </summary>
        public ICommand AddItemCommand
        {
            get { return (ICommand)GetValue(commandProperty); }
            set { SetValue(commandProperty, value); }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given <paramref name="itemPath"/> to the list.
        /// </summary>
        /// <param name="itemPath">The item path to add.</param>
        public void AddItemPath(string itemPath)
        {
            var currentSetups = (from s in Setups select s).ToList();

            currentSetups.Add(new PathItem() { FilePath = itemPath });

            Setups = currentSetups.ToArray();
        }


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
        /// Removes the item from the list that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to remove.</param>
        public void RemoveItem(string name)
        {
            Setups = (from p in Setups
                      where Path.GetFileNameWithoutExtension(p.FilePath) != name
                      select p).ToArray();
        }


        /// <summary>
        /// Refreshes the UI.
        /// </summary>
        /// <param name="ctrl">The control to apply the refresh to.</param>
        /// <param name="projPath">The path to the project.</param>
        public void Refresh()
        {
            var setupListItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            //Refresh each setup list item
            foreach (var item in setupListItems)
            {
                item.Refresh();

                if (item.Command == null)
                    item.Command = new RelayCommand(ListItemCommandExecuteAction, (param) => true);
            }
        }

        
        /// <summary>
        /// Dispose of the control.
        /// </summary>
        public void Dispose()
        {
            _refreshTaskTokenSrc.Cancel();

            UnsubscribeEvents();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Adds a new setup to the project.
        /// </summary>
        private void AddSetupButton_Click(object sender, EventArgs e)
        {
            var invalidValues = Setups?.Select(s =>
            {
                var sections = s.FilePath.Split('\\');

                if (sections.Length > 0)
                    return Path.GetFileNameWithoutExtension(sections[sections.Length - 1]);


                return "";
            }).ToArray();

            if (invalidValues != null && invalidValues.All(item => string.IsNullOrEmpty(item)))
                invalidValues = null;

            var inputDialog = new InputDialog("Add Setup", "Please type new setup name.", invalidChars: _illegalCharacters, invalidValues: invalidValues);

            var dialogResult = inputDialog.ShowDialog();

            if (dialogResult == true)
            {
                AddItemCommand?.Execute($"{inputDialog.InputValue}");

                Refresh();
            }
        }


        /// <summary>
        /// Invokes the <see cref="ItemRenamed"/> event.
        /// </summary>
        private void ListBoxItems_RenameClicked(object sender, RenameSetupItemEventArgs e)
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
        /// Invokes the <see cref="ItemDeleted"/> event.
        /// </summary>
        private void ListBoxItems_DeleteClicked(object sender, SetupItemEventArgs e)
        {
            var msg = $"Are you sure you want to delete the setup {e.Name}?";

            var dialogResult = MessageBox.Show(msg, "Delete Setup", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
            {
                ItemDeleted?.Invoke(this, e);
            }
        }


        /// <summary>
        /// Refreshes the list when the list of setup items change.
        /// </summary>
        private static void SetupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SetupList)d;

            if (ctrl == null)
                return;

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
        /// The action method to be invoked when a list item command has 
        /// been executed.
        /// </summary>
        /// <param name="param">The data to pass to the <see cref="SetupList"/> command.</param>
        private void ListItemCommandExecuteAction(object param)
        {
            ItemSelectedCommand?.Execute(param);
        }


        /// <summary>
        /// Subscribes to all of the events.
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
                    item.SubscribeDeleteClicked(ListBoxItems_DeleteClicked);
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
                item.UnsubscribeRenameClicked(ListBoxItems_RenameClicked);
                item.UnsubscribeDeleteClicked(ListBoxItems_DeleteClicked);
            }
        }
        #endregion
    }
}

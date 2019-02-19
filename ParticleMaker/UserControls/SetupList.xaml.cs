using ParticleMaker.CustomEventArgs;
using ParticleMaker.Dialogs;
using ParticleMaker.Exceptions;
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
    public partial class SetupList : UserControl
    {
        #region Fields
        private char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private Task _refreshTask;
        private CancellationTokenSource _refreshTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupList"/>.
        /// </summary>
        public SetupList()
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += (sender, e) => { _refreshTokenSrc.Cancel(); };

            _refreshTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTokenSrc.Token);

            _refreshTask.Start();

            Refresh();
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
        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="ItemRenamedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemRenamedCommandProperty =
            DependencyProperty.Register(nameof(ItemRenamedCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="ItemDeletedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemDeletedCommandProperty =
            DependencyProperty.Register(nameof(ItemDeletedCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="ItemSavedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemSavedCommandProperty =
            DependencyProperty.Register(nameof(ItemSavedCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));
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
        public PathItem SelectedItem { get; private set; }

        /// <summary>
        /// Gets or sets the command to be executed when a list item has been selected.
        /// </summary>
        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to be executed when a list item add button has been clicked.
        /// </summary>
        public ICommand AddItemCommand
        {
            get { return (ICommand)GetValue(AddItemCommandProperty); }
            set { SetValue(AddItemCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command that is executed when a list item rename button has been clicked.
        /// </summary>
        public ICommand ItemRenamedCommand
        {
            get { return (ICommand)GetValue(ItemRenamedCommandProperty); }
            set { SetValue(ItemRenamedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to be executed when a list item delete button has been clicked.
        /// </summary>
        public ICommand ItemDeletedCommand
        {
            get { return (ICommand)GetValue(ItemDeletedCommandProperty); }
            set { SetValue(ItemDeletedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to be executed when a list item save button has been clicked.
        /// </summary>
        public ICommand ItemSavedCommand
        {
            get { return (ICommand)GetValue(ItemSavedCommandProperty); }
            set { SetValue(ItemSavedCommandProperty, value); }
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
        /// Refreshes the UI.
        /// </summary>
        /// <param name="ctrl">The control to apply the refresh to.</param>
        /// <param name="projPath">The path to the project.</param>
        public void Refresh()
        {
            if (_refreshTokenSrc.IsCancellationRequested)
                return;

            var setupListItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            //Refresh each setup list item
            foreach (var item in setupListItems)
            {
                item.Refresh();

                if (item.Command == null)
                    item.Command = new RelayCommand((param) => ItemSelectedCommand?.Execute(param), (param) => true);
            }
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

            var inputDialog = new InputDialog("Add Setup", "Please type new setup name.", invalidChars: _illegalCharacters, invalidValues: invalidValues)
            {
                Owner = this.FindParent<Window>()
            };

            var dialogResult = inputDialog.ShowDialog();

            if (dialogResult == true)
            {
                AddItemCommand?.Execute($"{inputDialog.InputValue}");

                Refresh();
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
            if (!(SetupListBox.SelectedItem is PathItem selectedItem))
                return;

            SelectedItem = selectedItem;
        }


        /// <summary>
        /// Invokes the refresh method at a specified interval.
        /// </summary>
        private void RefreshAction()
        {
            while (!_refreshTokenSrc.IsCancellationRequested)
            {
                _refreshTokenSrc.Token.WaitHandle.WaitOne(1000);

                if (Dispatcher.HasShutdownFinished || _refreshTokenSrc.IsCancellationRequested)
                    break;

                Dispatcher.Invoke(() =>
                {
                    Refresh();
                    SetupCommands();
                });
            }
        }


        /// <summary>
        /// Sets up all of the commands for the list items.
        /// </summary>
        private void SetupCommands()
        {
            var listItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            foreach (var item in listItems)
            {
                if (item.RenameClickedCommand == null)
                    item.RenameClickedCommand = new RelayCommand(RenameCommandAction, (param) => true);

                if (item.DeleteClickedCommand == null)
                    item.DeleteClickedCommand = new RelayCommand(DeleteCommandAction, (param) => true);

                if (item.SaveClickedCommand == null)
                    item.SaveClickedCommand = new RelayCommand(SaveCommandAction, (param) => true);
            }
        }


        /// <summary>
        /// Destroys all of the list item commands.
        /// </summary>
        private void DestroyCommands()
        {
            var listItems = SetupListBox.FindVisualChildren<SetupListItem>().ToArray();

            foreach (var item in listItems)
            {
                item.RenameClickedCommand = null;
                item.DeleteClickedCommand = null;
                item.SaveClickedCommand = null;
            }
        }


        /// <summary>
        /// The method to execute when a list item rename button has been clicked.
        /// </summary>
        /// <param name="param">The rename related data.</param>
        private void RenameCommandAction(object param)
        {
            if (!(param is RenameItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(RenameCommandAction), nameof(param));

            var illegalNames = (from setup in Setups select Path.GetFileNameWithoutExtension(setup.FilePath)).ToArray();

            var inputDialog = new InputDialog("Rename setup", $"Rename the setup '{eventArgs.OldName}'.", eventArgs.OldName, _illegalCharacters, illegalNames)
            {
                Owner = this.FindParent<Window>()
            };

            inputDialog.ShowDialog();

            if (inputDialog.DialogResult == true)
            {
                eventArgs.NewName = inputDialog.InputValue;
                eventArgs.NewPath = $@"{Path.GetDirectoryName(eventArgs.OldPath)}\{inputDialog.InputValue}{Path.GetExtension(eventArgs.OldPath)}";

                ItemRenamedCommand?.Execute(param);
            }
        }


        /// <summary>
        /// The method to execute when a list item delete button has been clicked.
        /// </summary>
        /// <param name="param">The setup item related data.</param>
        private void DeleteCommandAction(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(DeleteCommandAction), nameof(param));

            var msg = $"Are you sure you want to delete the setup named '{eventArgs.Name}'?";

            var dialogResult = MessageBox.Show(msg, "Delete Setup", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
                ItemDeletedCommand?.Execute(eventArgs);
        }


        /// <summary>
        /// The method to execute when a list item save button has been clicked.
        /// </summary>
        /// <param name="param">The setup item related data.</param>
        private void SaveCommandAction(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(SaveCommandAction), nameof(param));

            ItemSavedCommand?.Execute(eventArgs);
        }
        #endregion
    }
}

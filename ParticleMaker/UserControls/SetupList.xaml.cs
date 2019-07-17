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
    /// Interaction logic for <see cref="SetupList"/> control.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class SetupList : UserControl
    {
        #region Private Fields
        private readonly char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private readonly Task _refreshTask;
        private readonly CancellationTokenSource _refreshTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupList"/>.
        /// </summary>
        public SetupList()
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += (sender, e) => 
            {
                _refreshTokenSrc.Cancel();
                Keyboard.RemoveKeyUpHandler(this, KeyUpHandler);
            };

            _refreshTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTokenSrc.Token);

            _refreshTask.Start();

            Refresh();

            Keyboard.AddKeyUpHandler(this, KeyUpHandler);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the selected setup.
        /// </summary>
        public PathItem SelectedItem { get; private set; }


        /// <summary>
        /// Gets or sets the list of setup paths.
        /// </summary>
        public PathItem[] Setups
        {
            get => (PathItem[])GetValue(SetupsProperty);
            set => SetValue(SetupsProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Setups"/> property.
        /// </summary>
        public static readonly DependencyProperty SetupsProperty =
            DependencyProperty.Register(nameof(Setups), typeof(PathItem[]), typeof(SetupList), new PropertyMetadata(new PathItem[0], SetupsChanged));
        

        /// <summary>
        /// Gets or sets the command to be executed when a list item has been selected.
        /// </summary>
        public ICommand ItemSelectedCommand
        {
            get => (ICommand)GetValue(ItemSelectedCommandProperty);
            set => SetValue(ItemSelectedCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="ItemSelectedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register(nameof(ItemSelectedCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command to be executed when a list item add button has been clicked.
        /// </summary>
        public ICommand AddItemCommand
        {
            get => (ICommand)GetValue(AddItemCommandProperty);
            set => SetValue(AddItemCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="AddItemCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command that is executed when a list item rename button has been clicked.
        /// </summary>
        public ICommand RenameItemCommand
        {
            get => (ICommand)GetValue(RenameItemCommandProperty);
            set => SetValue(RenameItemCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="RenameItemCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty RenameItemCommandProperty =
            DependencyProperty.Register(nameof(RenameItemCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command to be executed when a list item delete button has been clicked.
        /// </summary>
        public ICommand DeleteItemCommand
        {
            get => (ICommand)GetValue(DeleteItemCommandProperty);
            set => SetValue(DeleteItemCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="DeleteItemCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty DeleteItemCommandProperty =
            DependencyProperty.Register(nameof(DeleteItemCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command to be executed when a list item save button has been clicked.
        /// </summary>
        public ICommand SaveItemCommand
        {
            get => (ICommand)GetValue(SaveItemCommandProperty);
            set => SetValue(SaveItemCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="SaveItemCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty SaveItemCommandProperty =
            DependencyProperty.Register(nameof(SaveItemCommand), typeof(ICommand), typeof(SetupList), new PropertyMetadata(null));
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
            setupListItems.ToList().ForEach(i =>
            {
                i.Refresh();

                if (i.Command == null)
                    i.Command = new RelayCommand((param) => ItemSelectedCommand?.Execute(param), (param) => true);
            });
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
                    return Path.GetFileNameWithoutExtension(sections[^1]);


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
        /// Updates the currently selected item when the internal list box selected item changes.
        /// </summary>
        private void SetupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(SetupListBox.SelectedItem is PathItem selectedItem))
                return;

            SelectedItem = selectedItem;
        }


        /// <summary>
        /// Processes keys to dictate the behavior of the <see cref="SetupList"/> control.
        /// </summary>
        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Up && e.Key != Key.Down)
                return;

            var itemSections = SelectedItem.FilePath.Split('\\');

            if (itemSections.Length >= 1)
                ItemSelectedCommand?.Execute(itemSections[^1]);
        }


        /// <summary>
        /// Invokes the refresh method at a specified interval.
        /// </summary>
        private void RefreshAction()
        {
            while (!_refreshTokenSrc.IsCancellationRequested)
            {
                _refreshTokenSrc.Token.WaitHandle.WaitOne(2000);

                if (App.IsShuttingDown)
                    _refreshTokenSrc.Cancel();

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

            listItems.ToList().ForEach(i =>
            {
                if (i.RenameClickedCommand == null)
                    i.RenameClickedCommand = new RelayCommand(RenameCommandExecute, (param) => true);

                if (i.DeleteClickedCommand == null)
                    i.DeleteClickedCommand = new RelayCommand(DeleteCommandExecute, (param) => true);

                if (i.SaveClickedCommand == null)
                    i.SaveClickedCommand = new RelayCommand(SaveCommandExecute, (param) => true);
            });
        }


        /// <summary>
        /// The method to execute when a list item rename button has been clicked.
        /// </summary>
        /// <param name="param">The rename related data.</param>
        private void RenameCommandExecute(object param)
        {
            if (!(param is RenameItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(RenameCommandExecute), nameof(param));

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

                RenameItemCommand?.Execute(param);
            }
        }


        /// <summary>
        /// The method to execute when a list item delete button has been clicked.
        /// </summary>
        /// <param name="param">The setup item related data.</param>
        private void DeleteCommandExecute(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(DeleteCommandExecute), nameof(param));

            DeleteItemCommand?.Execute(eventArgs);
        }


        /// <summary>
        /// The method to execute when a list item save button has been clicked.
        /// </summary>
        /// <param name="param">The setup item related data.</param>
        private void SaveCommandExecute(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(SaveCommandExecute), nameof(param));

            SaveItemCommand?.Execute(eventArgs);
        }
        #endregion
    }
}

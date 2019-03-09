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
    /// Interaction logic for ParticleList.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ParticleList : UserControl
    {
        #region Public Events
        /// <summary>
        /// Occurs when any item in the list has been renamed.
        /// </summary>
        public event EventHandler<RenameItemEventArgs> ItemRenamed;

        /// <summary>
        /// Occurs when any item in the list has been deleted.
        /// </summary>
        public event EventHandler<ItemEventArgs> ItemDeleted;
        #endregion


        #region Fields
        private char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private Task _refreshTask;
        private CancellationTokenSource _refreshTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleList"/>.
        /// </summary>
        public ParticleList()
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += (sender, e) => { _refreshTokenSrc.Cancel(); };

            _refreshTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTokenSrc.Token);

            _refreshTask.Start();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="Particles"/> property.
        /// </summary>
        public static readonly DependencyProperty ParticlesProperty =
            DependencyProperty.Register(nameof(Particles), typeof(PathItem[]), typeof(ParticleList), new PropertyMetadata(new PathItem[0], ParticlesChanged));

        /// <summary>
        /// Registers the <see cref="ItemRenamedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemRenamedCommandProperty =
            DependencyProperty.Register(nameof(ItemRenamedCommand), typeof(ICommand), typeof(ParticleList), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="ItemDeletedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemDeletedCommandProperty =
            DependencyProperty.Register(nameof(ItemDeletedCommand), typeof(ICommand), typeof(ParticleList), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="AddItemCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(ParticleList), new PropertyMetadata(null));
        #endregion


        /// <summary>
        /// Gets or sets the list of particle paths.
        /// </summary>
        public PathItem[] Particles
        {
            get { return (PathItem[])GetValue(ParticlesProperty); }
            set { SetValue(ParticlesProperty, value); }
        }

        /// <summary>
        /// Gets the selected particle item in the list.
        /// </summary>
        public PathItem SelectedItem { get; private set; }

        /// <summary>
        /// Gets or sets the command that is executed when a list item rename button has been clicked.
        /// </summary>
        public ICommand ItemRenamedCommand
        {
            get { return (ICommand)GetValue(ItemRenamedCommandProperty); }
            set { SetValue(ItemRenamedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command that is executed when a list item delete button has been clicked.
        /// </summary>
        public ICommand ItemDeletedCommand
        {
            get { return (ICommand)GetValue(ItemDeletedCommandProperty); }
            set { SetValue(ItemDeletedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command that is executed when the add item button is clicked.
        /// </summary>
        public ICommand AddItemCommand
        {
            get { return (ICommand)GetValue(AddItemCommandProperty); }
            set { SetValue(AddItemCommandProperty, value); }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given <paramref name="itemPath"/> to the list.
        /// </summary>
        /// <param name="itemPath">The item path to add.</param>
        public void AddItemPath(string itemPath)
        {
            var currentParticles = (from s in Particles select s).ToList();

            currentParticles.Add(new PathItem() { FilePath = itemPath });

            Particles = currentParticles.ToArray();
        }


        /// <summary>
        /// Refreshes the UI based on the state of the user control.
        /// </summary>
        public void Refresh()
        {
            if (_refreshTokenSrc.IsCancellationRequested)
                return;

            var particleItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            //Refresh each particle list item
            foreach (var item in particleItems)
            {
                item.Refresh();
            }

            SetupCommands();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Invokes the <see cref="ItemDeleted"/> event.
        /// </summary>
        private void ListBoxItems_DeleteClicked(object sender, ItemEventArgs e)
        {
            var msg = $"Are you sure you want to delete the particle {e.Name}?";

            var dialogResult = MessageBox.Show(msg, "Delete Particle", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
            {
                ItemDeleted?.Invoke(this, e);
            }
        }


        /// <summary>
        /// Adds a new particle to the list.
        /// </summary>
        private void AddParticleButton_Click(object sender, EventArgs e)
        {
            var invalidValues = Particles.Select(s =>
            {
                var sections = s.FilePath.Split('\\');

                if (sections.Length > 0)
                    return Path.GetFileNameWithoutExtension(sections[sections.Length - 1]);


                return "";
            }).ToArray();

            if (invalidValues.All(item => string.IsNullOrEmpty(item)))
                invalidValues = null;

            var inputDialog = new InputDialog("Add Particle", "Please type new particle name.", invalidChars: _illegalCharacters, invalidValues: invalidValues)
            {
                Owner = this.FindParent<Window>(),
                IgnoreInvalidValueCasing = true
            };
            
            var dialogResult = inputDialog.ShowDialog();

            if (dialogResult == true)
            {
                AddItemCommand?.Execute(new AddItemClickedEventArgs($"{inputDialog.InputValue}.png"));

                Refresh();
            }
        }


        /// <summary>
        /// Sets the selected item property based on the list box selection.
        /// </summary>
        private void ParticleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(ParticleListBox.SelectedItem is PathItem selectedItem))
                return;

            SelectedItem = selectedItem;
        }


        /// <summary>
        /// Refreshes the list when the list of particle path items change.
        /// </summary>
        private static void ParticlesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ParticleList)d;

            if (ctrl == null)
                return;

            ctrl.Refresh();
        }


        /// <summary>
        /// Invokes the refresh method at a specified interval.
        /// </summary>
        private void RefreshAction()
        {
            while (!_refreshTokenSrc.IsCancellationRequested)
            {
                _refreshTokenSrc.Token.WaitHandle.WaitOne(2000);

                if (Dispatcher.HasShutdownFinished || _refreshTokenSrc.IsCancellationRequested)
                    break;

                Dispatcher.Invoke(() =>
                {
                    Refresh();
                });
            }
        }


        /// <summary>
        /// Sets up all of the commands for the list items.
        /// </summary>
        private void SetupCommands()
        {
            var listItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            foreach (var item in listItems)
            {
                if (item.RenameClickedCommand == null)
                    item.RenameClickedCommand = new RelayCommand(RenameCommandAction, (param) => true);

                if (item.DeleteClickedCommand == null)
                    item.DeleteClickedCommand = new RelayCommand(DeleteCommandAction, (param) => true);
            }
        }


        /// <summary>
        /// Destroys all of the list item commands.
        /// </summary>
        private void DestroyCommands()
        {
            var listItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            foreach (var item in listItems)
            {
                item.RenameClickedCommand = null;
                item.DeleteClickedCommand = null;
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

            var illegalNames = (from particle in Particles select Path.GetFileNameWithoutExtension(particle.FilePath)).ToArray();

            var inputDialog = new InputDialog("Rename particle", $"Rename the particle '{eventArgs.OldName}'.", eventArgs.OldName, _illegalCharacters, illegalNames)
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

            var msg = $"Are you sure you want to delete the particle named '{eventArgs.Name}'?";

            var dialogResult = MessageBox.Show(msg, "Delete Particle", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.Yes)
                ItemDeletedCommand?.Execute(eventArgs);
        }
        #endregion
    }
}

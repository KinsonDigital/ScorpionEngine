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
        #region Fields
        private readonly char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private readonly Task _refreshTask;
        private readonly CancellationTokenSource _refreshTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleList"/>.
        /// </summary>
        public ParticleList()
        {
            InitializeComponent();

            Dispatcher.ShutdownStarted += (sender, e) => _refreshTokenSrc.Cancel();

            _refreshTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTokenSrc.Token);

            _refreshTask.Start();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the selected particle item in the list.
        /// </summary>
        public PathItem SelectedItem { get; private set; }
        #endregion


        #region Dependency Props
        /// <summary>
        /// Gets or sets the list of particle paths.
        /// </summary>
        public PathItem[] Particles
        {
            get => (PathItem[])GetValue(ParticlesProperty);
            set => SetValue(ParticlesProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Particles"/> property.
        /// </summary>
        public static readonly DependencyProperty ParticlesProperty =
            DependencyProperty.Register(nameof(Particles), typeof(PathItem[]), typeof(ParticleList), new PropertyMetadata(new PathItem[0], ParticlesChanged));


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
            DependencyProperty.Register(nameof(RenameItemCommand), typeof(ICommand), typeof(ParticleList), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command that is executed when a list item delete button has been clicked.
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
            DependencyProperty.Register(nameof(DeleteItemCommand), typeof(ICommand), typeof(ParticleList), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command that is executed when the add item button is clicked.
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
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(ParticleList), new PropertyMetadata(null));
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
        /// Adds a new particle to the list.
        /// </summary>
        private void AddParticleButton_Click(object sender, EventArgs e)
        {
            AddItemCommand?.Execute(null);
            Refresh();
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

                if (App.IsShuttingDown)
                    _refreshTokenSrc.Cancel();

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
                    item.RenameClickedCommand = new RelayCommand(RenameItemCommandExecute, (param) => true);

                if (item.DeleteClickedCommand == null)
                    item.DeleteClickedCommand = new RelayCommand(DeleteItemCommandExecute, (param) => true);
            }
        }


        /// <summary>
        /// The method to execute when a list item rename button has been clicked.
        /// </summary>
        /// <param name="param">The rename related data.</param>
        private void RenameItemCommandExecute(object param)
        {
            if (!(param is RenameItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(RenameItemCommandExecute), nameof(param));

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

                RenameItemCommand?.Execute(param);
            }
        }


        /// <summary>
        /// The method to execute when a list item delete button has been clicked.
        /// </summary>
        /// <param name="param">The setup item related data.</param>
        private void DeleteItemCommandExecute(object param)
        {
            if (!(param is ItemEventArgs eventArgs))
                throw new InvalidCommandActionParamTypeException(nameof(DeleteItemCommandExecute), nameof(param));

            DeleteItemCommand?.Execute(eventArgs);
        }
        #endregion
    }
}

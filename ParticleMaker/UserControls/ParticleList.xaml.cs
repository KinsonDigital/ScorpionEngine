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
    /// Interaction logic for ParticleList.xaml
    /// </summary>
    public partial class ParticleList : UserControl, IDisposable
    {
        #region Public Events
        /// <summary>
        /// Occurs when then add particle button is clicked.
        /// </summary>
        public event EventHandler<AddParticleClickedEventArgs> AddParticleClicked;

        /// <summary>
        /// Invoked when the refresh button has been clicked.
        /// </summary>
        public event EventHandler<EventArgs> RefreshClicked;

        /// <summary>
        /// Occurs when any item in the list has been renamed.
        /// </summary>
        public event EventHandler<RenameParticleEventArgs> ItemRenamed;
        #endregion


        #region Fields
        private char[] _illegalCharacters = new[] { '\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.' };
        private Task _refreshTask;
        private CancellationTokenSource _refreshTaskTokenSrc;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleList"/>.
        /// </summary>
        public ParticleList()
        {
            InitializeComponent();

            Loaded += ParticleList_Loaded;

            _refreshTaskTokenSrc = new CancellationTokenSource();

            _refreshTask = new Task(RefreshAction, _refreshTaskTokenSrc.Token);

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
        #endregion


        #region Public Methods
        /// <summary>
        /// Finds the item that matches the given <paramref name="name"/> and updates its path to
        /// the given <paramref name="path"/>.
        /// </summary>
        /// <param name="name">The name of the item to update.</param>
        /// <param name="path">The new path to set the found item to.</param>
        public void UpdateItemPath(string name, string path)
        {
            var particleItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            foreach (var item in particleItems)
            {
                if (item.ParticleName == name)
                {
                    item.ParticleFilePath = path;
                    break;
                }
            }
        }


        /// <summary>
        /// Refreshes the UI based on the state of the user control.
        /// </summary>
        public void Refresh()
        {
            var particleItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            //Refresh each particle list item
            foreach (var item in particleItems)
            {
                item.Refresh();
            }

            SubscribeEvents();
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
        /// Subscribes all of the events for each listbox item.
        /// </summary>
        private void ParticleList_Loaded(object sender, RoutedEventArgs e)
        {
            SubscribeEvents();
        }


        /// <summary>
        /// Invokes the <see cref="ItemRenamed"/> event.
        /// </summary>
        private void ListBoxItems_RenameClicked(object sender, RenameParticleEventArgs e)
        {
            var illegalNames = (from item in Particles select Path.GetFileNameWithoutExtension(item.FilePath)).ToArray();

            var inputDialog = new InputDialog("Rename particle", $"Rename the particle '{e.OldParticleName}'.", e.OldParticleName, _illegalCharacters, illegalNames);

            inputDialog.ShowDialog();

            if (inputDialog.DialogResult == true)
            {
                e.NewParticleName = inputDialog.InputValue;
                e.NewParticleFilePath = $@"{Path.GetDirectoryName(e.OldParticleFilePath)}\{inputDialog.InputValue}{Path.GetExtension(e.OldParticleFilePath)}";

                ItemRenamed?.Invoke(this, e);
            }
        }


        /// <summary>
        /// Adds a new particle to the list.
        /// </summary>
        private void AddParticleButton_Click(object sender, EventArgs e)
        {
            //TODO: Add particle path to the event args constructor below
            AddParticleClicked?.Invoke(this, new AddParticleClickedEventArgs(""));

            //TODO: Check if the newly added item has its rename event subscribed to
            Refresh();
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
        /// Sets the selected item property based on the list box selection.
        /// </summary>
        private void ParticleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ParticleListBox.SelectedItem as PathItem;

            if (selectedItem == null)
                return;

            //TODO: Set the selected item as the current selected item of the internal list box
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
            while (!_refreshTaskTokenSrc.IsCancellationRequested)
            {
                _refreshTaskTokenSrc.Token.WaitHandle.WaitOne(4000);

                Dispatcher.Invoke(() =>
                {
                    Refresh();
                });
            }
        }


        /// <summary>
        /// Subscribes to all of the events
        /// </summary>
        private void SubscribeEvents()
        {
            var listItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

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
            var listItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            //Unsubsribe all of the events
            foreach (var item in listItems)
            {
                item.UnsubscribeRenameClicked(ListBoxItems_RenameClicked);
            }
        }
        #endregion
    }
}

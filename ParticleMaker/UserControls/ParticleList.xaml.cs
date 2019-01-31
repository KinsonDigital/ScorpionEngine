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
        public event EventHandler<RenameItemEventArgs> ItemRenamed;

        /// <summary>
        /// Occurs when any item in the list has been deleted.
        /// </summary>
        public event EventHandler<DeleteItemEventArgs> ItemDeleted;
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
        /// Finds the item that matches the given <paramref name="oldPath"/> and replaces it with
        /// the given <paramref name="newPath"/>.
        /// </summary>
        /// <param name="oldPath">The old path.</param>
        /// <param name="newPath">The new path.</param>
        public void UpdateItemPath(string oldPath, string newPath)
        {
            var updatedSetupList = (from p in Particles
                                    where p.FilePath != oldPath
                                    select p).ToList();

            updatedSetupList.Add(new PathItem() { FilePath = newPath });

            Particles = updatedSetupList.ToArray();
        }


        /// <summary>
        /// Removes the item from the lsit that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the item to remove.</param>
        public void RemoveItem(string name)
        {
            Particles = (from p in Particles
                         where Path.GetFileNameWithoutExtension(p.FilePath) != name
                         select p).ToArray();
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
        /// Invokes the <see cref="ItemRenamed"/> event.
        /// </summary>
        private void ListBoxItems_RenameClicked(object sender, RenameItemEventArgs e)
        {
            var illegalNames = (from item in Particles select Path.GetFileNameWithoutExtension(item.FilePath)).ToArray();

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
        private void ListBoxItems_DeleteClicked(object sender, DeleteItemEventArgs e)
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
            var listItems = ParticleListBox.FindVisualChildren<ParticleListItem>().ToArray();

            //Subsribe all of the events
            foreach (var item in listItems)
            {
                if (!item.IsRenameSubscribed)
                {
                    item.SubscribeRenameClicked(ListBoxItems_RenameClicked);
                }

                if (!item.IsDeleteSubscribed)
                {
                    item.SubscribeDeleteClicked(ListBoxItems_DeleteClicked);
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
                item.UnsubscribeDeleteClicked(ListBoxItems_DeleteClicked);
            }
        }
        #endregion
    }
}

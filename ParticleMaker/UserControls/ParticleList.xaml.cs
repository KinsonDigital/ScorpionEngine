using ParticleMaker.CustomEventArgs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ParticleList.xaml
    /// </summary>
    public partial class ParticleList : UserControl
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
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleList"/>.
        /// </summary>
        public ParticleList()
        {
            InitializeComponent();
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


        #region Private Methods
        /// <summary>
        /// Adds a new particle to the list.
        /// </summary>
        private void AddParticleButton_Click(object sender, EventArgs e)
        {
            //TODO: Add particle path to the event args constructor below
            AddParticleClicked?.Invoke(this, new AddParticleClickedEventArgs(""));
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
        /// Refreshes the UI based on the state of the user control.
        /// </summary>
        private void Refresh()
        {
            var total = ParticleListBox.Items.Count;

            //TODO: This isn't working.  Remove this code
            foreach (var item in ParticleListBox.Items)
            {
                if (!(item is ParticleListItem listItem))
                    continue;

                listItem.Refresh();
            }
        }
        #endregion
    }
}

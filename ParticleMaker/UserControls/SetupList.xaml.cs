using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupList.xaml
    /// </summary>
    public partial class SetupList : UserControl
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
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupList"/>.
        /// </summary>
        public SetupList()
        {
            InitializeComponent();
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
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        protected static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(SetupList), new PropertyMetadata(false));
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
        /// Gets or sets a value indicating if the user control has an error.
        /// </summary>
        protected bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        /// <summary>
        /// Gets the selected setup.
        /// </summary>
        public PathItem SelectedSetup { get; private set; }
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

            ctrl.Refresh();
        }


        /// <summary>
        /// Updates the currently selected item when the internal list box selectd item changes.
        /// </summary>
        private void SetupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var myType = sender.GetType();
            var originalSrcType = e.OriginalSource.GetType();
            var srcType = e.Source.GetType();

            var selectedItem = SetupListBox.SelectedItem as PathItem;

            if (selectedItem == null)
                return;

            SelectedSetup = selectedItem;
        }


        /// <summary>
        /// Refreshes the UI.
        /// </summary>
        /// <param name="ctrl">The control to apply the refresh to.</param>
        /// <param name="projPath">The path to the project.</param>
        private void Refresh()
        {
            foreach (var item in SetupListBox.Items)
            {
                if (!(item is SetupListItem listItem))
                    continue;

                listItem.Refresh(listItem);
            }
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
        #endregion
    }
}

using ParticleMaker.CustomEventArgs;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupListItem.xaml
    /// </summary>
    public partial class SetupListItem : UserControl
    {
        #region Public Events
        /// <summary>
        /// Invoked when the rename button has been clicked.
        /// </summary>
        public event EventHandler<RenameItemEventArgs> RenameClicked;

        /// <summary>
        /// Invoked when the delete button is clicked.
        /// </summary>
        public event EventHandler<DeleteItemEventArgs> DeleteClicked;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupListItem"/>.
        /// </summary>
        public SetupListItem()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="SetupPath"/>.
        /// </summary>
        public static readonly DependencyProperty SetupPathProperty =
            DependencyProperty.Register(nameof(SetupPath), typeof(string), typeof(SetupListItem), new PropertyMetadata("", SetupFilePathChanged));

        /// <summary>
        /// Registers the <see cref="SetupName"/> property.
        /// </summary>
        public static readonly DependencyProperty SetupNameProperty =
            DependencyProperty.Register(nameof(SetupName), typeof(string), typeof(SetupListItem), new PropertyMetadata(""));

        /// <summary>
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        protected static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(SetupListItem), new PropertyMetadata(false));
        #endregion


        /// <summary>
        /// Gets or sets the path to the setup file.
        /// Must be a full path with file name.
        /// </summary>
        public string SetupPath
        {
            get { return (string)GetValue(SetupPathProperty); }
            set { SetValue(SetupPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the setup name to be shown in the setup label.
        /// </summary>
        public string SetupName
        {
            get { return (string)GetValue(SetupNameProperty); }
            set { SetValue(SetupNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating if the control has an error.
        /// </summary>
        protected bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating if the rename event has been subscribed to.
        /// </summary>
        internal bool IsRenameSubscribed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the delete event has been subscribed to.
        /// </summary>
        internal bool IsDeleteSubscribed { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Refreshs the control by updating the control's UI based on if the file exists or not.
        /// </summary>
        public void Refresh()
        {
            var fileExists = File.Exists(SetupPath);

            if (DesignerProperties.GetIsInDesignMode(this) && fileExists)
            {
                SetupName = Path.GetFileNameWithoutExtension(SetupPath);
                HasError = false;
            }
            else
            {
                SetupName = string.IsNullOrEmpty(SetupPath) ||
                            !fileExists ?
                            "" :
                            SetupName = Path.GetFileNameWithoutExtension(SetupPath);

                HasError = !fileExists;
            }
        }
        #endregion


        #region Internal Methods
        /// <summary>
        /// Subscribes the given handler to the rename clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void SubscribeRenameClicked(EventHandler<RenameItemEventArgs> handler)
        {
            RenameClicked += handler;

            IsRenameSubscribed = true;
        }


        /// <summary>
        /// Subscribes the given handler to the delete clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void SubscribeDeleteClicked(EventHandler<DeleteItemEventArgs> handler)
        {
            DeleteClicked += handler;

            IsDeleteSubscribed = true;
        }


        /// <summary>
        /// Unsubscribes the given handler to the delete clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void UnsubscribeDeleteClicked(EventHandler<DeleteItemEventArgs> handler)
        {
            DeleteClicked -= handler;

            IsDeleteSubscribed = false;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the setup name to be the file name without the extension
        /// </summary>
        private static void SetupFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (string)e.NewValue;

            if (string.IsNullOrEmpty(newValue))
                return;

            var ctrl = (SetupListItem)d;

            if (ctrl == null)
                return;

            ctrl.Refresh();
        }


        /// <summary>
        /// Renames the file at the currently set path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameCustomButton_Click(object sender, EventArgs e)
        {
            RenameClicked?.Invoke(this, new RenameItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Deletes the file at the given path.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, new DeleteItemEventArgs(SetupName, SetupPath));

            Refresh();
        }
        #endregion
    }
}

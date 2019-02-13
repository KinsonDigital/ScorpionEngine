using ParticleMaker.CustomEventArgs;
using ParticleMaker.Dialogs;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ParticleListItem.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ParticleListItem : UserControl
    {
        #region Public Events
        /// <summary>
        /// Invoked when the rename button has been clicked.
        /// </summary>
        public event EventHandler<RenameSetupItemEventArgs> RenameClicked;

        /// <summary>
        /// Invoked when the delete button has been clicked.
        /// </summary>
        public event EventHandler<SetupItemEventArgs> DeleteClicked;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleListItem"/>.
        /// </summary>
        public ParticleListItem()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ParticleName"/> property.
        /// </summary>
        public static readonly DependencyProperty ParticleNameProperty =
            DependencyProperty.Register(nameof(ParticleName), typeof(string), typeof(ParticleListItem), new PropertyMetadata(""));

        /// <summary>
        /// Registers the <see cref="ParticleFilePath"/> property.
        /// </summary>
        public static readonly DependencyProperty ParticleFilePathProperty =
            DependencyProperty.Register(nameof(ParticleFilePath), typeof(string), typeof(ParticleListItem), new PropertyMetadata("", ParticleFilePathChanged));

        /// <summary>
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        protected static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(ParticleListItem), new PropertyMetadata(false));
        #endregion


        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        public string ParticleName
        {
            get { return (string)GetValue(ParticleNameProperty); }
            set { SetValue(ParticleNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the particle file path.
        /// </summary>
        public string ParticleFilePath
        {
            get { return (string)GetValue(ParticleFilePathProperty); }
            set { SetValue(ParticleFilePathProperty, value); }
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
        /// Refreshes the UI of the user control.
        /// </summary>
        public void Refresh()
        {
            var fileExists = File.Exists(ParticleFilePath);

            HasError = string.IsNullOrEmpty(ParticleFilePath) ? false : !fileExists;

            if (fileExists)
            {
                var thumbnailImage = new BitmapImage();
                thumbnailImage.BeginInit();

                //This prevents the file from being locked by loading ALL
                //of the image data into memory.  This prevents references to the image data
                //from having to go to the file itself which means it doesn't mean it has
                //to be locked.
                thumbnailImage.CacheOption = BitmapCacheOption.OnLoad;
                thumbnailImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                thumbnailImage.UriSource = new Uri(ParticleFilePath);
                thumbnailImage.EndInit();

                ThumbnailImage.Source = thumbnailImage;
            }
        }
        #endregion


        #region Internal Methods
        /// <summary>
        /// Subscribes the given handler to the rename clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void SubscribeRenameClicked(EventHandler<RenameSetupItemEventArgs> handler)
        {
            RenameClicked += handler;

            IsRenameSubscribed = true;
        }


        /// <summary>
        /// Unsubscribes the given handler to the rename clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void UnsubscribeRenameClicked(EventHandler<RenameSetupItemEventArgs> handler)
        {
            RenameClicked -= handler;

            IsRenameSubscribed = false;
        }


        /// <summary>
        /// Subscribes the given handler to the delete clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void SubscribeDeleteClicked(EventHandler<SetupItemEventArgs> handler)
        {
            DeleteClicked += handler;

            IsDeleteSubscribed = true;
        }


        /// <summary>
        /// Unsubscribes the given handler to the delete clicked event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void UnsubscribeDeleteClicked(EventHandler<SetupItemEventArgs> handler)
        {
            DeleteClicked -= handler;

            IsDeleteSubscribed = false;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Invokes the rename event.
        /// </summary>
        private void RenameCustomButton_Click(object sender, EventArgs e)
        {
            RenameClicked?.Invoke(this, new RenameSetupItemEventArgs(ParticleName, ParticleFilePath));

            Refresh();
        }


        /// <summary>
        /// Invokes the delete event.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, new SetupItemEventArgs(ParticleName, ParticleFilePath));

            Refresh();
        }


        /// <summary>
        /// Shows the thumbnail preview dialog of the clicked particle thumbnail.
        /// </summary>
        private void Overlay_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var thumbnailDialog = new ThumbnailViewerDialog($"Thumbnail Viewer - {ParticleName}")
            {
                ThumbnailPath = ParticleFilePath
            };

            thumbnailDialog.ShowDialog();
        }


        /// <summary>
        /// Refreshes the user control.
        /// </summary>
        private static void ParticleFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ParticleListItem)d;

            if (ctrl == null)
                return;

            ctrl.Refresh();
        }
        #endregion
    }
}

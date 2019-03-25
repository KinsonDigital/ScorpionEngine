﻿using ParticleMaker.CustomEventArgs;
using ParticleMaker.Dialogs;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        public event EventHandler<RenameItemEventArgs> RenameClicked;

        /// <summary>
        /// Invoked when the delete button has been clicked.
        /// </summary>
        public event EventHandler<ItemEventArgs> DeleteClicked;
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

        /// <summary>
        /// Registers the <see cref="RenameClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty RenameClickedCommandProperty =
            DependencyProperty.Register(nameof(RenameClickedCommand), typeof(ICommand), typeof(ParticleListItem), new PropertyMetadata(null));

        /// <summary>
        /// Registers the <see cref="DeleteClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty DeleteClickedCommandProperty =
            DependencyProperty.Register(nameof(DeleteClickedCommand), typeof(ICommand), typeof(ParticleListItem), new PropertyMetadata(null));
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
        /// Gets or sets the command to execute when the rename button has been clicked.
        /// </summary>
        public ICommand RenameClickedCommand
        {
            get { return (ICommand)GetValue(RenameClickedCommandProperty); }
            set { SetValue(RenameClickedCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to execute when the delete button has been clicked.
        /// </summary>
        public ICommand DeleteClickedCommand
        {
            get { return (ICommand)GetValue(DeleteClickedCommandProperty); }
            set { SetValue(DeleteClickedCommandProperty, value); }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Refreshes the UI of the user control.
        /// </summary>
        public void Refresh()
        {
            var fileExists = File.Exists(ParticleFilePath);

            var dirExists = DesignerProperties.GetIsInDesignMode(this) ? true : Directory.Exists(Path.GetDirectoryName(ParticleFilePath));
            var pathSections = string.IsNullOrEmpty(ParticleFilePath) || !dirExists ? new string[0] : ParticleFilePath.Split('\\');

            ParticleName = pathSections.Length >= 1 ? Path.GetFileNameWithoutExtension(pathSections[pathSections.Length - 1]) : "";

            HasError = !dirExists || DesignerProperties.GetIsInDesignMode(this) ? false : !File.Exists(ParticleFilePath);

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


        #region Private Methods
        /// <summary>
        /// Invokes the rename event.
        /// </summary>
        private void RenameCustomButton_Click(object sender, EventArgs e)
        {
            RenameClicked?.Invoke(this, new RenameItemEventArgs(ParticleName, ParticleFilePath));
            RenameClickedCommand?.Execute(new RenameItemEventArgs(ParticleName, ParticleFilePath));

            Refresh();
        }


        /// <summary>
        /// Invokes the delete event.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, new ItemEventArgs(ParticleName, ParticleFilePath));
            DeleteClickedCommand?.Execute(new ItemEventArgs(ParticleName, ParticleFilePath));

            Refresh();
        }


        /// <summary>
        /// Shows the thumbnail preview dialog of the clicked particle thumbnail.
        /// </summary>
        private void Overlay_MouseUp(object sender, MouseButtonEventArgs e)
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

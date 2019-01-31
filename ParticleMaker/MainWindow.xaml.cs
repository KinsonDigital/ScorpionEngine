﻿using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.ComponentModel;
using ParticleMaker.ViewModels;
using System.Diagnostics.CodeAnalysis;
using ThreadTimer = System.Threading.Timer;
using ParticleMaker.Dialogs;
using System;
using System.IO;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class MainWindow : Window
    {
        #region Fields
        private ThreadTimer _shutDownTimer;
        private MainViewModel _mainViewModel;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            _mainViewModel = App.DIContainer.GetInstance<MainViewModel>();

            ElementHost.EnableModelessKeyboardInterop(this);

            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Shuts down the graphics engine before closing the window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            _mainViewModel.ShutdownEngine();
            
            //Give the graphics engine some time to shutdown and cleanup
            _shutDownTimer = new ThreadTimer((state) =>
            {
                base.OnClosing(e);
            }, null, 2000, 0);
        }
        #endregion


        #region Event Methods
        /// <summary>
        /// Creates the <see cref="MainViewModel"/> with default settings.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainViewModel.RenderSurface = (winFormsHost.Child as PictureBox);
            _mainViewModel.UIDispatcher = Dispatcher;

            DataContext = _mainViewModel;

            _mainViewModel.RedMin = 0;
            _mainViewModel.RedMax = 255;
            _mainViewModel.GreenMin = 0;
            _mainViewModel.GreenMax = 255;
            _mainViewModel.BlueMin = 0;
            _mainViewModel.BlueMax = 255;
            _mainViewModel.SizeMin = 1;
            _mainViewModel.SizeMax = 2;

            _mainViewModel.StartEngine();
        }
        #endregion

        private void MySetupList_AddSetupClicked(object sender, EventArgs e)
        {
            var inputDialog = new InputDialog("Create new setup.", "Type in the new setup name.");
            var dialogResult = inputDialog.ShowDialog();

            if (dialogResult == true)
            {
                var newSetupName = inputDialog.InputValue;
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var thumbnailDialog = new ThumbnailViewerDialog("Thumbnail Viewer - Owl");

            thumbnailDialog.ThumbnailPath = @"C:\temp\owlparticle.png";

            thumbnailDialog.Owner = this;
            thumbnailDialog.ShowDialog();
        }

        private void ParticleList_ItemRenamed(object sender, CustomEventArgs.RenameParticleEventArgs e)
        {
            File.Move(e.OldParticleFilePath, e.NewParticleFilePath);

            ParticleList.UpdateItemPath(e.OldParticleName, e.NewParticleFilePath);
        }

        private void ParticleList_ItemDeleted(object sender, CustomEventArgs.DeleteParticleEventArgs e)
        {
            File.Delete(e.ParticleFilePath);

            ParticleList.RemoveItem(e.ParticleName);
        }
    }
}

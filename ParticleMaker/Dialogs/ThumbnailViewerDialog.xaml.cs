using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for ThumbnailViewerDialog.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ThumbnailViewerDialog : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ThumbnailViewerDialog"/>.
        /// </summary>
        /// <param name="title">The title of the dialog window.</param>
        public ThumbnailViewerDialog(string title = "")
        {
            InitializeComponent();

            Title = title;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the path to the thumbnail to view.
        /// </summary>
        public string ThumbnailPath
        {
            get { return (string)GetValue(ThumbnailPathProperty); }
            set { SetValue(ThumbnailPathProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="ThumbnailPath"/> property.
        /// </summary>
        public static readonly DependencyProperty ThumbnailPathProperty =
            DependencyProperty.Register(nameof(ThumbnailPath), typeof(string), typeof(ThumbnailViewerDialog), new PropertyMetadata("", ThumbnailPathChanged));


        /// <summary>
        /// Gets or sets a valid indicating if there is an error.
        /// </summary>
        protected bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        protected static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register("HasError", typeof(bool), typeof(ThumbnailViewerDialog), new PropertyMetadata(false));
        #endregion


        #region Private Methods
        /// <summary>
        /// Closes the dialog.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();


        /// <summary>
        /// Updates the <see cref="HasError"/> property when the path changes.
        /// </summary>
        private static void ThumbnailPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dialog = (ThumbnailViewerDialog)d;

            if (dialog == null)
                return;

            var path = e.NewValue as string;

            if (string.IsNullOrEmpty(path))
                return;

            dialog.HasError = !File.Exists(dialog.ThumbnailPath);
        }
        #endregion
    }
}

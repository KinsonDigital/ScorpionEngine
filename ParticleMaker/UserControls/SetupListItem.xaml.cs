using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IOPath = System.IO.Path;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupListItem.xaml
    /// </summary>
    public partial class SetupListItem : UserControl
    {
        #region Public Events
        public event EventHandler<EventArgs> DeleteClicked;
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
            DependencyProperty.Register(nameof(SetupPath), typeof(string), typeof(SetupListItem), new PropertyMetadata("", SetupPathChanged));

        /// <summary>
        /// Registers the <see cref="SetupName"/> property.
        /// </summary>
        protected static readonly DependencyProperty SetupNameProperty =
            DependencyProperty.Register(nameof(SetupName), typeof(string), typeof(SetupListItem), new PropertyMetadata(""));

        /// <summary>
        /// Registers the <see cref="ErrorBorderBrush"/> property.
        /// </summary>
        protected static readonly DependencyProperty ErrorBorderBrushProperty =
            DependencyProperty.Register(nameof(ErrorBorderBrush), typeof(SolidColorBrush), typeof(SetupListItem), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));
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
        protected string SetupName
        {
            get { return (string)GetValue(SetupNameProperty); }
            set { SetValue(SetupNameProperty, value); }
        }


        /// <summary>
        /// Gets or sets the brush color for the error border.
        /// </summary>
        public SolidColorBrush ErrorBorderBrush
        {
            get { return (SolidColorBrush)GetValue(ErrorBorderBrushProperty); }
            set { SetValue(ErrorBorderBrushProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the setup name to be the file name without the extension
        /// </summary>
        private static void SetupPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (string)e.NewValue;

            if (newValue == null)
                return;

            var ctrl = (SetupListItem)d;

            if (ctrl == null)
                return;

            Refresh(ctrl);
        }


        /// <summary>
        /// <summary>
        /// Deletes the file at the given path.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {
            if (FileExists(SetupPath))
            {
                try
                {
                    File.Delete(SetupPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            Refresh(this);

            DeleteClicked?.Invoke(this, new EventArgs());
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Refreshs the control by updating the control's UI based on if the file exists or not.
        /// </summary>
        /// <param name="ctrl">The control with the UI to update.</param>
        private static void Refresh(SetupListItem ctrl)
        {
            if (FileExists(ctrl.SetupPath))
            {
                ctrl.SetupName = IOPath.GetFileNameWithoutExtension(ctrl.SetupPath);

                ctrl.ErrorBorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            else
            {
                //TODO: Setup protected props to show an issue
                ctrl.ErrorBorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                ctrl.SetupName = "Error!!";
            }
        }


        /// <summary>
        /// Returns a value indicating if the file at the given <paramref name="filePath"/> exists.
        /// </summary>
        /// <param name="filePath">The path to the file to check for.</param>
        private static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion
    }
}

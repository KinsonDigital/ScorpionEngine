using ParticleMaker.Dialogs;
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
            DependencyProperty.Register(nameof(SetupPath), typeof(string), typeof(SetupListItem), new PropertyMetadata("", SetupFilePathChanged));

        /// <summary>
        /// Registers the <see cref="SetupName"/> property.
        /// </summary>
        protected static readonly DependencyProperty SetupNameProperty =
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
        protected string SetupName
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

            ctrl.Refresh(ctrl);
        }


        /// <summary>
        /// Renames the file at the currently set path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameCustomButton_Click(object sender, EventArgs e)
        {
            if (FileExists(SetupPath))
            {
                try
                {
                    var oldName = Path.GetFileNameWithoutExtension(SetupPath);

                    var inputDialog = new InputDialog("Rename setup", $"Rename the setup named \"{oldName}\".", oldName);

                    var dialogResult = inputDialog.ShowDialog();

                    if (dialogResult != null && dialogResult == true)
                    {
                        var destFileName = $@"{Path.GetDirectoryName(SetupPath)}\{inputDialog.InputResult}.json";

                        File.Move(SetupPath, destFileName);

                        SetupPath = destFileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            Refresh(this);
        }


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
        public void Refresh(SetupListItem ctrl)
        {
            if (DesignerProperties.GetIsInDesignMode(ctrl) || FileExists(ctrl.SetupPath))
            {
                ctrl.SetupName = Path.GetFileNameWithoutExtension(ctrl.SetupPath);
                ctrl.HasError = false;
            }
            else
            {
                ctrl.SetupName = "Error!!";
                ctrl.HasError = true;
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

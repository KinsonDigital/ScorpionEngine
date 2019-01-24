using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using IOPath = System.IO.Path;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupListItem.xaml
    /// </summary>
    public partial class SetupListItem : UserControl
    {
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

            //TODO: Check if the file exists and setup UI to display error/issue
            if (true)// <== file exists check here
            {
                ctrl.SetupPath = IOPath.GetFileNameWithoutExtension(newValue);

                //TODO: Setup protected props to show no issue
            }
            else
            {
                //TODO: Setup protected props to show an issue
            }
        }
        #endregion
    }
}

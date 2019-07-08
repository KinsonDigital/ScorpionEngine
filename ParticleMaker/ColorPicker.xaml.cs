using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ColorPicker : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorPicker"/>.
        /// </summary>
        public ColorPicker(Color color)
        {
            ChosenColor = color;

            InitializeComponent();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the chosen color.
        /// </summary>
        public Color ChosenColor { get; set; }

        private float ChosenRedValue { get; set; }

        private byte ChosenGreenValue { get; set; }

        private byte ChosenBlueValue { get; set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// Accepts the color that was chosen and returns dialog result of true.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;


        /// <summary>
        /// Cancels the color that was chosen by returned the dialog result of false.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
        #endregion
    }
}

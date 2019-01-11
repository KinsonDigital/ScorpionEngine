using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
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


        #region Properties
        /// <summary>
        /// Gets or sets the chosen color.
        /// </summary>
        public Color ChosenColor { get; set; }
        #endregion


        #region Private Methods
        /// <summary>
        /// Accepts the color that was chosen and returns dialog result of true.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }


        /// <summary>
        /// Cancels the color that was chosen by returned the dialog result of false.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        #endregion
    }
}

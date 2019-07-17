using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for <see cref="UserControlTestWindow"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class UserControlTestWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="UserControlTestWindow"/>.
        /// </summary>
        public UserControlTestWindow() => InitializeComponent();
        #endregion


        #region Private Methods
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var colorPickerDialog = new ColorPicker(Color.FromArgb(255, 255, 100, 50));

            colorPickerDialog.ShowDialog();
        }
        #endregion


        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(UserControlTestWindow), new PropertyMetadata(4));
    }
}

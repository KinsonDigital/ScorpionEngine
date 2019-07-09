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
        private static bool _skipUpdate = false;

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorPicker"/>.
        /// </summary>
        public ColorPicker(Color color)
        {
            ChosenColorBrush = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
            InitializeComponent();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the chosen color of the dialog.
        /// </summary>
        public SolidColorBrush ChosenColorBrush
        {
            get { return (SolidColorBrush)GetValue(ChosenColorBrushProperty); }
            set { SetValue(ChosenColorBrushProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="ChosenColorBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty ChosenColorBrushProperty =
            DependencyProperty.Register(nameof(ChosenColorBrush), typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), ChosenColorBrushChangedCallback));


        /// <summary>
        /// Gets or sets the red color component of the chosen color.
        /// </summary>
        public float ChosenRedValue
        {
            get { return (float)GetValue(ChosenRedValueProperty); }
            set { SetValue(ChosenRedValueProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="ChosenRedValue"/> property.
        /// </summary>
        public static readonly DependencyProperty ChosenRedValueProperty =
            DependencyProperty.Register(nameof(ChosenRedValue), typeof(float), typeof(ColorPicker), new PropertyMetadata(0f, ChosenRedValueChangedCallback));


        /// <summary>
        /// Gets or sets the green color component of the chosen color.
        /// </summary>
        public float ChosenGreenValue
        {
            get { return (float)GetValue(ChosenGreenValueProperty); }
            set { SetValue(ChosenGreenValueProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="ChosenGreenValue"/> property.
        /// </summary>
        public static readonly DependencyProperty ChosenGreenValueProperty =
            DependencyProperty.Register(nameof(ChosenGreenValue), typeof(float), typeof(ColorPicker), new PropertyMetadata(0f, ChosenGreenValueChangedCallback));


        /// <summary>
        /// Gets or sets the blue color component of the chosen color.
        /// </summary>
        public float ChosenBlueValue
        {
            get { return (float)GetValue(ChosenBlueValueProperty); }
            set { SetValue(ChosenBlueValueProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="ChosenBlueValue"/> property.
        /// </summary>
        public static readonly DependencyProperty ChosenBlueValueProperty =
            DependencyProperty.Register(nameof(ChosenBlueValue), typeof(float), typeof(ColorPicker), new PropertyMetadata(0f, ChosenBlueValueChangedCallback));
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


        /// <summary>
        /// Sets all of the individual color component values.
        /// </summary>
        private static void ChosenColorBrushChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ColorPicker colorPickerDialog))
                return;

            _skipUpdate = true;

            colorPickerDialog.ChosenRedValue = colorPickerDialog.ChosenColorBrush.Color.R;
            colorPickerDialog.ChosenGreenValue = colorPickerDialog.ChosenColorBrush.Color.G;
            colorPickerDialog.ChosenBlueValue = colorPickerDialog.ChosenColorBrush.Color.B;

            _skipUpdate = false;
        }


        /// <summary>
        /// Converts the incoming float red value to a byte value and sets the color value to the brush.
        /// </summary>
        private static void ChosenRedValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_skipUpdate)
                return;

            if (!(d is ColorPicker colorPickerDialog))
                return;

            var redClrValue = ((float)e.NewValue).ToByte();

            var currentClr = colorPickerDialog.ChosenColorBrush.Color;
            var newClr = Color.FromArgb(currentClr.A, redClrValue, currentClr.G, currentClr.B);

            colorPickerDialog.ChosenColorBrush = new SolidColorBrush(newClr);
        }


        /// <summary>
        /// Converts the incoming float green value to a byte value and sets the color value to the brush.
        /// </summary>
        private static void ChosenGreenValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_skipUpdate)
                return;

            if (!(d is ColorPicker colorPickerDialog))
                return;

            var greenClrValue = ((float)e.NewValue).ToByte();

            var currentClr = colorPickerDialog.ChosenColorBrush.Color;
            var newClr = Color.FromArgb(currentClr.A, currentClr.R, greenClrValue, currentClr.B);

            colorPickerDialog.ChosenColorBrush = new SolidColorBrush(newClr);
        }


        /// <summary>
        /// Converts the incoming float blue value to a byte value and sets the color value to the brush.
        /// </summary>
        private static void ChosenBlueValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_skipUpdate)
                return;

            if (!(d is ColorPicker colorPickerDialog))
                return;

            var blueClrValue = ((float)e.NewValue).ToByte();

            var currentClr = colorPickerDialog.ChosenColorBrush.Color;
            var newClr = Color.FromArgb(currentClr.A, currentClr.R, currentClr.G, blueClrValue);

            colorPickerDialog.ChosenColorBrush = new SolidColorBrush(newClr);
        }
        #endregion
    }
}

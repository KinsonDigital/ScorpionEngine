using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorValue.xaml
    /// </summary>
    public partial class ColorValue : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorBrush"/>.
        /// </summary>
        public ColorValue()
        {
            InitializeComponent();

            DataContext = this;

            RedNumericUpDown.OnValueChanged += RedNumericUpDown_OnValueChanged;
            GreenNumericUpDown.OnValueChanged += GreenNumericUpDown_OnValueChanged;
            BlueNumericUpDown.OnValueChanged += BlueNumericUpDown_OnValueChanged;
            AlphaNumericUpDown.OnValueChanged += AlphaNumericUpDown_OnValueChanged;

            RedNumericUpDown.Value = 255;
            GreenNumericUpDown.Value = 255;
            BlueNumericUpDown.Value = 255;
            AlphaNumericUpDown.Value = 255;

            ColorBrush = new SolidColorBrush(Color.FromArgb((byte)AlphaNumericUpDown.Value, (byte)RedNumericUpDown.Value, (byte)GreenNumericUpDown.Value, (byte)BlueNumericUpDown.Value));
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ColorBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register(nameof(ColorBrush), typeof(SolidColorBrush), typeof(ColorValue), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), ColorBrushValueChangedCallback));
        #endregion


        /// <summary>
        /// Gets or sets the color result.
        /// </summary>
        [Category("Common")]
        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush)GetValue(ColorBrushProperty); }
            set { SetValue(ColorBrushProperty, value); }
        }

        /// <summary>
        /// Gets the set color of the control.
        /// </summary>
        public Color ColorResult => ColorBrush.Color;

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="NumericUpDown"/> controls
        /// will be updatged when the <see cref="ColorBrush"/> is updated.
        /// </summary>
        internal bool SkipUpdate { get; set; }
        #endregion


        #region Events
        /// <summary>
        /// Updates the <see cref="ColorBrush"/>'s red component.
        /// </summary>
        private void RedNumericUpDown_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (SkipUpdate)
            {
                SkipUpdate = false;
                return;
            }

            var newColor = Color.FromArgb(ColorBrush.Color.A, (byte)e.NewValue, ColorBrush.Color.G, ColorBrush.Color.B);
            ColorBrush = new SolidColorBrush(newColor);
        }


        /// <summary>
        /// Updates the <see cref="ColorBrush"/>'s green component.
        /// </summary>
        private void GreenNumericUpDown_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (SkipUpdate)
            {
                SkipUpdate = false;
                return;
            }

            var newColor = Color.FromArgb(ColorBrush.Color.A, ColorBrush.Color.R, (byte)e.NewValue, ColorBrush.Color.B);
            ColorBrush = new SolidColorBrush(newColor);
        }


        /// <summary>
        /// Updates the <see cref="ColorBrush"/>'s blue component.
        /// </summary>
        private void BlueNumericUpDown_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (SkipUpdate)
            {
                SkipUpdate = false;
                return;
            }

            var newColor = Color.FromArgb(ColorBrush.Color.A, ColorBrush.Color.R, ColorBrush.Color.G, (byte)e.NewValue);
            ColorBrush = new SolidColorBrush(newColor);
        }


        /// <summary>
        /// Updates the <see cref="ColorBrush"/>'s alpha component.
        /// </summary>
        private void AlphaNumericUpDown_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (SkipUpdate)
            {
                SkipUpdate = false;
                return;
            }

            var newColor = Color.FromArgb((byte)e.NewValue, ColorBrush.Color.R, ColorBrush.Color.G, ColorBrush.Color.B);
            ColorBrush = new SolidColorBrush(newColor);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Make sure that the red, green, blue, and alpha values do not invoke this
        /// method do prevent a stack overflow.
        /// </summary>
        private static void ColorBrushValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorValue)d;

            if (ctrl == null)
                return;

            //Skip the update process of the numeric up down values themselves
            ctrl.SkipUpdate = true;
            ctrl.RedNumericUpDown.Value = ((SolidColorBrush)e.NewValue).Color.R;

            ctrl.SkipUpdate = true;
            ctrl.GreenNumericUpDown.Value = ((SolidColorBrush)e.NewValue).Color.G;

            ctrl.SkipUpdate = true;
            ctrl.BlueNumericUpDown.Value = ((SolidColorBrush)e.NewValue).Color.B;

            ctrl.SkipUpdate = true;
            ctrl.AlphaNumericUpDown.Value = ((SolidColorBrush)e.NewValue).Color.A;

            ctrl.SkipUpdate = false;
        }
        #endregion
    }
}

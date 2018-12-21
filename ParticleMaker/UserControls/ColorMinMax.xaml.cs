using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorMinMax.xaml
    /// </summary>
    public partial class ColorMinMax : UserControl
    {
        #region Fields
        private static SolidColorBrush DEFAULT_COLOR = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorMinMax"/>.
        /// </summary>
        public ColorMinMax()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="MinLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty MinLabelProperty =
            DependencyProperty.Register(nameof(MinLabel), typeof(string), typeof(ColorMinMax), new PropertyMetadata("Min"));

        /// <summary>
        /// Registers the <see cref="MaxLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxLabelProperty =
            DependencyProperty.Register(nameof(MaxLabel), typeof(string), typeof(ColorMinMax), new PropertyMetadata("Max"));

        /// <summary>
        /// Registers the <see cref="Min"/> property.
        /// </summary>
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(nameof(Min), typeof(int), typeof(ColorMinMax), new PropertyMetadata(0, MinChanged));

        /// <summary>
        /// Registers the <see cref="Max"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(nameof(Max), typeof(int), typeof(ColorMinMax), new PropertyMetadata(0, MaxChanged));

        /// <summary>
        /// Registers the <see cref="ChosenColorComponent"/> property.
        /// </summary>
        public static readonly DependencyProperty ChosenColorComponentProperty =
            DependencyProperty.Register(nameof(ChosenColorComponent), typeof(ColorComponent), typeof(ColorMinMax), new PropertyMetadata(ColorComponent.Red, ChosenColorChanged));

        /// <summary>
        /// Registers the <see cref="MinColor"/> property.
        /// </summary>
        protected static readonly DependencyProperty MinColorProperty =
            DependencyProperty.Register(nameof(MinColor), typeof(SolidColorBrush), typeof(ColorMinMax), new PropertyMetadata(DEFAULT_COLOR));

        /// <summary>
        /// Register the <see cref="MaxColor"/> property.
        /// </summary>
        protected static readonly DependencyProperty MaxColorProperty =
            DependencyProperty.Register(nameof(MaxColor), typeof(SolidColorBrush), typeof(ColorMinMax), new PropertyMetadata(DEFAULT_COLOR));
        #endregion


        /// <summary>
        /// Gets or sets the min label text.
        /// </summary>
        public string MinLabel
        {
            get { return (string)GetValue(MinLabelProperty); }
            set { SetValue(MinLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the max label text.
        /// </summary>
        public string MaxLabel
        {
            get { return (string)GetValue(MaxLabelProperty); }
            set { SetValue(MaxLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the chosen color to apply the min and max values to.
        /// </summary>
        public ColorComponent ChosenColorComponent
        {
            get { return (ColorComponent)GetValue(ChosenColorComponentProperty); }
            set { SetValue(ChosenColorComponentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the min color value based on the chosen color component.
        /// </summary>
        protected SolidColorBrush MinColor
        {
            get { return (SolidColorBrush)GetValue(MinColorProperty); }
            set { SetValue(MinColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the max color value based on the chosen color component.
        /// </summary>
        protected SolidColorBrush MaxColor
        {
            get { return (SolidColorBrush)GetValue(MaxColorProperty); }
            set { SetValue(MaxColorProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the <see cref="MinColor"/> and <see cref="MaxColor"/>.
        /// </summary>
        private static void ChosenColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorMinMax)d;

            if (ctrl == null)
                return;

            UpdateMinColor(ctrl);
            UpdateMaxColor(ctrl);
        }


        /// <summary>
        /// Updates the <see cref="MinColor"/> based on the min value.
        /// </summary>
        private static void MinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorMinMax)d;

            if (ctrl == null)
                return;

            UpdateMinColor(ctrl);
        }


        /// <summary>
        /// Updates the <see cref="MaxColor"/> based on the max value.
        /// </summary>
        private static void MaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorMinMax)d;

            if (ctrl == null)
                return;

            UpdateMaxColor(ctrl);
        }


        /// <summary>
        /// Updates the <see cref="MinColor"/> based on the min value.
        /// </summary>
        /// <param name="ctrl">The control to update.</param>
        private static void UpdateMinColor(ColorMinMax ctrl)
        {
            switch (ctrl.ChosenColorComponent)
            {
                case ColorComponent.Red:
                    ctrl.MinColor = new SolidColorBrush(Color.FromArgb(255, (byte)ctrl.Min, 0, 0));
                    break;
                case ColorComponent.Green:
                    ctrl.MinColor = new SolidColorBrush(Color.FromArgb(255, 0, (byte)ctrl.Min, 0));
                    break;
                case ColorComponent.Blue:
                    ctrl.MinColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, (byte)ctrl.Min));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Updates the <see cref="MaxColor"/> based on the max value.
        /// </summary>
        /// <param name="ctrl">The control to update.</param>
        private static void UpdateMaxColor(ColorMinMax ctrl)
        {
            switch (ctrl.ChosenColorComponent)
            {
                case ColorComponent.Red:
                    ctrl.MaxColor = new SolidColorBrush(Color.FromArgb(255, (byte)ctrl.Max, 0, 0));
                    break;
                case ColorComponent.Green:
                    ctrl.MaxColor = new SolidColorBrush(Color.FromArgb(255, 0, (byte)ctrl.Max, 0));
                    break;
                case ColorComponent.Blue:
                    ctrl.MaxColor = new SolidColorBrush(Color.FromArgb(255, 0, 0, (byte)ctrl.Max));
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
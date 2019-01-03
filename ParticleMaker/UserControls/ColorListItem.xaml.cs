﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorListItem.xaml
    /// </summary>
    public partial class ColorListItem : UserControl
    {

        #region Fields
        private static readonly SolidColorBrush DEFAULT_COLOR = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorListItem"/>.
        /// </summary>
        public ColorListItem()
        {
            ColorTextValue = "255, 255, 255, 255";
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ColorValue"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register(nameof(ColorValue), typeof(SolidColorBrush), typeof(ColorListItem), new PropertyMetadata(DEFAULT_COLOR, ColorValueChanged));

        /// <summary>
        /// Registers the <see cref="ColorTextValue"/> property.
        /// </summary>
        protected static readonly DependencyProperty ColorTextValueProperty =
            DependencyProperty.Register(nameof(ColorTextValue), typeof(string), typeof(ColorListItem), new PropertyMetadata("255, 255, 255"));

        /// <summary>
        /// Registers the <see cref="TextForecolor"/> property.
        /// </summary>
        protected static readonly DependencyProperty TextForecolorProperty =
            DependencyProperty.Register(nameof(TextForecolor), typeof(SolidColorBrush), typeof(ColorListItem), new PropertyMetadata(DEFAULT_COLOR.ToNegative()));
        #endregion


        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public SolidColorBrush ColorValue
        {
            get { return (SolidColorBrush)GetValue(ColorValueProperty); }
            set { SetValue(ColorValueProperty, value); }
        }


        /// <summary>
        /// Gets or sets the color value in text form.
        /// </summary>
        protected string ColorTextValue
        {
            get { return (string)GetValue(ColorTextValueProperty); }
            set { SetValue(ColorTextValueProperty, value); }
        }


        /// <summary>
        /// Gets or sets the forecolor of the text.
        /// </summary>
        protected SolidColorBrush TextForecolor
        {
            get { return (SolidColorBrush)GetValue(TextForecolorProperty); }
            set { SetValue(TextForecolorProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the forecolor to the negative value of the <see cref="ColorValue"/>.  This is to make sure
        /// that no matter what the <see cref="ColorValue"/> is, the text can be seen.
        /// </summary>
        private static void ColorValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ColorListItem)d;

            if (ctrl == null)
                return;

            var newColor = ((SolidColorBrush)e.NewValue).Color;

            var negativeForecolor = Color.FromArgb(255, (byte)(255 - newColor.R), (byte)(255 - newColor.G), (byte)(255 - newColor.B));

            ctrl.TextForecolor = newColor.ToNegativeBrush();
        }
        #endregion
    }
}

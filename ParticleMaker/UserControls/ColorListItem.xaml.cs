﻿using ParticleMaker.CustomEventArgs;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorListItem.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ColorListItem : UserControl
    {
        #region Public Events
        /// <summary>
        /// Invoked when the edit color button has been clicked.
        /// </summary>
        public event EventHandler<ColorItemClickedEventArgs> EditColorClicked;

        /// <summary>
        /// Invoked when the delete color button has been clicked.
        /// </summary>
        public event EventHandler<ColorItemClickedEventArgs> DeleteClicked;
        #endregion


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
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public SolidColorBrush ColorValue
        {
            get => (SolidColorBrush)GetValue(ColorValueProperty);
            set => SetValue(ColorValueProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="ColorValue"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register(nameof(ColorValue), typeof(SolidColorBrush), typeof(ColorListItem), new PropertyMetadata(DEFAULT_COLOR, ColorValueChanged));


        /// <summary>
        /// Gets or sets the color value in text form.
        /// </summary>
        protected string ColorTextValue
        {
            get => (string)GetValue(ColorTextValueProperty);
            set => SetValue(ColorTextValueProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="ColorTextValue"/> property.
        /// </summary>
        protected static readonly DependencyProperty ColorTextValueProperty =
            DependencyProperty.Register(nameof(ColorTextValue), typeof(string), typeof(ColorListItem), new PropertyMetadata("255, 255, 255"));


        /// <summary>
        /// Gets or sets the forecolor of the text.
        /// </summary>
        protected SolidColorBrush TextForecolor
        {
            get => (SolidColorBrush)GetValue(TextForecolorProperty);
            set => SetValue(TextForecolorProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="TextForecolor"/> property.
        /// </summary>
        protected static readonly DependencyProperty TextForecolorProperty =
            DependencyProperty.Register(nameof(TextForecolor), typeof(SolidColorBrush), typeof(ColorListItem), new PropertyMetadata(DEFAULT_COLOR.ToNegative()));


        /// <summary>
        /// Gets or sets the Id of the <see cref="ColorListItem"/>.
        /// </summary>
        public int Id
        {
            get => (int)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Id"/> property.
        /// </summary>
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(nameof(Id), typeof(int), typeof(ColorListItem), new PropertyMetadata(-1));
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

            ctrl.TextForecolor = newColor.ToNegativeBrush();
        }


        /// <summary>
        /// Invoked when the edit color button has been clicked.
        /// </summary>
        private void EditColorButton_Click(object sender, EventArgs e) => EditColorClicked?.Invoke(this, new ColorItemClickedEventArgs(Id, ColorValue.Color));


        /// <summary>
        /// Invoked when the delete image has been clicked.
        /// </summary>
        private void DeleteColorButton_Click(object sender, EventArgs e) => DeleteClicked?.Invoke(this, new ColorItemClickedEventArgs(Id, Color.FromRgb(255, 255, 255)));
        #endregion
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorComponentValue.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class NumericUpDown : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="NumericUpDown"/>.
        /// </summary>
        public NumericUpDown() => InitializeComponent();
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the number of decimal places that will be shown.
        /// </summary>
        [Category("Common")]
        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="DecimalPlaces"/> property.
        /// </summary>
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float Value
        {
            get => (float)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(float), typeof(NumericUpDown), new PropertyMetadata(0f, null, ValueCoerce));


        /// <summary>
        /// Gets or sets the minimum <see cref="NumericUpDown"/> control value.
        /// </summary>
        [Category("Common")]
        public float Min
        {
            get => (float)GetValue(MinProperty);
            set => SetValue(MinProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Min"/> property.
        /// </summary>
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(nameof(Min), typeof(float), typeof(NumericUpDown), new PropertyMetadata(0f, MinChanged));


        /// <summary>
        /// Gets or sets the maximum <see cref="NumericUpDown"/> control value.
        /// </summary>
        [Category("Common")]
        public float Max
        {
            get => (float)GetValue(MaxProperty);
            set => SetValue(MaxProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Max"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(nameof(Max), typeof(float), typeof(NumericUpDown), new PropertyMetadata(10f, MaxChanged));


        /// <summary>
        /// Gets or sets the amount to increment the <see cref="Value"/> property.
        /// </summary>
        [Category("Common")]
        public float Increment
        {
            get => (float)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Increment"/> property.
        /// </summary>
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(nameof(Increment), typeof(float), typeof(NumericUpDown), new PropertyMetadata(1f));


        /// <summary>
        /// Gets or sets the amount to decrement the <see cref="Value"/> property.
        /// </summary>
        [Category("Common")]
        public float Decrement
        {
            get => (float)GetValue(DecrementProperty);
            set => SetValue(DecrementProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Decrement"/> property.
        /// </summary>
        public static readonly DependencyProperty DecrementProperty =
            DependencyProperty.Register(nameof(Decrement), typeof(float), typeof(NumericUpDown), new PropertyMetadata(1f));


        /// <summary>
        /// Gets or sets the label text of the control.
        /// </summary>
        [Category("Common")]
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="LabelText"/> property.
        /// </summary>
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(NumericUpDown), new PropertyMetadata("", LabelTextChanged));


        /// <summary>
        /// Gets or sets a value indicating if the label will be visible.
        /// </summary>
        public Visibility IsLabelVisible
        {
            get => (Visibility)GetValue(IsLabelVisibleProperty);
            set => SetValue(IsLabelVisibleProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="IsLabelVisible"/> property.
        /// </summary>
        public static readonly DependencyProperty IsLabelVisibleProperty =
            DependencyProperty.Register(nameof(IsLabelVisible), typeof(Visibility), typeof(NumericUpDown), new PropertyMetadata(Visibility.Hidden));
        #endregion


        #region Event Methods
        /// <summary>
        /// Increments the numeric up down value.
        /// </summary>
        private void UpArrowButton_Click(object sender, EventArgs e)
        {
            Value += Increment;
            ValueNumberbox.MoveCaretToEnd();
        }


        /// <summary>
        /// Decrements the numeric up down value.
        /// </summary>
        private void DownArrowButton_Click(object sender, EventArgs e)
        {
            Value -= Decrement;
            ValueNumberbox.MoveCaretToEnd();
        }
        #endregion


        #region Static Methods
        /// <summary>
        /// Sets the if the colon label separator is visible or hidden depending if the label text is null.
        /// </summary>
        private static void LabelTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumericUpDown)d;

            if (ctrl == null)
                return;

            ctrl.IsLabelVisible = string.IsNullOrEmpty((string)e.NewValue) ? Visibility.Hidden : Visibility.Visible;
        }


        /// <summary>
        /// Restricts the value to the controls' <see cref="Min"/> and <see cref="Max"/> property values.
        /// </summary>
        /// <param name="d">The dependency object that contains the property to coerce.</param>
        /// <param name="baseValue">The base value of the property that needs coercion.</param>
        /// <returns></returns>
        private static object ValueCoerce(DependencyObject d, object baseValue)
        {
            var ctrl = (NumericUpDown)d;

            if (ctrl != null)
            {
                var numValue = (float)baseValue;

                //Apply the minimum if need be
                numValue = numValue < ctrl.Min ? ctrl.Min : numValue;

                //Apply the maximum if need be
                numValue = numValue > ctrl.Max ? ctrl.Max : numValue;

                numValue = (float)Math.Round(numValue, ctrl.DecimalPlaces);

                return numValue;
            }


            return ctrl;
        }


        /// <summary>
        /// Resets the <see cref="Value"/> property back to the <see cref="Min"/> if the 
        /// <see cref="Value"/> is less then the <see cref="Min"/> when it is changed.
        /// </summary>
        /// <param name="d">The dependency object that contains the property to coerce.</param>
        /// <param name="baseValue">The base value of the property that needs coercion.</param>
        private static void MinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = (NumericUpDown)d;

            if (numericUpDown != null)
            {
                var newValue = (float)e.NewValue;

                if (newValue > numericUpDown.Value)
                    numericUpDown.Value = newValue;
            }
        }


        /// <summary>
        /// Resets the <see cref="Value"/> property back to the <see cref="Max"/> if the 
        /// <see cref="Value"/> is greater then the <see cref="Max"/> when it is changed.
        /// </summary>
        /// <param name="d">The dependency object that contains the property to coerce.</param>
        /// <param name="baseValue">The base value of the property that needs coercion.</param>
        private static void MaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = (NumericUpDown)d;

            if (numericUpDown != null)
            {
                var newValue = (float)e.NewValue;

                if (newValue < numericUpDown.Value)
                    numericUpDown.Value = newValue;
            }
        }


        /// <summary>
        /// Increments or decrements the value if the up or down keys have been pressed.
        /// </summary>
        private void ValueNumberbox_NumberKeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    Value += Increment;
                    break;
                case Key.Down:
                    Value -= Decrement;
                    break;
            }
        }
        #endregion
    }
}

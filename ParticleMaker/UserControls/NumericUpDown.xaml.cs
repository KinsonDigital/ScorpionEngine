using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorComponentValue.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        #region Private Fields
        private static readonly Key[] _numberKeys = new Key[]
        {
            Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5,
            Key.D6, Key.D7, Key.D8, Key.D9
        };

        private static readonly Key[] _numpadKeys = new Key[]
        {
            Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3,
            Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7,
            Key.NumPad8, Key.NumPad9
        };

        private static readonly Key[] _otherAllowedKeys = new Key[]
        {
            Key.Left, Key.Right, Key.Home, Key.End,
            Key.Back, Key.Delete, Key.Decimal, Key.OemPeriod
        };
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="NumericUpDown"/>.
        /// </summary>
        public NumericUpDown()
        {
            InitializeComponent();

            DataContext = this;
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, null, ValueCoerceCallback));

        /// <summary>
        /// Registers the <see cref="Min"/> property.
        /// </summary>
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(nameof(Min), typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, MinChangedCallback));

        /// <summary>
        /// Registers the <see cref="Max"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(nameof(Max), typeof(int), typeof(NumericUpDown), new PropertyMetadata(10, MaxChangedCallback));

        /// <summary>
        /// Registers the <see cref="Increment"/> property.
        /// </summary>
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(nameof(Increment), typeof(int), typeof(NumericUpDown), new PropertyMetadata(1));

        /// <summary>
        /// Registers the <see cref="Decrement"/> property.
        /// </summary>
        public static readonly DependencyProperty DecrementProperty =
            DependencyProperty.Register(nameof(Decrement), typeof(int), typeof(NumericUpDown), new PropertyMetadata(1));
        #endregion


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum <see cref="NumericUpDown"/> control value.
        /// </summary>
        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum <see cref="NumericUpDown"/> control value.
        /// </summary>
        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount to increment the <see cref="Value"/> property.
        /// </summary>
        public int Increment
        {
            get { return (int)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount to decrement the <see cref="Value"/> property.
        /// </summary>
        public int Decrement
        {
            get { return (int)GetValue(DecrementProperty); }
            set { SetValue(DecrementProperty, value); }
        }
        #endregion


        #region Event Methods
        /// <summary>aef="Increment"/> value.
        /// </summary>
        private void UpArrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Value += Increment;
            ValueTextbox.CaretIndex = ValueTextbox.Text.Length;
        }


        /// <summary>
        /// Decrements the <see cref="Value"/> by the given <see cref="Decrement"/> value.
        /// </summary>
        private void Polygon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Value -= Increment;
            ValueTextbox.CaretIndex = ValueTextbox.Text.Length;
        }


        /// <summary>
        /// Limits the text box value to only numbers and various misc keys for 
        /// text manipulation.
        /// </summary>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_numberKeys.Contains(e.Key))
            {
                if (Keyboard.Modifiers != ModifierKeys.Shift)
                    return;
            }
            else if (_numpadKeys.Contains(e.Key))
            {
                if (Keyboard.Modifiers != ModifierKeys.Shift)
                    return;
            }
            else if (_otherAllowedKeys.Contains(e.Key))
            {
                if (Keyboard.IsKeyDown(Key.Decimal) || Keyboard.IsKeyDown(Key.OemPeriod))
                {
                    if (!Value.ToString().Contains('.') && Keyboard.Modifiers != ModifierKeys.Shift)
                        return;
                }
                else
                {
                    return;
                }
            }

            e.Handled = true;
        }
        #endregion


        #region Static Methods
        /// <summary>
        /// Restricts the value to the controls' <see cref="Min"/> and <see cref="Max"/> property values.
        /// </summary>
        /// <param name="d">The dependency object that contains the property to coerce.</param>
        /// <param name="baseValue">The base value of the property that needs coercion.</param>
        /// <returns></returns>
        private static object ValueCoerceCallback(DependencyObject d, object baseValue)
        {
            var numUpDown = (NumericUpDown)d;

            if (numUpDown != null)
            {
                var numValue = (int)baseValue;

                //Apply the minimum if need be
                numValue = numValue < numUpDown.Min ? numUpDown.Min : numValue;

                //Apply the maximum if need be
                numValue = numValue > numUpDown.Max ? numUpDown.Max : numValue;

                return numValue;
            }


            return numUpDown;
        }


        /// <summary>
        /// Resets the <see cref="Value"/> property back to the <see cref="Min"/> if the 
        /// <see cref="Value"/> is less then the <see cref="Min"/> when it is changed.
        /// </summary>
        /// <param name="d">The dependency object that contains the property to coerce.</param>
        /// <param name="baseValue">The base value of the property that needs coercion.</param>
        private static void MinChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = (NumericUpDown)d;

            if (numericUpDown != null)
            {
                var newValue = (int)e.NewValue;

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
        private static void MaxChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericUpDown = (NumericUpDown)d;

            if (numericUpDown != null)
            {
                var newValue = (int)e.NewValue;

                if (newValue < numericUpDown.Value)
                    numericUpDown.Value = newValue;
            }
        }
        #endregion
    }
}

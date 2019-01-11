using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {
        #region Private Fields
        /// <summary>
        /// The number keys above the letters on the keyboard.
        /// </summary>
        private static readonly Key[] _numberKeys = new Key[]
        {
            Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5,
            Key.D6, Key.D7, Key.D8, Key.D9
        };

        /// <summary>
        /// The numpad number keys.
        /// </summary>
        private static readonly Key[] _numpadKeys = new Key[]
        {
            Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3,
            Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7,
            Key.NumPad8, Key.NumPad9
        };

        /// <summary>
        /// All of the other keys that should be aloud to be used in a textbox like control for manipulating values.
        /// </summary>
        private static readonly Key[] _otherAllowedKeys = new Key[]
        {
            Key.Left, Key.Right, Key.Home, Key.End,
            Key.Back, Key.Delete, Key.Decimal, Key.OemPeriod
        };
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="NumberBox"/>.
        /// </summary>
        public NumberBox()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="NumberText"/> property.
        /// </summary>
        public static readonly DependencyProperty NumberTextProperty =
            DependencyProperty.Register(nameof(NumberText), typeof(string), typeof(NumberBox), new PropertyMetadata("0", NumberTextChanged));

        /// <summary>
        /// Registers the <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(float), typeof(NumberBox), new PropertyMetadata(0f, ValueChanged));
        #endregion


        /// <summary>
        /// Gets or sets the number text of the <see cref="NumberBox"/>.
        /// </summary>
        protected string NumberText
        {
            get { return (string)GetValue(NumberTextProperty); }
            set { SetValue(NumberTextProperty, value); }
        }

        /// <summary>
        /// Gets the value of the <see cref="NumberBox"/>.
        /// </summary>
        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Moves the caret to the end of the text.
        /// </summary>
        public void MoveCaretToEnd()
        {
            ValueTextBox.CaretIndex = ValueTextBox.Text.Length;
        }
        #endregion


        #region Event Methods
        /// <summary>
        /// Filters out any keys that have nothing to do with inputing a number or
        /// manipulating the text box contents.
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
                    if (!NumberText.ToString().Contains('.') && Keyboard.Modifiers != ModifierKeys.Shift)
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


        #region Private Methods
        /// <summary>
        /// Updates the value property by parsing the number text.
        /// </summary>
        private static void NumberTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumberBox)d;

            if (ctrl == null)
                return;

            if (float.TryParse(e.NewValue.ToString(), out float result))
                ctrl.Value = result;
        }


        /// <summary>
        /// Sets the number text of the control when the value has changed.
        /// </summary>
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (NumberBox)d;

            if (ctrl == null)
                return;

            ctrl.NumberText = e.NewValue.ToString();
        }
        #endregion
    }
}

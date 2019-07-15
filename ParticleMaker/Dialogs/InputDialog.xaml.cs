using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class InputDialog : Window
    {
        #region Private Fields
        private RelayCommand _okCommmand;
        private RelayCommand _cancelCommand;
        private readonly char[] _invalidCharacters;
        private readonly string[] _invalidValues;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="InputDialog"/>.
        /// </summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="message">The dialog message.</param>
        /// <param name="defaultValue">The default value of the dialog input text box.</param>
        /// <param name="invalidChars">The list of invalid characters that are not aloud to be input into the textbox.</param>
        /// <param name="invalidValues">The list of invalid values that will display an error if the input value matches one of these items.</param>
        public InputDialog(string title, string message, string defaultValue = "", char[] invalidChars = null, string[] invalidValues = null)
        {
            InitializeComponent();

            ElementHost.EnableModelessKeyboardInterop(this);

            _invalidCharacters = invalidChars;
            _invalidValues = invalidValues;

            DialogTitle = title;
            Message = message;
            InputValue = defaultValue;

            //Select all of the text so the user can start typing immediately
            InputTextBox.Focus();
            InputTextBox.SelectAll();

            Unloaded += InputDialog_Unloaded;

            Keyboard.AddKeyUpHandler(this, KeyUpHandler);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the command for the ok button.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                if (_okCommmand == null)
                    _okCommmand = new RelayCommand((param) =>
                    {
                        DialogResult = true;
                        Close();
                    }, (param) =>
                    {
                        return true;
                    });


                return _okCommmand;
            }
        }

        /// <summary>
        /// Gets or sets the command for the cancel button.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand((param) =>
                    {
                        DialogResult = false;
                        Close();
                    }, (param) => true);


                return _cancelCommand;
            }
        }

        /// <summary>
        /// Gets the result of the input that the user entered.
        /// </summary>
        public string InputResult => InputValue;

        /// <summary>
        /// Gets or sets a value that indicates if the casing of the invalid values should
        /// be ignored.
        /// </summary>
        public bool IgnoreInvalidValueCasing { get; set; }


        #region Dependency Props
        /// <summary>
        /// Gets or sets the <see cref="InputDialog"/>'s title.
        /// </summary>
        public string DialogTitle
        {
            get => (string)GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="DialogTitle"/> property.
        /// </summary>
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register(nameof(DialogTitle), typeof(string), typeof(InputDialog), new PropertyMetadata("Input Dialog"));


        /// <summary>
        /// Gets or sets the message of the <see cref="InputDialog"/>.
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Message"/> property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InputDialog), new PropertyMetadata("Enter some input and press enter"));


        /// <summary>
        /// Gets or sets the input textbox value.
        /// </summary>
        public string InputValue
        {
            get => (string)GetValue(InputValueProperty);
            set => SetValue(InputValueProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="InputValue"/> property.
        /// </summary>
        public static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register(nameof(InputValue), typeof(string), typeof(InputDialog), new PropertyMetadata(""));


        /// <summary>
        /// Gets or sets a value indicating if the input text box contains an invalid value.
        /// </summary>
        private bool ContainsInvalidValue
        {
            get => (bool)GetValue(ContainsInvalidValueProperty);
            set => SetValue(ContainsInvalidValueProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="ContainsInvalidValue"/> property.
        /// </summary>
        private static readonly DependencyProperty ContainsInvalidValueProperty =
            DependencyProperty.Register(nameof(ContainsInvalidValue), typeof(bool), typeof(InputDialog), new PropertyMetadata(false));
        #endregion
        #endregion


        #region Private Methods
        /// <summary>
        /// Unregisters the <see cref="KeyUpHandler(object, KeyEventArgs)"/> method.
        /// </summary>
        private void InputDialog_Unloaded(object sender, RoutedEventArgs e) => Keyboard.RemoveKeyUpHandler(this, KeyUpHandler);


        /// <summary>
        /// Prevents any invalid characters from entering the input text box.
        /// </summary>
        private void InputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = e.Text.Length > 0 && _invalidCharacters.Contains(e.Text[^1]);


        /// <summary>
        /// Shows an error if the input text box contains an invalid value.
        /// </summary>
        private void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var invalidValues = IgnoreInvalidValueCasing ? _invalidValues.ToLowerCase() : _invalidValues;

            //Check if the input text box value is an invalid value.  Take ignoring casing into account
            ContainsInvalidValue = invalidValues != null && invalidValues.Contains(IgnoreInvalidValueCasing ? InputTextBox.Text.ToLower() : InputTextBox.Text);
        }


        /// <summary>
        /// Processes key presses to add behavior to the dialog.
        /// </summary>
        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
            else if (e.Key == Key.Enter && !string.IsNullOrEmpty(InputTextBox.Text) && !ContainsInvalidValue)
            {
                DialogResult = true;
                Close();
            }
        }
        #endregion
    }
}

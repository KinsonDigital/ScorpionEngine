using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class InputDialog : Window
    {
        #region Fields
        private RelayCommand _okCommmand;
        private RelayCommand _cancelCommand;
        private char[] _invalidCharacters;
        private string[] _invalidValues;
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
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="DialogTitle"/> property.
        /// </summary>
        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register(nameof(DialogTitle), typeof(string), typeof(InputDialog), new PropertyMetadata("Input Dialog"));

        /// <summary>
        /// Registers the <see cref="Message"/> property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InputDialog), new PropertyMetadata("Enter some input and press enter"));

        /// <summary>
        /// Registers the <see cref="InputValue"/> property.
        /// </summary>
        public static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register(nameof(InputValue), typeof(string), typeof(InputDialog), new PropertyMetadata(""));

        /// <summary>
        /// Registers the <see cref="ContainsInvalidValue"/> property.
        /// </summary>
        public static readonly DependencyProperty ContainsInvalidValueProperty =
            DependencyProperty.Register(nameof(ContainsInvalidValue), typeof(bool), typeof(InputDialog), new PropertyMetadata(false));
        #endregion


        /// <summary>
        /// Gets or sets the <see cref="InputDialog"/>'s title.
        /// </summary>
        public string DialogTitle
        {
            get { return (string)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the message of the <see cref="InputDialog"/>.
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the input textbox value.
        /// </summary>
        public string InputValue
        {
            get { return (string)GetValue(InputValueProperty); }
            set { SetValue(InputValueProperty, value); }
        }

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

        /// <summary>
        /// Gets or sets a value indicating if the input text box contains an invalid value.
        /// </summary>
        public bool ContainsInvalidValue
        {
            get { return (bool)GetValue(ContainsInvalidValueProperty); }
            set { SetValue(ContainsInvalidValueProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Prevents any invalid characters from entering the input text box.
        /// </summary>
        private void InputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Length > 0 && _invalidCharacters.Contains(e.Text[e.Text.Length - 1]);
        }


        /// <summary>
        /// Shows an error if the input text box contains an invalid value.
        /// </summary>
        private void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var invalidValues = new string[0];

            //If casing is to be ignored, convert all of the invalid values to lower case
            if (IgnoreInvalidValueCasing)
            {
                var processedValues = new List<string>();

                foreach (var value in _invalidValues)
                {
                    processedValues.Add(value.ToLower());
                }

                invalidValues = processedValues.ToArray();
            }
            else
            {
                invalidValues = _invalidValues;
            }

            //Check if the input text box value is an invalid value.  Take ignoring casing into account
            ContainsInvalidValue = invalidValues != null && invalidValues.Contains(IgnoreInvalidValueCasing ? InputTextBox.Text.ToLower() : InputTextBox.Text);
        }
        #endregion

    }
}

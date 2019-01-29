using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Linq;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
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

            DialogTitle = title;
            Message = message;
            InputValue = defaultValue;

            _invalidCharacters = invalidChars;
            //Lower case all of the characters
            for (int i = 0; i < _invalidCharacters.Length; i++)
            {
                _invalidCharacters[i] = _invalidCharacters[i].ToString().ToLower()[0];
            }

            _invalidValues = invalidValues;

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
                    _okCommmand = new RelayCommand(() =>
                    {
                        DialogResult = true;
                        Close();
                    }, () =>
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
                    _cancelCommand = new RelayCommand(() =>
                    {
                        DialogResult = false;
                        Close();
                    }, () => true);


                return _cancelCommand;
            }
        }

        /// <summary>
        /// Gets the result of the input that the user entered.
        /// </summary>
        public string InputResult => InputValue;
        #endregion

        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            return;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                return;

            var isNumberKey = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9
                              ||
                              e.Key >= Key.D0 && e.Key <= Key.D9;

            var typedCharacter = isNumberKey ?
                e.Key.ToString().ToLower()[e.Key.ToString().Length - 1]:
                e.Key.ToString().ToLower()[0];

            if (_invalidCharacters.Contains(typedCharacter))
                e.Handled = true;


            /*
            Key.OemComma
            Key.OemPeriod
            Key.OemMinus
            Key.OemPlus
            Key.Divide
            Key.Multiply
            Key.Subtract
            Key.Add
            Key.Oem1
            Key.Oem2
            Key.Oem3
            Key.Oem4
            Key.Oem5
            Key.Oem6
            Key.Oem7
            */


        }
    }
}

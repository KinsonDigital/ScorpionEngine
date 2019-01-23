using System.Windows;
using System.Windows.Forms.Integration;

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
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="InputDialog"/>.
        /// </summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="message">The dialog message.</param>
        /// <param name="defaultValue">The default value of the dialog input text box.</param>
        public InputDialog(string title, string message, string defaultValue = "")
        {
            InitializeComponent();

            ElementHost.EnableModelessKeyboardInterop(this);

            DialogTitle = title;
            Message = message;
            InputValue = defaultValue;
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
    }
}

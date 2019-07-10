using ParticleMaker.CustomEventArgs;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupListItem.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class SetupListItem : UserControl, IDisposable
    {
        #region Public Events
        /// <summary>
        /// Invoked when the rename button has been clicked.
        /// </summary>
        public event EventHandler<RenameItemEventArgs> RenameClicked;

        /// <summary>
        /// Invoked when the delete button is clicked.
        /// </summary>
        public event EventHandler<ItemEventArgs> DeleteClicked;

        /// <summary>
        /// Invoked when the save button is clicked.
        /// </summary>
        public event EventHandler<ItemEventArgs> SaveClicked;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupListItem"/>.
        /// </summary>
        public SetupListItem() => InitializeComponent();
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the path to the setup file.
        /// Must be a full path with file name.
        /// </summary>
        public string SetupPath
        {
            get => (string)GetValue(SetupPathProperty);
            set => SetValue(SetupPathProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="SetupPath"/>.
        /// </summary>
        public static readonly DependencyProperty SetupPathProperty =
            DependencyProperty.Register(nameof(SetupPath), typeof(string), typeof(SetupListItem), new PropertyMetadata("", SetupFilePathChanged));


        /// <summary>
        /// Gets or sets the setup name to be shown in the setup label.
        /// </summary>
        public string SetupName
        {
            get => (string)GetValue(SetupNameProperty);
            set => SetValue(SetupNameProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="SetupName"/> property.
        /// </summary>
        public static readonly DependencyProperty SetupNameProperty =
            DependencyProperty.Register(nameof(SetupName), typeof(string), typeof(SetupListItem), new PropertyMetadata(""));


        /// <summary>
        /// Gets or sets a value indicating if the control has an error.
        /// </summary>
        protected bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        protected static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(SetupListItem), new PropertyMetadata(false));


        /// <summary>
        /// Gets or sets the command to be executed when the <see cref="SetupListItem"/> has been clicked.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Command"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(SetupListItem), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command that is executed when the rename button has been clicked.
        /// </summary>
        public ICommand RenameClickedCommand
        {
            get => (ICommand)GetValue(RenameClickedCommandProperty);
            set => SetValue(RenameClickedCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="RenameClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty RenameClickedCommandProperty =
            DependencyProperty.Register(nameof(RenameClickedCommand), typeof(ICommand), typeof(SetupListItem), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command that is executed when the delete button has been clicked.
        /// </summary>
        public ICommand DeleteClickedCommand
        {
            get => (ICommand)GetValue(DeleteClickedCommandProperty);
            set => SetValue(DeleteClickedCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="DeleteClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty DeleteClickedCommandProperty =
            DependencyProperty.Register(nameof(DeleteClickedCommand), typeof(ICommand), typeof(SetupListItem), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the command that is executed when the save button has been clicked.
        /// </summary>
        public ICommand SaveClickedCommand
        {
            get => (ICommand)GetValue(SaveClickedCommandProperty);
            set => SetValue(SaveClickedCommandProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="SaveClickedCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty SaveClickedCommandProperty =
            DependencyProperty.Register(nameof(SaveClickedCommand), typeof(ICommand), typeof(SetupListItem), new PropertyMetadata(null));
        #endregion


        #region Public Methods
        /// <summary>
        /// Refreshes the control by updating the control's UI based on if the file exists or not.
        /// </summary>
        public void Refresh()
        {
            var dirExists = DesignerProperties.GetIsInDesignMode(this) ? true : Directory.Exists(SetupPath);
            var pathSections = string.IsNullOrEmpty(SetupPath) || !dirExists ? new string[0] : SetupPath.Split('\\');

            SetupName = pathSections.Length >= 1 ? pathSections[^1] : "";

            HasError = !dirExists;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Updates the setup name to be the file name without the extension
        /// </summary>
        private static void SetupFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (string)e.NewValue;

            if (string.IsNullOrEmpty(newValue))
                return;

            var ctrl = (SetupListItem)d;

            if (ctrl == null)
                return;

            ctrl.Refresh();
        }


        /// <summary>
        /// Renames the selected file.
        /// </summary>
        private void RenameCustomButton_Click(object sender, EventArgs e)
        {
            RenameClicked?.Invoke(this, new RenameItemEventArgs(SetupName, SetupPath));
            RenameClickedCommand?.Execute(new RenameItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Deletes the selected file.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, new ItemEventArgs(SetupName, SetupPath));
            DeleteClickedCommand?.Execute(new ItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Saves the selected file.
        /// </summary>
        private void SaveCustomButton_Click(object sender, EventArgs e)
        {
            SaveClicked?.Invoke(this, new ItemEventArgs(SetupName, SetupPath));
            SaveClickedCommand?.Execute(new ItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Invokes the control clicked event.
        /// </summary>
        private void ItemBorder_MouseUp(object sender, MouseButtonEventArgs e) => Command?.Execute(SetupName);


        /// <summary>
        /// Unsubscribes from any events.
        /// </summary>
        public void Dispose()
        {
            RenameCustomButton.Click -= RenameCustomButton_Click;
            DeleteCustomButton.Click-= DeleteCustomButton_Click;
            SaveCustomButton.Click -= SaveCustomButton_Click;
            ItemBorder.MouseUp -= ItemBorder_MouseUp;
        }
        #endregion
    }
}

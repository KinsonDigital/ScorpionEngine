﻿using ParticleMaker.CustomEventArgs;
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
    public partial class SetupListItem : UserControl
    {
        #region Public Events
        /// <summary>
        /// Invoked when the rename button has been clicked.
        /// </summary>
        public event EventHandler<RenameSetupItemEventArgs> RenameClicked;

        /// <summary>
        /// Invoked when the delete button is clicked.
        /// </summary>
        public event EventHandler<SetupItemEventArgs> DeleteClicked;

        /// <summary>
        /// Invoked when the save button is clicked.
        /// </summary>
        public event EventHandler<SetupItemEventArgs> SaveClicked;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SetupListItem"/>.
        /// </summary>
        public SetupListItem()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="SetupPath"/>.
        /// </summary>
        public static readonly DependencyProperty SetupPathProperty =
            DependencyProperty.Register(nameof(SetupPath), typeof(string), typeof(SetupListItem), new PropertyMetadata("", SetupFilePathChanged));

        /// <summary>
        /// Registers the <see cref="SetupName"/> property.
        /// </summary>
        public static readonly DependencyProperty SetupNameProperty =
            DependencyProperty.Register(nameof(SetupName), typeof(string), typeof(SetupListItem), new PropertyMetadata(""));

        /// <summary>
        /// Registers the <see cref="HasError"/> property.
        /// </summary>
        protected static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(SetupListItem), new PropertyMetadata(false));

        /// <summary>
        /// Registers the <see cref="Command"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(SetupListItem), new PropertyMetadata(null));
        #endregion


        /// <summary>
        /// Gets or sets the path to the setup file.
        /// Must be a full path with file name.
        /// </summary>
        public string SetupPath
        {
            get { return (string)GetValue(SetupPathProperty); }
            set { SetValue(SetupPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the setup name to be shown in the setup label.
        /// </summary>
        public string SetupName
        {
            get { return (string)GetValue(SetupNameProperty); }
            set { SetValue(SetupNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating if the control has an error.
        /// </summary>
        protected bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating if the rename event has been subscribed to.
        /// </summary>
        internal bool IsRenameSubscribed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the delete event has been subscribed to.
        /// </summary>
        internal bool IsDeleteSubscribed { get; set; }

        /// <summary>
        /// Gets or sets the command to be executed when the <see cref="SetupListItem"/> has been clicked.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Refreshs the control by updating the control's UI based on if the file exists or not.
        /// </summary>
        public void Refresh()
        {
            var dirExists = DesignerProperties.GetIsInDesignMode(this) ? true : Directory.Exists(SetupPath);
            var pathSections = string.IsNullOrEmpty(SetupPath) || !dirExists ? new string[0] : SetupPath.Split('\\');

            SetupName = pathSections.Length >= 1 ? pathSections[pathSections.Length - 1] : "";

            HasError = !dirExists;
        }
        #endregion


        #region Internal Methods
        /// <summary>
        /// Subscribes the given handler to the <see cref="RenameClicked"/> event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void SubscribeRenameClicked(EventHandler<RenameSetupItemEventArgs> handler)
        {
            RenameClicked += handler;

            IsRenameSubscribed = true;
        }


        /// <summary>
        /// Subscribes the given handler to the <see cref="DeleteClicked"/> event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void SubscribeDeleteClicked(EventHandler<SetupItemEventArgs> handler)
        {
            DeleteClicked += handler;

            IsDeleteSubscribed = true;
        }


        /// <summary>
        /// Unsubscribes the given handler to the <see cref="RenameClicked"/> event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void UnsubscribeRenameClicked(EventHandler<RenameSetupItemEventArgs> handler)
        {
            RenameClicked -= handler;

            IsRenameSubscribed = false;
        }


        /// <summary>
        /// Unsubscribes the given handler to the <see cref="DeleteClicked"/> event.
        /// </summary>
        /// <param name="handler">The handler to subscribe to.</param>
        internal void UnsubscribeDeleteClicked(EventHandler<SetupItemEventArgs> handler)
        {
            DeleteClicked -= handler;

            IsDeleteSubscribed = false;
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
            RenameClicked?.Invoke(this, new RenameSetupItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Deletes the selected file.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, new SetupItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Saves the selected file.
        /// </summary>
        private void SaveCustomButton_Click(object sender, EventArgs e)
        {
            SaveClicked?.Invoke(this, new SetupItemEventArgs(SetupName, SetupPath));

            Refresh();
        }


        /// <summary>
        /// Invokes the control clicked event.
        /// </summary>
        private void ItemBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var eventArgs = new SetupItemEventArgs(SetupName, SetupPath);
            Command?.Execute(eventArgs);
        }
        #endregion
    }
}

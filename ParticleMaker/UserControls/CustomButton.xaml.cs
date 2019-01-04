using System;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        #region Events
        public event EventHandler<EventArgs> Click;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="CustomButton"/>.
        /// </summary>
        public CustomButton()
        {
            InitializeComponent();
        }
        #endregion

        
        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ButtonContent"/> property.
        /// </summary>
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(nameof(ButtonContent), typeof(FrameworkElement), typeof(CustomButton), new PropertyMetadata(null));
        #endregion


        /// <summary>
        /// Gets or sets the button content.
        /// </summary>
        public FrameworkElement ButtonContent
        {
            get { return (FrameworkElement)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Invokes the <see cref="Click"/> event.
        /// </summary>
        private void CoverRect_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(this, e);
        }
        #endregion
    }
}

using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for RenderSize.xaml
    /// </summary>
    public partial class RenderSize : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RenderSize"/>.
        /// </summary>
        public RenderSize()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the width of the <see cref="RenderSize"/>.
        /// </summary>
        public int RenderWidth
        {
            get { return (int)GetValue(RenderWidthProperty); }
            set { SetValue(RenderWidthProperty, value); }
        }

        /// <summary>
        /// Registers the <see cref="RenderWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty RenderWidthProperty =
            DependencyProperty.Register(nameof(RenderWidth), typeof(int), typeof(RenderSize), new PropertyMetadata(0));


        /// <summary>
        /// Gets or sets the height of the <see cref="RenderSize"/>.
        /// </summary>
        public int RenderHeight
        {
            get { return (int)GetValue(RenderHeightProperty); }
            set { SetValue(RenderHeightProperty, value); }
        }
        
        /// <summary>
        /// Registers the <see cref="RenderHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty RenderHeightProperty =
            DependencyProperty.Register(nameof(RenderHeight), typeof(int), typeof(RenderSize), new PropertyMetadata(0));
        #endregion
    }
}

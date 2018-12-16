using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for RandomSizeRange.xaml
    /// </summary>
    public partial class RandomSizeRange : UserControl
    {
        #region Constructors
        public RandomSizeRange()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="SizeMin"/> property.
        /// </summary>
        public static readonly DependencyProperty SizeMinProperty =
            DependencyProperty.Register(nameof(SizeMin), typeof(float), typeof(RandomSizeRange), new PropertyMetadata(0f));

        /// <summary>
        /// Registers the <see cref="SizeMax"/> property.
        /// </summary>
        public static readonly DependencyProperty SizeMaxProperty =
            DependencyProperty.Register(nameof(SizeMax), typeof(float), typeof(RandomSizeRange), new PropertyMetadata(0f));
        #endregion


        /// <summary>
        /// Gets or sets the minimum size of the size range.
        /// </summary>
        public float SizeMin
        {
            get { return (float)GetValue(SizeMinProperty); }
            set { SetValue(SizeMinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum size of the size range.
        /// </summary>
        public float SizeMax
        {
            get { return (float)GetValue(SizeMaxProperty); }
            set { SetValue(SizeMaxProperty, value); }
        }
        #endregion
    }
}

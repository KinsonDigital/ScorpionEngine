using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for NumberMinMax.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class RandomColorRanges : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RandomColorRanges"/>.
        /// </summary>
        public RandomColorRanges()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Depenency Props
        /// <summary>
        /// Registers the <see cref="RedMin"/> property.
        /// </summary>
        public static readonly DependencyProperty RedMinProperty =
            DependencyProperty.Register(nameof(RedMin), typeof(int), typeof(RandomColorRanges), new PropertyMetadata(0));

        /// <summary>
        /// Registers the <see cref="RedMax"/> property.
        /// </summary>
        public static readonly DependencyProperty RedMaxProperty =
            DependencyProperty.Register(nameof(RedMax), typeof(int), typeof(RandomColorRanges), new PropertyMetadata(255));

        /// <summary>
        /// Registers the <see cref="GreenMin"/> property.
        /// </summary>
        public static readonly DependencyProperty GreenMinProperty =
            DependencyProperty.Register(nameof(GreenMin), typeof(int), typeof(RandomColorRanges), new PropertyMetadata(0));

        /// <summary>
        /// Registers the <see cref="GreenMax"/> property.
        /// </summary>
        public static readonly DependencyProperty GreenMaxProperty =
            DependencyProperty.Register(nameof(GreenMax), typeof(int), typeof(RandomColorRanges), new PropertyMetadata(255));

        /// <summary>
        /// Registers the <see cref="BlueMin"/> property.
        /// </summary>
        public static readonly DependencyProperty BlueMinProperty =
            DependencyProperty.Register(nameof(BlueMin), typeof(int), typeof(RandomColorRanges), new PropertyMetadata(0));

        /// <summary>
        /// Registers the <see cref="BlueMax"/> property.
        /// </summary>
        public static readonly DependencyProperty BlueMaxProperty =
            DependencyProperty.Register(nameof(BlueMax), typeof(int), typeof(RandomColorRanges), new PropertyMetadata(255));
        #endregion


        /// <summary>
        /// Gets or sets the minimum value for the red color component.
        /// </summary>
        [Category("Colors")]
        public int RedMin
        {
            get { return (int)GetValue(RedMinProperty); }
            set { SetValue(RedMinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value for the red color component.
        /// </summary>
        [Category("Colors")]
        public int RedMax
        {
            get { return (int)GetValue(RedMaxProperty); }
            set { SetValue(RedMaxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum value for the green color component.
        /// </summary>
        [Category("Colors")]
        public int GreenMin
        {
            get { return (int)GetValue(GreenMinProperty); }
            set { SetValue(GreenMinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value for the green color component.
        /// </summary>
        [Category("Colors")]
        public int GreenMax
        {
            get { return (int)GetValue(GreenMaxProperty); }
            set { SetValue(GreenMaxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum value for the blue color component.
        /// </summary>
        [Category("Colors")]
        public int BlueMin
        {
            get { return (int)GetValue(BlueMinProperty); }
            set { SetValue(BlueMinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value for the blue color component.
        /// </summary>
        [Category("Colors")]
        public int BlueMax
        {
            get { return (int)GetValue(BlueMaxProperty); }
            set { SetValue(BlueMaxProperty, value); }
        }
        #endregion
    }
}

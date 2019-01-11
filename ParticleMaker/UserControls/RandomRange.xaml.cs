using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for RandomRange.xaml
    /// </summary>
    public partial class RandomRange : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RandomRange"/>.
        /// </summary>
        public RandomRange()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="MinValue"/> property.
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(float), typeof(RandomRange), new PropertyMetadata(0f));

        /// <summary>
        /// Registers the <see cref="MinLowerLimit"/>.
        /// </summary>
        public static readonly DependencyProperty MinLowerLimitProperty =
            DependencyProperty.Register(nameof(MinLowerLimit), typeof(float), typeof(RandomRange), new PropertyMetadata(0f));

        /// <summary>
        /// Registers the <see cref="MinUpperLimit"/>.
        /// </summary>
        public static readonly DependencyProperty MinUpperLimitProperty =
            DependencyProperty.Register(nameof(MinUpperLimit), typeof(float), typeof(RandomRange), new PropertyMetadata(10f));

        /// <summary>
        /// Registers the <see cref="MinValueIncrement"/> property.
        /// </summary>
        public static readonly DependencyProperty MinValueIncrementProperty =
            DependencyProperty.Register(nameof(MinValueIncrement), typeof(float), typeof(RandomRange), new PropertyMetadata(1f));

        /// <summary>
        /// Registers the <see cref="MinValueDecrement"/> property.
        /// </summary>
        public static readonly DependencyProperty MinValueDecrementProperty =
            DependencyProperty.Register(nameof(MinValueDecrement), typeof(float), typeof(RandomRange), new PropertyMetadata(1f));

        /// <summary>
        /// Registers the <see cref="MaxValue"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(float), typeof(RandomRange), new PropertyMetadata(0f));

        /// <summary>
        /// Registers the <see cref="MaxLowerLimit"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxLowerLimitProperty =
            DependencyProperty.Register(nameof(MaxLowerLimit), typeof(float), typeof(RandomRange), new PropertyMetadata(0f));

        /// <summary>
        /// Registers the <see cref="MaxUpperLimit"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxUpperLimitProperty =
            DependencyProperty.Register(nameof(MaxUpperLimit), typeof(float), typeof(RandomRange), new PropertyMetadata(10f));

        /// <summary>
        /// Registers the <see cref="MaxValueIncrement"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxValueIncrementProperty =
            DependencyProperty.Register(nameof(MaxValueIncrement), typeof(float), typeof(RandomRange), new PropertyMetadata(1f));

        /// <summary>
        /// Registers the <see cref="MaxValueDecrement"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxValueDecrementProperty =
            DependencyProperty.Register(nameof(MaxValueDecrement), typeof(float), typeof(RandomRange), new PropertyMetadata(1f));

        /// <summary>
        /// Registers the <see cref="Title"/> property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(RandomRange), new PropertyMetadata("Random Range"));

        /// <summary>
        /// Registers the <see cref="MinTitle"/> property.
        /// </summary>
        public static readonly DependencyProperty MinTitleProperty =
            DependencyProperty.Register(nameof(MinTitle), typeof(string), typeof(RandomRange), new PropertyMetadata("Min"));

        /// <summary>
        /// Registers the <see cref="MaxTitle"/> property.
        /// </summary>
        public static readonly DependencyProperty MaxTitleProperty =
            DependencyProperty.Register(nameof(MaxTitle), typeof(string), typeof(RandomRange), new PropertyMetadata("Max"));
        #endregion


        /// <summary>
        /// Gets or sets the range minimum value.
        /// </summary>
        public float MinValue
        {
            get { return (float)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the lower limit of the <see cref="MinValue"/>.
        /// </summary>
        public float MinLowerLimit
        {
            get { return (float)GetValue(MinLowerLimitProperty); }
            set { SetValue(MinLowerLimitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the upper limit of the <see cref="MinValue"/>.
        /// </summary>
        public float MinUpperLimit
        {
            get { return (float)GetValue(MinUpperLimitProperty); }
            set { SetValue(MinUpperLimitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for how much the <see cref="MinValue"/> will increment.
        /// </summary>
        public float MinValueIncrement
        {
            get { return (float)GetValue(MinValueIncrementProperty); }
            set { SetValue(MinValueIncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for how much the <see cref="MinValue"/> will decrement.
        /// </summary>
        public float MinValueDecrement
        {
            get { return (float)GetValue(MinValueDecrementProperty); }
            set { SetValue(MinValueDecrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the range maximum value.
        /// </summary>
        public float MaxValue
        {
            get { return (float)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the lower limit of the <see cref="MaxValue"/>.
        /// </summary>
        public float MaxLowerLimit
        {
            get { return (float)GetValue(MaxLowerLimitProperty); }
            set { SetValue(MaxLowerLimitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the upper limit of the <see cref="MaxValue"/>.
        /// </summary>
        public float MaxUpperLimit
        {
            get { return (float)GetValue(MaxUpperLimitProperty); }
            set { SetValue(MaxUpperLimitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for how much the <see cref="MaxValue"/> will increment.
        /// </summary>
        public float MaxValueIncrement
        {
            get { return (float)GetValue(MaxValueIncrementProperty); }
            set { SetValue(MaxValueIncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for how much the <see cref="MaxValue"/> will decrement.
        /// </summary>
        public float MaxValueDecrement
        {
            get { return (float)GetValue(MaxValueDecrementProperty); }
            set { SetValue(MaxValueDecrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets title of the min value.
        /// </summary>
        public string MinTitle
        {
            get { return (string)GetValue(MinTitleProperty); }
            set { SetValue(MinTitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title of the max value.
        /// </summary>
        public string MaxTitle
        {
            get { return (string)GetValue(MaxTitleProperty); }
            set { SetValue(MaxTitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title of the control.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion
    }
}

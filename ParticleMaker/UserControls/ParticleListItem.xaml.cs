using System.Windows;
using System.Windows.Controls;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ParticleListItem.xaml
    /// </summary>
    public partial class ParticleListItem : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleListItem"/>.
        /// </summary>
        public ParticleListItem()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Creates a new instance of <see cref="ParticleName"/>.
        /// </summary>
        protected static readonly DependencyProperty ParticleNameProperty =
            DependencyProperty.Register(nameof(ParticleName), typeof(string), typeof(ParticleListItem), new PropertyMetadata(""));
        #endregion


        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        protected string ParticleName
        {
            get { return (string)GetValue(ParticleNameProperty); }
            set { SetValue(ParticleNameProperty, value); }
        }
        #endregion
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ParticleList.xaml
    /// </summary>
    public partial class ParticleList : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleList"/>.
        /// </summary>
        public ParticleList()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="Particles"/> property.
        /// </summary>
        public static readonly DependencyProperty ParticlesProperty =
            DependencyProperty.Register(nameof(Particles), typeof(PathItem[]), typeof(SetupList), new PropertyMetadata(new PathItem[0], ParticlesChanged));
        #endregion

        /// <summary>
        /// Gets or sets the list of particle paths.
        /// </summary>
        public PathItem[] Particles
        {
            get { return (PathItem[])GetValue(ParticlesProperty); }
            set { SetValue(ParticlesProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Refreshes the list when the list of particle path items change.
        /// </summary>
        private static void ParticlesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SetupList)d;

            if (ctrl == null)
                return;
        }
        #endregion
    }
}

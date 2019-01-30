using System;
using System.IO;
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

        /// <summary>
        /// Registers the <see cref="ParticleFilePath"/> property.
        /// </summary>
        public static readonly DependencyProperty ParticleFilePathProperty =
            DependencyProperty.Register(nameof(ParticleFilePath), typeof(string), typeof(ParticleListItem), new PropertyMetadata("", ParticlePathChanged));

        /// <summary>
        /// Sets the <see cref="ParticleName"/> property.
        /// </summary>
        private static void ParticlePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (ParticleListItem)d;

            if (ctrl == null)
                return;

            var filePath = e.NewValue as string;

            if (string.IsNullOrEmpty(filePath))
            {
                ctrl.ParticleName = "";
            }
            else
            {
                if (File.Exists(filePath))
                {
                    ctrl.ParticleName = Path.GetFileNameWithoutExtension(filePath);

                    //TODO: Set the error bool prop to false
                }
                else
                {
                    ctrl.ParticleName = "";
                    //TODO: Set the error bool prop to false
                }
            }
        }
        #endregion


        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        protected string ParticleName
        {
            get { return (string)GetValue(ParticleNameProperty); }
            set { SetValue(ParticleNameProperty, value); }
        }


        /// <summary>
        /// Gets or sets the particle file path.
        /// </summary>
        public string ParticleFilePath
        {
            get { return (string)GetValue(ParticleFilePathProperty); }
            set { SetValue(ParticleFilePathProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Invokes the rename event.
        /// </summary>
        private void RenameCustomButton_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Invokes the delete eveent.
        /// </summary>
        private void DeleteCustomButton_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}

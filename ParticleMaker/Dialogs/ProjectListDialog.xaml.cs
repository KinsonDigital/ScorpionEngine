using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectListDialog.xaml
    /// </summary>
    public partial class ProjectListDialog : Window
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectListDialog"/>.
        /// </summary>
        public ProjectListDialog()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="ProjectPaths"/> property.
        /// </summary>
        public static readonly DependencyProperty ProjectPathsProperty =
            DependencyProperty.Register(nameof(ProjectPaths), typeof(string[]), typeof(ProjectListDialog), new PropertyMetadata(new string[0]));
        #endregion


        /// <summary>
        /// Gets or sets the paths to all of the projects.
        /// </summary>
        public string[] ProjectPaths
        {
            get { return (string[])GetValue(ProjectPathsProperty); }
            set { SetValue(ProjectPathsProperty, value); }
        }
        #endregion
    }
}

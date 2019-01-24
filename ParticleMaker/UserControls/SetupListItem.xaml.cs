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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for SetupListItem.xaml
    /// </summary>
    public partial class SetupListItem : UserControl
    {
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
            DependencyProperty.Register(nameof(SetupPath), typeof(string), typeof(SetupListItem), new PropertyMetadata(""));
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
        #endregion
    }
}

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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for RenderSurface.xaml
    /// </summary>
    public partial class RenderSurface : Window
    {
        public RenderSurface()
        {
            InitializeComponent();
        }


        public IntPtr WindowHandle => new WindowInteropHelper(this).Handle;
    }
}

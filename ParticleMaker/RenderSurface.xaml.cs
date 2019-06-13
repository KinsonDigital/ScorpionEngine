using System;
using System.Windows;
using System.Windows.Interop;

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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for RenderSurface.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class RenderSurface : Window
    {
        public RenderSurface()
        {
            InitializeComponent();
        }


        public IntPtr WindowHandle => new WindowInteropHelper(this).Handle;
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for the <see cref="RenderSurface"/> window.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class RenderSurface : Window
    {
        #region Constructors
        public RenderSurface() => InitializeComponent();
        #endregion


        #region Props
        /// <summary>
        /// Gets the pointer/window handle of the windo.
        /// </summary>
        public IntPtr WindowHandle => new WindowInteropHelper(this).Handle;
        #endregion
    }
}

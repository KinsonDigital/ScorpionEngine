using KDScorpionCore;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using MediaColor = System.Windows.Media.Color;

namespace ParticleMaker
{
    /// <summary>
    /// Provides various extension methods for use throughout the application.
    /// </summary>
    public static class ExtensionMethods
    {
        #region PInvoke Functions
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion


        #region Fields
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int SW_HIDE = 0x00;
        private const int SW_SHOW = 0x05;
        private const float PI = 3.1415926535897931f;
        private static CancellationTokenSource _tokenSrc;
        private static Task _hideWindowTask;
        #endregion


        #region Methods
        /// <summary>
        /// Converts the given degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns></returns>
        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }


        /// <summary>
        /// Returns the given <see cref="GameColor"/> type to a <see cref="ColorItem"/>.
        /// </summary>
        /// <param name="clr">The color to convert.</param>
        /// <returns></returns>
        public static ColorItem ToColorItem(this GameColor clr)
        {
            return new ColorItem()
            {
                ColorBrush = new SolidColorBrush(MediaColor.FromArgb(clr.Alpha, clr.Red, clr.Green, clr.Blue))
            };
        }


        /// <summary>
        /// Converts the givent <paramref name="gameTime"/> to the type <see cref="EngineTime"/>.
        /// </summary>
        /// <param name="gameTime">The <see cref="GameTime"/> object to convert.</param>
        /// <returns></returns>
        public static EngineTime ToEngineTime(this GameTime gameTime)
        {
            return new EngineTime() { ElapsedEngineTime = gameTime.ElapsedGameTime };
        }


        /// <summary>
        /// Joins all of the strings in the given list of <paramref name="items"/> and will exclude any items
        /// in the list that match the given <paramref name="excludeValue"/>.
        /// </summary>
        /// <param name="items">The list of items to join.</param>
        /// <param name="excludeValue">The item to exclude from the join process.</param>
        /// <returns></returns>
        public static string Join(this string[] items, string excludeValue = "")
        {
            var result = new StringBuilder();

            for (int i = 0; i < items.Length; i++)
            {
                var joinItem = items[i] != excludeValue;

                if (joinItem)
                    result.Append($@"{items[i]}\");
            }


            return result.ToString().TrimEnd('\\');
        }


        /// <summary>
        /// Gets all of the visual children of the given type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of children to retrieve.</typeparam>
        /// <param name="parent">The parent object that owns the children being searched for.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


        /// <summary>
        /// Hides the game window.
        /// </summary>
        /// <param name="window">The window to hide.</param>
        [ExcludeFromCodeCoverage]
        public static void Hide(this GameWindow window)
        {
            var windowHandle = window?.Handle;

            if (windowHandle == null)
                return;

            _tokenSrc = new CancellationTokenSource();

            _hideWindowTask = new Task(() =>
            {
                while (!_tokenSrc.IsCancellationRequested)
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        _tokenSrc.Token.WaitHandle.WaitOne(62);

                        var objWrapper = new object();
                        var handle = new HandleRef(objWrapper, windowHandle.Value);

                        SetWindowLong32(handle, GWL_EXSTYLE, ~WS_EX_APPWINDOW);
                        ShowWindow(handle.Handle, SW_HIDE);

                        _tokenSrc.Cancel();
                    });
                }
            });

            _hideWindowTask.Start();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NETColor = System.Drawing.Color;
using System.Windows.Forms;
using System.Windows.Media;
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
        public static float ToRadians(this float degrees) => degrees * PI / 180f;


        /// <summary>
        /// Returns the given <see cref="GameColor"/> type to a <see cref="ColorItem"/>.
        /// </summary>
        /// <param name="clr">The color to convert.</param>
        /// <returns></returns>
        public static ColorItem ToColorItem(this NETColor clr) =>
            new ColorItem() { ColorBrush = new SolidColorBrush(MediaColor.FromArgb(clr.A, clr.R, clr.G, clr.B)) };


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
        /// Returns the parent of this <see cref="DependencyObject"/> that matches the
        /// given <see cref="DependencyObject"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The parent type of <see cref="DependencyObject"/> to find.</typeparam>
        /// <param name="child">The child that is contained/owned by the parent.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //Get parent item
            var parentObject = VisualTreeHelper.GetParent(child);

            //Return if we have reached the end of the tree
            if (parentObject == null) return null;

            //Check if the parent matches the type we're looking for
            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
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
        /// Finds all of the children <see cref="UIElement"/>s of the given type <typeparamref name="T"/>
        /// that are contained by the given <paramref name="parent"/>.
        /// </summary>
        /// <typeparam name="T">Any type that inherits <see cref="UIElement"/></typeparam>
        /// <param name="parent">The parent <see cref="DependencyObject"/> that contains the child elements.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static List<T> AllChildren<T>(this DependencyObject parent) where T : UIElement
        {
            var result = new List<T>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                    result.Add(child as T);

                result.AddRange(AllChildren<T>(child));
            }


            return result;
        }


        /// <summary>
        /// Returns a value indicating if the given string <paramref name="value"/> contains any 
        /// illegal project name characters.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns></returns>
        public static bool ContainsIllegalFileNameCharacters(this string value)
        {
            var characters = Path.GetInvalidPathChars();

            foreach (var c in characters)
            {
                if (value.Contains(c.ToString()))
                    return true;
            }


            return false;
        }


        /// <summary>
        /// Gets a list of all the property names for this object.
        /// </summary>
        /// <param name="obj">The object to get the property names from.</param>
        /// <returns></returns>
        public static string[] GetPropertyNames(this object obj) => (from n in obj.GetType().GetProperties() select n.Name).ToArray();


        /// <summary>
        /// Returns all of the strings as lowercase.
        /// </summary>
        /// <param name="values">The string values to convert.</param>
        /// <returns></returns>
        public static string[] ToLowerCase(this string[] values)
        {
            var result = new List<string>();

            foreach (var value in values)
                result.Add(value.ToLower());


            return result.ToArray();
        }


        /// <summary>
        /// Converts the given width <paramref name="value"/> from WPF points to pixels.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static int WidthToPixels(this int value) => (int)(value * Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.WorkArea.Width);


        /// <summary>
        /// Converts the given height <paramref name="value"/> from WPF points to pixels.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static int HeightToPixels(this int value) => (int)(value * Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.WorkArea.Height);


        /// <summary>
        /// Converts the given width <paramref name="value"/> from pixels to WPF points.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static int WidthToPoints(this int value) => (int)(value * SystemParameters.WorkArea.Width / Screen.PrimaryScreen.WorkingArea.Width);


        /// <summary>
        /// Converts the given height <paramref name="value"/> from pixels to WPF points.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static int HeightToPoints(this int value) => (int)(value * SystemParameters.WorkArea.Width / Screen.PrimaryScreen.WorkingArea.Width);


        /// <summary>
        /// Converts the given width <paramref name="value"/> from WPF points to pixels.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static double WidthToPixels(this double value) => value * Screen.PrimaryScreen.WorkingArea.Width / SystemParameters.WorkArea.Width;


        /// <summary>
        /// Converts the given height <paramref name="value"/> from WPF points to pixels.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static double HeightToPixels(this double value) => value * Screen.PrimaryScreen.WorkingArea.Height / SystemParameters.WorkArea.Height;


        /// <summary>
        /// Converts the given width <paramref name="value"/> from pixels to WPF points.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static double WidthToPoints(this double value) => value * SystemParameters.WorkArea.Width / Screen.PrimaryScreen.WorkingArea.Width;


        /// <summary>
        /// Converts the given height <paramref name="value"/> from pixels to WPF points.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        public static double HeightToPoints(this double value) => value * SystemParameters.WorkArea.Width / Screen.PrimaryScreen.WorkingArea.Width;


        /// <summary>
        /// Converts the element width from WPF points to pixels.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static double WidthToPixels(this FrameworkElement element) => element.Width.WidthToPixels();


        /// <summary>
        /// Converts the element height from WPF points to pixels.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static double HeightToPixels(this FrameworkElement element) => element.Height.HeightToPixels();


        /// <summary>
        /// Converts the element width from pixels to WPF points.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static double WidthToPoints(this FrameworkElement element) => element.Height.WidthToPoints();


        /// <summary>
        /// Converts the element height from pixels to WPF points.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static double HeightToPoints(this FrameworkElement element) => element.Height.HeightToPoints();
        #endregion
    }
}

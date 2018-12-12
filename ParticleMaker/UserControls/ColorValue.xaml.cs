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
    /// Interaction logic for ColorValue.xaml
    /// </summary>
    public partial class ColorValue : UserControl
    {
        #region Constructors
        public ColorValue()
        {
            InitializeComponent();

            DataContext = this;
        }
        #endregion


        #region Props
        #region Dependency Props
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register(nameof(Red), typeof(int), typeof(ColorValue), new PropertyMetadata(0, RedChangedCallback));

        private static void RedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register(nameof(Green), typeof(int), typeof(ColorValue), new PropertyMetadata(0));

        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register(nameof(Blue), typeof(int), typeof(ColorValue), new PropertyMetadata(0));

        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register(nameof(Alpha), typeof(int), typeof(ColorValue), new PropertyMetadata(255));
        #endregion


        public int Red
        {
            get { return (int)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }



        public int Green
        {
            get { return (int)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }



        public int Blue
        {
            get { return (int)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }



        public int Alpha
        {
            get { return (int)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }
        #endregion


        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}

using ParticleMaker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ParticleMaker
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private MainViewModel _mainViewModel;


        public TestWindow()
        {
            InitializeComponent();

            _mainViewModel = new MainViewModel
            {
                Colors = new ObservableCollection<ColorItem>()
                {
                    new ColorItem()
                    {
                        Id = 11,
                        ColorBrush = new SolidColorBrush(Color.FromRgb(50, 125, 50))
                    },
                    new ColorItem()
                    {
                        Id = 22,
                        ColorBrush = new SolidColorBrush(Color.FromRgb(50, 50, 200))
                    }
                }
            };

            DataContext = _mainViewModel;
        }
    }
}

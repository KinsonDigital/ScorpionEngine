using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorList.xaml
    /// </summary>
    public partial class ColorList : UserControl
    {
        public ColorList()
        {
            //TODO: Test Data, remove
            Colors = new ObservableCollection<ColorItem>
            {
                new ColorItem { ColorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)), Id = 1 },
                new ColorItem { ColorBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)), Id = 2 }
            };

            InitializeComponent();
        }


        public ObservableCollection<ColorItem> OldColors { get; set; }

        public ObservableCollection<ColorItem> Colors
        {
            get { return (ObservableCollection<ColorItem>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register(nameof(Colors), typeof(ObservableCollection<ColorItem>), typeof(ColorList), new PropertyMetadata(new ObservableCollection<ColorItem>()));



        private void ColorListItem_DeleteClicked(object sender, ColorItemClickedEventArgs e)
        {
            MessageBox.Show($"Item '{e.Id}' was clicked for deletion.");
        }


        private void ColorListItem_EditColorClicked(object sender, ColorItemClickedEventArgs e)
        {
            var colorPicker = new ColorPicker(e.Color);

            colorPicker.ShowDialog();

            if (colorPicker.DialogResult == true)
            {
                for (int i = 0; i < Colors.Count; i++)
                {
                    if (Colors[i].Id == e.Id)
                    {
                        Colors[i] = new ColorItem()
                        {
                            Id = e.Id,
                            ColorBrush = new SolidColorBrush(colorPicker.ChosenColor)
                        };

                        break;
                    }
                }
            }
        }
    }
}

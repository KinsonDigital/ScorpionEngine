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
            //Test Data
            MyItems = new[]
            {
                new ColorItem { ColorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)), Id = 1 },
                new ColorItem { ColorBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)), Id = 2 }
            };

            InitializeComponent();
        }


        public ColorItem[] MyItems { get; set; }

        private void ColorListItem_DeleteClicked(object sender, ColorItemClickedEventArgs e)
        {
            MessageBox.Show($"Item '{e.Id}' was clicked for deletion.");
        }

        private void ColorListItem_EditColorClicked(object sender, ColorItemClickedEventArgs e)
        {
            MessageBox.Show($"Item '{e.Id}' was clicked for color edit.");
        }
    }
}

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
        #region Constructors
        public ColorList()
        {
            InitializeComponent();
        }
        #endregion


        #region Props
        #region Dependency Props
        /// <summary>
        /// Registers the <see cref="Colors"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register(nameof(Colors), typeof(ObservableCollection<ColorItem>), typeof(ColorList), new PropertyMetadata(new ObservableCollection<ColorItem>()));
        #endregion


        /// <summary>
        /// Gets or sets the list of colors in the list.
        /// </summary>
        public ObservableCollection<ColorItem> Colors
        {
            get { return (ObservableCollection<ColorItem>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Edits a color in the color list.
        /// </summary>
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


        /// <summary>
        /// Deletes a color list item from the list.
        /// </summary>
        private void ColorListItem_DeleteClicked(object sender, ColorItemClickedEventArgs e)
        {
            var deleteResult = MessageBox.Show("Are you sure you want to delete this color?", "Delete Color?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (deleteResult == MessageBoxResult.Yes)
            {
                for (int i = 0; i < Colors.Count; i++)
                {
                    if (Colors[i].Id == e.Id)
                    {
                        Colors.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}

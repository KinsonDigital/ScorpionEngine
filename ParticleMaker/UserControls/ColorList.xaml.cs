﻿using ParticleMaker.CustomEventArgs;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Interaction logic for ColorList.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ColorList : UserControl
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorList"/>.
        /// </summary>
        public ColorList() => InitializeComponent();
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the list of colors in the list.
        /// </summary>
        public ColorItem[] Colors
        {
            get => (ColorItem[])GetValue(ColorsProperty);
            set => SetValue(ColorsProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="Colors"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register(nameof(Colors), typeof(ColorItem[]), typeof(ColorList), new PropertyMetadata(new ColorItem[0]));
        #endregion


        #region Private Methods
        /// <summary>
        /// Adds a new color item to the list.
        /// </summary>
        private void AddNewItemButton_Click(object sender, System.EventArgs e)
        {
            var colorPicker = new ColorPicker(Color.FromRgb(255, 255, 255));

            colorPicker.ShowDialog();

            if (colorPicker.DialogResult == true)
            {
                var colors = new List<ColorItem>(Colors);

                var newId = Colors.Length <= 0 ? 1 : Colors.Max(c => c.Id) + 1;

                colors.Add(new ColorItem()
                {
                    Id = newId,
                    ColorBrush = new SolidColorBrush(colorPicker.ChosenColor)
                });

                Colors = colors.ToArray();
            }
        }


        /// <summary>
        /// Edits a color in the color list.
        /// </summary>
        private void ColorListItem_EditColorClicked(object sender, ColorItemClickedEventArgs e)
        {
            var colorPicker = new ColorPicker(e.Color);

            colorPicker.ShowDialog();

            if (colorPicker.DialogResult == true)
            {
                for (int i = 0; i < Colors.Length; i++)
                {
                    if (Colors[i].Id == e.Id)
                    {
                        Colors[i] = new ColorItem()
                        {
                            Id = e.Id,
                            ColorBrush = new SolidColorBrush(colorPicker.ChosenColor)
                        };

                        SetValue(ColorsProperty, Colors);
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
                var colors = new List<ColorItem>(Colors);

                for (int i = 0; i < colors.Count; i++)
                {
                    if (Colors[i].Id == e.Id)
                    {
                        colors.RemoveAt(i);

                        Colors = colors.ToArray();
                        break;
                    }
                }
            }
        }
        #endregion
    }
}

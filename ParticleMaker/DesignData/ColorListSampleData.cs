using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ParticleMaker.DesignData
{
    public class ColorListSampleData
    {
        public ObservableCollection<ColorItem> Colors { get; set; }

        public SolidColorBrush ColorValue { get; set; }
    }
}

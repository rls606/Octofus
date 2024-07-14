using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Octofus.Views.Visualizer
{
    /// <summary>
    /// Logique d'interaction pour Portrait.xaml
    /// </summary>
    public partial class Portrait : UserControl
    {
        public string Name { get; private set; }

        public int Order { get; set; }

        public Portrait()
        {
            InitializeComponent();

            Border.Opacity = 0.5;
            Border.BorderBrush = Brushes.Black;
        }

        public void SetImage(string name)
        {
            Name = name;

            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var image = System.IO.Path.Combine(path, "Images/", name + ".png");
            if (System.IO.File.Exists(image))
            {
                Image.Source = new BitmapImage(new Uri(image));
            }
            else
            {
                var defaultImage = System.IO.Path.Combine(path, "Images/", "nopic.png");
                Image.Source = new BitmapImage(new Uri(defaultImage));
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                Border.Opacity = 1.0;
                Border.BorderBrush = Brushes.Yellow;
            }
            else
            {
                Border.Opacity = 0.5;
                Border.BorderBrush = Brushes.Black;
            }
        }
    }
}

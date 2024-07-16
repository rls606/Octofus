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
using System.Xml.Linq;

namespace Octofus.Views.Visualizer
{
    /// <summary>
    /// Logique d'interaction pour Portrait.xaml
    /// </summary>
    public partial class Portrait : UserControl
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public Portrait(string name)
        {
            InitializeComponent();
            Name = name;
            Border.Opacity = 0.5;
            Border.BorderBrush = Brushes.Black;
        }

        public void SetImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                Image.Source = new BitmapImage(new Uri(path));
            }
            else
            {
                var executingPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var defaultImage = System.IO.Path.Combine(executingPath, "Images/", "nopic.png");
                Image.Source = new BitmapImage(new Uri(defaultImage));
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                Border.Opacity = 1.0;
                Border.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 60, 65));
            }
            else
            {
                Border.Opacity = 0.2;
                Border.BorderBrush = new SolidColorBrush(Color.FromRgb(179, 60, 65));
            }
        }
    }
}

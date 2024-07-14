using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Octofus
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AppController _controller { get; set; }

        public App()
        {
            _controller = new AppController();
            _controller.Start();
        }
    }
}

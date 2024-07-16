using Octofus.Common;
using Octofus.Data;
using Octofus.Options;
using Octofus.Options.Configuration;
using Octofus.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Octofus.Views.Visualizer
{
    /// <summary>
    /// Logique d'interaction pour VisualizerView.xaml
    /// </summary>
    public partial class VisualizerView : Window
    {
        #region ImagePath

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(Portrait), new PropertyMetadata(string.Empty));

        #endregion

        private AppController AppController { get; set; } 
        private bool isDragging { get; set; }

        private Point offset { get; set; }

        public VisualizerView(WindowPosition windowPosition, AppController appController)
        {
            InitializeComponent();
            ApplyConfiguration(windowPosition);
            this.DataContext = this;

            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ImagePath = System.IO.Path.Combine(path, "assets", "logo.png");

            isDragging = false;

            ProcessManager.InstancesUpdated += ProcessManager_InstancesUpdated;
            ProcessManager.FocusChanged += ProcessManager_FocusChanged;

            MouseLeftButtonDown += VisualizerView_MouseLeftButtonDown;
            MouseLeftButtonUp += VisualizerView_MouseLeftButtonUp;
            MouseMove += VisualizerView_MouseMove;
            AppController = appController;
        }

        #region MovingWindow

        private void VisualizerView_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point mousePosition = e.GetPosition(this);
                double deltaX = mousePosition.X - offset.X;
                double deltaY = mousePosition.Y - offset.Y;
                Left += deltaX;
                Top += deltaY;
            }
        }

        private void VisualizerView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            ReleaseMouseCapture();
        }

        private void VisualizerView_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDragging = true;
            offset = e.GetPosition(this);
            CaptureMouse();
        }

        #endregion

        private void ApplyConfiguration(WindowPosition windowPosition)
        {
            Top = windowPosition.Top;
            Left = windowPosition.Left;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //WinAPI.SetTransparent(this);
        }

        private void ProcessManager_FocusChanged(object sender, string name)
        {
            foreach (Portrait portrait in Stack.Children)
            {
                portrait.SetSelected(portrait.Name == name);
            }
        }

        private void ProcessManager_InstancesUpdated(object sender, List<string> names)
        {
            RemoveInvalidPortraits(names);
            AddNewPortraits(names);
            AdjustWindowSize(names.Count);
            Reorder();
        }

        private void AddNewPortraits(List<string> names)
        {
            for(var i = 0; i < names.Count; i++) {        
                var portrait = new Portrait();
                portrait.SetImage(AppController.GetCharacterImage(names[i]));
                Stack.Children.Insert(i, portrait);
            }
        }

        private void RemoveInvalidPortraits(List<string> names)
        {
            var toRemove = new List<Portrait>();
            foreach (Portrait portrait in Stack.Children)
            {
                if (names.Contains(portrait.Name))
                {
                    names.Remove(portrait.Name);
                }
                else
                {
                    toRemove.Add(portrait);
                }
            }

            foreach (var portrait in toRemove)
            {
                Stack.Children.Remove(portrait);
            }
        }

        private void Reorder()
        {
            var mustReorder = false;
            var lastIndex = 0;
            var portraits = new List<Portrait>();
            foreach (Portrait portrait in Stack.Children)
            {
                portraits.Add(portrait);

                if (portrait.Order < lastIndex)
                {
                    mustReorder = true;
                }
                lastIndex = portrait.Order;
            }

            if (mustReorder)
            {
                Stack.Children.Clear();
                foreach (var portrait in portraits.OrderBy(a => a.Order))
                {
                    Stack.Children.Add(portrait);
                }
            }
        }

        private void AdjustWindowSize(int portraitNumber)
        {
            //if (portraitNumber == 0)
            //{
            //    Width = 0;
            //    Height = 0;
            //}
            //else
            //{
            //    Width = (50 + 5) * portraitNumber;
            //    Height = (210 + 5) * portraitNumber;
            //}
        }

        private void SavePosition()
        {
            var options = new WindowPosition()
            {
                Left = Left,
                Top = Top
            };

            OptionsService.SaveWindowPosition(options);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SavePosition();

            base.OnClosing(e);

            ProcessManager.InstancesUpdated -= ProcessManager_InstancesUpdated;
            ProcessManager.FocusChanged -= ProcessManager_FocusChanged;

            MouseLeftButtonDown -= VisualizerView_MouseLeftButtonDown;
            MouseLeftButtonUp -= VisualizerView_MouseLeftButtonUp;
            MouseMove -= VisualizerView_MouseMove;
        }
    }
}

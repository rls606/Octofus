using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace Octofus.Views.Configuration.SubView
{
    /// <summary>
    /// Logique d'interaction pour AccountKey.xaml
    /// </summary>
    public partial class AccountKey : UserControl
    {
        #region ImagePath

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapImage), typeof(AccountKey), new PropertyMetadata(null));

        #endregion

        private AccountKeyInformations _informations { get; set; }

        public event EventHandler RemoveRequested;

        private void OnRemoveAccount(object sender, RoutedEventArgs e)
        {
            RemoveRequested?.Invoke(this, EventArgs.Empty);
        }

        public AccountKey(List<string> items, string accountName, string key, string imagePath)
        { 
            InitializeComponent();
            _informations = new AccountKeyInformations();
            _informations.Key = key;
            _informations.AccountName = accountName;
            _informations.ImagePath = imagePath;

            InitializePictures(imagePath);

            if (string.IsNullOrWhiteSpace(key))
            {
                key = "Non assigné";
            }

            button.Content = key;
            items.Add(accountName);
            comboBox.ItemsSource = items;
            comboBox.Text = accountName;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void InitializePictures(string imagePath)
        {
            var path = string.Empty;
            if(!string.IsNullOrWhiteSpace(imagePath))
            {
                path = imagePath;
            }
            else
            {
                var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                path = Path.Combine(executingPath, "Images", "nopic.png");
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.DecodePixelWidth = 200;
            bitmap.EndInit();

            avatar.Source = bitmap;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _informations.AccountName = comboBox.SelectedItem as string;
        }

        private void OnAddImage(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.DefaultExt = ".png"; 
            dialog.Filter = "png file (.png)|*.png";      
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.DecodePixelWidth = 200;
                bitmap.EndInit();

                Console.WriteLine((int)bitmap.PixelHeight);
                Console.WriteLine((int)bitmap.PixelWidth);
                avatar.Source = bitmap;
                _informations.ImagePath = dialog.FileName;
            }
        }

        private void OnAssignKey(object sender, RoutedEventArgs e)
        {
            button.Content = "Appuyez sur une touche";
            this.KeyDown += AccountKey_KeyDown;
        }

        private void AccountKey_KeyDown(object sender, KeyEventArgs e)
        {
            var pressedKey = e.Key.ToString();
            button.Content = pressedKey;
            _informations.Key = pressedKey;
            this.KeyDown -= AccountKey_KeyDown;
        }

        public AccountKeyInformations GetAccountBinding()
        {
            if (_informations.IsInformationsValid()) 
            {
                return _informations;
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Octofus.Views.Configuration.SubView
{
    /// <summary>
    /// Logique d'interaction pour AccountKey.xaml
    /// </summary>
    public partial class AccountKey : UserControl
    {
        private string Key { get; set; }

        private string AccountName { get; set; }

        public AccountKey(List<string> items, string accountName, string key)
        { 
            InitializeComponent();

            Key = key;
            AccountName = accountName;

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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccountName = comboBox.SelectedItem as string;
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
            Key = pressedKey;
            this.KeyDown -= AccountKey_KeyDown;
        }

        public Tuple<string, string> GetAccountBinding()
        {
            if (!string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(AccountName)) 
            { 
                return Tuple.Create(Key, AccountName);
            }

            return null;
        }
    }
}

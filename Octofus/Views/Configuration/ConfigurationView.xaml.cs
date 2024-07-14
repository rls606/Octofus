using System.Collections.Generic;
using System.Windows;
using Octofus.Data;
using Octofus.Options.Configuration;
using Octofus.Views.Configuration.SubView;

namespace Octofus.Views.Configuration
{
    /// <summary>
    /// Logique d'interaction pour ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        private ConfigurationModel Model { get; set; }

        private List<string> Accounts { get; set; }

        public ConfigurationView()
        {
            InitializeComponent();
        }

        public void FillView(List<string> accounts, Settings configuration)
        {
            Accounts = accounts;
            foreach (var account in configuration.Characters)
            {
                stackPanel.Children.Add(new AccountKey(accounts, account.Name, account.Key)
                {
                    Margin = new Thickness(5)
                });
            }
        }

        public List<CharacterSettings> GetCharactersSettings() 
        {
            var settings = new List<CharacterSettings>();

            foreach(var child in stackPanel.Children)
            {
                if(child is AccountKey accountKey)
                {
                    var result = accountKey.GetAccountBinding();
                    if(result.Item1 != null && result.Item2!= null)
                    {
                        settings.Add(new CharacterSettings { Key = result.Item1, AccountName = result.Item2 });
                    }
                }
            }

            return settings;
        }

        private void OnAddAccount(object sender, RoutedEventArgs e)
        {
            stackPanel.Children.Add(new AccountKey(Accounts, string.Empty, string.Empty)
            {
                Margin = new Thickness(5)
            });
        }
    }
}

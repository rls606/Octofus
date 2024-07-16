using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private List<AccountKey> KeyList { get; set; }

        private List<string> Accounts { get; set; }

        public ConfigurationView()
        {
            InitializeComponent();
            KeyList = new List<AccountKey>();
        }

        public void FillView(List<string> accounts, Settings configuration)
        {
            Accounts = accounts;
            foreach (var account in configuration.Characters)
            {
                var accountKey = new AccountKey(accounts, account.Name, account.Key, account.ImagePath);

                accountKey.RemoveRequested += OnRemoveAccount;
                KeyList.Add(accountKey);
                stackPanel.Children.Add(accountKey);
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
                    if(result != null)
                    {
                        settings.Add(new CharacterSettings { Key = result.Key, AccountName = result.AccountName, ImagePath = result.ImagePath});
                    }
                }
            }

            return settings;
        }

        private void OnAddAccount(object sender, RoutedEventArgs e)
        {
            var accountKey = new AccountKey(Accounts, string.Empty, string.Empty, string.Empty);

            accountKey.RemoveRequested += OnRemoveAccount;
            KeyList.Add(accountKey);
            stackPanel.Children.Add(accountKey);
        }

        private void OnRemoveAccount(object sender, EventArgs e)
        {
            if (sender is AccountKey item)
            {
                KeyList.Remove(item);
                stackPanel.Children.Remove(item);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            foreach(var key in KeyList)
            {
                key.RemoveRequested -= OnRemoveAccount;
            }

            base.OnClosing(e);
        }
    }
}

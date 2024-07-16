using Octofus.Common;
using Octofus.Data;
using Octofus.Options.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octofus.Views.Configuration
{
    public class ConfigurationViewModel
    {
        public ConfigurationView View { get; set; }

        public ConfigurationModel Model { get; set; }

        public AppController AppController { get; set; }

        private bool _exitWithoutSaving { get; set; }

        public ConfigurationViewModel(AppController controller)
        {
            _exitWithoutSaving = true;
            AppController = controller;
            InitializeView();
            InitializeModel();
            InitializeCommands();
        }

        #region Initialize

        private void InitializeView()
        {
            View = new ConfigurationView();
            View.Closing += OnClosing;
        }

        private void InitializeModel()
        {
            Model = new ConfigurationModel();
            View.DataContext = Model;
        }

        private void InitializeCommands()
        {
            Model.SaveSettingsCommand = new Utilities.Command(ExecuteSaveSettingsCommand);
            Model.CancelCommand = new Utilities.Command(ExecuteCancelCommand);
        }

        #endregion

        #region CancelCommand

        private void ExecuteCancelCommand(object obj)
        {
            Close();
        }

        #endregion

        #region SaveSettingsCommand

        private void ExecuteSaveSettingsCommand(object obj)
        {
            var settings = View.GetCharactersSettings();
            SaveCharactersSettings(settings);

            _exitWithoutSaving = false;
            Close();
        }

        #endregion

        #region Public methods

        public void Show()
        {
            View.Show();
            AppController.UnregisterHotKeys();
        }

        public void FillViewModel(List<string> accounts, Settings configuration)
        {
            View.FillView(accounts, configuration);
        }

        #endregion

        private void SaveCharactersSettings(List<CharacterSettings> characterSettings) 
        { 
            AppController.SaveCharactersSettings(characterSettings);
        }

        public void Close()
        {
            View.Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (_exitWithoutSaving)
            {
                AppController.RegisterHotKeys();
            }

            _exitWithoutSaving = true;
            View.Closing -= OnClosing;
        }
    }
}

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
            View.DialogResult = false;
            View.Close();
        }

        #endregion

        #region SaveSettingsCommand

        private void ExecuteSaveSettingsCommand(object obj)
        {
            View.DialogResult = true;
            View.Close();
        }

        #endregion

        #region Public methods

        public bool? ShowDialog()
        {
            AppController.UnregisterHotKeys();
            return View.ShowDialog();
        }

        public List<CharacterSettings> GetSettings()
        {
            var settings = View.GetCharactersSettings();
            return settings;
        }

        public void FillViewModel(List<string> accounts, Settings configuration)
        {
            View.FillView(accounts, configuration);
        }

        #endregion

        private void SaveCharactersSettings(List<CharacterSettings> characterSettings) 
        {
            View.DialogResult = true;
            AppController.SaveCharactersSettings(characterSettings);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            View.Closing -= OnClosing;
        }
    }
}

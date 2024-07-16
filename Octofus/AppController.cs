using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Octofus.Options.Configuration;
using Octofus.Options;
using Octofus.Views.Visualizer;
using Octofus.Views.Configuration;
using Octofus.Data;
using Octofus.Common;

namespace Octofus
{
    public class AppController
    {
        private Settings _configuration;

        private ProcessManager _processManager;

        private VisualizerView _visualizer;

        private ConfigurationViewModel _configurationViewModel;

        public AppController()
        {
            System.Windows.Application.Current.Startup += Current_Startup; 
        }

        private void Current_Startup(object sender, StartupEventArgs e)
        {
            System.Windows.Application.Current.Startup -= Current_Startup;
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/Octofus;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        public void Start()
        {
            InitializeNotifier();
            LoadConfiguration();

            _visualizer = new VisualizerView(new Data.WindowPosition { Left = _configuration.Visualizer.Left, Top = _configuration.Visualizer.Top }, this);
            _visualizer.Show();

            _processManager = ProcessManager.GetInstance();
            _processManager.Start(_configuration.Characters);

            RegisterHotKeys();
        }

        public void RegisterHotKeys()
        {
            HotKeyManager.RegisterHotKeys(_visualizer, _processManager.SetFocus, _configuration.Characters.Select(x => x.Key).ToArray());
        }

        public void UnregisterHotKeys() 
        {
            HotKeyManager.UnregisterHotKeys();
        }

        public string GetCharacterImage(string name)
        {
            return _configuration.Characters.FirstOrDefault(x => x.Name == name).ImagePath;
        }

        private void InitializeNotifier()
        {
            var iconPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            iconPath = Path.Combine(iconPath, "Icon.ico");
            var notifier = new NotifyIcon();
            notifier.Icon = new Icon(iconPath);
            notifier.ContextMenu = new ContextMenu();
            notifier.ContextMenu.MenuItems.Add(new MenuItem("Configuration", OnConfigure));
            notifier.ContextMenu.MenuItems.Add(new MenuItem("-"));
            notifier.ContextMenu.MenuItems.Add(new MenuItem("Fermer", OnQuit));
            notifier.Visible = true;
        }

        private void LoadConfiguration()
        {
            _configuration = OptionsService.ReadOptions();
        }

        #region Public methods

        public void SaveCharactersSettings(List<CharacterSettings> characterSettings)
        {
            OptionsService.SaveCharactersSettings(characterSettings);
            Reload();
        }

        #endregion

        #region Events

        private void OnConfigure(object sender, EventArgs e)
        {
            var accounts = _processManager.GetRunningAccounts();

            _configurationViewModel = new ConfigurationViewModel(this);
            _configurationViewModel.FillViewModel(accounts, _configuration);
            _configurationViewModel.Show();
        }

        private void Reload()
        {
            UnregisterHotKeys();
            _processManager.Stop();

            LoadConfiguration();

            _processManager = ProcessManager.GetInstance();
            _processManager.Start(_configuration.Characters);

            RegisterHotKeys();
        }

        private void OnQuit(object sender, EventArgs e)
        {
            if(_configurationViewModel != null)
            {
                _configurationViewModel.Close();
            }

            UnregisterHotKeys();
            _processManager.Stop();
            _visualizer.Close();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        public void Start()
        {
            InitializeNotifier();
            LoadConfiguration();

            _visualizer = new VisualizerView(new Data.WindowPosition { Left = _configuration.Visualizer.Left, Top = _configuration.Visualizer.Top });
            _visualizer.Show();

            _processManager = ProcessManager.GetInstance();
            _processManager.Start(_configuration.Characters);

            HotKeyManager.RegisterHotKeys(_visualizer, _processManager.SetFocus, _configuration.Characters.Select(x => x.Key).ToArray());
        }

        private void InitializeNotifier()
        {
            var iconPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            iconPath = Path.Combine(iconPath, "Icon.ico");
            var notifier = new NotifyIcon();
            notifier.Icon = new Icon(iconPath);
            notifier.ContextMenu = new ContextMenu();
            notifier.ContextMenu.MenuItems.Add(new MenuItem("Configurer", OnConfigure));
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

            var configurationViewModel = new ConfigurationViewModel(this);
            configurationViewModel.FillViewModel(accounts, _configuration);
            configurationViewModel.Show();
        }

        private void Reload()
        {
            HotKeyManager.UnregisterHotKeys();
            _processManager.Stop();

            LoadConfiguration();

            _processManager = ProcessManager.GetInstance();
            _processManager.Start(_configuration.Characters);

            HotKeyManager.RegisterHotKeys(_visualizer, _processManager.SetFocus, _configuration.Characters.Select(x => x.Key).ToArray());
        }

        private void OnQuit(object sender, EventArgs e)
        {
            HotKeyManager.UnregisterHotKeys();
            _processManager.Stop();
            _visualizer.Close();
        }

        #endregion
    }
}

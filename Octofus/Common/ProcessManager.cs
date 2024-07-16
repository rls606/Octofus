using Octofus.Options.Configuration;
using Octofus.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;

namespace Octofus.Common
{
    public class ProcessManager
    {
        #region Singleton Declaration

        private static ProcessManager _instance;

        private static readonly object _lock = new object();

        public static ProcessManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProcessManager();
                    }
                }
            }

            return _instance;
        }

        #endregion

        private bool _isRunning;

        private List<Character> _characterNames { get; set; }

        public void Start(List<Character> characterNames)
        {
            _characterNames = characterNames;
            _isRunning = true;
            Task.Delay(1000).ContinueWith(CheckInstancesState);
        }

        private void CheckInstancesState(Task task)
        {
            task.Dispose();

            if (_isRunning)
            {
                UpdateInstancesStates();
                Task.Delay(50).ContinueWith(CheckInstancesState);
            }
        }

        public List<string> GetRunningAccounts()
        {
            var result = new List<string>();
            var processes = Process.GetProcessesByName("Dofus");
            foreach (var process in processes)
            {
                if (!string.IsNullOrWhiteSpace(process.MainWindowTitle))
                {
                    var accountName = process.MainWindowTitle.Split(' ')[0];
                    if (accountName != "Dofus")
                    {
                        result.Add(accountName);
                    }
                }
            }

            return result;
        }

        public void SetFocus(string key)
        {
            var processes = Process.GetProcessesByName("Dofus");
            var name = _characterNames.FirstOrDefault(c => c.Key == key).Name;

            foreach (var process in processes)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    var charName = process.MainWindowTitle.Split(' ')[0].Trim();

                    if (charName == name)
                    {
                        WinAPI.SetForegroundWindow(process.MainWindowHandle);
                    }
                }

                process.Dispose();
            }
        }

        private void UpdateInstancesStates()
        {
            if (_isRunning)
            {
                var processes = Process.GetProcessesByName("Dofus");

                var focus = WinAPI.GetForegroundWindow();
                var newFocus = string.Empty;
                var availableCharacters = new List<string>();

                foreach (var process in processes)
                {
                    var name = process.MainWindowTitle.Split(' ')[0].Trim();
                    if (!string.IsNullOrEmpty(process.MainWindowTitle))
                    {
                        if (_characterNames.Any(e => e.Name == name))
                        {
                            if (process.MainWindowHandle == focus)
                            {
                                newFocus = name;
                            }

                            availableCharacters.Add(name);
                        }
                    }

                    process.Dispose();
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => OnUpdate(availableCharacters, newFocus)));
            }
        }

        private static void OnUpdate(List<string> availableCharacters, string name)
        {
            OnInstancesUpdated(availableCharacters.ToList());

            if (!string.IsNullOrEmpty(name))
            {
                OnFocusChanged(name);
            }
            else if (availableCharacters.Any())
            {
                OnFocusChanged(string.Empty);
            }
        }

        public void Stop()
        {
            _characterNames.Clear();
            _isRunning = false;
        }

        #region Events

        public static event EventHandler<List<string>> InstancesUpdated;

        private static void OnInstancesUpdated(List<string> portraits)
        {
            InstancesUpdated?.Invoke(null, portraits);
        }

        public static event EventHandler<string> FocusChanged;

        private static void OnFocusChanged(string name)
        {
            FocusChanged?.Invoke(null, name);
        }

        #endregion
    }
}

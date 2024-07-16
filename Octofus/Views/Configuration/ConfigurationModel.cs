using Octofus.Options.Configuration;
using Octofus.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Octofus.Views.Configuration
{
    public class ConfigurationModel : Model
    {
        #region SaveSettingsCommand

        private Command holderSaveSettingsCommand;

        public Command SaveSettingsCommand
        {
            get { return holderSaveSettingsCommand; }
            set
            {
                if (holderSaveSettingsCommand != value)
                {
                    holderSaveSettingsCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region CancelCommand

        private Command holderCancelCommand;

        public Command CancelCommand
        {
            get { return holderCancelCommand; }
            set
            {
                if (holderCancelCommand != value)
                {
                    holderCancelCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}

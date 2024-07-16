using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Octofus.Views.Configuration.SubView
{
    public class AccountKeyInformations
    {
        public string Key { get; set; }

        public string AccountName { get; set; }

        public string ImagePath { get; set; }

        public bool IsInformationsValid()
        {
            if (!string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(AccountName))
            {
                if (!string.IsNullOrEmpty(ImagePath))
                {
                    if (File.Exists(ImagePath))
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;

namespace AECI.ICM.Data.Entities
{
    public class Setting
    {
        public int Id { get; set; }

        public string SignatureLocation { get; set; }
        public bool EnableWarning { get; set; }
        public DateTime WarningCuttOffDate { get; set; }
        public string WarningEmail { get; set; }

        public List<SettingEmail> Emails { get; set; } = new List<SettingEmail>();
    }
}

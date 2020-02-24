using AECI.ICM.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AECI.ICM.Api.ViewModels
{
    public class SettingsViewModel
    {
        #region Properties

        public int Id { get; set; }
        public string SignatureLocation { get; set; }
        public bool EnableWarning { get; set; }
        public DateTime WarningCuttOffDate { get; set; }
        public string WarningEmail { get; set; }
        public string AccountantName { get; set; }
        public int SettingId { get; set; }
        public string OnlineApiUrl { get; set; }
        public List<SettingEmailEntity> Emails { get; set; } = new List<SettingEmailEntity>();

        #endregion
    }
}

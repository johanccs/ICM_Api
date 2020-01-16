using AECI.ICM.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace AECI.ICM.Domain.Entities
{
    public class SettingEntity
    {
        public SettingId Id { get; set; }
        public string SignatureLocation { get; set; }
        public bool EnableWarning { get; set; }
        public DateTime WarningCuttOffDate { get; set; }
        public string WarningEmail { get; set; }
        public string OnlineApiUrl { get; set; }
        public List<SettingEmailEntity> Emails { get; set; } = new List<SettingEmailEntity>();
    }
}

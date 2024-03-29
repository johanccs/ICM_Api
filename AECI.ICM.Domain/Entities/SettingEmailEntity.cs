﻿namespace AECI.ICM.Domain.Entities
{
    public class SettingEmailEntity
    {
        public int Id { get; set; }
        public string BranchManagerName { get; set; }
        public string BranchManagerEmail { get; set; }
        public string RegionalAccountantName { get; set; }
        public string RegionalAccountantEmail { get; set; }
        public string Site { get; set; }
        public int SettingId { get; set; }
    }
}

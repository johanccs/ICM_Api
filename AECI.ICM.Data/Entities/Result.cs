using System;

namespace AECI.ICM.Data.Entities
{
    public class Result
    {
        public int Id { get; set; }
        public string Branch { get; set; }
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public string BranchManagerName { get; set; }
        public string FinanceName { get; set; }
        //public string RegionalAccountantName { get; set; }
        public DateTime DateSigned { get; set; }
    }
}

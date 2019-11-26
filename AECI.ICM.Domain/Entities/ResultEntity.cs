using System;

namespace AECI.ICM.Domain.Entities
{
    public class ResultEntity
    {
        public int Id { get; set; }
        public string Branch { get; set; }
        public DateTime Date { get; set; }
        public string Month { get; set; }        
        public string BMName { get; set; }
        public string FinName { get; set; }
        //public string RegionalACC { get; set; }
        public DateTime DateSigned { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AECI.ICM.Shared.ViewModels
{
    public class ResponseViewModel
    {
        //public int Id { get; set; }
        public string Branch { get; set; }
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public string GenComments { get; set; }
        public string BMSigPath { get; set; }
        public string FinSigPath { get; set; }
        //public string RegionalAcc { get; set; }
        public string BMName { get; set; }
        public string FinName { get; set; }
        public DateTime DateSigned { get; set; }

        public List<ICMViewModel> ICMElements { get; set; } = new List<ICMViewModel>();
    }
}

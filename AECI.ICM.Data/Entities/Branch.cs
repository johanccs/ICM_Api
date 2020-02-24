using System;
using System.Collections.Generic;
using System.Text;

namespace AECI.ICM.Data.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string CompanyCode { get; set; }
        public int Plant { get; set; }
        public string Code { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }
        public string ServerUrl { get; set; }
    }
}

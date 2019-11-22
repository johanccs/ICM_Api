using System.Collections.Generic;

namespace AECI.ICM.Domain.Entities
{
    public class ResponseEntity
    {
        public int Id { get; set; }
        public string Branch { get; set; }
        public string Date { get; set; }
        public string Month { get; set; }
        public string GenComments { get; set; }
        public string BMSig { get; set; }
        public string FinSig { get; set; }
        public string BMName { get; set; }
        public string FinName { get; set; }
        public string DateSigned { get; set; }

        public List<ICMEntity> ICMList { get; set; } = new List<ICMEntity>();
    }
}

using AECI.ICM.Application.Interfaces;

namespace AECI.ICM.Application.Models
{
    public class Branch:IBranch
    {
        public int SiteId { get; set; }
        public string AbbrevName { get; set; }
        public string Fullname { get; set; }
    }
}

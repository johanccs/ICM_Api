using AECI.ICM.Application.Interfaces;

namespace AECI.ICM.Application.Models
{
    public class NullRefBranch : IBranch
    {
        public int SiteId { get; set; }
        public string AbbrevName { get; set; }
        public string Fullname { get; set; }

        public NullRefBranch()
        {
            SiteId = -1;
            AbbrevName = null;
            Fullname = null;
        }
    }
}

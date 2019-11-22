using AECI.ICM.Domain.Framework;
using AECI.ICM.Domain.ValueObjects;

namespace AECI.ICM.Domain.Entities
{
    public class ICMEntity:Value<ICMEntity>
    {
        #region Properties

        public ICMId Id { get; set; }
        public int FK { get; set; }
        public string ControlStatement { get; set; }
        public bool BranchManager { get; set; } = true;
        public bool RegionalAccountant { get; set; } = true;
        public bool FinanceFunctionCheck { get; set; } = true;
        public string Section { get; set; }
        public string SectionName { get; set; }
        public string Comments { get; set; }

        #endregion

        #region Contructor

        public ICMEntity()
        {
        }

        public ICMEntity(ICMId id)
        {
            Id = id;
        }

        #endregion
    }
}

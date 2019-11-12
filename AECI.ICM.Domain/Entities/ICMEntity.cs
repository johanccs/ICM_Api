using AECI.ICM.Domain.Framework;
using AECI.ICM.Domain.ValueObjects;
using System.Collections.Generic;

namespace AECI.ICM.Domain.Entities
{
    public class ICMEntity:Value<ICMEntity>
    {
        #region Properties

        public ICMId Id { get; set; }      
        public int Section { get; set; }
        public string ControlStatement { get; set; }        
        public bool BranchManager { get; set; } = true;
        public bool RegionalAccountant { get; set; } = true;
        public bool FinanceFunctionCheck { get; set; } = true;

        public ICollection<SectionDetail> SectionDetail { get; set; }

        #endregion

        #region Contructor

        public ICMEntity(ICMId id)
        {
            Id = id;
        }

        #endregion
    }
}

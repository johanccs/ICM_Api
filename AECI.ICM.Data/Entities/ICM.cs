using System.Collections.Generic;

namespace AECI.ICM.Data.Entities
{
    public class ICM
    {
        #region Properties

        public int Id { get; set; }
        public string ControlStatement { get; set; }
        public bool BranchManager { get; set; } = true;
        public bool RegionalAccountant { get; set; } = true;
        public bool FinanceFunctionCheck { get; set; } = true;

        public SectionDetail SectionDetail { get; set; }

        #endregion
    }
}

namespace AECI.ICM.Shared.ViewModels
{
    public class ICMViewModel
    {
        public string ControlStatement { get; set; }
        public bool BranchManager { get; set; } = true;
        public bool RegionalAccountant { get; set; } = true;
        public bool FinanceFunctionCheck { get; set; } = true;
        public string Section { get; set; }
        public string SectionName { get; set; }
        public string Comments { get; set; }
        public string CtrlEffectiveness { get; set; }
    }
}

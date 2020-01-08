namespace AECI.ICM.Application.Interfaces
{
    public interface IBranch
    {
        int SiteId { get; set; }
        string AbbrevName { get; set; }
        string Fullname { get; set; }
    }
}

using System.Web;
using System.Web.Mvc;

namespace AECI.ICM.PrintReports
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

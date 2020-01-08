using AECI.ICM.Application.Interfaces;
using System.Collections.Generic;

namespace AECI.ICM.Application.Models
{
    public class Inner : Region
    {
     
        public override List<string> GetAbbrevList(List<IBranch>branches)
        {
            var branchList = new List<string>();

            branches.ForEach(p =>
            {
                if(p.AbbrevName.ToLower() == "pom" || 
                   p.AbbrevName.ToLower() == "rdp" || 
                   p.AbbrevName.ToLower() == "eik" 
                  )
                {
                    branchList.Add(p.AbbrevName);
                }
            });

            return branchList;
        }
    }
}

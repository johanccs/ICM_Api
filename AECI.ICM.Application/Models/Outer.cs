using AECI.ICM.Application.Interfaces;
using System.Collections.Generic;

namespace AECI.ICM.Application.Models
{
    public class Outer : Region
    {
        public override List<string> GetAbbrevList(List<IBranch> branches)
        {
            var branchList = new List<string>();

            branches.ForEach(p =>
            {
                if (p.AbbrevName.ToLower() == "wit" ||
                    p.AbbrevName.ToLower() == "bfn" ||
                    p.AbbrevName.ToLower() == "pol"
                  )
                {
                    branchList.Add(p.AbbrevName);
                }
            });

            return branchList;
        }
    }
}

using AECI.ICM.Application.ApplicationEnums;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AECI.ICM.Application.Services
{
    public class BranchDirectoryService : IBranchDirectoryService
    {
        #region Readonly Fields

        private readonly List<IBranch> branches;

        #endregion

        #region Constants

        private const string NODATA = "nodata";

        #endregion

        #region Constructor

        public BranchDirectoryService()
        {
            branches = LoadBranches();
        }

        #endregion

        #region Public Methods

        public string Locate(int siteid)
        {
            return GetById(siteid).AbbrevName;
        }

        public IBranch LocatebyUserId(object siteid)
        {
            return GetById(siteid);
        }

        public IBranch LocateByFullBranchName(string fullBranchName, bool single)
        {
            return GetByName(fullBranchName);
        }

        public List<IBranch> GetAll()
        {
            return this.branches;
        }

        public List<string> LocateByFullBranchName(string name)
        {
            if (name.ToLower() == RegionEnum.Inner.ToString().ToLower())
                return new Inner().GetAbbrevList(branches);

            else if (name.ToLower() == RegionEnum.Outer.ToString().ToLower())
                return new Outer().GetAbbrevList(branches);

            else
            {
                var list = new List<string>();
                list.Add(GetByName(name).AbbrevName);             

                return list;
            }
        }

        #endregion

        #region Private Methods

        private List<IBranch>LoadBranches()
        {
            List<IBranch> branchDir = new List<IBranch>();
            branchDir.Add(new Branch { SiteId = 11, AbbrevName = "ER", Fullname = "Eerste River"});
            branchDir.Add(new Branch { SiteId = 14, AbbrevName = "CK", Fullname = "Contermanskloof"});
            branchDir.Add(new Branch { SiteId = 23, AbbrevName = "GEO", Fullname = "George" } );
            branchDir.Add(new Branch { SiteId = 21, AbbrevName = "PE", Fullname = "Port Elizabeth"});
            branchDir.Add(new Branch { SiteId = 41, AbbrevName = "BEN", Fullname = "Benoni"});
            branchDir.Add(new Branch { SiteId = 40, AbbrevName = "BEN2", Fullname = "Benoni 2"});
            branchDir.Add(new Branch { SiteId = 44, AbbrevName = "POM", Fullname = "Pomona"});
            branchDir.Add(new Branch { SiteId = 47, AbbrevName = "POL", Fullname = "Polokwane"});
            branchDir.Add(new Branch { SiteId = 48, AbbrevName = "Wit", Fullname = "Witbank" });
            branchDir.Add(new Branch { SiteId = 52, AbbrevName = "EMP", Fullname = "Empangeni" } );
            branchDir.Add(new Branch { SiteId = 31, AbbrevName = "BFN", Fullname = "Bloemfontein"});
            branchDir.Add(new Branch { SiteId = 51, AbbrevName = "COED", Fullname = "Coedmore" });
            branchDir.Add(new Branch { SiteId = 42, AbbrevName = "EIK", Fullname = "Eikenhof" });
            branchDir.Add(new Branch { SiteId = 43, AbbrevName = "RDP", Fullname = "Roodepoort" });
            branchDir.Add(new Branch { SiteId = 82, AbbrevName = "ECA", Fullname = "Eastern Cape" });
            branchDir.Add(new Branch { SiteId = 84, AbbrevName = "UMT", Fullname = "Umtata" });
            branchDir.Add(new Branch { SiteId = 58, AbbrevName = "PMB", Fullname = "Pietermaritzburg" });
            branchDir.Add(new Branch { SiteId = 01, AbbrevName = "HO", Fullname = "Head Office" });

            return branchDir;
        }
   
        private IBranch GetById(object id)
        {
            if (branches.Any(p => p.SiteId == Convert.ToInt32(id)))
                return branches.SingleOrDefault(p => p.SiteId == Convert.ToInt32(id));
            else
                return new NullRefBranch();
        }

        private IBranch GetByName(string name)
        {
            if (branches == null || branches.Count == 0)
                LoadBranches();

            if (branches.Any(p => p.Fullname == name))
                return branches.SingleOrDefault(p => p.Fullname == name);
            else
                return new NullRefBranch();
        }

        #endregion
    }
}

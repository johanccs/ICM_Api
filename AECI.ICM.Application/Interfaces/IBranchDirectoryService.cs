using AECI.ICM.Data.Context;
using System.Collections.Generic;

namespace AECI.ICM.Application.Interfaces
{
    public interface IBranchDirectoryService
    {
        string Locate(int siteid);

        IBranch LocatebyUserId(object siteid);

        IBranch LocateByFullBranchName(string fullBranchName, bool single);

        List<string> LocateByFullBranchName(string fullBranchname);

        List<IBranch> GetAll();

        List<Data.Entities.Branch> LoadBranches(MapConfigDbCtx mapConfigDb);
    }
}

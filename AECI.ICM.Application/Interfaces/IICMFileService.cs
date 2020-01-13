using AECI.ICM.Application.Commands;
using System.Collections.Generic;

namespace AECI.ICM.Application.Interfaces
{
    public interface IICMFileService
    {
        List<ICMFile.V1.List> GetFiles(string pathName, string branch);
        ICMFile.V1.List GetFile(string filename);
    }
}

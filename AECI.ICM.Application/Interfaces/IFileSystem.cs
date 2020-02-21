using AECI.ICM.Application.Commands;

namespace AECI.ICM.Application.Interfaces
{
    public interface IFileSystem
    {
        FileSystemService.V1.List List(string folder);
        FileSystemService.V1.List List(string folder, string siteFolder);
    }
}

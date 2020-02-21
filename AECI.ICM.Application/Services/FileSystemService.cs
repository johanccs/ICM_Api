using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace AECI.ICM.Application.Services
{
    public class FileSystemService : IFileSystem
    {
        public Commands.FileSystemService.V1.List List(string folder)
        {
            return null;
        }

        public Commands.FileSystemService.V1.List List(string folder, string siteFolder)
        {
            var results = Directory.GetFiles(Path.Combine(folder,siteFolder), "*.pdf");

            if (results == null || results.Length == 0)
                return new Commands.FileSystemService.V1.List();
            else
            {
                var icmReports = new List<Models.File>();

                foreach(var file in results)
                {
                    icmReports.Add(
                        new Models.File {
                            DateSent = DateTime.Now, 
                            Name = Path.GetFileName(file),
                            Path = Path.GetFullPath(file),
                            Extension = Path.GetExtension(file),
                            Created = File.GetCreationTime(file),
                            RootFolder = Path.GetPathRoot(file),
                            Modified = File.GetLastWriteTime(file),
                            Size = new FileInfo(file).Length,
                            Drive = Path.GetPathRoot(file)
                        }
                     );;
                }

                return new Commands.FileSystemService.V1.List { Files = icmReports };
            }
        }
    }
}

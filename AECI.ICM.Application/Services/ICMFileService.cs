using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace AECI.ICM.Application.Services
{
    public class ICMFileService : IICMFileService
    {
        public ICMFile.V1.List GetFile(string filename)
        {
            throw new NotImplementedException();
        }

        public List<ICMFile.V1.List> GetFiles(string pathName, string branch)
        {
            var folder = Path.Combine(pathName, branch);

            if (!folder.EndsWith("\\"))
                folder = $"{folder}\\";

            var files = GetFilesByBranch(folder);

            var icmFiles = ConvertToICMFile(files, branch);

            return icmFiles;
        }

        #region Private Methods

        private string[] GetFilesByBranch(string pathName)
        {
            var results = Directory.GetFiles(pathName, "*.pdf");

            return results;
        }

        private List<ICMFile.V1.List> ConvertToICMFile(string[] files, string branch)
        {
            var icmFiles = new List<ICMFile.V1.List>();

            foreach(var file in files)
            {
                var icmFile = new ICMFile.V1.List();
                string[] values = file.Split("_");

                icmFile.Branch = branch;
                icmFile.FileName = file;
                icmFile.Month = Convert.ToInt32(values[2]);
                icmFile.Size = new FileInfo(file).Length;

                if(values[3].Contains("."))
                {
                    string[] yearVals = values[3].Split(".");
                    icmFile.Year = Convert.ToInt32(yearVals[0]);
                }

                icmFiles.Add(icmFile);
            }

            return icmFiles;
        }

        #endregion
    }
}

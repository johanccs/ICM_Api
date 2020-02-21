using System;

namespace AECI.ICM.Application.Models
{
    public class File
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string RootFolder { get; set; }
        public string Path { get; set; }
        public string Drive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime DateSent { get; set; }
    }
}

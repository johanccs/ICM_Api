using System.Collections.Generic;

namespace AECI.ICM.Application.Commands
{
    public static class FileSystemService
    {
        public static class V1
        {
            public class List
            {
                public List<Models.File> Files { get; set; } = 
                    new List<Models.File>();
            }
        }
    }
}

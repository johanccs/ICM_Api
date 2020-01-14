namespace AECI.ICM.Application.Commands
{
    public static class ICMFile
    {
        public static class V1
        {
            public class List
            {
                public string Branch { get; set; }
                public string FileName { get; set; }
                public long Size { get; set; }
                public int? Month { get; set; }
                public int Year { get; set; }
            }
        }
    }
}

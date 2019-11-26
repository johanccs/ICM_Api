using System;

namespace MonitorService.Entities
{
    public class ResultException
    {
        public int Month { get; set; }
        public string Site { get; set; }
        public DateTime Date { get; set; }
    }
}

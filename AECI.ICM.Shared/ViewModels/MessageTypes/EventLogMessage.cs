using AECI.ICM.Shared.Interfaces;
using System;

namespace AECI.ICM.Shared.ViewModels.MessageTypes
{
    public class EventLogMessage : ILogMessageType
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Application { get; set; }
        public string User { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public string Importance { get; set; }
        public string Stacktrace { get; set; }
        public string Comment { get;set; }
    }
}

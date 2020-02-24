using System;

namespace AECI.ICM.Shared.Interfaces
{
    public interface ILogMessageType
    {
        int Id { get; set; }

        DateTime Date { get; set; }

        string Application { get; set; }

        string User { get; set; }

        string Category { get; set; }

        string Message { get; set; }

        string Importance { get; set; }

        string Stacktrace { get; set; }

        string Comment { get; set; }
    }
}

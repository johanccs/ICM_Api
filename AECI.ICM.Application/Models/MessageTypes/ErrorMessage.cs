using AECI.ICM.Application.Interfaces;
using System;
using System.Reflection;
using static AECI.ICM.Application.ApplicationEnums.InfrastructureEnums;

namespace AECI.ICM.Application.Models.MessageTypes
{
    public class ErrorMessage:ILogMessageType
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Application { get; set; }

        public string User { get; set; }

        public string Category { get; set; }

        public string Message { get; set; }

        public string Importance { get; set; }

        public string Stacktrace { get; set; }

        public string Comment { get; set; }

        public string Plant { get; set; }

        public ErrorMessage Set(Exception ex, string plant, string user)
        {
            Category = CategoryEnum.Warning.ToString();
            Importance = ImportanceEnum.Critical.ToString();
            Message = ex.Message;
            Plant = plant;
            Date = DateTime.Now;
            Application = Assembly.GetCallingAssembly().ToString();
            Stacktrace = ex.StackTrace;
            User = user;

            return null;
        }
    }
}

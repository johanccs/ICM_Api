using AECI.ICM.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static AECI.ICM.Application.ApplicationEnums.InfrastructureEnums;

namespace AECI.ICM.Application.Models.MessageTypes
{
    public class InfoMessage:ILogMessageType
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

        public InfoMessage Set(string message, string plant, string user)
        {
            Category = CategoryEnum.Info.ToString();
            Importance = ImportanceEnum.Medium.ToString();
            Message = message;
            Plant = plant;

            Date = DateTime.Now;
            Application = Assembly.GetCallingAssembly().ToString();
            Stacktrace = Assembly.GetCallingAssembly().CodeBase;
            User = user;

            return this;
        }
    }
}

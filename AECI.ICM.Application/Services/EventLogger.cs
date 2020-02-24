using AECI.ICM.Application.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AECI.ICM.Application.Services
{
    public class EventLogger : ILogger
    {
        #region Fields

        private readonly EventLog _eventlog;

        #endregion

        #region Constructor

        public EventLogger()
        {

        }

        #endregion

        public Task<bool> LogAsync(ILogMessageType message, string url)
        {
            throw new NotImplementedException();
        }
    }
}

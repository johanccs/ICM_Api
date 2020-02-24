using AECI.ICM.Shared.Interfaces;
using AECI.ICM.Shared.ViewModels.MessageTypes;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AECI.ICM.Shared.Service
{
    public class EventLogger : ILogger
    {
        #region Fields

        private readonly EventLog _eventlog = null;

        #endregion

        #region Constructor

        public EventLogger()
        {
            _eventlog = new EventLog();
            if(!EventLog.SourceExists("ICMMonitorService"))
            {
                EventLog.CreateEventSource(
                    "ICMMonitorService", "ICMMonitorService"
                );
            }

            _eventlog.Source = "ICMMonitorService";
            _eventlog.Log = "ICMMonitorService";
        }

        #endregion

        public async Task<bool> LogAsync(
            ILogMessageType message, string _src = "API", string url="")
        {
            _eventlog.Source = _src;

            if (message.GetType() == typeof(EventLogMessage))
              return await Task.Run(() => {
                  if (message.Category.ToLower() == "Info".ToLower())
                  {
                      _eventlog.WriteEntry(message.Message, EventLogEntryType.Information, message.Id, 1);
                  }
                  else if(message.Category.ToLower() == "Error".ToLower())
                  {
                      _eventlog.WriteEntry(message.Message, EventLogEntryType.Error, message.Id, 2);
                  }
                  return true;
               });

            return false;
        }
    }
}

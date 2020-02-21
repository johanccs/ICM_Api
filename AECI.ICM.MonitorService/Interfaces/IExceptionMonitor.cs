using System.Diagnostics;

namespace MonitorService.Interfaces
{
    public interface IExceptionMonitor
    {
        void Start(EventLog eventLog);
    }
}

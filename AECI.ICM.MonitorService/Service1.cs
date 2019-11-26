using MonitorService.Interfaces;
using MonitorService.Services;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace MonitorService
{
    public partial class Service1 : ServiceBase
    {
        #region Readonly Fields

        private IExceptionMonitor _exceptionMonitor = new ExceptionMonitor();

        #endregion

        #region Fields

        private EventLog eventlog;
        private Timer timer = new Timer();

        #endregion

        #region Constructor

        public Service1()
        {
            InitializeComponent();

            //Debugger.Launch();

           
            //eventlog = new EventLog();
            //if (!EventLog.SourceExists("ICMMonitorService"))
            //{
            //    EventLog.CreateEventSource(
            //        "ICMMonitorService", "ICMMonitorService"
            //    );
            //}

            //eventlog.Source = "ICMMonitorService";
            //eventlog.Log = "ICMMonitorService";

        }

        #endregion

        #region Methods

        protected override void OnStart(string[] args)
        {
            //eventlog.WriteEntry("Service Start");

            //Debugger.Break();

            _exceptionMonitor.Start();

            timer.Interval = 600000;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            //Debugger.Break();

            _exceptionMonitor.Start();
        }

        protected override void OnStop()
        {
            //eventlog.WriteEntry("Service Stop");
            timer.Stop();
        }

        #endregion
    }
}

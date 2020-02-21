using MonitorService.Interfaces;
using MonitorService.Services;
using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace MonitorService
{
    public partial class Service1 : ServiceBase
    {
        #region Readonly Fields

        private IExceptionMonitor _exceptionMonitor = new ExceptionMonitor();
        private EventLog eventlog;

        #endregion

        #region Fields
       
        private Timer timer = new Timer();

        #endregion

        #region Constructor

        public Service1()
        {
            InitializeComponent();

            //Debugger.Launch();

            eventlog = new EventLog();
            if (!EventLog.SourceExists("ICMMonitorService"))
            {
                EventLog.CreateEventSource(
                    "ICMMonitorService", "ICMMonitorService"
                );
            }

            eventlog.Source = "ICMMonitorService";
            eventlog.Log = "ICMMonitorService";
        }

        #endregion

        #region Methods

        protected override void OnStart(string[] args)
        {
            eventlog.WriteEntry("Service Starting");

            //Debugger.Break();

            _exceptionMonitor.Start(eventlog);

            int timerInterval = Convert.ToInt32(
                ConfigurationManager.AppSettings.
                Get(MappingConfiguration.MappingConfig.DEBUGTIMERVALUE));
            
            timer.Interval = timerInterval;
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();

            eventlog.WriteEntry("Service Started");
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            eventlog.WriteEntry("Timer fired");
            _exceptionMonitor.Start(eventlog);
        }

        protected override void OnStop()
        {
            eventlog.WriteEntry("Service Stop");
            timer.Stop();
            timer.Dispose();
        }

        #endregion
    }
}

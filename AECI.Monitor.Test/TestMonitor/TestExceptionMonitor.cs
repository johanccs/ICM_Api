using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorService.Services;
using System.Diagnostics;

namespace AECI.Monitor.Test.TestMonitor
{
    [TestClass]
    public class TestExceptionMonitor
    {
        [TestMethod]
        public void TestSendEmail()
        {
            var service = new ExceptionMonitor();
            var _event = new EventLog();
            service.Start(_event);
        }
    }
}

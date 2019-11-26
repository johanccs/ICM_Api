using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorService.Services;

namespace AECI.Monitor.Test.TestMonitor
{
    [TestClass]
    public class TestExceptionMonitor
    {
        [TestMethod]
        public void TestSendEmail()
        {
            var service = new ExceptionMonitor();

            service.Start();
        }
    }
}

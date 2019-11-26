using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorService;
using System.Linq;

namespace AECI.Monitor.Test.TestEF
{
    [TestClass]
    public class TestResults
    {
        [TestMethod]
        public void TestResult()
        {
            var ctx = new MonDbContext();

            var results = ctx.Results.ToList();

            Assert.IsNotNull(results);
        }
    }
}

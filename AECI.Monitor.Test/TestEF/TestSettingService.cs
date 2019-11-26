using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorService;
using System.Data.Entity;
using System.Linq;

namespace AECI.Monitor.Test.TestEF
{
    [TestClass]
    public class TestSettingService
    {
        [TestMethod]
        public void TestSetting()
        {
            var ctx = new MonDbContext();

            var setting = ctx.Setting.Include(p=>p.Emails).FirstOrDefault();

            Assert.IsNotNull(setting);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonitorService.Interfaces;
using MonitorService.Service;

namespace AECI.Monitor.Test
{
    [TestClass]
    public class SendEmail
    {
        [TestMethod]
        public void Send()
        {
            INotificationService emailService = new EmailNotificationService(
                   server:"muchsmtp", 
                   fromEmail:"muchasphalt@muchasphalt.com"     
                );

            emailService.Send();
        }
    }
}

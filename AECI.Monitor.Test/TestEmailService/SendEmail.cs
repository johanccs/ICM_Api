using AECI.ICM.Shared.Interfaces;
using AECI.ICM.Shared.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AECI.Monitor.Test
{
    [TestClass]
    public class SendEmail
    {
        [TestMethod]
        public void Send()
        {
            ISharedNotificationService emailService = 
                new SharedEmailNotificationService();

            emailService.Body = "test email service";
            emailService.Subject = "Test";
            emailService.ToEmail = "johan.potgieter@muchasphalt.com";          
            emailService.Server = "muchsmtp";
            emailService.ToEmail = "johan.potgieter@muchasphalt.com";
        }
    }
}

using System.Collections.Generic;
using System.Net.Mail;

namespace MonitorService.Interfaces
{
    public interface INotificationService
    {
        void Send();
        void Send(List<MailMessage> emails);
    }
}

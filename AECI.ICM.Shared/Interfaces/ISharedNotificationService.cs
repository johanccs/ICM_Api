using System.Collections.Generic;
using System.Net.Mail;

namespace AECI.ICM.Shared.Interfaces
{
    public interface ISharedNotificationService
    {
        string ToEmail { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        string Server { get; set; }
        string FromEmail { get; set; }
        string CC { get; set; }
        string Attachment { get; set; }
        
        void Send();        
        void Send(List<MailMessage> emails);
    }
}

using AECI.ICM.Shared.Entities;
using AECI.ICM.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace AECI.ICM.Shared.Service
{
    public class SharedEmailNotificationService : ISharedNotificationService
    {
        #region Fields

        private  MailMessage _notificationClient;
        
        #endregion

        #region Properties

        public string Server { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CC { get; set; } = null;
        public string Attachment { get; set; } = null;

        #endregion

        #region Constructor

        public SharedEmailNotificationService()
        {
        }

        #endregion

        #region Methods

        public void Send()
        {
                _notificationClient = EmailClient
                .Create(FromEmail,
                        ToEmail,
                        Subject,
                        Body,
                        CC, 
                        Attachment);   
            try
            {
                SmtpClient client = new SmtpClient(Server);
                client.UseDefaultCredentials = true;

                client.Send(_notificationClient);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }
        }

        public void Send(List<MailMessage> emails)
        {
            try
            {
                SmtpClient client = new SmtpClient(Server);
                client.UseDefaultCredentials = true;

                foreach (var email in emails)
                    client.Send(email);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion
    }
}

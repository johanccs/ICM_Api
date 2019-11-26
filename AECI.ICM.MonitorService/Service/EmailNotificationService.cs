using MonitorService.Entities;
using MonitorService.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MonitorService.Service
{
    public class EmailNotificationService : INotificationService
    {
        #region Fields

        private  MailMessage _notificationClient;

        private string _server;
        private string _fromEmail;
       
        #endregion

        #region Properties

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        #endregion

        #region Constructor

        public EmailNotificationService(string server, string fromEmail)
        {
            if (server == default)
                throw new ArgumentNullException(nameof(server), "Supply server name or address");

            if (fromEmail == default)
                throw new ArgumentNullException(nameof(server), "Supply the source email address");

            _fromEmail = fromEmail;
            _server = server;
        }

        #endregion

        #region Methods

        public void Send()
        {
            _notificationClient = EmailClient.Create(_fromEmail, 
                                                     ToEmail, 
                                                     Subject, 
                                                     Body); 
            try
            {
                SmtpClient client = new SmtpClient(_server);
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
                SmtpClient client = new SmtpClient(_server);
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

﻿using AECI.ICM.Shared.Entities;
using AECI.ICM.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace AECI.ICM.Shared.Service
{
    public class SharedEmailNotificationService : ISharedNotificationService
    {
        #region Fields

       // private  MailMessage _notificationClient;
        
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
                var success = EmailClient
                .Create(FromEmail,
                        ToEmail,
                        Subject,
                        Body,
                        Server,
                        CC, 
                        Attachment);
        }

        public void Send(List<MailMessage> emails)
        {
            SmtpClient client = new SmtpClient(Server);
            try
            {
                client.UseDefaultCredentials = true;

                foreach (var email in emails)
                    client.Send(email);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                client = null;
            }
        }
        
        #endregion
    }
}

using MonitorService.Interfaces;
using System;
using System.Net.Mail;

namespace MonitorService.Entities
{
    public class EmailClient
    {
        #region Fields

        internal string _fromEmail;
        internal string _toEmail;
        internal string _subject;
        internal string _body;

        #endregion

        #region Constructor

        private EmailClient(string fromEmail, 
                            string toEmail, string subject, 
                            string body)
        {
            if (fromEmail == default)
                throw new ArgumentNullException(nameof(_fromEmail), "Supply the source email address");

            if (toEmail == default)
                throw new ArgumentNullException(nameof(_toEmail), "Supply the destination email address");

            if (subject == default)
                throw new ArgumentNullException(nameof(_subject), "Supply the subject");

            if (body == default)
                throw new ArgumentNullException(nameof(_body), "Supply the body");

            _fromEmail = fromEmail;
            _toEmail = toEmail;
            _subject = subject;
            _body = body;

        }

        #endregion

        #region Methods

        public static MailMessage Create(string fromEmail,
                                         string toEmail, 
                                         string subject,
                                         string body)
        {
            EmailClient client = new EmailClient(fromEmail, toEmail, subject, body);
            MailMessage message = new MailMessage(client._fromEmail, client._toEmail);        
            message.Subject = client._subject;
            message.Body = client._body;

            return message;
        }

        #endregion

        #region Private Methods

        

        #endregion
    }
}

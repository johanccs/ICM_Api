using System;
using System.Net.Mail;

namespace AECI.ICM.Shared.Entities
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

        public static bool Create(string fromEmail,
                                        string toEmail,
                                        string subject,
                                        string body,
                                        string server,
                                        string cc=null,
                                        string attachmentPath=null)
        {
            //EmailClient client = new EmailClient(fromEmail, toEmail, subject, body);
            using (var message = new MailMessage(fromEmail, toEmail))
            {
                message.Subject = subject;
                message.Body = body;

                if (!string.IsNullOrEmpty(cc))
                    message.CC.Add(cc);

                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    if(attachmentPath.Contains(";"))
                    {
                        var attachments = FormatAttachments(attachmentPath);

                        foreach (var attach in attachments)
                            if(attach.Contains("pdf"))
                                message.Attachments.Add(new Attachment(attach));
                    }
                    else
                        message.Attachments.Add(new Attachment(attachmentPath));
                }

                SmtpClient client = new SmtpClient(server);
                client.UseDefaultCredentials = true;

                client.Send(message);

                return true;
            }
        }

        public static void ClearAttachments(ref MailMessage message)
        {
            message.Attachments.Clear();
            message.Attachments.Dispose();
            message = null;
        }

        private static string[] FormatAttachments(string attachments)
        {
            var values = attachments.Split(';');

            return values;
        }

        #endregion
    }
}

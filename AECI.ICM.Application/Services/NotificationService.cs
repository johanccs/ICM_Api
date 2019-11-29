using AECI.ICM.Application.Interfaces;
using AECI.ICM.Shared.Interfaces;
using System;

namespace AECI.ICM.Application.Services
{
    public class NotificationService : INotificationService
    {
        #region Fields

        private readonly ISharedNotificationService _notificationService;

        #endregion

        #region Properties

        public string From { get; set; }
        public string To { get; set; }
        public string Server { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CC { get; set; } = null;
        public string AttachmentPath { get; set; } = null;

        #endregion

        #region Constructor

        public NotificationService(ISharedNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion

        #region Methods

        public void Send()
        {
            try
            {
                _notificationService.Body = Body;
                _notificationService.Subject = Subject;
                _notificationService.ToEmail = To;
                _notificationService.Server = Server;
                _notificationService.FromEmail = From;
                _notificationService.CC = CC;
                _notificationService.Attachment = AttachmentPath;

                _notificationService.Send();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}

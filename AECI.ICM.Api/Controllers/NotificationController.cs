using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region Readonly Fields

        private readonly INotificationService _notificationService;
        private readonly ISettingsService _settingsService;

        #endregion

        #region Constructor

        public NotificationController(INotificationService notificationService, 
                                      ISettingsService _settingsService)
        {
            _notificationService = notificationService;
            this._settingsService = _settingsService;
        }

        #endregion

        #region Methods

        [HttpPost]
        public IActionResult Post(Notification.V1.CreateEmail request)
        {
            try
            {
                if (EmailReport(request))
                    return Ok("Email sent successfully");
                else
                    return BadRequest("Email not sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private bool EmailReport(Notification.V1.CreateEmail request)
        {
            try
            {
                var setting = _settingsService.GetAllAsync();
                var mailTo = setting.Emails.FirstOrDefault(p => p.Site == request.Branch).BranchManagerEmail.Trim();

                _notificationService.Body = request.Message;
                _notificationService.From = setting.WarningEmail;
                _notificationService.Server = "muchsmtp";
                _notificationService.Subject = request.Subject;
                _notificationService.To = mailTo;

                _notificationService.Send();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
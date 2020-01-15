using AECI.ICM.Api.Constants;
using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region Readonly Fields

        private readonly INotificationService _notificationService;
        private readonly ISettingsService _settingsService;
        private readonly IConfiguration _config;

        #endregion

        #region Constructor

        public NotificationController(INotificationService notificationService,
                                      IConfiguration config,
                                      ISettingsService settingsService)
        {
            _notificationService = notificationService;
            _config = config;
            _settingsService = settingsService;
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
                var smtp = _config[ApiConstants.SMTPServer];

                _notificationService.Body = request.Message;
                _notificationService.From = setting.WarningEmail;
                _notificationService.Server = smtp;
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
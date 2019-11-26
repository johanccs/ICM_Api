using AECI.ICM.Api.ViewModels;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        #region Readonly Fields

        private readonly ISettingsService _settingsCtx;

        #endregion

        #region Constructor

        public SettingController(ISettingsService settingsCtx)
        {
            _settingsCtx = settingsCtx;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetSettingsAsync()
        {
            try
            {
                var result = _settingsCtx.GetAllAsync();

                if (result == null)
                    return NotFound(result);

                return Ok(Map(result));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveSetting(SettingsViewModel setting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("An error occurred");
            }

            _settingsCtx.SaveSettingAsync(MapTo(setting));

            return Ok(0);
        }

        #endregion

        #region Private Methods

        private SettingsViewModel Map(SettingEntity unmapped)
        {
            var mapped = new SettingsViewModel();
            mapped.Id = unmapped.Id.GetId();
            mapped.Emails = unmapped.Emails;
            mapped.EnableWarning = unmapped.EnableWarning;
            mapped.SignatureLocation = unmapped.SignatureLocation;
            mapped.WarningCuttOffDate = unmapped.WarningCuttOffDate;
            mapped.WarningEmail = unmapped.WarningEmail;

            return mapped;
        }

        private SettingEntity MapTo(SettingsViewModel unmapped)
        {
            var mapped = new SettingEntity();
            mapped.Id = new SettingId(unmapped.Id);
            mapped.Emails = unmapped.Emails;
            mapped.EnableWarning = unmapped.EnableWarning;
            mapped.SignatureLocation = unmapped.SignatureLocation;
            mapped.WarningCuttOffDate = unmapped.WarningCuttOffDate;
            mapped.WarningEmail = unmapped.WarningEmail;

            return mapped;
        }

        #endregion
    }
}
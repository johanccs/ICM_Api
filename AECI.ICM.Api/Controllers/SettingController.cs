using AECI.ICM.Api.ViewModels;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Entities;
using AECI.ICM.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        #region Readonly Fields

        private readonly ISettingsService _settingsCtx;
        private readonly IConfiguration _config;

        #endregion

        #region Constructor

        public SettingController(ISettingsService settingsCtx, IConfiguration config)
        {
            _settingsCtx = settingsCtx;
            _config = config;
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

                Log("Inside setting");

                return Ok(Map(result));
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message} : Stacktrace: {ex.StackTrace}");
                return BadRequest(ex.Message);
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

        private bool Log(string message)
        {
            var folder = _config[Constants.ApiConstants.BASEREPORTFOLDER];
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if(!folder.EndsWith("\\"))
            {
                folder = $"{folder}\\Exceptions";
            }

            folder = $"{folder}\\log.txt";

            using (var sb = new StreamWriter(folder, true))
            {
                sb.WriteLine(message);
            }

            return true;
        }

        #endregion
    }
}
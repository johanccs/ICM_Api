using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionsController : ControllerBase
    {
        #region Readonly Fields

        private readonly ISettingsService _settingService;
        private readonly IResultService _resultService;

        #endregion

        #region Constructor

        public ExceptionsController(ISettingsService settingService, 
                                    IResultService resultService)
        {
            _settingService = settingService;
            _resultService = resultService;
        }

        #endregion

        #region Methods

        [HttpGet("{site}")]
        public IActionResult Get(string site)
        {
            var setting = GetSetting();
            var cutOffDay = setting.WarningCuttOffDate.Day;

            if(DateTime.Now.Day > cutOffDay)
            {
                var isException = GetExceptionResult(site);

                return Ok(isException);
            }

            return Ok(false);
        }

        #endregion

        #region Private Methods

        private bool GetExceptionResult(string site)
        {
            var monthNo = DateTime.Now.Month;
            var results = GetResults(monthNo.ToString());

            foreach(var result in results)
            {
                if (result.Branch == site)
                    return false;
            }

            return true;
        }

        private SettingEntity GetSetting()
        {
            var setting = _settingService.GetAllAsync();

            return setting;
        }

        private List<ResultEntity> GetResults(string monthNo)
        {
            var results = _resultService.GetAllAsync().Where(p=>p.Month == monthNo).ToList();

            return results;
        }

        #endregion
    }
}
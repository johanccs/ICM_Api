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
        private readonly IBranchDirectoryService _branchDirectoryService;


        #endregion

        #region Constructor

        public ExceptionsController(ISettingsService settingService,
                                    IBranchDirectoryService branchDirectoryService,
                                    IResultService resultService)
        {
            _settingService = settingService;
            _branchDirectoryService = branchDirectoryService;
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

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var setting = GetSetting();
            var cutOffDay = setting.WarningCuttOffDate.Day;

            if (DateTime.Now.Day > cutOffDay)
            {
                var exceptions = GetExceptions(setting);

                return Ok(exceptions);
            }

            return Ok();
        }

        [HttpGet]
        [Route("getByMonthNo/{num}")]
        public IActionResult GetByMonthNo(string num)
        {
            var setting = GetSetting();
            var cutOffDay = setting.WarningCuttOffDate.Day;

            if (DateTime.Now.Day > cutOffDay)
            {
                var exceptions = GetExceptions(setting).Where(p=>p.Month == num);

                return Ok(exceptions);
            }

            return Ok();
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

        private List<ResultEntity> GetExceptions(SettingEntity setting)
        {
            var monthNo = DateTime.Now.Month;
            var results = GetResults(monthNo.ToString());
            var exceptions = new List<ResultEntity>();
            var allBranches = _branchDirectoryService.GetAll();

            foreach (var branch in allBranches)
            {
                var result = results.FirstOrDefault(p => p.Branch == branch.AbbrevName);
                if (result == null)
                {
                    var settingEmail = setting.Emails.FirstOrDefault(p => 
                                    p.Site.ToLower() == branch.AbbrevName.ToLower());
                    var exception = new ResultEntity { BMName = settingEmail.BranchManagerName, Branch = branch.AbbrevName, 
                                                       FinName = settingEmail.RegionalAccountantName, Month = monthNo.ToString() };

                    exceptions.Add(exception);
                }
            }

            return exceptions;
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
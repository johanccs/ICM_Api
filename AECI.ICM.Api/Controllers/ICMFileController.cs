using AECI.ICM.Api.Constants;
using AECI.ICM.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ICMFileController : ControllerBase
    {
        #region Readonly Fields

        private readonly ICMFileService _fileService;
        private readonly IConfiguration _config;
        private readonly string _baseReportFolder;

        #endregion

        #region Constructor

        public ICMFileController(ICMFileService fileService, IConfiguration config)
        {
            _fileService = fileService;
            _config = config;
            _baseReportFolder = _config[ApiConstants.BASEREPORTFOLDER];
        }

        #endregion

        #region Methods

        [HttpGet("{site}")]
        public IActionResult Get(string site)
        {
            try
            {
                string filePath = _baseReportFolder;

                if (string.IsNullOrEmpty(filePath))
                    return BadRequest("Base Report path not found");
           
                var result = _fileService.GetFiles(filePath, site);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
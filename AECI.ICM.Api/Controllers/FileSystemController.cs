using AECI.ICM.Api.Constants;
using AECI.ICM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileSystemController:ControllerBase
    {
        #region Readonly Fields

        private readonly IConfiguration _config;
        private readonly IFileSystem _fileSystem;
        private readonly string _reportsFolder;

        #endregion

        #region Constructor

        public FileSystemController(IConfiguration config, IFileSystem fileSystem)
        {
            _config = config;
            _fileSystem = fileSystem;

            _reportsFolder = _config[ApiConstants.BASEREPORTFOLDER];
        }

        
        #endregion

        #region Methods

        [HttpGet("{folder}")]
        public IActionResult Get(string folder)
        {
            try
            {
                var result = _fileSystem.List(_reportsFolder, folder);

                if (result == null)
                    return NotFound("No reports were found");

                return Ok(result.Files);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        } 

        #endregion
    }
}

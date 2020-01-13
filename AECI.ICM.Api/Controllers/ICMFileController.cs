using AECI.ICM.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ICMFileController : ControllerBase
    {
        #region Readonly Fields

        private readonly ICMFileService _fileService;

        #endregion

        #region Constructor

        public ICMFileController(ICMFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion

        #region Methods

        [HttpGet("{site}")]
        public IActionResult Get(string site)
        {
            try
            {
                string filePath = @"D:\TestReports";
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
using AECI.ICM.Api.Constants;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        #region Readonly Fields

        private readonly IConfiguration _config;
        private readonly IBranchDirectoryService _branchDirService;

        #endregion

        #region Constructor

        public CommonController(IConfiguration config, 
                                IBranchDirectoryService branchDirService)
        {
            _config = config;
            _branchDirService = branchDirService;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("getAllBranches")]
        public IActionResult GetAllBranches()
        {
            try
            {
                var results = _branchDirService.LoadBranches(
                    new MapConfigDbCtx(
                        _config[ApiConstants.MAPCONFIGDBCTX]));

                if (results == null || results.Count == 0)
                    return NotFound("No branches found");

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
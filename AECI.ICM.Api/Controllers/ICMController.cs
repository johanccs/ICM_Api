using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AECI.ICM.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ICMController : ControllerBase
    {
        #region Readonly Fields

        private readonly IICMService _icmService;

        #endregion

        #region Constructor

        public ICMController(IICMService icmService)
        {
            _icmService = icmService;
        }

        #endregion

        #region MEthods

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _icmService.GetAllAsync();

                if (result == null)
                    return NotFound("No ICM Records found");
                else
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
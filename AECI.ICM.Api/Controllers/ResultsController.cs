using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Shared.Interfaces;
using AECI.ICM.Shared.ViewModels.MessageTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AECI.ICM.Application.Commands.ResultCommand.V1;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        #region Readonly Fields

        private readonly IResultService _resultService;
     
        #endregion

        #region Constructor
        public ResultsController(
            IResultService resultService)
        {
            _resultService = resultService;           
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Get()
        {
            try
            {
               var results = await _resultService.GetAllAsync();

                if (results == null)
                    return NotFound();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("all/{region}")]
        public async Task<IActionResult>All(string region)
        {
            try
            {
                var results = await _resultService.GetAllAsync(region);

                if (results == null)
                    return NotFound();
                
                return Ok(ResultCommand.MapFrom(results.ToList()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("getunauthorised/{region}")]
        public async Task<IActionResult> GetUnAuthorised(string region)
        {
            try
            {
                var results = await _resultService.GetAllAsync(region);

                if (results == null)
                    return NotFound();

                return Ok(ResultCommand.MapFrom(results.ToList()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("authorise")]
        public async Task<IActionResult>Post(Update request)
        {
            try
            {
                var result = await _resultService.Authorise(ResultCommand.MapTo(request));

                //LogToEventLog("",
                //    new StringBuilder().
                //    AppendLine($"{request.Branch} in {request.Region} is authorised").
                //    ToString()
                //);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        #region Private Methods

        //private void LogToEventLog(string user, string message, string cat = "Info")
        //{
        //    _logger.LogAsync(new EventLogMessage()
        //    {
        //        Application = "ICM SPA",
        //        Category = cat,
        //        Comment = "None",
        //        Date = DateTime.Now,
        //        Id = 1,
        //        Importance = "Low",
        //        Message = message,
        //        Stacktrace = null,
        //        User = user
        //    });
        //}

        #endregion
    }
}
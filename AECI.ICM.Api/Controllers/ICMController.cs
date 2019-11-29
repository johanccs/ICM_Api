using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ICMController : ControllerBase
    {
        #region Readonly Fields

        private readonly IICMService _icmService;
        private readonly INotificationService _notificationService;
        private readonly ISettingsService _settingsService;

        #endregion

        #region Constructor

        public ICMController(IICMService icmService, 
                             INotificationService notificationService,
                             ISettingsService settingsService)
        {
            _icmService = icmService;
            _notificationService = notificationService;
            _settingsService = settingsService;
        }

        #endregion

        #region Methods

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

        [HttpPost]       
        public async Task<IActionResult> Post(ResponseViewModel args)
        {
            HttpResponseMessage response = null;
            args.BMSigPath = @"C:\TestReports\bmSig.png";
            args.FinSigPath = @"C:\TestReports\bmSig.png";

            try
            {
                _icmService.Add(args);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:62176/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
                                    "application/json"));

                    var serialised = JsonConvert.SerializeObject(args);
                    var param = new StringContent(serialised, Encoding.UTF8, "application/json");

                    response = await client.PostAsync("api/Print/PrintReport", param);

                    if (response.IsSuccessStatusCode)
                    {
                        var path = response.Content.ReadAsStringAsync().Result;
                        var deserialisedPath = JsonConvert.DeserializeObject<string>(path);
                        EmailReport(deserialisedPath, args.Branch);

                        return Ok(path);
                    }
                    else
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine(response.ReasonPhrase);
                        builder.AppendLine("Ensure that print service is running and");
                        builder.AppendLine("the report is not open already");
                        return BadRequest(builder.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("PostNewICM")]
        public IActionResult PostNewICM(ICMViewModel entity)
        {
            try
            {
                _icmService.Add(entity);

                return Ok(0);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateExistICM")]
        public IActionResult UpdateExistICM(ICMViewModel entity)
        {
            try
            {
                _icmService.Add(entity);

                return Ok(0);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getPdf")]
        public HttpResponseMessage GetPdf()
        {
            return null;
        }

        #endregion

        #region Private Methods

        private void EmailReport(string reportPath, string site)
        {
            var setting = _settingsService.GetAllAsync();
            var mailTo = setting.Emails.FirstOrDefault(p => p.Site == site).BranchManagerEmail;

            _notificationService.Body = "Please find the ICM report as attachment";
            _notificationService.From = "muchreply@muchasphalt.com";
            _notificationService.Server = "muchsmtp";
            _notificationService.Subject = $"ICM report - {DateTime.Now.ToShortDateString()}";
            _notificationService.To = mailTo;
            _notificationService.AttachmentPath = reportPath;

            if (setting.EnableWarning)
                _notificationService.CC = setting.WarningEmail;

            _notificationService.Send();
        }
        
        #endregion
    }
}
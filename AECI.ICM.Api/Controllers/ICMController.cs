using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
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
        public async Task<FileResult> Post(ResponseViewModel args)
        {
            HttpResponseMessage response = null;
            args.BMSigPath = BuildSignaturePath(args);
            args.FinSigPath = BuildFinSigPath();

            try
            {
                _icmService.Add(args);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:62176/");
                    //client.BaseAddress = new Uri("http://madev04:8090/");
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

                        byte[] filebytes = System.IO.File.ReadAllBytes(deserialisedPath);
                        var file = Path.GetFileName(path);

                        return File(filebytes, System.Net.Mime.MediaTypeNames.Application.Pdf, file);
                        //return Ok(path);
                      
                    }
                    else
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine(response.ReasonPhrase);
                        builder.AppendLine("Ensure that print service is running and");
                        builder.AppendLine("the report is not open already");
                        //return BadRequest(builder.ToString());
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                //return BadRequest(ex.Message);
                return null;
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

        [HttpGet("{filename}")]
        [Route("getPdf")]
        public FileResult GetPdf(string filename)
        {
            byte[] filebytes = System.IO.File.ReadAllBytes(filename);
            var file = Path.GetFileName(filename);

            return File(filebytes, System.Net.Mime.MediaTypeNames.Application.Pdf, file);
        }

        #endregion

        #region Private Methods

        private string BuildSignaturePath(ResponseViewModel args)
        {
            var basePath = @"D:\TestReports\";
            var sigPath = basePath;
            if (args.Branch.ToLower() == "GEO".ToLower())
                sigPath += @"GEO\";
            else if (args.Branch.ToLower() == "UMT".ToLower())
                sigPath += @"UMT\";
            else if (args.Branch.ToLower() == "POM".ToLower())
                sigPath += @"POM\";
            else if (args.Branch.ToLower() == "EMP".ToLower())
                sigPath += @"EMP\";
            else
            {
                sigPath += "NoSig.png";
                return sigPath;
            }

            sigPath += "bmsig.jpg";

            if (System.IO.File.Exists(sigPath))
                return sigPath;

            return basePath += "NoSig.png";
        }

        private string BuildFinSigPath()
        {
            var basePath = @"D:\TestReports\";
            var finsigPath = basePath;
            finsigPath += "finsig.png";

            var nosigPath = basePath += "NoSig.png";

            if (!System.IO.File.Exists(basePath))
                return nosigPath;

            return finsigPath;
        }

        private bool EmailReport(string reportPath, string site)
        {
            try
            {
                var setting = _settingsService.GetAllAsync();
                var mailTo = setting.Emails.FirstOrDefault(p => p.Site == site).BranchManagerEmail;

                _notificationService.Body = BuildBody(site);
                _notificationService.From = "muchreply@muchasphalt.com";
                _notificationService.Server = "muchsmtp";
                _notificationService.Subject = $"ICM report - {DateTime.Now.ToShortDateString()}";
                _notificationService.To = mailTo;
                _notificationService.AttachmentPath = reportPath;

                if (setting.EnableWarning)
                    _notificationService.CC = setting.WarningEmail;

                _notificationService.Send();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string BuildBody(string site)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Please find the ICM report as attachment");
            sb.AppendLine($"Site: {site}");
            sb.AppendLine($"Date: {DateTime.Now.ToString("dd/MM/yyyy")}");

            return sb.ToString();
        }

        public FileStreamResult DeSerialize(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            //var stream = new FileStream(@"C:\TestReports\t.pdf", FileMode.Open);

            return new FileStreamResult(stream, "application/pdf");
        }

        #endregion
    }
}
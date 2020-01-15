using AECI.ICM.Api.Constants;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Domain.Interfaces;
using AECI.ICM.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        private readonly string _systemStatus;
        private readonly string _smtp;
       
        #endregion

        #region Fields

        private string _baseReportPath;

        #endregion

        #region Constructor

        public ICMController(IICMService icmService, 
                             INotificationService notificationService,
                             IConfiguration config,
                             ISettingsService settingsService)
        {
            _icmService = icmService;
            _notificationService = notificationService;
            _config = config;
            _settingsService = settingsService;
            _systemStatus = _config[ApiConstants.SYSTEMSTATUS];
            _baseReportPath = config[ApiConstants.BASEREPORTFOLDER];
            _smtp = config[ApiConstants.SMTPServer];
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
        public async Task<FileResult> Post(ResponseViewModel request)
        {
            HttpResponseMessage response = null;
            request.BMSigPath = BuildSignaturePath(request);
            request.FinSigPath = BuildFinSigPath();

            try
            {
                using (var client = new HttpClient())
                {
                    var debugReportApi = _config[ApiConstants.DEBUGREPORTAPIURL];
                    var prodReportApi = _config[ApiConstants.PRODREPORTAPIURL];
                    
                    if(_systemStatus.ToLower() == "debug".ToLower() || 
                       _systemStatus.ToLower() == "Test".ToLower())
                        client.BaseAddress = new Uri(debugReportApi);
                    else
                        client.BaseAddress = new Uri(prodReportApi);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
                                    "application/json"));

                    var serialised = JsonConvert.SerializeObject(request);
                    var param = new StringContent(serialised, Encoding.UTF8, "application/json");

                    response = await client.PostAsync("api/Print/PrintReport", param);

                    if (response.IsSuccessStatusCode)
                    {
                        var path = response.Content.ReadAsStringAsync().Result;
                        var deserialisedPath = JsonConvert.DeserializeObject<string>(path);

                        EmailReport(deserialisedPath, request.Branch);
                        _icmService.Add(request);

                        byte[] filebytes = System.IO.File.ReadAllBytes(deserialisedPath);
                        var file = Path.GetFileName(path);

                        return File(filebytes, System.Net.Mime.MediaTypeNames.Application.Pdf, file);                      
                    }
                    else
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine(response.ReasonPhrase);
                        builder.AppendLine("Ensure that print service is running and");
                        builder.AppendLine("the report is not open already");
            
                        return null;
                    }
                }
            }
            catch (Exception)
            {
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

        [HttpPost]
        [Route("getPdf")]
        public FileResult GetPdf(Application.Commands.ICM.V1.Reprint request)
        {
            var folder = "D:\\TestReports\\";
            var file = Path.Combine(folder,request.Branch,request.FileName);

            byte[] filebytes = System.IO.File.ReadAllBytes(file);
            var filename = Path.GetFileName(file);

            return File(filebytes, System.Net.Mime.MediaTypeNames.Application.Pdf, filename);
        }

        #endregion

        #region Private Methods

        private string BuildSignaturePath(ResponseViewModel args)
        {
            var sigPath = _baseReportPath;

            if(!sigPath.EndsWith(@"\"))
                sigPath = $"{sigPath}\\";

            if (args.Branch.ToLower() == "GEO".ToLower())
                sigPath += @"GEO\";
            
            else if (args.Branch.ToLower() == "ER".ToLower())
                sigPath += @"ER\";
            
            else if (args.Branch.ToLower() == "CK".ToLower())
                sigPath += @"CK\";

            else if (args.Branch.ToLower() == "PE".ToLower())
                sigPath += @"PE\";

            else if (args.Branch.ToLower() == "BEN".ToLower())
                sigPath += @"BEN\";

            else if (args.Branch.ToLower() == "BEN2".ToLower())
                sigPath += @"BEN2\";

            else if (args.Branch.ToLower() == "POM".ToLower())
                sigPath += @"POM\";

            else if (args.Branch.ToLower() == "POL".ToLower())
                sigPath += @"POL\";

            else if (args.Branch.ToLower() == "Wit".ToLower())
                sigPath += @"Wit\";

            else if (args.Branch.ToLower() == "EMP".ToLower())
                sigPath += @"EMP\";

            else if (args.Branch.ToLower() == "BFN".ToLower())
                sigPath += @"BFN\";

            else if (args.Branch.ToLower() == "COED".ToLower())
                sigPath += @"COED\";

            else if (args.Branch.ToLower() == "EIK".ToLower())
                sigPath += @"EIK\";

            else if (args.Branch.ToLower() == "RDP".ToLower())
                sigPath += @"RDP\";

            else if (args.Branch.ToLower() == "ECA".ToLower())
                sigPath += @"ECA\";

            else if (args.Branch.ToLower() == "HO".ToLower())
                sigPath += @"HO\";

            else if (args.Branch.ToLower() == "PMB".ToLower())
                sigPath += @"PMB\";

            else if (args.Branch.ToLower() == "UMT".ToLower())
                sigPath += @"UMT\";
            else
            {
                sigPath += "NoSig.png";
                return sigPath;
            }

            sigPath += "bmsig.png";

            if (System.IO.File.Exists(sigPath))
                return sigPath;

            return _baseReportPath += "NoSig.png";
        }

        private string BuildFinSigPath()
        {
            var basePath = _baseReportPath;
            if (!basePath.EndsWith(@"\"))
                basePath = $"{basePath}\\";

            var finsigPath = basePath;
            finsigPath += "finsig.png";

            var nosigPath = basePath += "NoSig.png";

            if (!System.IO.File.Exists(finsigPath))
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
                _notificationService.Server =_smtp;
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

        #endregion
    }
}
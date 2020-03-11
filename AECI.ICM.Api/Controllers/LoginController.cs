using AECI.ICM.Api.Constants;
using AECI.ICM.Api.ViewModels;
using AECI.ICM.Application.ApplicationEnums;
using AECI.ICM.Application.ApplicationExceptions;
using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Shared.Interfaces;
using AECI.ICM.Shared.ViewModels.MessageTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Private readonly fields

        private SystemStatusEnum _debug;
        private readonly IBranchDirectoryService _branchDirectoryService;
        private readonly IConfiguration _config;        
        private readonly ISettingsService _settingsService;

        #endregion

        #region Constructor

        public LoginController(IBranchDirectoryService branchDirectory,                              
                               ISettingsService settingsService,
                               IConfiguration config)
        {
            _branchDirectoryService = branchDirectory;            
            _config = config;         
            _settingsService = settingsService;

            _debug = (SystemStatusEnum)Enum.Parse(typeof(SystemStatusEnum), 
                    _config[ApiConstants.SYSTEMSTATUS]);
        }

        #endregion

        [HttpPost]
        public IActionResult Post(Login.V1.Login request)
        {
            try
            {
                if (_debug == SystemStatusEnum.Prod)
                {
                    if (!Authenticate(request))
                        return Unauthorized("Invalid username or password");
                }
                var loggedUser = QueryADUser(request.Username);
                loggedUser = SetSiteAbbreviation(loggedUser);
                loggedUser.Region = _settingsService.GetRegion(request.Site);

                if (ValidateLoggedUser(loggedUser))
                    return NotFound(0);

                loggedUser = SetRoles(loggedUser);

                LogToEventLog(request.Username, $"{loggedUser.ADUser} logged in @ {DateTime.Now}");

                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                LogToEventLog(request.Username, $"{ex.Message}: @ {DateTime.Now}", "Error");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}")]
        public IActionResult Get(string username)
        {
            try
            {
                LogToEventLog(username, $"{username} - Before QueryAD @ {DateTime.Now}", "Error");
                var loggedUser = QueryADUser(username);
                var branches = _branchDirectoryService.LocateByFullBranchName(loggedUser.Site);

                LogToEventLog(username, $"{username} Get method was hit. Branches found: @ {DateTime.Now}", "Error");

                return Ok(branches);
            }
            catch (Exception ex)
            {
                LogToEventLog(username, $"{ex.Message}: @ {DateTime.Now}", "Error");
                return BadRequest(ex.Message);
            }
        }

        #region Private Methods

        private void LogToEventLog(string user, string message, string cat = "Info")
        {
            //_logger.LogAsync(new EventLogMessage()
            //{
            //    Application = "ICM SPA",
            //    Category = cat,
            //    Comment = "None",
            //    Date = DateTime.Now,
            //    Id = 1,
            //    Importance = "High",
            //    Message = message,
            //    Stacktrace = null,
            //    User = user
            //});

            var errorFile = @"C:\TestReports\Exceptions\Exceptions.txt";
            System.IO.File.AppendAllText(errorFile, message + "\n");
        }

        private void LogToOnlineApi(Login.V1.Login request, string message)
        {
            //_logger.LogAsync(new InfoMessage().Set(
            //          $"{message} {DateTime.Now}",
            //          request.Site, request.Username
            //    ), "http://madev04:8081");
        }

        private ILoginCommand SetSiteAbbreviation(ILoginCommand loggedUser)
        {
            loggedUser.Site = _branchDirectoryService
                                .LocateByFullBranchName(loggedUser.Site, true)
                                .AbbrevName;
            return loggedUser;
        }

        private bool ValidateLoggedUser(ILoginCommand loggedUser)
        {
            return (string.IsNullOrEmpty(loggedUser.Email) &&
                    string.IsNullOrEmpty(loggedUser.DisplayName) &&
                    string.IsNullOrEmpty(loggedUser.Username));
        }

        private ILoginCommand SetRoles(ILoginCommand loggedUser)
        {
            if (loggedUser.ADUser == "mrma86423")
                loggedUser.Role = Enums.Enums.RoleEnum.Administrator.ToString();
            else
                loggedUser.Role = Enums.Enums.RoleEnum.User.ToString();

            return loggedUser;
        }

        private ILoginCommand QueryADUser(string username)
        {
            try
            {
                //todo:
                if (_debug == SystemStatusEnum.Debug)
                    throw new AuthException(@"ma\majobs", "AD connection could not be established");

                var domPassword = "6a13tatqd9XRFkNUOFsC55GUrmiAjKelHokNDS2nW4u7Ipf2sswbUYDLMVXmkOq";
                    var domain = "192.168.210.45";
                    var user = new Login.V1.Login();

                    using (DirectoryEntry entry = new DirectoryEntry(domain, "ma\\majobs", domPassword))
                    {
                        entry.Path = $"LDAP://{domain}";

                        using (DirectorySearcher searcher = new DirectorySearcher(entry))
                        {
                            searcher.Filter = $"SAMAccountName={username}";
                       
                            var result = searcher.FindAll();

                            foreach (SearchResult sr in result)
                            {
                                user.ADUser = sr.Properties["samaccountname"][0].ToString();
                                user.Email = sr.Properties["mail"][0].ToString();
                                user.Username = sr.Properties["samaccountname"][0].ToString();
                                user.DisplayName = sr.Properties["displayname"][0].ToString();                                
                                user.IsGM = sr.Properties["title"][0].ToString().ToLower() == "General Manager".ToLower();
                                user.SystemStatus = _debug.ToString();

                            if (sr.Properties["office"].Count > 0)
                                user.Site = sr.Properties["office"][0].ToString();
                            else
                                user.Site = sr.Properties["physicaldeliveryofficename"][0].ToString();
                            }
                        }
                    }
                    return user;
            }
            catch (AuthException)
            {
                var exception = new NullRefLogin
                {
                    ADUser = "mrma86423",
                    DisplayName = "Johan Potgieter",
                    Email = "johan1.potgieter@muchasphalt.com",
                    Site = "Head Office",
                    Username = "mrma86423",
                    SystemStatus = SystemStatusEnum.Debug.ToString(),
                };

                return exception;
            }
            catch (Exception ex)
            {
                LogToEventLog(username, $"{ex.Message}: @ {DateTime.Now}", "Error");
                throw;
            }
        }

        private bool Authenticate(Login.V1.Login request)
        {
            using(PrincipalContext pc = new PrincipalContext(ContextType.Domain, "192.168.210.45"))
            {
                bool isValid = pc.ValidateCredentials(request.Username, request.Password);

                return isValid;
            }
        }

        #endregion

    }
}
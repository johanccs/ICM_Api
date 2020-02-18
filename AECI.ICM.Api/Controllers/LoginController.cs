using AECI.ICM.Api.Constants;
using AECI.ICM.Api.ViewModels;
using AECI.ICM.Application.ApplicationEnums;
using AECI.ICM.Application.ApplicationExceptions;
using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using AECI.ICM.Application.Models.MessageTypes;
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
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        public LoginController(IBranchDirectoryService branchDirectory,
                               ILogger logger,
                               IConfiguration config)
        {
            _branchDirectoryService = branchDirectory;
            _logger = logger;
            _config = config;

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
               
                if (ValidateLoggedUser(loggedUser))
                    return NotFound(0);

                loggedUser = SetRoles(loggedUser);

                LogToOnlineApi(request, "User has logged on at");

                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}")]
        public IActionResult Get(string username)
        {
            try
            {
                var loggedUser = QueryADUser(username);
                var branches = _branchDirectoryService.LocateByFullBranchName(loggedUser.Site);

                return Ok(branches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Private Methods

        private void LogToOnlineApi(Login.V1.Login request, string message)
        {
            _logger.LogAsync(new InfoMessage().Set(
                      $"{message} {DateTime.Now}",
                      request.Site, request.Username
                ), "http://madev04:8081");
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
                    Email = "johan.ccs@gmail.com",
                    Site = "Head Office",
                    Username = "mrma86423",
                    SystemStatus = SystemStatusEnum.Debug.ToString()
                };

                return exception;
            }
            catch (Exception)
            {
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
using AECI.ICM.Api.Constants;
using AECI.ICM.Api.ViewModels;
using AECI.ICM.Application.ApplicationEnums;
using AECI.ICM.Application.ApplicationExceptions;
using AECI.ICM.Application.Commands;
using AECI.ICM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.DirectoryServices;

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

        #endregion

        #region Constructor

        public LoginController(IBranchDirectoryService branchDirectory, 
                               IConfiguration config)
        {
            _branchDirectoryService = branchDirectory;
            _config = config;
            _debug = (SystemStatusEnum)Enum.Parse(typeof(SystemStatusEnum), _config[ApiConstants.SYSTEMSTATUS]);
        }

        #endregion

        [HttpPost]
        public IActionResult Post(Login.V1.Login request)
        {
            try
            {
                var loggedUser = Authenticate(request.Username);
                loggedUser.Site = _branchDirectoryService
                                   .LocateByFullBranchName(loggedUser.Site,true)
                                   .AbbrevName;
               
                if (string.IsNullOrEmpty(loggedUser.Email) && 
                    string.IsNullOrEmpty(loggedUser.DisplayName) &&
                    string.IsNullOrEmpty(loggedUser.Username))
                        return NotFound(0);

                if (loggedUser.ADUser == "mrma86423")
                    loggedUser.Role = Enums.Enums.RoleEnum.Administrator.ToString();
                else
                    loggedUser.Role = Enums.Enums.RoleEnum.User.ToString();

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
                var loggedUser = Authenticate(username);
                var branches = _branchDirectoryService.LocateByFullBranchName(loggedUser.Site);

                return Ok(branches);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Private Methods

        public ILoginCommand Authenticate(string username)
        {
            try
            {
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
                            //searcher.PropertiesToLoad.Add("DisplayName");
                            //searcher.PropertiesToLoad.Add("SAMAccountName");
                            //searcher.PropertiesToLoad.Add("Mail");
                            //searcher.PropertiesToLoad.Add("");

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

        #endregion

    }
}
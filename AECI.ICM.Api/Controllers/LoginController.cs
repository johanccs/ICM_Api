using AECI.ICM.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.DirectoryServices;

namespace AECI.ICM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            try
            {
                var loggedUser = Authenticate(user.Username);

                if (string.IsNullOrEmpty(loggedUser.Email) && 
                    string.IsNullOrEmpty(loggedUser.DisplayName) &&
                    string.IsNullOrEmpty(loggedUser.Username))
                        return NotFound(0);

                if (loggedUser.ADUser == "mrma86423")
                    loggedUser.Role = Enums.Enums.RoleEnum.Administrator.ToString();
                else
                    loggedUser.Role = Enums.Enums.RoleEnum.User.ToString();

                loggedUser.Site = user.Site;
                
                return Ok(loggedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Private Methods

        public LoginViewModel Authenticate(string username)
        {
            try
            {
                var domPassword = "6a13tatqd9XRFkNUOFsC55GUrmiAjKelHokNDS2nW4u7Ipf2sswbUYDLMVXmkOq";
                var domain = "192.168.210.45";
                var user = new LoginViewModel();

                using (DirectoryEntry entry = new DirectoryEntry(domain, "ma\\majobs", domPassword))
                {
                    entry.Path = $"LDAP://{domain}";

                    using(DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = $"SAMAccountName={username}";
                        searcher.PropertiesToLoad.Add("DisplayName");
                        searcher.PropertiesToLoad.Add("SAMAccountName");
                        searcher.PropertiesToLoad.Add("Mail");
                        //searcher.PropertiesToLoad.Add("");

                        var result = searcher.FindAll();                       

                        foreach(SearchResult sr in result)
                        {
                            user.ADUser = sr.Properties["samaccountname"][0].ToString();
                            user.Email = sr.Properties["mail"][0].ToString();
                            user.Username = sr.Properties["samaccountname"][0].ToString();
                            user.DisplayName = sr.Properties["displayname"][0].ToString();
                        }
                    }
                }
                return user;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
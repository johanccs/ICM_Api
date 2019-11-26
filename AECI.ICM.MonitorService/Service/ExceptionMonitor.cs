using MonitorService.Entities;
using MonitorService.Interfaces;
using MonitorService.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace MonitorService.Services
{
    public class ExceptionMonitor : IExceptionMonitor
    {
        #region Fields

        private MonDbContext _dbContext;
        private Setting _currSetting;
        private string muchEmail = "muchReply@muchasphalt.com";
        private string _lock;
      
        #endregion

        #region Readonly Fields

        private readonly INotificationService _notificationService;

        #endregion

        #region Constructor

        public ExceptionMonitor()
        {
            _dbContext = new MonDbContext();
            _notificationService = new EmailNotificationService(
                   server: "muchsmtp",
                   fromEmail: muchEmail
            );
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            _currSetting = GetSetting();
            var cuttOffDay = _currSetting.WarningCuttOffDate.Day;

            if(DateTime.Now.Day > cuttOffDay)
            {
                var exceptions = CheckOutstandingSites();
                var mailMessages = new List<MailMessage>();

                foreach (var exception in exceptions)
                {
                    var mailTo = _currSetting.Emails.First(p => p.Site == exception.Site).BranchManagerEmail;
                    var message = new MailMessage(muchEmail, mailTo);
                    
                    message.Subject = "AECI ICM Exception List";
                    message.Body = BuildBody(exception);

                    mailMessages.Add(message);
                }

                _notificationService.Send(mailMessages);
            }
        }

        #endregion

        #region Private Methods

        private List<ResultException> CheckOutstandingSites()
        {
            try
            {
                var exceptionSites = new List<ResultException>();
                var results = GetICMResults();
                var sites = BuildSites();

                foreach(KeyValuePair<int,string>site in sites)
                {
                    var ret = results.FirstOrDefault(p => p.Branch == site.Value);

                    if (ret == null)
                        exceptionSites.Add(new ResultException {
                            Date = DateTime.Now,
                            Month = Convert.ToInt16(
                                DateTime.Now.Month),
                                Site = site.Value});
                }

                if (exceptionSites == null)
                    return new List<ResultException>();
                else
                    return exceptionSites;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Setting GetSetting()
        {
            return _dbContext.Setting.Include(p => p.Emails).FirstOrDefault();
        }

        private List<Result> GetICMResults()
        {
            var currMonth = DateTime.Now.Month.ToString();
            var currResults = _dbContext.Results.Where(p => p.Month == currMonth).ToList();
            if (currResults == null)
                return new List<Result>();

            return currResults;
        }

        private Dictionary<int,string> BuildSites()
        {
            var sites = new Dictionary<int, string>();

            //sites.Add(11, "ER");
            //sites.Add(14, "CK");
            //sites.Add(23, "George");
            sites.Add(21, "PE");
            //sites.Add(31, "BFN");
            //sites.Add(40, "BEN2");
            //sites.Add(41, "BEN");
            //sites.Add(42, "EIK");
            //sites.Add(43, "RDP");
            //sites.Add(44, "POM");
            //sites.Add(47, "POL");
            //sites.Add(48, "WIT");
            //sites.Add(51, "COED");
            //sites.Add(52, "EMP");
            //sites.Add(53, "PMB");
            //sites.Add(82, "ECA");
            //sites.Add(84, "UMT");

            return sites;
        }

        private string BuildBody(ResultException ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Please note that the ICM has not been completed for {DateTime.Now.ToShortDateString()}");
            sb.AppendLine($"Outstanding site - {ex.Site}");
            sb.AppendLine($"Month - {ex.Month}");

            return sb.ToString();
        }

        #endregion
    }
}

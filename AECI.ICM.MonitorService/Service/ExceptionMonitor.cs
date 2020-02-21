using AECI.ICM.Shared.Interfaces;
using AECI.ICM.Shared.Service;
using MonitorService.Entities;
using MonitorService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
        private string muchEmail = "muchreply@muchasphalt.com";
        private string _currMonth = string.Empty;
        private EventLog _eventLog;

        #endregion

        #region Readonly Fields

        private readonly ISharedNotificationService _notificationService;

        #endregion

        #region Constructor

        public ExceptionMonitor()
        {
            _dbContext = new MonDbContext();
            _notificationService = new SharedEmailNotificationService();
            _notificationService.Body = "";
            _notificationService.FromEmail = "muchasphalt@muchasphalt.com";
            _notificationService.Server = "muchsmtp";
            _notificationService.Subject = "ICM Report";
            _notificationService.ToEmail = "";
        }

        #endregion

        #region Public Methods

        public void Start(EventLog eventLog)
        {
            try
            {
                _eventLog = eventLog;
                _currSetting = GetSetting();
                var cuttOffDay = _currSetting.WarningCuttOffDate.Day;
                
                if (DateTime.Now.Day > cuttOffDay)
                {
                    var outstandingSites = CheckOutstandingSites();
                    if (SendNotificationToAccountant(outstandingSites))
                        SendNotificationToBranchManagers(outstandingSites);

                    WriteLogToEventViewer(
                        "Start method completed. See Log entries for individual messages",
                        "Exception Monitor Start");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Methods

        private void WriteLogToEventViewer(Exception ex, String source)
        {
            var errBuilder = new StringBuilder();

            errBuilder.AppendLine($"An error occurred in: {source}");
            errBuilder.AppendLine($"Message: {ex.Message}");
            errBuilder.AppendLine($"Stacktrace: {ex.StackTrace}");
            errBuilder.AppendLine($"Date: {DateTime.Now}");

            _eventLog.WriteEntry(errBuilder.ToString(), EventLogEntryType.Error);
        }

        private void WriteLogToEventViewer(string message, String source)
        {
            var msgBuilder = new StringBuilder();

            msgBuilder.AppendLine($"Task is completed in source: {source}");
            msgBuilder.AppendLine($"Message: {message}");
            msgBuilder.AppendLine($"Date: {DateTime.Now}");

            _eventLog.WriteEntry(msgBuilder.ToString(), EventLogEntryType.SuccessAudit);
        }

        private bool SendNotificationToBranchManagers(List<ResultException>outstandingSites)
        {
            _eventLog.WriteEntry("Sending emails to BranchManagers");
            var mailMessages = new List<MailMessage>();

            try
            {
                foreach (var site in outstandingSites)
                {
                    mailMessages.Add(BuildMessage(site, _currSetting));
                }

                _notificationService.Send(mailMessages);

                WriteLogToEventViewer(
                    "Email sent successfully",
                    "SendNotificationToBranchManagers");

                return true;
            }
            catch (Exception ex)
            {
                WriteLogToEventViewer(ex, "SendNotificationToBranchManagers");
                throw;                
            }
        }

        private bool SendNotificationToAccountant(List<ResultException>outstandingSites)
        {
            var accountant = GetUserFromEmail(_currSetting.WarningEmail);
            var builder = new StringBuilder();
            var mailMessages = new List<MailMessage>();

            try
            {
                foreach (var outstandingSite in outstandingSites)
                {
                    builder.AppendLine(
                        $"Outstanding site: {outstandingSite.Site} - month: {GetMonthName(_currMonth)}");
                }

                mailMessages.Add(BuildMessage(
                    builder.ToString(),
                    _currSetting.WarningEmail,
                    accountant
                ));

                _notificationService.Send(mailMessages);

                WriteLogToEventViewer(
                    "Email sent successfully", 
                    "SendNotificationToBranchManagers");

                return true;
            }
            catch (Exception ex)
            {
                WriteLogToEventViewer(ex, "SendNotificationToBranchManagers");
                throw;
            }
        }

        private string GetUserFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            string[] values = email.Split('.');
            string name = values[0].ToString();
            string firstChar = name.Substring(0, 1).ToUpper();
            string restOfName = name.Substring(1);
            string fullName = $"{firstChar}{restOfName}";

            return fullName;
        }
        
        private List<ResultException> CheckOutstandingSites()
        {
            try
            {
                var exceptionSites = new List<ResultException>();
                var results = GetICMResults();
                var sites = BuildSites();

                foreach(KeyValuePair<int,string>site in sites)
                {
                    var ret = results.Any(p => p.Branch.ToLower() == site.Value.ToLower());

                    if (!ret)
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
            var currDate = DateTime.Now.AddMonths(-1);
            _currMonth = currDate.Month.ToString();
            var currResults = _dbContext.Results.Where(p => p.Month == _currMonth).ToList();
            if (currResults == null)
                return new List<Result>();

            return currResults;
        }

        private Dictionary<int,string> BuildSites()
        {
            var sites = new Dictionary<int, string>();

            sites.Add(11, "ER");
            sites.Add(14, "CK");
            sites.Add(23, "GEO");
            sites.Add(21, "PE");
            sites.Add(31, "BFN");
            sites.Add(40, "BEN2");
            sites.Add(41, "BEN");
            sites.Add(42, "EIK");
            sites.Add(43, "RDP");
            sites.Add(44, "POM");
            sites.Add(47, "POL");
            sites.Add(48, "WIT");
            sites.Add(51, "COED");
            sites.Add(52, "EMP");
            sites.Add(53, "PMB");
            sites.Add(82, "ECA");
            sites.Add(84, "UMT");
            sites.Add(01, "HO");

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

        private string BuildBody(string sites, string accountant)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Hi {accountant}");
            sb.AppendLine();
            sb.AppendLine($"Please note that the ICM has not been completed for " +
                $"{ConvertToShortDate(DateTime.Now.AddMonths(-1))}");
            sb.AppendLine($"Outstanding sites:");
            sb.AppendLine($"{sites}");

            sb.AppendLine();
            sb.AppendLine("Regards");
            sb.AppendLine("ICM Monitor Service");

            return sb.ToString();
        }

        private string BuildBody(string bmName)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Hi {bmName}");
            builder.AppendLine($"Please note that ICM submission for {GetMonthName(_currMonth)} is overdue");
            builder.AppendLine("Please ensure that it is completed before the close of day");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("Regards");
            builder.AppendLine("Much Asphalt");

            return builder.ToString();
        }

        private MailMessage BuildMessage(string body, string toAddress, string accountant)
        {
            var message = new MailMessage(muchEmail, toAddress);

            message.Subject = "AECI ICM Exception List";
            
            message.Body = BuildBody(body, accountant);

            return message;
        }

        private MailMessage BuildMessage(ResultException exception, string cc)
        {
            var mailTo = _currSetting.Emails
                        .First(p => p.Site.ToLower() == exception.Site.ToLower())
                        .BranchManagerEmail;

            var message = new MailMessage(muchEmail, mailTo);

            message.Subject = "AECI ICM Exception List";
            message.Body = BuildBody(exception);
            message.CC.Add(cc);

            return message;
        }

        private MailMessage BuildMessage(ResultException site, 
                                         Setting currSetting)
        {
            string mailTo = currSetting.
                   Emails.FirstOrDefault(p => p.Site.ToLower() ==
                   site.Site.ToLower()).BranchManagerEmail;

            var recipientName = currSetting.
                                Emails.FirstOrDefault(p=>p.Site.ToLower() == 
                                site.Site.ToLower()).BranchManagerName;

            var message = new MailMessage(muchEmail, mailTo);
            message.Subject = "Overdue ICM submission";
            message.Body = BuildBody(recipientName);

            return message;
        }

        #endregion

        #region Util Methods

        private string ConvertToShortDate(DateTime date)
        {
            var month = date.Month;

            var convertedDate = GetMonthName(month.ToString());

            return convertedDate;
        }

        private string GetMonthName(string month)
        {
            System.Globalization.DateTimeFormatInfo mfi = 
                new System.Globalization.DateTimeFormatInfo();

            string monthName = mfi.GetMonthName(Convert.ToInt32(month));
            string year = DateTime.Now.Year.ToString();

            return $"{monthName} {year}";
        }

        #endregion


    }
}

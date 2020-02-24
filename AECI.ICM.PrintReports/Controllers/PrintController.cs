using AECI.ICM.Shared.ViewModels;
using CreateTestReports.Reports;
using GrapeCity.ActiveReports.Export;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AECI.ICM.PrintReports.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PrintController : ApiController
    {
        public HttpResponseMessage PrintReport(ResponseViewModel param)
        {
            var imgPath = Path.Combine(param.ReportBasePath, "muchlogo.png");
            var ext = "pdf";

            var filePath = CreateFilePath(param);

            var fileName = CreateFileName(param);

            if(fileName == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, fileName);
            }
           
            var fullPath = Path.Combine(filePath, fileName);
            fullPath = $"{fullPath}.{ext}";

            IDocumentExportEx docEx = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
            var report = BuildReport(param, imgPath);

            report.PageSettings.Orientation = 
                GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;

            report.DataSource = param.ICMElements;

            report.Run();

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    docEx.Export(report.Document, stream);
                }
              
                return Request.CreateResponse(HttpStatusCode.OK, fullPath);
            }
            catch (Exception ex)
            {
                Logging(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private arICM BuildReport(ResponseViewModel param, string imgPath)
        {
            var report = new arICM();
            report.lblBranch.Text = param.Branch;
            report.lblDate.Text = param.Date.ToShortDateString();
            report.lblMonth.Text = param.Month;
            report.pictLogo.Image = Image.FromFile(imgPath);
            report.pictLogo.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;

            report.picBMSig.Image = Image.FromFile(param.BMSigPath);
            report.picFinSig.Image = Image.FromFile(param.FinSigPath);

            report.picBMSig.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;
            report.picFinSig.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Clip;

            report.picBMSig.PictureAlignment = GrapeCity.ActiveReports.SectionReportModel.PictureAlignment.TopLeft;
            report.picFinSig.PictureAlignment = GrapeCity.ActiveReports.SectionReportModel.PictureAlignment.TopLeft;

            report.lblGenComments.Text = param.GenComments;
            report.lblBMName.Text = param.BMName;
            report.lblFinName.Text = param.FinName;
            report.lblSignedDate.Text = param.DateSigned.ToShortDateString();

            return report;
        }
        
        private string CreateFileName(ResponseViewModel entity)
        {
            var result = $"ICMReport_{entity?.Branch}_{entity?.Month}_{DateTime.Now.Year}";

            return result;
        }

        private string CreateFilePath(ResponseViewModel param)
        {
            var folderPath = Path.Combine(param.ReportBasePath, param.Branch);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }

        private bool Logging(string msg)
        {
            try
            {
                if (!Directory.Exists("C:\\TestReports\\Exceptions"))
                    Directory.CreateDirectory("C:\\TestReports\\Exceptions");

                using (var sr = new StreamWriter(@"C:\TestReports\Exceptions\Exceptions.txt",true))
                {
                    sr.WriteLine(msg);
                    sr.Flush();
                }

                return true;
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw;
            }
        }
    }
}

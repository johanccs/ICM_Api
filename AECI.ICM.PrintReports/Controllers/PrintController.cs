using AECI.ICM.Shared.ViewModels;
using CreateTestReports.Reports;
using GrapeCity.ActiveReports.Export;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AECI.ICM.PrintReports.Controllers
{
    public class PrintController : ApiController
    {   
        public HttpResponseMessage PrintReport(ResponseViewModel param)
        {
            var imgPath = @"C:\TestReports\MuchLogo.png";
            var filePath = @"C:\TestReports\";
            var fileName = CreateFileName(param);
            var ext = "pdf";
            var fullPath = Path.Combine(filePath, fileName);
            fullPath = $"{fullPath}.{ext}";

            IDocumentExportEx docEx = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
            var report = BuildReport(param, imgPath);
            report.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;

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
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Report already open");
            }
        }

        private arICM BuildReport(ResponseViewModel param, string imgPath)
        {
            var report = new arICM();
            report.lblBranch.Text = param.Branch;
            report.lblDate.Text = param.Date.ToShortDateString();
            report.lblMonth.Text = param.Month;
            report.pictLogo.Image = System.Drawing.Image.FromFile(imgPath);
            report.pictLogo.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;

            report.picBMSig.Image = Image.FromFile(param.BMSigPath);
            report.picFinSig.Image = Image.FromFile(param.BMSigPath);
            report.picBMSig.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;
            report.picFinSig.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Zoom;
            report.lblGenComments.Text = param.GenComments;
            report.lblBMName.Text = param.BMName;
            report.lblFinName.Text = param.FinName;
            report.lblSignedDate.Text = param.DateSigned.ToShortDateString();

            return report;
        }

        private string CreateFileName(ResponseViewModel entity)
        {
            //Create report filename by taking the Branch + Month + Literal ICMReport;
            var result = $"ICMReport_{entity?.Branch}_{entity?.Month}_{DateTime.Now.Year}";

            return result;
        }
    }
}

using System.Linq;
using System.Web.Mvc;
using oforms.Models;
using oforms.Services;
using oforms.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;

namespace oforms.Controllers
{
    [Admin]
    public class ResultsAdminController : Controller
    {
        private readonly IRepository<OFormFileRecord> _fileRepo;
        private readonly IRepository<OFormResultRecord> _resultsRepo;
        private readonly IOrchardServices _services;
        private readonly ISerialService _serialService;

        public ResultsAdminController(
            IOrchardServices services,
            ISerialService serialService,
            IShapeFactory shapeFactory,
            IRepository<OFormResultRecord> resultsRepo,
            IRepository<OFormFileRecord> fileRepo)
        {
            this._services = services;
            this._serialService = serialService;
            this._resultsRepo = resultsRepo;
            this._fileRepo = fileRepo;
            this.Shape = shapeFactory;

            T = NullLocalizer.Instance;
        }

        dynamic Shape { get; set; }

        public Localizer T { get; set; }

        public ActionResult ShowFormResults(int id, PagerParameters pagerParameters)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view form results")))
                return new HttpUnauthorizedResult();

            var form = _services.ContentManager.Get<OFormPart>(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();

            CheckValidSerial();
            var pager = new Pager(_services.WorkContext.CurrentSite, pagerParameters);

            var formResults = _resultsRepo.Table.Where(x => x.OFormPartRecord == form.Record);

            var pagerShape = Shape.Pager(pager).TotalItemCount(formResults.Count());
            var results = formResults
                .OrderByDescending(x => x.CreatedDate)
                .Skip(pager.GetStartIndex()).Take(pager.PageSize);

            return View(new FormResultViewModel
            {
                FormId = form.Id,
                FormName = form.Name,
                Results = results.ToList(),
                Pager = pagerShape
            });
        }

        public ActionResult ShowResultDetails(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view form results")))
                return new HttpUnauthorizedResult();

            var result = _resultsRepo.Fetch(x => x.Id == id).SingleOrDefault();
            if (result == null)
            {
                return HttpNotFound();
            }

            this.CheckValidSerial();

            return View(result);
        }

        [HttpPost]
        public ActionResult DeleteResult(int id, int formId, int? currentPage, int? pageSize)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view form results")))
                return new HttpUnauthorizedResult();

            var result = _resultsRepo.Fetch(x => x.Id == id && x.OFormPartRecord.Id == formId).SingleOrDefault();
            if (result == null)
            {
                return HttpNotFound();
            }

            this.CheckValidSerial();

            _resultsRepo.Delete(result);

            _services.Notifier.Information(T("Result was successfully removed"));

            return RedirectToAction("ShowFormResults", new { id = formId, page = currentPage, pageSize });
        }

        public ActionResult DownloadCsv(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to download form results")))
                return new HttpUnauthorizedResult();

            var form = _services.ContentManager.Get<OFormPart>(id, VersionOptions.Latest);
            if (form == null)
                return HttpNotFound();


            var formResults = _resultsRepo.Table.Where(x => x.OFormPartRecord == form.Record)
                .OrderByDescending(x => x.CreatedDate);
            var csvBuilder = new StringBuilder();
            var csvColumns = new List<string>();
            foreach (var result in formResults)
            {
                var reader = new OFormResultReader(result.Xml);
                bool isFirstCol = true;
                foreach (var col in csvColumns)
                {
                    csvBuilder.AppendFormat("{1}\"{0}\"", reader.GetColumnValue(col), isFirstCol ? string.Empty : ",");
                    isFirstCol = false;
                }

                foreach (var col in reader.Columns.Where(c => !csvColumns.Contains(c))) 
                {
                    csvBuilder.AppendFormat("{1}\"{0}\"", reader.GetColumnValue(col), isFirstCol ? string.Empty : ",");
                    isFirstCol = false;
                    csvColumns.Add(col);
                }

                csvBuilder.AppendLine();
            }

            var memoryStream = GetCsvStream(csvBuilder, csvColumns);
            return File(memoryStream, "text/csv", form.Name + "-results.csv");
        }

        public ActionResult DownloadResultFile(int id)
        {
            if (!_services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not authorized to view result files")))
                return new HttpUnauthorizedResult();

            var file = _fileRepo.Fetch(x => x.Id == id).SingleOrDefault();
            if (file == null)
            {
                return HttpNotFound();
            }

            return File(file.Bytes, file.ContentType ?? "application/octet-stream", file.OriginalName);
        }

        private void CheckValidSerial()
        {
            ViewData["validSn"] = _serialService.ValidateSerial();
        }

        private static MemoryStream GetCsvStream(StringBuilder csvBuilder, List<string> csvColumns)
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            writer.WriteLine(string.Join(",", csvColumns));
            writer.Write(csvBuilder.ToString());
            writer.Flush();

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
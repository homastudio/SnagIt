using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberStride.Contacts.Services;
using Orchard.Settings;
using Orchard.DisplayManagement;
using Orchard;
using Orchard.Logging;
using Orchard.Localization;
using Orchard.UI.Navigation;
using CyberStride.Contacts.Models;
using Orchard.ContentManagement;
using CyberStride.Contacts.ViewModels;
using Orchard.UI.Notify;
using Orchard.Mvc.Extensions;

namespace CyberStride.Contacts.Controllers
{
    public class AdminController : Controller
    {
        private readonly IContactService _contactService;
        private readonly ISiteService _siteService;
        private readonly INotifier _notifier;

        public AdminController(IOrchardServices orchardService, 
            IContactService contactService, ISiteService siteService, 
            IShapeFactory shapeFactory, INotifier notifier)
        {
            OrchardService = orchardService;
            _contactService = contactService;
            _siteService = siteService;
            Shape = shapeFactory;
            Log = NullLogger.Instance;
            T = NullLocalizer.Instance;
            _notifier = notifier;
        }

        public IOrchardServices OrchardService { get; set; }
        dynamic Shape { get; set; }
        public ILogger Log { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index(PagerParameters pagerParameters)
        {
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
            IContentQuery<ContactPart, ContactPartRecord> contacts = _contactService.GetContacts();
            var pagerShape = Shape.Pager(pager).TotalItemCount(contacts.Count());
            var contactRequests = contacts
                .OrderByDescending<ContactPartRecord, DateTime?>(cpr => cpr.ContactDateUtc)
                .Slice(pager.GetStartIndex(), pager.PageSize)
                .ToList(); // TODO: see if we need to return something other than the raw contact items here

            var model = new ContactRequestListViewModel
            {
                ContactRequests = contactRequests,
                Pager = pagerShape
            };
            return View(model); 
        }

        public ActionResult Details(int id)
        {
            var contactRequest = _contactService.GetContact(id);
            if (contactRequest == null)
            {
                _notifier.Warning(T("Contact request not found, please check your URL"));
                return RedirectToAction("index");
            }

            return View(contactRequest);
        }

        [HttpPost]
        public ActionResult Delete(int id, string returnUrl)
        {
            if (!OrchardService.Authorizer.Authorize(Permissions.ManageContacts, T("Couldn't delete contactRequest")))
                return new HttpUnauthorizedResult();
            var item = OrchardService.ContentManager.Get(id);
            if (item == null)
            {
                _notifier.Error(T("Item not found"));
            }
            else
            {
                OrchardService.ContentManager.Remove(item);
                _notifier.Information(T("Contact request deleted"));
            }

            return this.RedirectLocal(returnUrl, "~/");
        }
    }
}
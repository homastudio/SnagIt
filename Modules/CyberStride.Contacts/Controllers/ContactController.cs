using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberStride.Contacts.Services;
using Orchard;
using Orchard.UI.Notify;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using CyberStride.Contacts.Models;
using Orchard.Messaging.Services;
using Orchard.ContentManagement;

namespace CyberStride.Contacts.Controllers
{
    public class ContactController : Controller
    {
        public IOrchardServices Services { get; set; }
        private readonly IContactService _contactService;
        private readonly IMessageManager _messenger;
        private readonly INotifier _notifier;
        public Localizer T { get; set; }
        
        public ContactController(IOrchardServices services, IContactService contactService, 
            IMessageManager messenger, INotifier notifier)
        {
            Services = services;
            _contactService = contactService;
            _messenger = messenger;
            _notifier = notifier;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(int id, string returnUrl)
        {
            var model = Services.ContentManager.Create<ContactPart>("Contact");
            var form = Services.ContentManager.Get(id);
            if (InvalidForm(form))
            {                
                _notifier.Error(T("Form submission was not from a valid source"));
                return this.RedirectLocal(returnUrl, "~/");
            }
            
            var contactForm = form.Parts.FirstOrDefault(p=> p is ContactFormPart);

            TryUpdateModel(model, new[]{"Company","CurrentWebsite","Email","Message","Name","Phone","Topic"});

            if (ModelState.IsValid)
            {
                var contact = _contactService.MakeContact(model.Record);
                _messenger.Send(contactForm.ContentItem.Record, MessageTypes.ContactRequest, "email", mapProperties(contact)); 
                _notifier.Information(T("Thanks for your inquiry, someone will respond to you shortly.")); // TODO: great place for a setting
            }
            else
            {
                notifyUserOfErrors(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                populateTempDataWithViewModelValues(model);
            }

            return this.RedirectLocal(returnUrl, "~/");
        }

        private Dictionary<string, string> mapProperties(ContactPart contact)
        {
            return new Dictionary<string, string>
            {
                {"Company", contact.Company},
                {"Current Website", contact.CurrentWebsite},
                {"Email", contact.Email},
                {"Message", contact.Message},
                {"Name",contact.Name},
                {"Phone",contact.Phone},
                {"Topic",contact.Topic}
            };
        }

        private bool InvalidForm(Orchard.ContentManagement.ContentItem form)
        {
            return form == null || !form.Has(typeof(ContactFormPart));
        }

        private void populateTempDataWithViewModelValues(ContactPart record)
        {
            /// HACK: better to just stuff the record in there?  Or should we not be using the record here at all?
            TempData["Contact.Name"] = record.Name;
            TempData["Contact.Company"] = record.Company;
            TempData["Contact.CurrentWebsite"] = record.CurrentWebsite;
            TempData["Contact.Email"] = record.Email;
            TempData["Contact.Message"] = record.Message;
            TempData["Contact.Phone"] = record.Phone;
            TempData["Contact.Topic"] = record.Topic;
        }

        private void notifyUserOfErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
                _notifier.Error(T(error));
        }
    }
}
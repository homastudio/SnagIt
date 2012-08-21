using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using CyberStride.Contacts.Models;
using Orchard.Services;
using Orchard.ContentManagement;

namespace CyberStride.Contacts.Services
{
    [UsedImplicitly]
    public class ContactService : IContactService
    {
        public IOrchardServices Services { get; set; }

        public ContactService(IOrchardServices services)
        {
            Services = services;
        }
        public ContactPart MakeContact(ContactPartRecord record)
        {
            ContactPart contact = Services.ContentManager.Create<ContactPart>("Contact");
            contact.Company = record.Company;
            contact.CurrentWebsite = record.CurrentWebsite;
            contact.Email = record.Email;
            contact.Message = record.Message;
            contact.Name = record.Name;
            contact.Phone = record.Phone;
            contact.Topic = record.Topic;
            contact.ContactDateUtc = DateTime.UtcNow;

            return contact;
        }

        public IContentQuery<ContactPart, ContactPartRecord> GetContacts()
        {
            return Services.ContentManager
                .Query<ContactPart, ContactPartRecord>();
        }

        public ContactPart GetContact(int id)
        {
            return Services.ContentManager.Get<ContactPart>(id);
        }
    }
}
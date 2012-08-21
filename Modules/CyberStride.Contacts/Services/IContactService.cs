using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyberStride.Contacts.Models;
using Orchard;
using Orchard.ContentManagement;

namespace CyberStride.Contacts.Services
{
    public interface IContactService : IDependency
    {
        ContactPart MakeContact(ContactPartRecord record);
        
        IContentQuery<ContactPart, ContactPartRecord> GetContacts();

        ContactPart GetContact(int id);
    }
}
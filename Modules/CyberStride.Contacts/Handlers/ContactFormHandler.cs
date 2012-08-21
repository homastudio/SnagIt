using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using CyberStride.Contacts.Models;

namespace CyberStride.Contacts.Handlers
{
    public class ContactFormHandler : ContentHandler
    {
        public ContactFormHandler(IRepository<ContactFormPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
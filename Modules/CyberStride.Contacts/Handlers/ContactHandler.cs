using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Handlers;
using CyberStride.Contacts.Models;
using Orchard.Data;

namespace CyberStride.Contacts.Handlers
{
    public class ContactHandler : ContentHandler
    {
        public ContactHandler(IRepository<ContactPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
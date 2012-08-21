using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberStride.Contacts.ViewModels
{
    public class ContactRequestListViewModel
    {
        public List<Models.ContactPart> ContactRequests { get; set; }
        public dynamic Pager { get; set; }
    }
}
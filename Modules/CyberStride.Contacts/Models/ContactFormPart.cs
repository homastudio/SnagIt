using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System.ComponentModel;
using System;
using Orchard.Security;

namespace CyberStride.Contacts.Models
{
    public class ContactFormPartRecord : ContentPartRecord
    {
        public virtual string EmailTo { get; set; }
    }
    public class ContactFormPart : ContentPart<ContactFormPartRecord>, IUser
    {
        [DataType(DataType.EmailAddress)]
        public string EmailTo
        {
            get { return Record.EmailTo; }
            set { Record.EmailTo = value; }
        }

        public string Email
        {
            get { return EmailTo; }
        }

        public string UserName
        {
            get { return EmailTo; }
        }
    }
}
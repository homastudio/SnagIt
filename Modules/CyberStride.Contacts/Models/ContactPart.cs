using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System.ComponentModel;
using System;
using Orchard.Security;

namespace CyberStride.Contacts.Models
{
    public class ContactPartRecord : ContentPartRecord
    {
        public virtual string Name { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Email { get; set; }
        public virtual string Company { get; set; }
        public virtual string CurrentWebsite { get; set; }
        public virtual string Topic { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime? ContactDateUtc { get; set; }

    }

    public class ContactPart : ContentPart<ContactPartRecord>
    {
        public ContactPart()
        {
            Record = new ContactPartRecord();
        }
        [Required]
        public string Name
        {
            get { return Record.Name; }
            set { Record.Name = value; }
        }

        [RegularExpression(Validations.PHONE_PATTERN, ErrorMessage="Phone number is not in a valid format")]
        public string Phone
        {
            get { return Record.Phone; }
            set { Record.Phone = value; }
        }

        [RegularExpression(Validations.EMAIL_ADDRESS_PATTERN, ErrorMessage="Email does not appear to be in a valid format")]
        [Required]
        public string Email
        {
            get { return Record.Email; }
            set { Record.Email = value; }
        }

        public string Company
        {
            get { return Record.Company; }
            set { Record.Company = value; }
        }

        [RegularExpression(Validations.URL_PATTERN, ErrorMessage="Web address does not appear to be in a valid format")]
        [DisplayName("Current Website")]
        public string CurrentWebsite
        {
            get { return Record.CurrentWebsite; }
            set { Record.CurrentWebsite = value; }
        }

        public string Topic
        {
            get { return Record.Topic; }
            set { Record.Topic = value; }
        }

        [Required]
        public string Message
        {
            get { return Record.Message; }
            set { Record.Message = value; }
        }

        public DateTime? ContactDateUtc
        {
            get { return Record.ContactDateUtc; }
            set { Record.ContactDateUtc = value; }
        }
    }
}
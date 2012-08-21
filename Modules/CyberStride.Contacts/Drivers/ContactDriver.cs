using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using CyberStride.Contacts.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;

namespace CyberStride.Contacts.Drivers
{
    [UsedImplicitly]
    public class ContactDriver : ContentPartDriver<ContactPart>
    {
        protected override DriverResult Display(ContactPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Contact", () => shapeHelper.Parts_Contact(
                    Company: part.Company,
                    CurrentWebsite: part.CurrentWebsite,
                    Email: part.Email,
                    Message: part.Message,
                    Name: part.Name,
                    Phone: part.Phone,
                    Topic: part.Topic
            ));
        }
    }
}
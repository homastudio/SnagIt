using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using CyberStride.Contacts.Models;
using Orchard;

namespace CyberStride.Contacts.Drivers
{
    public class ContactFormDriver : ContentPartDriver<ContactFormPart>
    {
        public ContactFormDriver(IOrchardServices orchardService)
        {
            this.orchardService = orchardService;
        }

        protected override DriverResult Display(ContactFormPart part, string displayType, dynamic shapeHelper)
        {
            if(displayType.StartsWith("Detail"))
                return ContentShape("Parts_ContactForm", () => shapeHelper.Parts_ContactForm(
                        ContentPart: part
                ));
            return null;
        }
        
        protected override DriverResult Editor(ContactFormPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ContactForm_Edit",
                ()=> shapeHelper.EditorTemplate(TemplateName: "Parts.ContactForm", Model: part, Prefix: Prefix)
                    );
        }

        protected override DriverResult Editor(ContactFormPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper)
        {
            bool success = updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        private readonly IOrchardServices orchardService;
    }
}
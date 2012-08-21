using System;
using Orchard.Localization;
using Orchard.Messaging.Events;
using Orchard.Messaging.Models;
using Orchard.ContentManagement;
using Orchard.Settings;
using CyberStride.Contacts.Models;

namespace Orchard.Users.Handlers {
    public class ContactMessagesAlteration : IMessageEventHandler {
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public ContactMessagesAlteration(IContentManager contentManager, ISiteService siteService) {
            _contentManager = contentManager;
            _siteService = siteService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Sending(MessageContext context) {
            if (context.MessagePrepared)
                return;

            var contentItem = _contentManager.Get(context.Recipient.Id);
            if ( contentItem == null )
                return;

            var recipient = contentItem.As<ContactFormPart>();
            if ( recipient == null )
                return;
           
            /*
             * {"Company", contact.Company},
                {"Current Website", contact.CurrentWebsite},
                {"Email", contact.Email},
                {"Message", contact.Message},
                {"Name",contact.Name},
                {"Phone",contact.Phone},
                {"Topic",contact.Topic}
             */

            switch (context.Type) {
                case MessageTypes.ContactRequest:
                    context.MailMessage.Subject = T("[New Contact Request]: {0}", context.Properties["Topic"]).Text;
                    context.MailMessage.Body =
                        T("<p>The visitor <b>{0}</b> with email <b>{1}</b> has requested contact.</p><ul><li>Company: {2}</li><li>Current Website: {3}</li><li>Phone: {4}</li><li>Message: {5}</li></ul>",
                            context.Properties["Name"], context.Properties["Email"], context.Properties["Company"],context.Properties["Current Website"], context.Properties["Phone"], context.Properties["Message"]).Text;
                    FormatEmailBody(context);
                    context.MessagePrepared = true;
                    break;
            }
        }

        private static void FormatEmailBody(MessageContext context) {
            context.MailMessage.Body = "<p style=\"font-family:Arial, Helvetica; font-size:10pt;\">" + context.MailMessage.Body;
            context.MailMessage.Body += "</p>";
        }

        public void Sent(MessageContext context) {
        }
    }
}

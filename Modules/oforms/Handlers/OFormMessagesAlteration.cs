using System;
using System.Linq;
using System.Net.Mail;
using Orchard.ContentManagement;
using Orchard.Messaging.Events;
using Orchard.Messaging.Models;
using oforms.Models;
using Orchard.Data;
using System.IO;

namespace oforms.Handlers
{
    public class OFormMessagesAlteration : IMessageEventHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IRepository<OFormResultRecord> _resultRepo;

        public OFormMessagesAlteration(IContentManager contentManager, IRepository<OFormResultRecord> resultRepo)
        {
            this._contentManager = contentManager;
            this._resultRepo = resultRepo;
        }

        public void Sending(MessageContext context)
        {
            if (context.MessagePrepared)
                return;

            switch (context.Type)
            { 
                case MessageTypes.SendFormResult:
                    var form = _contentManager.Get<OFormPart>(context.Recipients.First().Id);
                    if (form == null)
                        return;

                    foreach (var email in form.EmailSendTo.Split(new []{';', ' ', ','}, StringSplitOptions.RemoveEmptyEntries))
                    { 
                        var mailAddress = new MailAddress(email);
                        context.MailMessage.To.Add(mailAddress); 
                    }

                    context.MailMessage.Subject = form.EmailSubject ?? "no subject";
                    var template = form.EmailTemplate ?? "no template";
                    foreach (var key in context.Properties.Keys)  
                    {
                        template = template.Replace("{" + key + "}", context.Properties[key]);
                    }

                    template += @"
Form submitted on " + context.Properties[OFormGlobals.CreatedDateKey] + " from ip " + context.Properties[OFormGlobals.IpKey];

                    context.MailMessage.Body = template.Replace("\r\n", "\n").Replace("\n", "<br />");

                    // add message attachments, if any
                    var resultId = Convert.ToInt32(context.Properties["oforms.formResult.Id"]);
                    var result = this._resultRepo.Get(resultId);
                    if (result != null)
                    {
                        foreach (var file in result.Files)
                        {
                            var ms = new MemoryStream(file.Bytes);
                            var attachment = new Attachment(ms, 
                                String.Format("{1}_{0}", file.OriginalName, file.FieldName), 
                                file.ContentType);
                            context.MailMessage.Attachments.Add(attachment);
                        }
                    }

                    context.MessagePrepared = true;
                    break;
            }
        }

        public void Sent(MessageContext context)
        {
        }
    }
}